using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using FilaRapida.Utils;

namespace FilaRapida.Repositories
{
    public class RelatorioRepository
    {
        public DataTable GetVendasPorDia(DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            var dataTable = new DataTable();
            string sql = @"
                SELECT 
                    DATE(p.criado_em) as Dia, 
                    COUNT(p.id) as Quantidade_Pedidos, 
                    SUM(p.total) as Total_Vendas, 
                    AVG(p.total) as Ticket_Medio 
                FROM pedidos p 
                WHERE 1=1 ";
            
            if (dataInicio.HasValue)
                sql += " AND DATE(p.criado_em) >= @dataInicio";
            if (dataFim.HasValue)
                sql += " AND DATE(p.criado_em) <= @dataFim";
            
            sql += @"
                GROUP BY DATE(p.criado_em) 
                ORDER BY Dia DESC";

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        if (dataInicio.HasValue)
                            cmd.Parameters.AddWithValue("@dataInicio", dataInicio.Value.Date);
                        if (dataFim.HasValue)
                            cmd.Parameters.AddWithValue("@dataFim", dataFim.Value.Date);
                        
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
                
                // Formata as colunas
                if (dataTable.Columns.Contains("Dia"))
                    dataTable.Columns["Dia"].ColumnName = "Data";
                if (dataTable.Columns.Contains("Quantidade_Pedidos"))
                    dataTable.Columns["Quantidade_Pedidos"].ColumnName = "Qtd Pedidos";
                if (dataTable.Columns.Contains("Total_Vendas"))
                    dataTable.Columns["Total_Vendas"].ColumnName = "Total Vendas (R$)";
                if (dataTable.Columns.Contains("Ticket_Medio"))
                    dataTable.Columns["Ticket_Medio"].ColumnName = "Ticket Médio (R$)";
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao gerar relatório de vendas: {ex.Message}");
            }
            return dataTable;
        }

        public DataTable GetTopProdutos(DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            var dataTable = new DataTable();
            string sql = @"
                SELECT 
                    pr.nome as Produto, 
                    SUM(pi.qtd) as Quantidade_Vendida, 
                    SUM(pi.qtd * pi.preco_unit) as Valor_Total, 
                    AVG(pi.preco_unit) as Preco_Medio 
                FROM pedido_itens pi 
                JOIN produtos pr ON pi.produto_id = pr.id 
                JOIN pedidos p ON pi.pedido_id = p.id 
                WHERE 1=1 ";
            
            if (dataInicio.HasValue)
                sql += " AND DATE(p.criado_em) >= @dataInicio";
            if (dataFim.HasValue)
                sql += " AND DATE(p.criado_em) <= @dataFim";
            
            sql += @"
                GROUP BY pr.id, pr.nome 
                ORDER BY Quantidade_Vendida DESC 
                LIMIT 10";

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        if (dataInicio.HasValue)
                            cmd.Parameters.AddWithValue("@dataInicio", dataInicio.Value.Date);
                        if (dataFim.HasValue)
                            cmd.Parameters.AddWithValue("@dataFim", dataFim.Value.Date);
                        
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
                
                // Formata as colunas
                if (dataTable.Columns.Contains("Quantidade_Vendida"))
                    dataTable.Columns["Quantidade_Vendida"].ColumnName = "Qtd Vendida";
                if (dataTable.Columns.Contains("Valor_Total"))
                    dataTable.Columns["Valor_Total"].ColumnName = "Valor Total (R$)";
                if (dataTable.Columns.Contains("Preco_Medio"))
                    dataTable.Columns["Preco_Medio"].ColumnName = "Preço Médio (R$)";
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao gerar relatório de produtos: {ex.Message}");
            }
            return dataTable;
        }

        public DataTable GetVendasPorAtendente(DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            var dataTable = new DataTable();
            string sql = @"
                SELECT 
                    u.nome as Atendente, 
                    COUNT(p.id) as Quantidade_Pedidos, 
                    SUM(p.total) as Total_Vendas, 
                    AVG(p.total) as Ticket_Medio_Atendente 
                FROM pedidos p 
                JOIN usuarios u ON p.usuario_id = u.id 
                WHERE u.perfil = 'ATENDENTE' 
                AND u.ativo = 1 ";
            
            if (dataInicio.HasValue)
                sql += " AND DATE(p.criado_em) >= @dataInicio";
            if (dataFim.HasValue)
                sql += " AND DATE(p.criado_em) <= @dataFim";
            
            sql += @"
                GROUP BY u.id, u.nome 
                ORDER BY Total_Vendas DESC";

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        if (dataInicio.HasValue)
                            cmd.Parameters.AddWithValue("@dataInicio", dataInicio.Value.Date);
                        if (dataFim.HasValue)
                            cmd.Parameters.AddWithValue("@dataFim", dataFim.Value.Date);
                        
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
                
                // Formata as colunas
                if (dataTable.Columns.Contains("Quantidade_Pedidos"))
                    dataTable.Columns["Quantidade_Pedidos"].ColumnName = "Qtd Pedidos";
                if (dataTable.Columns.Contains("Total_Vendas"))
                    dataTable.Columns["Total_Vendas"].ColumnName = "Total Vendas (R$)";
                if (dataTable.Columns.Contains("Ticket_Medio_Atendente"))
                    dataTable.Columns["Ticket_Medio_Atendente"].ColumnName = "Ticket Médio (R$)";
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao gerar relatório por atendente: {ex.Message}");
            }
            return dataTable;
        }

        public DataTable GetProdutosInativos()
        {
            var dataTable = new DataTable();
            string sql = @"
                SELECT 
                    nome as Produto, 
                    preco as Preco, 
                    DATE(criado_em) as Data_Cadastro, 
                    CASE 
                        WHEN ativo = 1 THEN 'Ativo' 
                        ELSE 'Inativo' 
                    END as Status 
                FROM produtos 
                WHERE ativo = 0 
                ORDER BY nome";

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
                
                // Formata as colunas
                if (dataTable.Columns.Contains("Data_Cadastro"))
                    dataTable.Columns["Data_Cadastro"].ColumnName = "Data Cadastro";
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao gerar relatório de produtos inativos: {ex.Message}");
            }
            return dataTable;
        }

        public string ExportarParaCSV(DataTable dataTable)
        {
            if (dataTable == null || dataTable.Rows.Count == 0)
                return "Nenhum dado para exportar";

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            // Cabeçalhos
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                sb.Append(dataTable.Columns[i].ColumnName);
                if (i < dataTable.Columns.Count - 1)
                    sb.Append(";");
            }
            sb.AppendLine();

            // Dados
            foreach (DataRow row in dataTable.Rows)
            {
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    string value = row[i].ToString();
                    // Trata valores com ponto e vírgula
                    if (value.Contains(";"))
                        value = $"\"{value}\"";
                    sb.Append(value);
                    if (i < dataTable.Columns.Count - 1)
                        sb.Append(";");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}