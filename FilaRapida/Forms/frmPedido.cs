using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using FilaRapida.Models;
using FilaRapida.Repositories;
using FilaRapida.Services;

namespace FilaRapida.Forms
{
    public partial class frmPedido : Form
    {
        private readonly Pedido pedido;
        private readonly ProdutoRepository produtoRepo;
        private readonly PedidoService pedidoService;
        private readonly Usuario usuarioLogado;
        private List<Produto> produtosDisponiveis;
        
        public frmPedido(Usuario usuario)
        {
            InitializeComponent();
            usuarioLogado = usuario;
            pedido = new Pedido { UsuarioId = usuario.Id };
            produtoRepo = new ProdutoRepository();
            pedidoService = new PedidoService();
            
            ConfigurarInterface();
            CarregarProdutos();
        }
        
        private void ConfigurarInterface()
        {
            lblAtendente.Text = $"Atendente: {usuarioLogado.Nome}";
            lblData.Text = $"Data: {DateTime.Now:dd/MM/yyyy HH:mm}";
            
            cboProduto.DisplayMember = "Nome";
            cboProduto.ValueMember = "Id";
            
            dgvItens.AutoGenerateColumns = false;
        }
        
        private void CarregarProdutos()
        {
            try
            {
                produtosDisponiveis = produtoRepo.GetAll(true);
                
                var produtosComVazio = new List<Produto> { new Produto { Id = 0, Nome = "-- Selecione um produto --" } };
                produtosComVazio.AddRange(produtosDisponiveis);
                
                cboProduto.DataSource = produtosComVazio;
                cboProduto.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar produtos: {ex.Message}", 
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            if (cboProduto.SelectedIndex <= 0)
            {
                MessageBox.Show("Selecione um produto válido", "Aviso", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if (numQuantidade.Value <= 0)
            {
                MessageBox.Show("Quantidade deve ser maior que zero", "Aviso", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numQuantidade.Focus();
                return;
            }
            
            var produtoSelecionado = cboProduto.SelectedItem as Produto;
            if (produtoSelecionado == null || produtoSelecionado.Id == 0)
                return;
            
            var novoItem = new PedidoItem
            {
                ProdutoId = produtoSelecionado.Id,
                Produto = produtoSelecionado,
                Quantidade = (int)numQuantidade.Value,
                PrecoUnitario = produtoSelecionado.Preco
            };
            
            novoItem.CalcularSubtotal();
            
            try
            {
                pedido.AdicionarItem(novoItem);
                AtualizarListaItens();
                AtualizarTotal();
                
                cboProduto.SelectedIndex = 0;
                numQuantidade.Value = 1;
                cboProduto.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao adicionar item: {ex.Message}", 
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnRemover_Click(object sender, EventArgs e)
        {
            if (dgvItens.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um item para remover", "Aviso", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            var item = dgvItens.SelectedRows[0].DataBoundItem as PedidoItem;
            if (item == null) return;
            
            var confirm = MessageBox.Show($"Remover '{item.Produto.Nome}' do pedido?", 
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (confirm == DialogResult.Yes)
            {
                pedido.RemoverItem(item.ProdutoId);
                AtualizarListaItens();
                AtualizarTotal();
            }
        }
        
        private void AtualizarListaItens()
        {
            dgvItens.DataSource = null;
            dgvItens.DataSource = pedido.Itens.ToList();
            
            lblTotalItens.Text = $"Itens: {pedido.Itens.Count}";
        }
        
        private void AtualizarTotal()
        {
            lblTotalPedido.Text = $"Total do Pedido: {pedido.Total:C2}";
        }
        
        private void btnFinalizar_Click(object sender, EventArgs e)
        {
            if (pedido.Itens.Count == 0)
            {
                MessageBox.Show("Adicione pelo menos um item ao pedido", "Aviso", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            var confirm = MessageBox.Show(
                $"Confirmar pedido?\n\n" +
                $"Itens: {pedido.Itens.Count}\n" +
                $"Total: {pedido.Total:C2}\n\n" +
                $"Deseja finalizar a venda?",
                "Confirmar Pedido", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);
            
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    
                    var resultado = pedidoService.ProcessarPedido(pedido);
                    
                    if (resultado.Sucesso)
                    {
                        string comprovante = $"COMPROVANTE DE PEDIDO\n" +
                            $"=========================\n" +
                            $"Número: #{resultado.NumeroPedido}\n" +
                            $"Data: {resultado.DataHora:dd/MM/yyyy HH:mm}\n" +
                            $"Atendente: {usuarioLogado.Nome}\n" +
                            $"Itens: {pedido.Itens.Count}\n" +
                            $"Total: {resultado.ValorTotal:C2}\n" +
                            $"=========================\n" +
                            "Pedido registrado com sucesso!";
                        
                        MessageBox.Show(comprovante, "Pedido Finalizado", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        pedido.Itens.Clear();
                        AtualizarListaItens();
                        AtualizarTotal();
                        cboProduto.SelectedIndex = 0;
                        numQuantidade.Value = 1;
                    }
                    else
                    {
                        MessageBox.Show(resultado.Mensagem, "Erro ao Finalizar", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao processar pedido: {ex.Message}", 
                        "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }
        
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (pedido.Itens.Count > 0)
            {
                var confirm = MessageBox.Show("Cancelar pedido? Todos os itens serão perdidos.", 
                    "Confirmar Cancelamento", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                
                if (confirm == DialogResult.No)
                    return;
            }
            
            this.Close();
        }
        
        private void cboProduto_SelectedIndexChanged(object sender, EventArgs e)
        {
            var produto = cboProduto.SelectedItem as Produto;
            if (produto != null && produto.Id > 0)
            {
                lblPrecoUnitario.Text = $"Preço: {produto.Preco:C2}";
            }
            else
            {
                lblPrecoUnitario.Text = "Preço: --";
            }
        }
        
        private void dgvItens_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvItens.Columns[e.ColumnIndex].Name == "colSubtotal" && e.Value != null)
            {
                if (decimal.TryParse(e.Value.ToString(), out decimal valor))
                {
                    e.Value = valor.ToString("C2");
                    e.FormattingApplied = true;
                }
            }
        }
    }
}