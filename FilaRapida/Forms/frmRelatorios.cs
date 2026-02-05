using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using FilaRapida.Models;
using FilaRapida.Repositories;

namespace FilaRapida.Forms
{
    public partial class frmRelatorios : Form
    {
        private readonly RelatorioRepository relatorioRepo;
        private readonly Usuario usuarioLogado;
        
        public frmRelatorios(Usuario usuario)
        {
            InitializeComponent();
            relatorioRepo = new RelatorioRepository();
            usuarioLogado = usuario;
            
            ConfigurarInterface();
            CarregarRelatorioPadrao();
        }
        
        private void ConfigurarInterface()
        {
            cboRelatorio.Items.AddRange(new string[] {
                "Vendas por Dia",
                "Top Produtos",
                "Vendas por Atendente",
                "Produtos Inativos"
            });
            cboRelatorio.SelectedIndex = 0;
            
            dgvRelatorio.AutoGenerateColumns = true;
            dgvRelatorio.ReadOnly = true;
            dgvRelatorio.AllowUserToAddRows = false;
            dgvRelatorio.AllowUserToDeleteRows = false;
            
            dtpDataInicio.Value = DateTime.Now.AddDays(-30);
            dtpDataFim.Value = DateTime.Now;
            
            lblTotal.Text = "Total de registros: 0";
        }
        
        private void CarregarRelatorioPadrao()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                
                DataTable data = null;
                
                switch (cboRelatorio.SelectedIndex)
                {
                    case 0:
                        data = relatorioRepo.GetVendasPorDia(dtpDataInicio.Value, dtpDataFim.Value);
                        break;
                    case 1:
                        data = relatorioRepo.GetTopProdutos(dtpDataInicio.Value, dtpDataFim.Value);
                        break;
                    case 2:
                        data = relatorioRepo.GetVendasPorAtendente(dtpDataInicio.Value, dtpDataFim.Value);
                        break;
                    case 3:
                        data = relatorioRepo.GetProdutosInativos();
                        pnlFiltroData.Enabled = false;
                        break;
                }
                
                if (data != null)
                {
                    dgvRelatorio.DataSource = data;
                    lblTotal.Text = $"Total de registros: {data.Rows.Count}";
                    FormatDataGridView();
                    CalcularTotais(data);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar relatório: {ex.Message}", 
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        
        private void FormatDataGridView()
        {
            foreach (DataGridViewColumn column in dgvRelatorio.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                
                if (column.Name.Contains("(R$)"))
                {
                    column.DefaultCellStyle.Format = "C2";
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                
                if (column.Name.Contains("Qtd") || column.Name.Contains("Quantidade"))
                {
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    column.DefaultCellStyle.Format = "N0";
                }
                
                if (column.Name.Contains("Data"))
                {
                    column.DefaultCellStyle.Format = "dd/MM/yyyy";
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
        }
        
        private void CalcularTotais(DataTable data)
        {
            if (cboRelatorio.SelectedIndex == 0)
            {
                decimal totalGeral = 0;
                int pedidosGeral = 0;
                
                foreach (DataRow row in data.Rows)
                {
                    if (row["Total Vendas (R$)"] != DBNull.Value)
                        totalGeral += Convert.ToDecimal(row["Total Vendas (R$)"]);
                    
                    if (row["Qtd Pedidos"] != DBNull.Value)
                        pedidosGeral += Convert.ToInt32(row["Qtd Pedidos"]);
                }
                
                lblResumo.Text = $"Resumo: {pedidosGeral} pedidos | Total Geral: {totalGeral:C2}";
            }
            else if (cboRelatorio.SelectedIndex == 1)
            {
                decimal valorTotal = 0;
                int quantidadeTotal = 0;
                
                foreach (DataRow row in data.Rows)
                {
                    if (row["Valor Total (R$)"] != DBNull.Value)
                        valorTotal += Convert.ToDecimal(row["Valor Total (R$)"]);
                    
                    if (row["Qtd Vendida"] != DBNull.Value)
                        quantidadeTotal += Convert.ToInt32(row["Qtd Vendida"]);
                }
                
                lblResumo.Text = $"Resumo: {quantidadeTotal} itens vendidos | Valor Total: {valorTotal:C2}";
            }
            else
            {
                lblResumo.Text = string.Empty;
            }
        }
        
        private void btnGerar_Click(object sender, EventArgs e)
        {
            CarregarRelatorioPadrao();
        }
        
        private void btnExportar_Click(object sender, EventArgs e)
        {
            if (dgvRelatorio.Rows.Count == 0)
            {
                MessageBox.Show("Não há dados para exportar", "Aviso", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Arquivo CSV (*.csv)|*.csv|Arquivo Texto (*.txt)|*.txt";
                saveDialog.FileName = $"Relatorio_{cboRelatorio.Text}_{DateTime.Now:yyyyMMdd}.csv";
                saveDialog.DefaultExt = "csv";
                
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var dataTable = (DataTable)dgvRelatorio.DataSource;
                        string csvContent = relatorioRepo.ExportarParaCSV(dataTable);
                        
                        File.WriteAllText(saveDialog.FileName, csvContent, System.Text.Encoding.UTF8);
                        
                        MessageBox.Show($"Relatório exportado com sucesso!\n{saveDialog.FileName}", 
                            "Exportação Concluída", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro ao exportar relatório: {ex.Message}", 
                            "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Funcionalidade de impressão implementada na próxima versão", 
                "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void cboRelatorio_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlFiltroData.Enabled = cboRelatorio.SelectedIndex != 3;
            lblResumo.Text = string.Empty;
            CarregarRelatorioPadrao();
        }
        
        private void dtpDataInicio_ValueChanged(object sender, EventArgs e)
        {
            if (dtpDataInicio.Value > dtpDataFim.Value)
            {
                dtpDataFim.Value = dtpDataInicio.Value;
            }
        }
        
        private void dtpDataFim_ValueChanged(object sender, EventArgs e)
        {
            if (dtpDataFim.Value < dtpDataInicio.Value)
            {
                dtpDataInicio.Value = dtpDataFim.Value;
            }
        }
    }
}