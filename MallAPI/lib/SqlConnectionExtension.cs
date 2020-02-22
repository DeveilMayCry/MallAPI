using Dapper;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace MallAPI.lib
{
    public static class SqlConnectionExtension
    {
        public static int Update(this MySqlConnection connection, string tableName, int id, object data)
        {
            var type = data.GetType();
            var properties = type.GetProperties().ToList();

            var conditions = new List<string>();
            properties.ForEach(item =>
            {
                if (item.PropertyType == typeof(int) )
                {
                    if (!item.GetValue(data).ToString().Equals(default(int).ToString()))
                    {
                        conditions.Add($" {item.Name} = '{item.GetValue(data)}'");
                    }
                }
                else if(item.PropertyType == typeof(decimal))
                {
                    if (!item.GetValue(data).ToString().Equals(default(decimal).ToString()))
                    {
                        conditions.Add($" {item.Name} = '{item.GetValue(data)}'");
                    }
                }
                else if (item.GetValue(data) != default)
                {
                    conditions.Add($" {item.Name} = '{item.GetValue(data)}'");
                }
            });
            var setStr = string.Join(",", conditions.ToArray());
            var targetSql = $"update {tableName} set {setStr} where id ={id} and status = 0";
            return connection.Execute(targetSql);
        }
    }
}
