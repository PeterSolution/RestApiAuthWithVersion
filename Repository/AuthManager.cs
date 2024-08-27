using AutoMapper;
using CoreApiInNet.Contracts;
using CoreApiInNet.Data;
using CoreApiInNet.Model;
using CoreApiInNet.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace CoreApiInNet.Repository
{
    public class AuthManager : InterfaceAuthManager
    {
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> usermanager;

        private IdentityUser usernick;

        public IConfiguration Configuration;

        public AuthManager(IMapper mapper, UserManager<IdentityUser> usermanager,
            IConfiguration configuration)
        {
            this.mapper = mapper;
            this.usermanager = usermanager;
            Configuration = configuration;
        }

        public async Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto)
        {
            IdentityUser user = mapper.Map<IdentityUser>(userDto);
            user.UserName = userDto.nick;


            var execute = await usermanager.CreateAsync(user, userDto.password);

            if (execute.Succeeded)
            {
                await usermanager.AddToRoleAsync(user, "User");
            }

            return execute.Errors; // it can return null
        }

        public async Task<AuthResponse> login(ApiUserDto userDto)
        {

            usernick = await usermanager.FindByNameAsync(userDto.nick);
            var userpassword = await usermanager.CheckPasswordAsync(usernick, userDto.password);

            //return userpassword;

            if (usernick == null || userpassword == null)
            {
                return null;
            }
            var token = await GenToken(usernick);
            return new AuthResponse
            {
                Token = token,
                UserId = usernick.Id,
                Refreshtoken = await CreateNewToken()
            };
        }

        async Task<string> GenToken(IdentityUser user)
        {
            var SecKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            Configuration["AuthSetting:Key"]));

            var credencial = new SigningCredentials(SecKey, SecurityAlgorithms.HmacSha256);
            var roles = await usermanager.GetRolesAsync(user);

            var roleclaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();


            var userclaims = await usermanager.GetClaimsAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim("uid",user.Id),
                
            }.Union(userclaims).Union(roleclaims);
            var token = new JwtSecurityToken(
                issuer: Configuration["AuthSetting:Issuer"],
                audience: Configuration["AuthSetting:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(Configuration["AuthSetting:DurationInMinutes"])),
                signingCredentials: credencial
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        string loginn = "CoreApiInNet";
        string reftoken = "RefreashToken";
        public async Task<string> CreateNewToken()
        {
            await usermanager.RemoveAuthenticationTokenAsync(usernick, loginn, reftoken);
            var newtoken = await usermanager.GenerateUserTokenAsync(usernick, loginn, reftoken);
            var result = await usermanager.SetAuthenticationTokenAsync(usernick, loginn, reftoken, newtoken);
            return newtoken;
        }

        public async Task<AuthResponse> VerifyToken(AuthResponse authResponse)
        {
            var JsonTokenHandler=new JwtSecurityTokenHandler();
            var tokencontent=JsonTokenHandler.ReadToken(authResponse.Token) as JwtSecurityToken;
            var username=tokencontent.Claims.ToList()
                .FirstOrDefault(q=>q.Type== JwtRegisteredClaimNames.Name).Value;
            usernick = await usermanager.FindByNameAsync(username);
            if (usernick == null)
            {
                return null;
            }
            var IsValidRefreshedToken = await usermanager.VerifyUserTokenAsync(
                usernick, loginn, reftoken, authResponse.Refreshtoken);
            if (!IsValidRefreshedToken)
            {
                var token = await CreateNewToken();
                return new AuthResponse
                {
                    Token = token,
                    UserId = usernick.Id,
                    Refreshtoken = await CreateNewToken()
                };
            }
            await usermanager.UpdateSecurityStampAsync(usernick);
            return null;
        }
    }
}
