using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using WebApiCore.Common;
using WebApiCore.Model;

namespace WebApiCore.Repository
{
    public class ProductRepository : IProductRepository
    {
        public async Task<bool> AddAsync(Product prod)
        {
            using (IDbConnection conn = DbConfig.GetSqlConnection())
            {
                string sql = @"INSERT INTO Product 
                            (Name
                            ,Quantity
                            ,Price
                            ,CategoryId)
                        VALUES
                            (@Name
                            ,@Quantity
                            ,@Price
                            ,@CategoryId)";
                return await conn.ExecuteAsync(sql, prod) > 0;
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            using (IDbConnection conn = DbConfig.GetSqlConnection())
            {
                return await conn.QueryAsync<Product>(@"SELECT Id
                                            ,Name
                                            ,Quantity
                                            ,Price
                                            ,CategoryId
                                        FROM Product");
            }
        }

        public async Task<Product> GetByIDAsync(int id)
        {
            using (IDbConnection conn = DbConfig.GetSqlConnection())
            {
                string sql = @"SELECT Id
                                ,Name
                                ,Quantity
                                ,Price 
                                ,CategoryId
                            FROM Product
                            WHERE Id = @Id";
                return await conn.QueryFirstOrDefaultAsync<Product>(sql, new { Id = id });
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (IDbConnection conn = DbConfig.GetSqlConnection())
            {
                string sql = @"DELETE FROM Product
                            WHERE Id = @Id";
                return await conn.ExecuteAsync(sql, new { Id = id }) > 0;
            }
        }

        public async Task<bool> UpdateAsync(Product prod)
        {
            using (IDbConnection conn = DbConfig.GetSqlConnection())
            {
                string sql = @"UPDATE Product SET 
                                Name = @Name
                                ,Quantity = @Quantity
                                ,Price= @Price
                                ,CategoryId= @CategoryId
                           WHERE Id = @Id";
                return await conn.ExecuteAsync(sql, prod) > 0;
            }
        }
    }
}
