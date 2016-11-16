namespace Compilador_Arthur.Diversos
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    /// <summary>
    /// Classe estática com funções diversas do sistema
    /// </summary>
    public static class Funcoes
    {
        #region Métodos Públicos
        #region Funções de Objetivo Geral
        /// <summary>
        /// Apresenta uma mensagem de erro personalizada de acordo com os padrões do projeto
        /// </summary>
        /// <param name="mensagemPersonalizada">Mensagem personalizada a ser apresentada originalmente</param>
        /// <param name="ex">Excessão que causou o erro</param>
        public static void ApresentarMensagemErro(string mensagemPersonalizada, Exception ex)
        {
            MessageBox.Show(
                string.Format("{0}{1}{2}", mensagemPersonalizada, Environment.NewLine, ex.Message),
                "Atenção",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        /// <summary>
        /// Inicializa as listas que irão compor a tabela de símbolos
        /// palavras reservadas
        /// </summary>
        public static void InicializarTabelaSimbolos()
        {
            // Lista de caracteres inválidos
            Propriedades.CaracteresIncorretos.Add("@");
            Propriedades.CaracteresIncorretos.Add("~");
            Propriedades.CaracteresIncorretos.Add("^");
            Propriedades.CaracteresIncorretos.Add("`");
            Propriedades.CaracteresIncorretos.Add("&");
            Propriedades.CaracteresIncorretos.Add("$");
            Propriedades.CaracteresIncorretos.Add("#");
            Propriedades.CaracteresIncorretos.Add("!");

            // Lista de operadores lógicos
            Propriedades.OperadoresRelacionais.Add("<=");
            Propriedades.OperadoresRelacionais.Add(">=");
            Propriedades.OperadoresRelacionais.Add("<>");
            Propriedades.OperadoresRelacionais.Add("=");
            Propriedades.OperadoresRelacionais.Add("<");
            Propriedades.OperadoresRelacionais.Add(">");

            // Lista dos operadores aritméticos
            Propriedades.OperadoresAritmeticos.Add("+");
            Propriedades.OperadoresAritmeticos.Add("-");
            Propriedades.OperadoresAritmeticos.Add("*");
            Propriedades.OperadoresAritmeticos.Add("/");

            // Lista dos operadores de atribuição
            Propriedades.OperadoresAtribuicao.Add("<-");

            // Lista das palavras reservadas
            Propriedades.PalavrasReservadas.Add("inicio\r\n");
            Propriedades.PalavrasReservadas.Add("fim");
            Propriedades.PalavrasReservadas.Add("enquanto");
            Propriedades.PalavrasReservadas.Add("para");
            Propriedades.PalavrasReservadas.Add("se");
            Propriedades.PalavrasReservadas.Add("senao");
            Propriedades.PalavrasReservadas.Add("faca");
            Propriedades.PalavrasReservadas.Add("caso");
            Propriedades.PalavrasReservadas.Add("ou");
            Propriedades.PalavrasReservadas.Add("verdadeiro");
            Propriedades.PalavrasReservadas.Add("falso");
            Propriedades.PalavrasReservadas.Add("literal");
            Propriedades.PalavrasReservadas.Add("inteiro");
            Propriedades.PalavrasReservadas.Add("real");
            Propriedades.PalavrasReservadas.Add("caractere");
            Propriedades.PalavrasReservadas.Add("logico");
        }

        /// <summary>
        /// Realiza a quebra dos tokens
        /// </summary>
        /// <param name="programa">String contendo o programa</param>
        public static List<string> QuebrarTokensPrograma(string programa)
        {
            // Monta uma lista principal para quebrar os tokens
            List<string> tabelaSimbolos = new List<string>();
            tabelaSimbolos.AddRange(Propriedades.CaracteresIncorretos);
            tabelaSimbolos.AddRange(Propriedades.OperadoresRelacionais);
            tabelaSimbolos.AddRange(Propriedades.OperadoresAritmeticos);
            tabelaSimbolos.AddRange(Propriedades.OperadoresAtribuicao);
            tabelaSimbolos.AddRange(Propriedades.PalavrasReservadas);

            // Lista com tokens compostos
            List<string> tokensCompostos = new List<string>();
            tokensCompostos.Add("<=");
            tokensCompostos.Add(">=");
            tokensCompostos.Add("<>");
            tokensCompostos.Add("<-");

            // Cria a formula para quebra dos tokens
            string formula = string.Format("({0})",
                string.Join("|", tabelaSimbolos.Select(p => Regex.Escape(p)).ToArray()));

            // Cria a formula para verificação dos tokens compostos
            string formulaTC = string.Format("({0})",
                string.Join("|", tokensCompostos.Select(p => Regex.Escape(p)).ToArray()));

            // Quebra os tokens
            string[] tokensTemporarios = Regex.Split(programa.Trim(), formula);

            // Monta a lista final dos tokens quebrados
            List<string> tokens = new List<string>();
            for (int i = 0; i < tokensTemporarios.Length; i++)
            {
                // Adiciona o token à lista se ele não estiver vazio
                if (tokensTemporarios[i] != string.Empty)
                {
                    tokens.Add(tokensTemporarios[i].Trim('\x20', '\t'));
                }
            }

            // Por fim, unifica os tokens compostos
            List<string> tokensFinal = new List<string>();
            bool tokenComposto = false;
            for (int i = 0; i < tokens.Count; i++)
            {
                // Se o token atual já foi adicionado por ser um token composto, pula para o próximo
                if (tokenComposto)
                {
                    tokenComposto = false;
                    continue;
                }

                // Monta a string do token atual
                string adicionar = tokens[i];
                if (i + 1 == tokens.Count)
                {
                    tokensFinal.Add(adicionar);
                    break;
                }

                // Verifica se é um token composto
                string temporario = adicionar + tokens[i + 1];
                if (Regex.IsMatch(temporario, formulaTC))
                {
                    adicionar = temporario;
                    tokenComposto = true;
                }

                tokensFinal.Add(adicionar);
            }

            // Retorna os tokens
            return tokensFinal;
        }

        /// <summary>
        /// Remove as strings de uma linha, no caso delas existirem
        /// </summary>
        /// <param name="linha">Linha do programa a ser analisada</param>
        /// <returns>Nova linha, sem as strings</returns>
        public static string RemoverString(string linha)
        {
            // Remove as strings, no caso delas existirem
            int indiceInicial = linha.IndexOf('"');
            if (indiceInicial < 0)
            {
                return linha;
            }

            int indiceFinal = linha.IndexOf('"', indiceInicial + 1);
            if (indiceFinal < 0)
            {
                throw new Exception(
                    "String inválida, ela não foi finalizada." + Environment.NewLine);
            }

            return linha.Remove(indiceInicial - 1);
        }

        /// <summary>
        /// Remove os comentários de uma linha, no caso deles existirem
        /// </summary>
        /// <param name="linha">Linha do programa a ser analisada</param>
        /// <returns>Nova linha, sem os comentários</returns>
        public static string RemoverComentarios(string linha)
        {
            // Remove as strings, no caso delas existirem
            int indiceInicial = linha.IndexOf("//");
            if (indiceInicial < 0)
            {
                return linha;
            }

            return linha.Remove(indiceInicial);
        }
        #endregion

        #region Funções de Validação
        /// <summary>
        /// Realiza a validação da estrutura do programa
        /// </summary>
        /// <param name="programa">String contendo o programa</param>
        public static void VerificarEstruturaPrincipal(string[] programa)
        {
            // Verifica se o programa não está vazio
            if (programa[0].Length < 6)
            {
                throw new Exception(
                    "Erro de estrutura. O programa deve respeitar a estrutura \"inicio fim\".");
            }

            // Verifica qual é a primeira linha válida
            string primeiraLinhaValida = string.Empty;
            string ultimaLinhaValida = string.Empty;
            for (int i = 0; i < programa.Length; i++)
            {
                programa[i] = Funcoes.RemoverComentarios(programa[i]);
            }

            // Obtém a primeira e a última linha do programa
            primeiraLinhaValida = programa.FirstOrDefault(x => !string.IsNullOrEmpty(x)).Trim();
            ultimaLinhaValida = programa.LastOrDefault(x => !string.IsNullOrEmpty(x)).Trim();

            // Verifica se o programa começa com início
            if (primeiraLinhaValida != "inicio")
            {
                throw new Exception("Não foi encontrado o \"inicio\" do programa.");
            }

            // Verifica se o programa termina com fim
            if (ultimaLinhaValida != "fim")
            {
                throw new Exception("Não foi encontrado o \"fim\" do programa.");
            }
        }

        /// <summary>
        /// Verifica se existem caracteres inválidos
        /// </summary>
        /// <param name="linhaCodigo">Linha de código a ser analisada</param>
        /// <returns>Erro ou string completa</returns>
        public static string VerificarCaracteresInvalidos(string linhaCodigo)
        {
            // Remove as strings, no caso delas existirem
            try
            {
                linhaCodigo = Funcoes.RemoverString(linhaCodigo);
            }
            catch (Exception ex)
            {
                // Em caso de erro retorna a mensagem
                // Necessário para que a compilação não seja interrompida
                return ex.Message;
            }

            // Verifica se foi encontrado algum dos caracteres inválidos
            foreach (string caractereInvalido in Propriedades.CaracteresIncorretos)
            {
                if (linhaCodigo.Contains(caractereInvalido))
                {
                    return string.Format("Caractere inválido encontrado. ('{0}'){1}",
                        caractereInvalido,
                        Environment.NewLine);
                }
            }

            // Se não foi encontrado nenhum caractere inválido, retorna string vazia
            return string.Empty;
        }

        /// <summary>
        /// Verifica se a linha possui uma variável e adiciona ela à lista
        /// </summary>
        /// <param name="linhaCodigo">Linha de código a ser analisada</param>
        public static string VerificarVariavel(string linhaCodigo)
        {
            // Remove as strings, no caso delas existirem
            try
            {
                linhaCodigo = Funcoes.RemoverString(linhaCodigo);
            }
            catch (Exception ex)
            {
                // Em caso de erro retorna a mensagem
                // Necessário para que a compilação não seja interrompida
                return ex.Message;
            }

            // Fórmula com os operadores aritmeticos
            string aritmeticos = string.Format("({0})",
                string.Join(
                    "|",
                    Propriedades.OperadoresAritmeticos.Select(p => Regex.Escape(p)).ToArray()));

            string atribuicao = string.Format("({0})",
                string.Join(
                    "|", Propriedades.OperadoresAtribuicao.Select(p => Regex.Escape(p)).ToArray()));

            // Cria a expressão regular para verificação da variável
            Regex regex = new Regex(
                @"(.*)" + atribuicao + "( |)(\\(|)(.*|" + aritmeticos + ")(.*)+");
            if (!regex.IsMatch(linhaCodigo))
            {
                return "Estrutura inválida de declaração da variável." + Environment.NewLine;
            }

            // Adiciona a variável à lista
            Propriedades.Variaveis.Add(
                linhaCodigo.Substring(0, linhaCodigo.IndexOf("<-")).Trim());

            // Se não foi encontrado nenhum erro, retorna string vazia
            return string.Empty;
        }

        /// <summary>
        /// Verifica a função "SE"
        /// </summary>
        /// <param name="linhaCodigo">Linha de código a ser analisada</param>
        /// <returns>Erro ou string completa</returns>
        public static string VerificarSe(string linhaCodigo)
        {
            // Converte as variáveis para o formato aceito pelo Regex
            string variaveis = string.Format("({0})",
                string.Join("|", Propriedades.Variaveis.Select(p => Regex.Escape(p)).ToArray()));
            string condicionais = string.Format("({0})",
                string.Join(
                    "|",
                    Propriedades.OperadoresRelacionais.Select(p => Regex.Escape(p)).ToArray()));

            // Cria a expressão regular para verificação do se
            Regex regex = new Regex(
                @"se( |)(\(|)( |)" + variaveis + "( |)" + condicionais + "[^)]*(\\)|)( |)entao+");
            if (!regex.IsMatch(linhaCodigo))
            {
                return "Estrutura inválida do \"se\"." + Environment.NewLine;
            }

            // Se não foi encontrado nenhum erro, retorna string vazia
            return string.Empty;
        }

        /// <summary>
        /// Verifica a função "PARA"
        /// </summary>
        /// <param name="linhaCodigo">Linha de código a ser analisada</param>
        /// <returns>Erro ou string completa</returns>
        public static string VerificarPara(string linhaCodigo)
        {
            // Converte as variáveis para o formato aceito pelo Regex
            string variaveis = string.Format("({0})",
                string.Join("|", Propriedades.Variaveis.Select(p => Regex.Escape(p)).ToArray()));

            // Cria a expressão regular para verificação do para
            Regex regex = new Regex(
                @"para( |)(\(|)( |)(" + variaveis + "|[0-9]*)( |)ate( |)(" + variaveis + "|[0-9]*)(\\)|)( |)entao+");
            if (!regex.IsMatch(linhaCodigo))
            {
                return "Estrutura inválida do \"para\"." + Environment.NewLine;
            }

            // Se não foi encontrado nenhum erro, retorna string vazia
            return string.Empty;
        }
        #endregion
        #endregion
    }
}