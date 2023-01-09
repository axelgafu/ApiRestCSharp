using ApiRest.Model;

namespace ApiRest.Repository
{
    public interface IRepositoryUser
    {
        public Task<User?> LoginAsync(LoginAPI api);
    }
}
