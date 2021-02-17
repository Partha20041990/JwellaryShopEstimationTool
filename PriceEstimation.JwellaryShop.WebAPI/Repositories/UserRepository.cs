using Microsoft.Extensions.DependencyInjection;
using PriceEstimation.JwellaryShop.WebApi.Context;
using PriceEstimation.JwellaryShop.WebApi.Entities;
using PriceEstimation.JwellaryShop.WebApi.Helpers;
using PriceEstimation.JwellaryShop.WebApi.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PriceEstimation.JwellaryShop.WebApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly EstimationDBContext _dbContext;
        private readonly IServiceProvider _serviceProvider;

        public UserRepository(EstimationDBContext dBContext, IServiceProvider service)
        {
            _serviceProvider = service;
            LoadInitialData();
            _dbContext = dBContext;
        }

        private void LoadInitialData()
        {
            DataGenerator.Initialize(_serviceProvider);
        }

        public IOrderedQueryable<User> GetAll()
        {
            var result = _dbContext.Users.OrderBy(x=>x.Id);

            return result;
        }
    }
}
