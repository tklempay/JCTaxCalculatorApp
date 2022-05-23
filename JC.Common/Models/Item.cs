namespace JC.Common.Models
{
    public class Item
    {
        public string ItemName { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice => Price * Quantity;
    }
}