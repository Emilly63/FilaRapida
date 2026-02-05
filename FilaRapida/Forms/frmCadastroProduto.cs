using System;
using System.Windows.Forms;
using FilaRapida.Models;
using FilaRapida.Repositories;

namespace FilaRapida.Forms
{
    public partial class frmCadastroProduto : Form
    {
        private readonly Produto produto;
        private readonly ProdutoRepository produtoRepo;
        private bool isEditMode;
        
        public frmCadastroProduto(Produto produtoExistente)
        {
            InitializeComponent();
            produtoRepo = new ProdutoRepository();
            
            if (produtoExistente == null)
            {
                produto = new Produto();
                isEditMode = false;
                Text = "Novo Produto";
            }
            else
            {
                produto = produtoExistente;
                isEditMode = true;
                Text = "Editar Produto";
                CarregarDados();
            }
        }
        
        private void CarregarDados()
        {
            txtNome.Text = produto.Nome;
            numPreco.Value = produto.Preco;
            chkAtivo.Checked = produto.Ativo;
            
            if (!produto.Ativo)
            {
                chkAtivo.Enabled = false;
                lblStatusInfo.Text = "Produto inativo - não pode ser reativado aqui";
                lblStatusInfo.Visible = true;
            }
        }
        
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Informe o nome do produto", "Validação", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNome.Focus();
                return;
            }
            
            if (txtNome.Text.Length < 3)
            {
                MessageBox.Show("Nome deve ter pelo menos 3 caracteres", "Validação", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNome.Focus();
                return;
            }
            
            if (numPreco.Value <= 0)
            {
                MessageBox.Show("Preço deve ser maior que zero", "Validação", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numPreco.Focus();
                return;
            }
            
            if (numPreco.Value > 9999.99m)
            {
                MessageBox.Show("Preço máximo é R$ 9.999,99", "Validação", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numPreco.Focus();
                return;
            }
            
            try
            {
                produto.Nome = txtNome.Text.Trim();
                produto.Preco = numPreco.Value;
                produto.Ativo = chkAtivo.Checked;
                
                bool sucesso;
                
                if (isEditMode)
                {
                    sucesso = produtoRepo.Update(produto);
                    if (sucesso)
                        MessageBox.Show("Produto atualizado com sucesso!", "Sucesso", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    int novoId = produtoRepo.Create(produto);
                    if (novoId > 0)
                    {
                        produto.Id = novoId;
                        MessageBox.Show("Produto cadastrado com sucesso!", "Sucesso", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        sucesso = true;
                    }
                    else
                    {
                        sucesso = false;
                    }
                }
                
                if (sucesso)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar produto: {ex.Message}", 
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
        
        private void txtNome_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (txtNome.Text.Length >= 80 && e.KeyChar != '\b')
            {
                e.Handled = true;
                MessageBox.Show("Nome deve ter no máximo 80 caracteres", "Validação", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void numPreco_Enter(object sender, EventArgs e)
        {
            numPreco.Select(0, numPreco.Text.Length);
        }
    }
}