
using MinimalAPI.Entities;

public interface ILivroService
{
    void AddByEf(Livro livro);
    Livro GetByEf(int id);
    void AddBySql(Livro livro, string connectionString);
    Livro GetBySql(int id, string connectionString);
}