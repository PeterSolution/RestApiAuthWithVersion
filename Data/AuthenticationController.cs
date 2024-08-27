using CoreApiInNet.Contracts;
using CoreApiInNet.Model;
using CoreApiInNet.Users;
using Microsoft.AspNetCore.Mvc;

namespace CoreApiInNet.Data
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly InterfaceAuthManager authManager;

        public ILogger<AuthenticationController> Logger { get; set; }

        public AuthenticationController(InterfaceAuthManager authManager,ILogger<AuthenticationController>logger)
        {
            this.authManager = authManager;
            Logger = logger;
        }
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Register([FromBody] ApiUserDto userDto)
        {
            Logger.LogInformation($"Register attempt for: {userDto.nick}");
            try
            {
                var errors = await authManager.Register(userDto);

                if (errors.Any())
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);

                    }
                    return BadRequest(ModelState);
                }
            }
            catch(Exception ex) 
            {
                Logger.LogInformation($"Error: {ex.Message}");
            }
            return Ok();

        }


        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> login(ApiUserDto userDto)
        {
            Logger.LogError($"User {userDto.nick} try to log");
            try
            {
                var authResponse = await authManager.login(userDto);
                if (authResponse == null)
                {
                    Logger.LogError("User does not exist ");
                    return Unauthorized("User does not exist");
                }
                else
                {

                    return Ok(authResponse);
                }
            }
            catch(Exception ex)
            {
                Logger.LogError("Log error: "+ex.ToString());
                return BadRequest();
            }
            
        }
        [HttpPost]
        [Route("refreshtoken")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Refreshtoken(AuthResponse response)
        {
            Logger.LogInformation($"Token refreshed: {response}");
            var authResponse = await authManager.VerifyToken(response);
            if (authResponse == null)
            {
                return Unauthorized();
            }
            else
            {

                return Ok(authResponse);
            }

        }
    }
}
