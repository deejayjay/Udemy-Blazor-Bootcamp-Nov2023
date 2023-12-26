using System.ComponentModel.DataAnnotations;

namespace Tangy_Models
{
    public class ProductPriceDto
    {
        public int Id { get; set; }
        [Required]
        public string Size { get; set; }
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Price must be greater than 1.")]
        public double Price { get; set; }
        [Required]
        public int ProductId { get; set; }
    }
}
