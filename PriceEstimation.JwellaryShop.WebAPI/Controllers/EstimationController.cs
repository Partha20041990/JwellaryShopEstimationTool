using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceEstimation.JwellaryShop.WebApi.Entities;
using PriceEstimation.JwellaryShop.WebApi.Services.Contracts;
using System;

namespace PriceEstimation.JwellaryShop.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class EstimationController : ControllerBase
    {
        private IEstimationService _estimationService;

        public EstimationController(IEstimationService estimationService)
        {
            _estimationService = estimationService;
        }

        [HttpPost("calculate")]
        public IActionResult CalculatePrice([FromBody]EstimationModel estimationParam)
        {
            if (estimationParam == null)
                return NotFound(new { message = "Parameter can not be null" });

            var price = _estimationService.CalculatePrice(estimationParam);

            return Ok(price);
        }
    }
}