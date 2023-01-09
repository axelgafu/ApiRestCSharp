using ApiRest.DTO;
using ApiRest.Model;

namespace ApiRest
{
    public static class Util
    {
        public static DtoProduct? ToDTO( this Product p )
        {
            if (p == null) { return null; }

            return new DtoProduct
            {
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                SKU = p.SKU,
            };
        }

        public static DTOUser? toDTO( this User user )
        {
            if (user == null) { return null; }

            return new DTOUser
            {
                Token = user.Token,
                Name = user.Name,
            };
        }
    }
}
