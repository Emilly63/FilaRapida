using System;
using System.Windows.Forms;
using FilaRapida.Models;
using FilaRapida.Repositories;

namespace FilaRapida.Forms
{
    public partial class frmProdutos : Form
    {
        private ProdutoRepository produtoRepo;
        private bool apenasAtivos = true;
        private Usuario usuarioLogado;

        public frmProdutos(Usuario usuario)
        {
            InitializeComponent();
            usuarioLogado = usuario;
            produtoRepo = new ProdutoRepository();
            
            // Configura eventos manualmente (se Designer não configurou)
            ConfigurarEventosManualmente();
            
            CarregarProdutos();
        }

        private void ConfigurarEventosManualmente()
        {
            // Procura os controles e configura eventos
            foreach (Control control in this.Controls)
            {
                if (control is Button btn)
                {
                    switch (btn.Name)
                    {
                        case "btnNovo":
                            btn.Click += btnNovo_Click;
                            break;
                        case "btnEditar":
                            btn.Click += btnEditar_Click;
                            break;
                        case "btnInativar":
                            btn.Click += btnInativar_Click;
                            break;
                        case "btnReativar":
                            btn.Click += btnReativar_Click;
                            break;
                        case "btnAtualizar":
                            btn.Click += btnAtualizar_Click;
                            break;
                        case "btnFechar":
                            btn.Click += btnFechar_Click;
                            break;
                    }
                }
            }
        }

        private void CarregarProdutos()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                // Tenta carregar produtos
                var produtos = produtoRepo.GetAll(apenasAtivos);
                
                // Se dgvProdutos existir, usa ele
                var dgv = this.Controls.Find("dgvProdutos", true).FirstOrDefault() as DataGridView;
                if (dgv != null)
                {
                    dgv.DataSource = produtos;
                }

                // Atualiza labels
                var lblTotal = this.Controls.Find("lblTotal", true).FirstOrDefault() as Label;
                var lblFiltro = this.Controls.Find("lblFiltro", true).FirstOrDefault() as Label;
                
                if (lblTotal != null) lblTotal.Text = $"Total: {produtos.Count} produto(s)";
                if (lblFiltro != null) lblFiltro.Text = apenasAtivos ? "Mostrando apenas ativos" : "Mostrando todos";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar produtos: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        // Métodos de evento SIMPLIFICADOS
        private void btnNovo_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Abrindo cadastro de novo produto...", "Novo Produto");
            // Aqui você colocaria: new frmCadastroProduto(null).ShowDialog();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Editando produto selecionado...", "Editar Produto");
        }

        private void btnInativar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Inativando produto...", "Inativar");
        }

        private void btnReativar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Reativando produto...", "Reativar");
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            CarregarProdutos();
            MessageBox.Show("Lista atualizada!", "Atualizar");
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkMostrarTodos_CheckedChanged(object sender, EventArgs e)
        {
            var chk = sender as CheckBox;
            if (chk != null)
            {
                apenasAtivos = !chk.Checked;
                CarregarProdutos();
            }
        }
    }
}
