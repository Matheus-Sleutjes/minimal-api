using Microsoft.EntityFrameworkCore;
using MinimalAPI.Data;
using MinimalAPI.Entities;
using Npgsql;
using System.Data;

public class LivroService : ILivroService
{
    private LivrosContext _context;
    public LivroService(LivrosContext context)
    {
        _context = context;
    }

    public void AddBySql(Livro livro, string connectionString)
    {
        string sqlScript = "INSERT INTO \"Livros\" (\"Descricao\") VALUES (@Descricao)";

        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    using (NpgsqlCommand command = new NpgsqlCommand(sqlScript, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@Descricao", livro.Descricao);
                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Erro ao executar o comando SQL: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }

    public Livro GetBySql(int id, string connectionString)
    {
        var sqlScript = "SELECT * FROM \"Livros\" l WHERE l.\"Id\" = @Id LIMIT 100";

        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            try
            {
                var lista = new List<Livro>();
                using (NpgsqlCommand command = new NpgsqlCommand(sqlScript, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int livroId = reader.GetInt32(0);
                            string descricao = reader.GetString(1);

                            var entity = new Livro(livroId, descricao);
                            lista.Add(entity);
                        }
                    }
                }
                return lista.First();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao executar o comando SQL: {ex.Message}");
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }
    }

    public void AddByEf(Livro livro)
    {
        _context.Livros.Add(livro);
        _context.SaveChanges();
    }

    public Livro GetByEf(int id)
    {
        return _context.Livros.AsNoTracking().Where(t => t.Id == id).FirstOrDefault();
    }
}