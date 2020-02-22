using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MallAPI.Authorization
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly string USERID = "userId";
        private readonly IConfiguration _configuration;

        public PermissionHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var permissionId = requirement.PermissionId;
            string userId = context.User.Claims.FirstOrDefault(c => c.Type.Equals(USERID))?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Task.CompletedTask;
            }

            //从数据库读取权限
            using (var conn = new MySqlConnection(_configuration["ConnectString"]))
            {
                var sql = $"SELECT 1 FROM mall.user u INNER JOIN mall.userandrole r ON u.id = r.userId INNER JOIN mall.roleandpermission p on p.roleId =r.roleId WHERE u.id = {userId} AND p.permissionId ={permissionId} and p.status = 0 and u.status=0";
                var result = conn.ExecuteScalar(sql);
                if (result != null)
                {
                    //有权访问
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}
