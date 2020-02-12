using Dapper;
using MallAPI.DataModel.Requset;
using MallAPI.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MallAPI.Model
{
    public class User : BaseModel
    {

        public Int64 ID { get; set; }

        public String Name { get; set; }

        public string Email { get; set; }

        public string Tel { get; set; }

        public string Password { get; set; }

        public int Status { get; set; }

        /// <summary>
        /// 多个角色id使用英文逗号隔开,example 1,2,3
        /// </summary>
        public string RolesId{ get; set; }


        public User Query(UserParam param,IConfiguration configuration)
        {
            using (var conn = new MySqlConnection(configuration["ConnectString"]))
            {
                var sql = $"SELECT A.*,GROUP_CONCAT(B.ROLEID) AS ROLESID FROM `USER` A INNER JOIN USERANDROLE B  ON A.ID=B.USERID AND A.`STATUS`=0 AND B.`STATUS`= 0 WHERE A.TEL='{param.Tel}' AND A.PASSWORD='{param.Password}' GROUP BY A.ID";
                return conn.QueryFirstOrDefault<User>(sql);
            }
        }
    }
}
