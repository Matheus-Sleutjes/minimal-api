using System.ComponentModel.DataAnnotations;

namespace MinimalAPI.Entities
{
    public class Livro
    {
        public Livro(int id, string descricao)
        {
            Id = id;
            Descricao = descricao;
        }

        [Key]
        public int Id { get; set; }
        public string Descricao { get; set; } = String.Empty;
    }
}
