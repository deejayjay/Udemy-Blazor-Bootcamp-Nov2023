using System.ComponentModel.DataAnnotations;

namespace Tangy_Models
{
    public class CategoryDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
