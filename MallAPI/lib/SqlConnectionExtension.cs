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

            List<string> conditions = ExtractConditions(data, properties);
            var setStr = string.Join(",", conditions.ToArray());
            var targetSql = $"update {tableName} set {setStr} where id ={id} and status = 0";
            return connection.Execute(targetSql);
        }

        public static int Insert(this MySqlConnection connection, string tableName, object data)
        {
            var type = data.GetType();
            var properties = type.GetProperties().ToList();

            var dict = ExtractConditionsDict(data,properties);
            var fields = new List<string>();
            var values = new List<string>();
            foreach (var item in dict)
            {
                fields.Add($"`{item.Key}`");
                values.Add($"'{item.Value}'");
            }
            var sql = $"INSERT INTO {tableName} ({string.Join(",", fields)}) VALUE ({string.Join(",", values)})";
           
            return connection.Execute(sql);
        }

        /// <summary>
        /// 将有值的属性以字符串的形式返回
        /// </summary>
        /// <param name="data"></param>
        /// <param name="properties"></param>
        /// <returns> for example: a=1,b=2</returns>
        private static List<string> ExtractConditions(object data, List<System.Reflection.PropertyInfo> properties)
        {
            var conditions = new List<string>();
            properties.ForEach(item =>
            {
                if (item.GetValue(data) != null)
                {
                    conditions.Add($" {item.Name} = '{item.GetValue(data)}'");
                }
            });
            return conditions;
        }

        /// <summary>
        /// 将有值的属性以键值对的形式返回
        /// </summary>
        /// <param name="data"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        private static Dictionary<string, string> ExtractConditionsDict(object data, List<System.Reflection.PropertyInfo> properties)
        {
            var dict = new Dictionary<string, string>();
            properties.ForEach(item =>
            {
                if (item.GetValue(data) != null)
                {
                    dict.Add(item.Name, item.GetValue(data).ToString());
                }
            });
            return dict;
        }
    }
}
