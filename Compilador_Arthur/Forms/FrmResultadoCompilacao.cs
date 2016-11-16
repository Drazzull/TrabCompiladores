namespace Compilador_Arthur.Forms
{
    using System.Windows.Forms;

    /// <summary>
    /// Classe do formulário de resultado da compilação
    /// </summary>
    public partial class FrmResultadoCompilacao : Form
    {
        /// <summary>
        /// Obtém uma instância da classe <see cref="FrmResultadoCompilacao"/> 
        /// </summary>
        public FrmResultadoCompilacao(string resultado)
        {
            // Inicializa os componentes do form
            this.InitializeComponent();

            // Define o resultado
            this.txtResultado.Text = resultado;
        }
    }
}
