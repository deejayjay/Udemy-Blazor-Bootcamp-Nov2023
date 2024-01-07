namespace Tangy_Models
{
    public class StripePaymentDto
    {
        public OrderDto Order { get; set; }
        public string SuccessUrl => "OrderConfirmation";
        public string CancelUrl => "Summary";
    }
}
