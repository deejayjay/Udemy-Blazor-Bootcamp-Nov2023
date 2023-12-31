using System.ComponentModel.DataAnnotations;

namespace Tangy_Models
{
    public class OrderDetailDto
    {
        public int Id { get; set; }

        [Required]
        public int OrderHeaderId { get; set; }

        [Required]
        public int ProductId { get; set; }
        
        public ProductDto Product { get; set; }

        [Required]
        public int Count { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string Size { get; set; }

        [Required]
        public string ProductName { get; set; }
    }
}
