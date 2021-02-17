using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PriceEstimation.JwellaryShop.WebApi.Entities;
using PriceEstimation.JwellaryShop.WebApi.Helpers;
using PriceEstimation.JwellaryShop.WebApi.Repositories.Interfaces;
using PriceEstimation.JwellaryShop.WebApi.Services.Contracts;

namespace PriceEstimation.JwellaryShop.WebApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly AppSettings _appSettings;
        private readonly ILogger _logger;

        public UserService(IOptions<AppSettings> appSettings, IUserRepository userRepository, ILogger<UserService> logger)
        {
            _appSettings = appSettings.Value;
            _userRepository = userRepository;
            _logger = logger;
        }

        public User Authenticate(string username, string password)
        {
            try
            {
                _logger.LogInformation($"Going to authenticate user at {DateTime.Now}, user id: {username}");

                var user = _userRepository.GetAll().ToList().SingleOrDefault(x => x.Username == username && x.Password == password);

                // return null if user not found
                if (user == null)
                    return null;

                // authentication successful so generate jwt token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                user.Token = tokenHandler.WriteToken(token);

                // remove password before returning
                user.Password = null;

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Authentication for user {username} failed at {DateTime.Now}");
                throw;
            }
        }

        public IEnumerable<User> GetAll()
        {
            _logger.LogInformation($"Going to return all the users at {DateTime.Now}");
            // return users without passwords
            return _userRepository.GetAll().ToList().Select(x => {
                x.Password = null;
                return x;
            });
        }
    }
}