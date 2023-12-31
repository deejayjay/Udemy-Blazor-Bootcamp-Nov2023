namespace Tangy_Models
{
    public class OrderDto
    {
        public OrderHeaderDto OrderHeader { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; }
    }
}
