using ApiRest.Model;

namespace ApiRest.Repository
{
    public class RepoInMemoryProducts : IRepositoryProducts
    {
        private readonly List<Product> products = new()
        {
            new Product{ Id = 1, Name = "Hammer", Description = "Accurate hammer.", Price = 12.99, CreationDate = DateTime.Now, SKU = "THA001"},
            new Product{ Id = 2, Name = "Nails", Description = "100 units.", Price = 10, CreationDate = DateTime.Now, SKU = "SNA001"},
            new Product{ Id = 3, Name = "Screwdriver", Description = "Amazing tool.", Price = 9.99, CreationDate = DateTime.Now, SKU = "TSA001"},
            new Product{ Id = 4, Name = "Spotlight", Description = "Luminous item.", Price = 3, CreationDate = DateTime.Now, SKU = "SSA001"},
        };

        public async Task<IEnumerable<Product>> ReadAsync() => await Task.FromResult(products);

        public async Task<Product?> ReadAsync(string sku) => await Task.FromResult(products.Where( p => p.SKU==sku).SingleOrDefault());

        public async Task<IEnumerable<Product>> ReadAsync(int page, int size) => await Task.FromResult(products);

        public void Create(Product product)
        {
            if( product is null )
            {
                throw new ArgumentNullException(
                    paramName: nameof(product), 
                    message: "Ilegal argument, null is not allowed.");
            }

            products.Add(product);
        }

        public void Update(Product product)
        {
            int index = products.FindIndex(p => p.SKU== product.SKU);
            if (index == -1) throw new InvalidDataException();
            products[index] = product;
        }

        public void Delete(string sku)
        {
            int index = products.FindIndex(p => p.SKU == sku);
            if (index == -1) throw new InvalidDataException();
            products.RemoveAt(index);
        }
    }
}
