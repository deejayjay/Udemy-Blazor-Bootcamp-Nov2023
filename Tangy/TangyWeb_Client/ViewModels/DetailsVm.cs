using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tangy_Models;

namespace TangyWeb_Client.ViewModels
{
    public class DetailsVm
    {
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value greater than 0.")]
        public int Count { get; set; }
        
        [Required]
        public int SelectedProductPriceId { get; set; }
        [ForeignKey(nameof(SelectedProductPriceId))]
        public ProductPriceDto ProductPrice { get; set; }
    }
}
