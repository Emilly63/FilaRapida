using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using FilaRapida.Models;
using FilaRapida.Utils;

namespace FilaRapida.Repositories
{
    public class ProdutoRepository
    {
        public List<Produto> GetAll(bool apenasAtivos = true)
        {
            var produtos = new List<Produto>();
            
            string sql = apenasAtivos
                ? "SELECT id, nome, preco, ativo, criado_em FROM produtos WHERE ativo = 1 ORDER BY nome"
                : "SELECT id, nome, preco, ativo, criado_em FROM produtos ORDER BY nome";

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                produtos.Add(new Produto
                                {
                                    Id = reader.GetInt32("id"),
                                    Nome = reader.GetString("nome"),
                                    Preco = reader.GetDecimal("preco"),
                                    Ativo = reader.GetBoolean("ativo"),
                                    CriadoEm = reader.GetDateTime("criado_em")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar produtos: {ex.Message}");
            }

            return produtos;
        }

        public List<Produto> SearchByName(string nome, bool apenasAtivos = true)
        {
            var produtos = new List<Produto>();
            string sql = @"
                SELECT id, nome, preco, ativo, criado_em 
                FROM produtos 
                WHERE nome LIKE @nome ";
            
            if (apenasAtivos)
                sql += " AND ativo = 1";
            
            sql += " ORDER BY nome";

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@nome", $"%{nome}%");
                        
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                produtos.Add(new Produto
                                {
                                    Id = reader.GetInt32("id"),
                                    Nome = reader.GetString("nome"),
                                    Preco = reader.GetDecimal("preco"),
                                    Ativo = reader.GetBoolean("ativo"),
                                    CriadoEm = reader.GetDateTime("criado_em")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar produtos: {ex.Message}");
            }
            return produtos;
        }

        public Produto GetById(int id)
        {
            string sql = "SELECT id, nome, preco, ativo, criado_em FROM produtos WHERE id = @id";
            
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Produto
                            {
                                Id = reader.GetInt32("id"),
                                Nome = reader.GetString("nome"),
                                Preco = reader.GetDecimal("preco"),
                                Ativo = reader.GetBoolean("ativo"),
                                CriadoEm = reader.GetDateTime("criado_em")
                            };
                        }
                    }
                }
            }
            return null;
        }

        public int Create(Produto produto)
        {
            if (!produto.Validar())
                throw new ArgumentException("Dados do produto inválidos");

            string sql = @"
                INSERT INTO produtos (nome, preco, ativo, criado_em) 
                VALUES (@nome, @preco, @ativo, @criadoEm); 
                SELECT LAST_INSERT_ID();";

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@nome", produto.Nome);
                        cmd.Parameters.AddWithValue("@preco", produto.Preco);
                        cmd.Parameters.AddWithValue("@ativo", produto.Ativo);
                        cmd.Parameters.AddWithValue("@criadoEm", produto.CriadoEm);
                        
                        object result = cmd.ExecuteScalar();
                        return Convert.ToInt32(result);
                    }
                }
            }
            catch (MySqlException ex) when (ex.Number == 1062) // Duplicate entry
            {
                throw new Exception("Já existe um produto com este nome");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar produto: {ex.Message}");
            }
        }

        public bool Update(Produto produto)
        {
            if (!produto.Validar())
                throw new ArgumentException("Dados do produto inválidos");

            string sql = @"
                UPDATE produtos 
                SET nome = @nome, preco = @preco, ativo = @ativo 
                WHERE id = @id";

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", produto.Id);
                    cmd.Parameters.AddWithValue("@nome", produto.Nome);
                    cmd.Parameters.AddWithValue("@preco", produto.Preco);
                    cmd.Parameters.AddWithValue("@ativo", produto.Ativo);
                    
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public bool Inactivate(int id)
        {
            // Verifica se produto existe e está ativo
            var produto = GetById(id);
            if (produto == null || !produto.Ativo)
                return false;

            string sql = "UPDATE produtos SET ativo = 0 WHERE id = @id";
            
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public bool Reactivate(int id)
        {
            string sql = "UPDATE produtos SET ativo = 1 WHERE id = @id";
            
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
    }
}