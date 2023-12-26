using System.ComponentModel.DataAnnotations.Schema;

namespace Tangy_DataAccess
{
    public class ProductPrice
    {
        public int Id { get; set; }
        public string Size { get; set; }
        public double Price { get; set; }
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
    }
}
