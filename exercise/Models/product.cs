namespace exercise.Models
{
    public class Product
    {
        public int pId { get; set; }
        public string? pName { get; set; }
        public int Price { get; set; }

        public int Quantity { get; set; }
        public string? Seller{ get; set; }
        public string? Img { get; set; }

        public DateTime creTime { get; set; }
    }
}
