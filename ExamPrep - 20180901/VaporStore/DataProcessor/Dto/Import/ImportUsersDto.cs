namespace VaporStore.DataProcessor.Dto.Import
{
    using System.ComponentModel.DataAnnotations;

    public class ImportUsersDto
    {
        [Required]
        [RegularExpression("^[A-Z][a-z]+ [A-Z][a-z]+$")]
        public string FullName { get; set; }

        [Required]
        [MinLength(3), MaxLength(20)]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Range(3, 103)]
        public int Age { get; set; }

        [MinLength(1)]
        public CardDto[] Cards { get; set; }
    }

    public class CardDto
    {
        [Required]
        [RegularExpression("^[0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4}$")] //TODO: one or more space ^[0-9]{4}\\s+[0-9]{4}\\s+[0-9]{4}\\s+[0-9]{4}$
        public string Number { get; set; }

        [Required]
        [RegularExpression("^[0-9]{3}")]
        public string CVC { get; set; }

        [Required]
        public string Type { get; set; }
    }
}