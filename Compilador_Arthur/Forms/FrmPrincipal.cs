namespace Compilador_Arthur.Forms
{
    using Diversos;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Forms;

    /// <summary>
    /// Classe do formulário principal do sistema
    /// </summary>
    public partial class FrmPrincipal : Form
    {
        #region Construtores
        /// <summary>
        /// Obtém uma instância da classe <see cref="FrmPrincipal"/> 
        /// </summary>
        public FrmPrincipal()
        {
            // Inicializa os componentes do form
            this.InitializeComponent();

            // Inicializa a tabela de símbolos
            Funcoes.InicializarTabelaSimbolos();
        }
        #endregion

        #region Métodos
        #region Métodos de Componentes
        /// <summary>
        /// Método executado ao ativar o evento click do botão salvar
        /// </summary>
        /// <param name="sender">Parâmetro sender</param>
        /// <param name="e">Parâmetro com os argumentos do evento</param>
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog dialogo = new SaveFileDialog())
                {
                    // Define as opções da caixa de diálogo
                    dialogo.Filter = "Arquivos Drazz (*.drz)|*.drz";
                    dialogo.Title = "Selecione o local e o nome do arquivo.";

                    // Abre a caixa de diálogo
                    if (dialogo.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    // Escreve o arquivo de texto conforme o nome selecionado
                    StreamWriter arquivo = new StreamWriter(dialogo.FileName);
                    arquivo.WriteLine(this.txtDesenvolvimento.Text);

                    // Fecha o arquivo
                    arquivo.Close();
                }
            }
            catch (Exception ex)
            {
                Funcoes.ApresentarMensagemErro("Ocorreu um erro ao salvar o arquivo", ex);
            }
        }

        /// <summary>
        /// Método executado ao ativar o evento click do botão abrir
        /// </summary>
        /// <param name="sender">Parâmetro sender</param>
        /// <param name="e">Parâmetro com os argumentos do evento</param>
        private void btnAbrir_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog dialogo = new OpenFileDialog())
                {
                    // Define as opções da caixa de diálogo
                    dialogo.Filter = "Arquivos Drazz (*.drz)|*.drz";
                    dialogo.Title = "Selecione o local e o nome do arquivo.";

                    // Abre a caixa de diálogo
                    if (dialogo.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    // Abre o arquivo Drazz conforme o nome selecionado
                    StreamReader arquivo = new StreamReader(dialogo.FileName);
                    this.txtDesenvolvimento.Text = string.Empty;
                    this.txtDesenvolvimento.Text = arquivo.ReadToEnd();

                    // Fecha o arquivo
                    arquivo.Close();
                }
            }
            catch (Exception ex)
            {
                Funcoes.ApresentarMensagemErro("Ocorreu um erro ao abrir o arquivo", ex);
            }
        }

        /// <summary>
        /// Método executado ao ativar o evento click do botão compilar
        /// </summary>
        /// <param name="sender">Parâmetro sender</param>
        /// <param name="e">Parâmetro com os argumentos do evento</param>
        private void btnCompilar_Click(object sender, EventArgs e)
        {
            try
            {
                // String contendo o resultado da compilação
                string resultadoCompilacao = string.Empty;
                string resultadoTemp = string.Empty;

                // Define se houve erro de compilação
                bool temErro = false;

                // Quebra as expressoes do programa
                string[] lista = this.txtDesenvolvimento.Text.Trim().Split(
                    "\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                // Faz a verificação da estrutura principal do sistema (inicio fim)
                Funcoes.VerificarEstruturaPrincipal(lista);

                // Quebrar os token do programa
                List<string> tokens =
                    Funcoes.QuebrarTokensPrograma(this.txtDesenvolvimento.Text.Trim());

                // Define o número de palavras existentes
                resultadoCompilacao = "Número de palavras: " + tokens.Count + Environment.NewLine;

                // Verifica se o número de "se" e "para" iniciados
                int seIniciado = 0;
                int seFinalizado = 0;
                int paraIniciado = 0;
                int paraFinalizado = 0;
                foreach (string comando in lista)
                {
                    // Remove os comentários da linha
                    string comandoSemComentarios = Funcoes.RemoverComentarios(comando.Trim());
                    if (string.IsNullOrEmpty(comandoSemComentarios))
                    {
                        continue;
                    }

                    // Verifica se existem caracteres inválidos
                    string caracteresInvalidos =
                        Funcoes.VerificarCaracteresInvalidos(comandoSemComentarios);
                    if (!string.IsNullOrEmpty(caracteresInvalidos))
                    {
                        resultadoCompilacao += caracteresInvalidos;
                        temErro = true;
                        continue;
                    }

                    // Verifica e adiciona as variáveis
                    if (comandoSemComentarios.Contains("<-"))
                    {
                        resultadoTemp = Funcoes.VerificarVariavel(comandoSemComentarios);
                        if (!string.IsNullOrEmpty(resultadoTemp))
                        {
                            temErro = true;
                            resultadoCompilacao = resultadoTemp;
                        }
                    }

                    // Verifica a condição "SE"
                    if ((comandoSemComentarios.Length >= 2) &&
                        (comandoSemComentarios.Substring(0, 2) == "se"))
                    {
                        seIniciado++;
                        resultadoTemp = Funcoes.VerificarSe(comandoSemComentarios);
                        if (!string.IsNullOrEmpty(resultadoTemp))
                        {
                            temErro = true;
                            resultadoCompilacao = resultadoTemp;
                        }
                        continue;
                    }

                    // Verifica a condição "PARA"
                    if ((comandoSemComentarios.Length >= 4) &&
                        (comandoSemComentarios.Substring(0, 4) == "para"))
                    {
                        paraIniciado++;
                        resultadoTemp = Funcoes.VerificarPara(comandoSemComentarios);
                        if (!string.IsNullOrEmpty(resultadoTemp))
                        {
                            temErro = true;
                            resultadoCompilacao = resultadoTemp;
                        }
                        continue;
                    }

                    // Verifica se o "SE" foi finalizado
                    if ((comandoSemComentarios.Length >= 5) &&
                        (comandoSemComentarios.Substring(0, 5) == "fimse"))
                    {
                        seFinalizado++;
                    }

                    // Verifica se o "PARA" foi finalizado
                    if ((comandoSemComentarios.Length >= 7) &&
                        (comandoSemComentarios.Substring(0, 7) == "fimpara"))
                    {
                        paraFinalizado++;
                    }
                }

                // Verifica os erros de estrutura
                if (seIniciado != seFinalizado)
                {
                    resultadoCompilacao +=
                        "Erro de estrutura. O número de \"se\" não é igual ao número de \"fimse\"" + Environment.NewLine;
                    temErro = true;
                }

                if (paraFinalizado != paraIniciado)
                {
                    resultadoCompilacao +=
                        "Erro de estrutura. O número de \"para\" não é igual ao número de \"fimpara\"" + Environment.NewLine;
                    temErro = true;
                }

                // Verifica se existem erros
                if (!temErro)
                {
                    resultadoCompilacao +=
                        "Verificação realizada com sucesso. Não foram encontrados erros.";
                }

                // Apresenta o resultado da compilação
                FrmResultadoCompilacao resultado =
                    new FrmResultadoCompilacao(resultadoCompilacao);
                resultado.ShowDialog();
            }
            catch (Exception ex)
            {
                // Apresenta os erros
                FrmResultadoCompilacao resultado = new FrmResultadoCompilacao(
                    string.Format(
                        "Ocorreu um ou mais erros ao compilar o programa.{0}Erros:{0}{1}",
                        Environment.NewLine,
                        ex.Message));
                resultado.ShowDialog();
            }
        }
        #endregion
        #endregion
    }
}