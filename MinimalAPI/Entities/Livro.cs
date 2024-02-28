using System.ComponentModel.DataAnnotations;

namespace MinimalAPI.Entities
{
    public class Livro
    {
        [Key]
        public int Id { get; set; }
        public string Descricao { get; set; } = String.Empty;
    }
}
