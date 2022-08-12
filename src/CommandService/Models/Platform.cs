using System.ComponentModel.DataAnnotations;

namespace CommandService.Models
{   
    public class Platform
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public int ExternalId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Cost { get; set; }
        public ICollection<Command> Commands { get; set; } = new List<Command>();

    }
}
