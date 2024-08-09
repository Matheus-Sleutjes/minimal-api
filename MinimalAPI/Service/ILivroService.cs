
using MinimalAPI.Entities;

public interface ILivroService
{
    void Add(Livro livro);
    Livro Find(int id);
    void AddBySql(Livro livro, string connectionString);
}