using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PriceEstimation.JwellaryShop.WebApi.Entities
{
    public class EstimationModel
    {
        public decimal PricePerGram { get; set; }
        public decimal Weight { get; set; }
        public decimal DiscountPercentage { get; set; }
    }
}
