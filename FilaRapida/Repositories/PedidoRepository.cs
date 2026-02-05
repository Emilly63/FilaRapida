using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using FilaRapida.Models;
using FilaRapida.Utils;

namespace FilaRapida.Repositories
{
    public class PedidoRepository
    {
        public int Save(Pedido pedido)
        {
            MySqlTransaction transaction = null;
            MySqlConnection conn = null;
            
            try
            {
                conn = DbConnection.GetConnection();
                transaction = conn.BeginTransaction();

                // 1. Insere o cabeçalho do pedido
                string sqlPedido = @"
                    INSERT INTO pedidos (usuario_id, criado_em, total) 
                    VALUES (@usuarioId, @criadoEm, @total);
                    SELECT LAST_INSERT_ID();";
                
                int pedidoId;
                using (MySqlCommand cmd = new MySqlCommand(sqlPedido, conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@usuarioId", pedido.UsuarioId);
                    cmd.Parameters.AddWithValue("@criadoEm", pedido.CriadoEm);
                    cmd.Parameters.AddWithValue("@total", pedido.Total);
                    pedidoId = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // 2. Insere todos os itens do pedido
                string sqlItem = @"
                    INSERT INTO pedido_itens (pedido_id, produto_id, qtd, preco_unit) 
                    VALUES (@pedidoId, @produtoId, @quantidade, @precoUnit);";
                
                foreach (var item in pedido.Itens)
                {
                    using (MySqlCommand cmd = new MySqlCommand(sqlItem, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@pedidoId", pedidoId);
                        cmd.Parameters.AddWithValue("@produtoId", item.ProdutoId);
                        cmd.Parameters.AddWithValue("@quantidade", item.Quantidade);
                        cmd.Parameters.AddWithValue("@precoUnit", item.PrecoUnitario);
                        cmd.ExecuteNonQuery();
                    }
                }

                // 3. Confirmar a transação
                transaction.Commit();
                return pedidoId;
            }
            catch (Exception ex)
            {
                // Rollback em caso de erro
                transaction?.Rollback();
                throw new Exception($"Erro ao salvar pedido: {ex.Message}");
            }
            finally
            {
                conn?.Close();
            }
        }

        public List<Pedido> GetAll(DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            var pedidos = new List<Pedido>();
            
            string sql = @"
                SELECT p.id, p.usuario_id, p.criado_em, p.total, u.nome as usuario_nome 
                FROM pedidos p 
                JOIN usuarios u ON p.usuario_id = u.id 
                WHERE 1=1 ";
            
            if (dataInicio.HasValue)
                sql += " AND DATE(p.criado_em) >= @dataInicio";
            if (dataFim.HasValue)
                sql += " AND DATE(p.criado_em) <= @dataFim";
            
            sql += " ORDER BY p.criado_em DESC";

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
                        
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var pedido = new Pedido
                                {
                                    Id = reader.GetInt32("id"),
                                    UsuarioId = reader.GetInt32("usuario_id"),
                                    CriadoEm = reader.GetDateTime("criado_em"),
                                    Total = reader.GetDecimal("total"),
                                    Usuario = new Usuario { Nome = reader.GetString("usuario_nome") }
                                };
                                
                                // Carrega itens do pedido
                                pedido.Itens = GetItensByPedidoId(pedido.Id);
                                pedidos.Add(pedido);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar pedidos: {ex.Message}");
            }
            return pedidos;
        }

        private List<PedidoItem> GetItensByPedidoId(int pedidoId)
        {
            var itens = new List<PedidoItem>();
            string sql = @"
                SELECT pi.id, pi.pedido_id, pi.produto_id, pi.qtd, pi.preco_unit, 
                       pr.nome as produto_nome 
                FROM pedido_itens pi 
                JOIN produtos pr ON pi.produto_id = pr.id 
                WHERE pi.pedido_id = @pedidoId";
            
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@pedidoId", pedidoId);
                    
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            itens.Add(new PedidoItem
                            {
                                Id = reader.GetInt32("id"),
                                PedidoId = reader.GetInt32("pedido_id"),
                                ProdutoId = reader.GetInt32("produto_id"),
                                Quantidade = reader.GetInt32("qtd"),
                                PrecoUnitario = reader.GetDecimal("preco_unit"),
                                Produto = new Produto { Nome = reader.GetString("produto_nome") }
                            });
                        }
                    }
                }
            }
            return itens;
        }

        public Pedido GetById(int id)
        {
            string sql = @"
                SELECT p.id, p.usuario_id, p.criado_em, p.total, u.nome as usuario_nome 
                FROM pedidos p 
                JOIN usuarios u ON p.usuario_id = u.id 
                WHERE p.id = @id";
            
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var pedido = new Pedido
                            {
                                Id = reader.GetInt32("id"),
                                UsuarioId = reader.GetInt32("usuario_id"),
                                CriadoEm = reader.GetDateTime("criado_em"),
                                Total = reader.GetDecimal("total"),
                                Usuario = new Usuario { Nome = reader.GetString("usuario_nome") }
                            };
                            
                            pedido.Itens = GetItensByPedidoId(pedido.Id);
                            return pedido;
                        }
                    }
                }
            }
            return null;
        }
    }
}