using Dapper;
using MallAPI.DTO.Requset.Category;
using MallAPI.lib;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MallAPI.Model
{
    public class Category : BaseModel
    {
        private IConfiguration _configuration;
        private readonly string _tableName = "MALL.CATEGORY";
        public Category()
        {

        }

        public Category(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public long ID { get; set; }

        public long? ParentId { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 根据id获取品类信息
        /// </summary>
        /// <returns></returns>
        public List<Category> GetCategories()
        {
            using (var conn = new MySqlConnection(_configuration["ConnectString"]))
            {
                var sql = $"select * from {_tableName} where id =@id and status = 0";
                var result = conn.Query<Category>(sql, new { ID });
                if (!result.Any())
                {
                    throw new Exception("品类不存在");
                }
                return result.ToList();
            }
        }

        /// <summary>
        /// 设置品类名称
        /// </summary>
        public void SetCategoryName()
        {
            using (var conn = new MySqlConnection(_configuration["ConnectString"]))
            {
                if (!ExistCategory(ID))
                {
                    throw new Exception("品类不存在");
                }

                var sql = $"Update {_tableName} set `name` = @name where id =@id and status =0";
                var result = conn.Execute(sql, new { ID, Name });
                if (result == 0)
                {
                    throw new Exception("操作失败");
                }
            }
        }


        /// <summary>
        /// 新增商品目录
        /// </summary>
        public void InsertCategory()
        {

            if (ParentId.HasValue && !ExistCategory(ParentId.Value))
            {
                throw new Exception("父级品类不存在");
            }

            if (ExistCategory(Name))
            {
                throw new Exception("品类名称重复");
            }

            using (var conn = new MySqlConnection(_configuration["ConnectString"]))
            {
                var result = conn.Insert(_tableName, new { ParentId, Name });
                if (result == 0)
                {
                    throw new Exception("操作失败");
                }
            }
        }



        /// <summary>
        /// 根据id查询是否存在品类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ExistCategory(long id)
        {
            using (var conn = new MySqlConnection(_configuration["ConnectString"]))
            {
                var sql = $"select 1 from {_tableName} where id =@id and status =0";
                var result = conn.ExecuteScalar(sql, new { id });
                return result != null;
            }
        }

        /// <summary>
        /// 根据名称查询是否存在品类
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool ExistCategory(string name)
        {
            using (var conn = new MySqlConnection(_configuration["ConnectString"]))
            {
                var sql = $"select 1 from {_tableName} where name =@name and status =0";
                var result = conn.ExecuteScalar(sql, new { name = name.Trim() });
                return result != null;
            }
        }

        /// <summary>
        /// 递归获取子品类节点
        /// </summary>
        /// <returns></returns>
        public List<Category> GetCaregoryRecursive()
        {
            using (var conn = new MySqlConnection(_configuration["ConnectString"]))
            {
                var sql = $@"with recursive t1 as (
                            select * from {_tableName} where parentId is null and  id=@id and status =0  -- Anchor member.
                             union all
                            select t2.* from {_tableName} t2 INNER JOIN t1 on t2.parentId = t1.id  -- Recursive member.
                             where   t2.status =0
                            )
                            select * from t1;
";
                var result = conn.Query<Category>(sql, new { ID }).ToList();
                if (result.Count == 0)
                {
                    throw new Exception("品类不存在");
                }

                return result;
            }
        }

    }
}
