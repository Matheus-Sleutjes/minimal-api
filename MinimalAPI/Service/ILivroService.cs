using MinimalAPI.Domain;

public interface ILivroService
{
    void Add(Livro livro);
    Livro Find(int id);
    void Delete(Livro livro);
}