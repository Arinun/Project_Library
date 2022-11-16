using System.ComponentModel.DataAnnotations;

namespace Project_Library.Models
{
    public class Library
    {
        public int Id { get; set; }
        [Required]
        public string IdPerson { get; set; }
        [Required]
        public string PersonName { get; set; }
        public int BookID { get; set; }
        public string BookName { get; set; }
        public DateTime DateBorrow { get; set; }
        public DateTime? DateReturn { get; set; }
    }
}
