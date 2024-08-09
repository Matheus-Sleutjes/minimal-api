using Microsoft.EntityFrameworkCore;
using MinimalAPI.Data;
using MinimalAPI.Entities;
using System.Data.SqlClient;

public class LivroService : ILivroService
{
    private LivrosContext _context;
    public LivroService(LivrosContext context)
    {
        _context = context;
    }

    public void AddBySql(Livro livro, string connectionString)
    {

    }

    private void ExecutePostSQL(string sqlScript, string connectionString, List<SqlParameter> parameters)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    using (SqlCommand command = new SqlCommand(sqlScript, connection, transaction))
                    {
                        command.Parameters.Add(parameters);
                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Erro ao executar o comando SQL: {ex.Message}");
                }
            }
        }
    }

    private void ExecuteGetSQL(string sqlScript, string connectionString, List<SqlParameter> parameters)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            try
            {
                using (SqlCommand command = new SqlCommand(sqlScript, connection))
                {
                    command.Parameters.Add(parameters);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int userId = reader.GetInt32(0); // Lê o valor da primeira coluna (UserId)
                            string name = reader.GetString(1); // Lê o valor da segunda coluna (Name)

                            Console.WriteLine($"ID: {userId}, Name: {name}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao executar o comando SQL: {ex.Message}");
            }
        }
    }

    public void Add(Livro livro)
    {
        _context.Livros.Add(livro);
        _context.SaveChanges();
    }

    public Livro Find(int id)
    {
        return _context.Livros.AsNoTracking().Where(t => t.Id == id).FirstOrDefault();
    }
}