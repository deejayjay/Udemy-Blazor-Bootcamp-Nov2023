using System.ComponentModel.DataAnnotations;

namespace Tangy_DataAccess
{
    public class OrderHeader
    {
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; }
        // TODO: Add navigation property for User

        [Required]
        public double OrderTotal { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public DateTime ShippingDate { get; set; }

        [Required]
        public string Status { get; set; }

        // Stripe Payment
        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string StreetAddress { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z]\d[A-Za-z] \d[A-Za-z]\d$")]
        public string PostalCode { get; set; }

        public string? Tracking { get; set; }

        public string? Carrier { get; set; }
    }
}
