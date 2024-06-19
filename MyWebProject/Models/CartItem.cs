namespace MyWebProject.Models
{
    public class CartItem
    { 
        public int CartId { get; set; } 
        public AllProduct Product { get; set; }
        public int Quantity { get; set; }
    }
}
