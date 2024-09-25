using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Arekat.Server.Helpers
{
    public class JwtHelper(IConfiguration configuration)
    {
        private readonly IConfiguration _config = configuration;

        public static readonly string[] chartDesignerLevel =
        {
            "ChartDesigner lv0",
            "ChartDesigner lv1",
            "ChartDesigner lv2",
            "ChartDesigner lv3",
        };
        public static readonly string[] adminLevel =
        {
            "User",
            "Admin",
            "SuperAdmin",
        };

        public string CreateToken(int userId, string userName, int chartDesignerLv, int adminLv)
        {
            // 定义需要使用到的Claims
            var claims = new[]
            {
                new Claim("UserId", userId.ToString()),
                new Claim(ClaimTypes.Name, userName), //HttpContext.User.Identity.Name
                new Claim("ChartDesignerLevel", chartDesignerLevel[chartDesignerLv]), //HttpContext.User.IsInRole("SuperAdmin")
                new Claim(ClaimTypes.Role, adminLevel[adminLv]),
            };
            // 从 appsettings.ini 中读取SecretKey
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            // 选择加密算法
            var algorithm = SecurityAlgorithms.HmacSha256;
            // 生成Credentials
            var signingCredentials = new SigningCredentials(secretKey, algorithm);
            // 根据以上，生成token
            var jwtSecurityToken = new JwtSecurityToken(
                _config["Jwt:Issuer"],     //Issuer
                _config["Jwt:Audience"],   //Audience
                claims,                          //Claims,
                DateTime.Now,                    //notBefore
                DateTime.Now.AddHours(48),       //expires
                signingCredentials               //Credentials
            );
            // 将token变为string
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return token;
        }
    }
}
