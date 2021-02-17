using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PriceEstimation.JwellaryShop.WebApi.Entities;

namespace PriceEstimation.JwellaryShop.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi =true)]
    public class ErrorController : ControllerBase
    {
        private readonly ILogger _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }
        [Route("error")]
        public ApiResponse Error()
        {
            var ctx = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = ctx.Error;
            Response.StatusCode = 500;
            _logger.LogError($"[App Level Error Raised]: Error occurred while processing a request at {DateTime.Now}, exception details:{exception.Message}, error stacktrace:{exception.StackTrace}");
            return new ApiResponse
            {
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                Message = exception.Message,
                Exception = exception,
                Result = null
            };
        }
    }
}