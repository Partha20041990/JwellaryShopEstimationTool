using PriceEstimation.JwellaryShop.WebApi.Entities;
using System.Linq;

namespace PriceEstimation.JwellaryShop.WebApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        IOrderedQueryable<User> GetAll();
    }
}
