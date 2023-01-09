using ApiRest.Model;

namespace ApiRest.Repository
{
    public interface IRepositoryProducts
    {
        public Task<IEnumerable<Product>> ReadAsync();

        public Task<Product?> ReadAsync(string sku);

        public Task<IEnumerable<Product>> ReadAsync(int page, int size);

        public void Create(Product product);

        public void Update(Product product);

        public void Delete(string sku);
    }
}
