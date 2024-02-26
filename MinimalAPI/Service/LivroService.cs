using Microsoft.EntityFrameworkCore;
using MinimalAPI.Data;
using MinimalAPI.Domain;

public class LivroService : ILivroService
{
    private LivrosContext _context;
    public LivroService(LivrosContext context)
    {
        _context = context;
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

    public void Delete(Livro livro)
    {
        _context.Remove(livro);
        _context.SaveChanges();
    }
}