using PriceEstimation.JwellaryShop.WebApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PriceEstimation.JwellaryShop.WebApi.Services.Contracts
{
    public interface IEstimationService
    {
        ApiResponse CalculatePrice(EstimationModel estimationModel);
    }
}
