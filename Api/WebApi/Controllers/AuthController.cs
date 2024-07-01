using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Service.DTO.User;
using Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    /// <summary>
    /// Controller for managing user authentication
    /// </summary>
    [ApiController]
    [Route("[controller]/[Action]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IUser _userServices;

        private readonly IConfiguration _config;

        private readonly ITokenService _tokenServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="userServices"><see cref="IUser"/> </param>
        /// <param name="config"><see cref="IConfiguration"/></param>
        public AuthController(IUser userServices, IConfiguration config, ITokenService tokenServices)
        {
            _userServices = userServices;
            _config = config;
            _tokenServices = tokenServices; 
        }

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="login">The user details to login</param>
        /// <returns> The JWT token for the logged in user.</returns>
        /// <exception cref="Exception">Thrown when the user credentials are invalid.</exception>
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRegisterModel login)
        {
            var loggedInUser = await _userServices.Login(login);

            if (loggedInUser != null)
            {
                var jwttoken = _tokenServices.GenerateAccessToken(loggedInUser.Id);
                var refreshToken = _tokenServices.GenerateRefreshToken(loggedInUser.Id);
                var response =new {jwttoken, refreshToken};
                return Ok(response);
            }
            throw new Exception("Invalid credentials");
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="user">The new user to register.</param>
        /// <returns>The registered User.</returns>
        /// <exception cref="Exception">Thrown when the user could not be added.</exception>
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] LoginRegisterModel user)
        {
            if(await _userServices.IsUserExisted(user.UserName))
            {
                return BadRequest("User already exists");
            }

            if (await _userServices.Register(user))
            {
                return Created(nameof(Register), user);
            }

            throw new Exception("Failed to Add User");
        }
    }
}
