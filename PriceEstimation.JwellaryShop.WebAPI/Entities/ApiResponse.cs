using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PriceEstimation.JwellaryShop.WebApi.Entities
{
    public class ApiResponse
    {
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public object Result { get; set; }
        public object Exception { get; set; }
    }
}
