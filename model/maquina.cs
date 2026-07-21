using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace mapa_asp.net.Models
{
    public class Maquina
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id  {get; set;}

        public string Patrimonio {get; set;}

        public string Setor {get; set;}

        public string Descricao {get; set;}

        public int X {get; set;}

        public int Y {get; set;}

        public DateTime CreatedAt {get; set;} = DateTime.Now;
        
        public DateTime UpdatedAt {get; set;} = DateTime.Now;
    }
}