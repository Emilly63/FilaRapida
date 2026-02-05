namespace FilaRapida.Forms
{
    partial class frmProdutos
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // frmProdutos
            // 
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "frmProdutos";
            this.Text = "Gerenciamento de Produtos";
            this.ResumeLayout(false);
        }
    }
}
