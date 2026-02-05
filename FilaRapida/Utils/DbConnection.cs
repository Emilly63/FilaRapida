using System;
using MySql.Data.MySqlClient;

namespace FilaRapida.Utils
{
    /// <summary>
    /// Classe responsável por gerenciar a conexão com o banco de dados
    /// Implementa o padrão Singleton para reutilização de conexão
    /// </summary>
    public static class DbConnection
    {
        // String de conexão para XAMPP (MySQL padrão)
        private static readonly string ConnectionString = 
            "Server=localhost;" +
            "Database=filarapida;" +
            "Uid=root;" +           // Usuário padrão do XAMPP
            "Pwd=;" +               // Senha (vazia se não configurou)
            "Port=3306;" +
            "SslMode=None;" +
            "CharSet=utf8mb4;" +
            "AllowPublicKeyRetrieval=true;";  // Importante para versões mais novas

        /// <summary>
        /// Cria e retorna uma nova conexão com o banco de dados
        /// </summary>
        public static MySqlConnection GetConnection()
        {
            try
            {
                var connection = new MySqlConnection(ConnectionString);
                connection.Open();
                return connection;
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Erro ao conectar ao banco de dados: {ex.Message}");
            }
        }

        /// <summary>
        /// Testa a conexão com o banco de dados
        /// </summary>
        public static bool TestConnection()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    return conn.State == System.Data.ConnectionState.Open;
                }
            }
            catch
            {
                return false;
            }
        }
        
    }
}