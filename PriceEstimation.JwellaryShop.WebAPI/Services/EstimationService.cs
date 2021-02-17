using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PriceEstimation.JwellaryShop.WebApi.Entities;
using PriceEstimation.JwellaryShop.WebApi.Repositories.Interfaces;
using PriceEstimation.JwellaryShop.WebApi.Services.Contracts;
using System;

namespace PriceEstimation.JwellaryShop.WebApi.Services
{
    public class EstimationService:IEstimationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;

        public EstimationService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, ILogger<EstimationService> logger)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        public ApiResponse CalculatePrice(EstimationModel estimationModel)
        {
            try
            {
                _logger.LogInformation($"Going to calculate price at {DateTime.Now}");
                //validate parameter
                var validationResult = Validate(estimationModel);
                if (!validationResult.isValidated)
                {
                    _logger.LogInformation($"Failed Validating estimation request parameter at {DateTime.Now}");

                    return new ApiResponse
                    {
                        StatusCode = System.Net.HttpStatusCode.Forbidden,
                        Message = validationResult.validationMessage,
                        Exception = new FormatException(validationResult.validationMessage),
                        Result = null
                    };
                }

                //calculate price
                var price = Calculate(estimationModel);

                return new ApiResponse
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = validationResult.validationMessage,
                    Exception = null,
                    Result = price
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"[Error Occurred]: Exception raised hile calculating price at {DateTime.Now}",ex);
                return new ApiResponse
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Message = $"Error Occurred While Calculating Price. Detailed Error:{ex.Message}",
                    Exception = ex,
                    Result = null
                };
            }
        }

        private (bool isValidated,string validationMessage) Validate(EstimationModel estimationModel)
        {
            _logger.LogInformation($"Validating estimation request parameter at {DateTime.Now}");
            //non-negative value check
            if (estimationModel.DiscountPercentage < 0 || estimationModel.PricePerGram < 0 || estimationModel.Weight < 0)
            {
                return (false, "Parameter Can Not Be Negative");
            }

            //dicount percent logic check
            if (estimationModel.DiscountPercentage>0)
            {
                //only allow privileged user to put discount
                var currentUserId = int.Parse(_httpContextAccessor.HttpContext.User.Identity.Name);
                if (!_httpContextAccessor.HttpContext.User.IsInRole(Role.Privileged))
                {
                    return (false, "Only Privileged User Can Put Discount");
                }
            }

            return (true, "Success");
        }

        private decimal Calculate(EstimationModel estimationModel)
        {
            _logger.LogInformation($"Calculating price at {DateTime.Now}");

            var priceAfterDiscount = default(decimal);
            var priceBeforeDiscount = (estimationModel.PricePerGram * estimationModel.Weight);

            if(estimationModel.DiscountPercentage>0)
            {
                var discountMultiplier = (estimationModel.DiscountPercentage / 100);
                var discountPrice = priceBeforeDiscount * discountMultiplier;
                priceAfterDiscount = (priceBeforeDiscount - discountPrice);
            }
            else
            {
                priceAfterDiscount = priceBeforeDiscount;
            }

            return priceAfterDiscount;
        }
    }
}
