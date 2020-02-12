using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MallAPI.DataModel.Requset;
using MallAPI.DataModel.Response;
using MallAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MallAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : Controller
    {
        private User _user;

        public TokenController(User user)
        {
            _user = user;
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="param"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        [HttpPost]
        public Response Index(UserParam param, [FromServices]IConfiguration configuration)
        {
            var currentUser = _user.Query(param, configuration);
            if (currentUser == null)
            {
                return new Response(Enum.ResultEnum.Fail, "账号或密码不正确");
            }

            var credential = new SigningCredentials(new RsaSecurityKey(new RSACryptoServiceProvider(2048)), SecurityAlgorithms.RsaSha256Signature);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,currentUser.Name)
            };
            claims.AddRange(currentUser.RolesId.Split(',').Select(u => new Claim(ClaimTypes.Role, u)));

            var token = new JwtSecurityToken(
                configuration["JwtSettiing:ValidIssuer"],
                configuration["JwtSettiing:ValidAudience"],
                claims,
                null,
                DateTime.Now.AddMinutes(30),
                credential);
            return new Response(new JwtSecurityTokenHandler().WriteToken(token));

        }
    }
}