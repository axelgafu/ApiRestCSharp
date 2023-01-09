namespace ApiRest.Model
{
    public class Product
    {
        public int Id { get; init; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public double Price { get; set; }

        public DateTime CreationDate { get; init; }

        public string? SKU { get; set; }
    }
}
