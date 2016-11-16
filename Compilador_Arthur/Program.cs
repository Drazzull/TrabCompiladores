namespace Compilador_Arthur
{
    using Diversos;
    using Forms;
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// Classe de funcionamento do programa
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Início do programa
        /// </summary>
        [STAThread]
        private static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FrmPrincipal());
            }
            catch (Exception ex)
            {
                Funcoes.ApresentarMensagemErro("Ocorreu um erro ao executar a aplicação.", ex);
            }
        }
    }
}