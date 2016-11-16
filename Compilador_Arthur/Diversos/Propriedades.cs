namespace Compilador_Arthur.Diversos
{
    using System.Collections.Generic;

    /// <summary>
    /// Classe estática com propriedades diversas do sistema
    /// </summary>
    public static class Propriedades
    {
        #region Campos
        /// <summary>
        /// Lista contendo os caracteres proibidos da linguagem
        /// </summary>
        private static List<string> caracteresIncorretos;

        /// <summary>
        /// Lista contendo os caracteres correspondentes aos operadores relacionais
        /// </summary>
        private static List<string> operadoresRelacionais;

        /// <summary>
        /// Lista contendo os caracteres correspondentes aos operadores aritméticos
        /// </summary>
        private static List<string> operadoresAritmeticos;

        /// <summary>
        /// Lista contendo os caracteres correspondentes aos operadores de atribuição
        /// </summary>
        private static List<string> operadoresAtribuicao;

        /// <summary>
        /// Lista contendo as palavras reservadas
        /// </summary>
        private static List<string> palavrasReservadas;

        /// <summary>
        /// Lista contendo as variáveis
        /// </summary>
        private static List<string> variaveis;
        #endregion

        #region Propriedades
        /// <summary>
        /// Obtém a lista dos caracteres proibidos
        /// </summary>
        public static List<string> CaracteresIncorretos
        {
            get
            {
                if (caracteresIncorretos == null)
                {
                    caracteresIncorretos = new List<string>();
                }

                return caracteresIncorretos;
            }
        }

        /// <summary>
        /// Obtém a lista dos operadores relacionais
        /// </summary>
        public static List<string> OperadoresRelacionais
        {
            get
            {
                if (operadoresRelacionais == null)
                {
                    operadoresRelacionais = new List<string>();
                }

                return operadoresRelacionais;
            }
        }

        /// <summary>
        /// Obtém a lista dos operadores aritméticos
        /// </summary>
        public static List<string> OperadoresAritmeticos
        {
            get
            {
                if (operadoresAritmeticos == null)
                {
                    operadoresAritmeticos = new List<string>();
                }

                return operadoresAritmeticos;
            }
        }

        /// <summary>
        /// Obtém a lista dos operadores de atribuição
        /// </summary>
        public static List<string> OperadoresAtribuicao
        {
            get
            {
                if (operadoresAtribuicao == null)
                {
                    operadoresAtribuicao = new List<string>();
                }

                return operadoresAtribuicao;
            }
        }

        /// <summary>
        /// Obtém a lista das palavras reservadas
        /// </summary>
        public static List<string> PalavrasReservadas
        {
            get
            {
                if (palavrasReservadas == null)
                {
                    palavrasReservadas = new List<string>();
                }

                return palavrasReservadas;
            }
        }

        /// <summary>
        /// Obtém a lista das variáveis
        /// </summary>
        public static List<string> Variaveis

        {
            get
            {
                if (variaveis == null)
                {
                    variaveis = new List<string>();
                }

                return variaveis;
            }
        }
        #endregion
    }
}