using System.ComponentModel.DataAnnotations.Schema;

namespace Tangy_DataAccess
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsShopFavorite { get; set; }
        public bool IsCustomerFavorite { get; set; }
        public string Color { get; set; }
        public string ImageUrl { get; set; }
        
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }

        public ICollection<ProductPrice> ProductPrices { get; set; }
    }
}
