using System;
using MySql.Data.MySqlClient;
using FilaRapida.Models;
using FilaRapida.Utils;

namespace FilaRapida.Repositories
{
    public class UsuarioRepository
    {
        public Usuario Authenticate(string login, string senha)
        {
            string senhaHash = SecurityHelper.GenerateSHA256Hash(senha);

            string sql = @"
                SELECT id, nome, login, perfil, ativo 
                FROM usuarios 
                WHERE login = @login AND senha_hash = @senhaHash AND ativo = 1";

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@login", login);
                        cmd.Parameters.AddWithValue("@senhaHash", senhaHash);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Usuario
                                {
                                    Id = reader.GetInt32("id"),
                                    Nome = reader.GetString("nome"),
                                    Login = reader.GetString("login"),
                                    Perfil = reader.GetString("perfil"),
                                    Ativo = reader.GetBoolean("ativo")
                                };
                            }
                        }
                    }
                }
                return null; // Usuário não encontrado
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao autenticar usuário: {ex.Message}");
            }
        }

        public Usuario GetById(int id)
        {
            string sql = "SELECT id, nome, login, perfil, ativo FROM usuarios WHERE id = @id";
            
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Usuario
                            {
                                Id = reader.GetInt32("id"),
                                Nome = reader.GetString("nome"),
                                Login = reader.GetString("login"),
                                Perfil = reader.GetString("perfil"),
                                Ativo = reader.GetBoolean("ativo")
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}
