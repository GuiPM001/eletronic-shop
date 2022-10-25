using Dapper;
using EletronicShop.Domain.Entities;
using EletronicShop.Domain.Repositories;
using System.Data;
using System.Data.SqlClient;

namespace EletronicShop.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly SqlConnection sqlConnection;

        public ProductRepository(IDbConnection sqlConnection)
        {
            this.sqlConnection = (SqlConnection)sqlConnection;
        }

        #region Querys
        private const string INSERT_PRODUCT = @"
            INSERT INTO dbo.products (
                name,
                quantity,
                price,
                brand
            )
            VALUES (
                @Name,
                @Quantity,
                @Price,
                @Brand
            )
            SELECT SCOPE_IDENTITY()
        ";

        private const string ADD_IMAGE = @"
            UPDATE dbo.products 
            SET image = @Image
            WHERE id = @Id
        ";

        private const string GET_ALL_PRODUCTS = @"
            SELECT 
                id AS Id,
                name AS Name,
                quantity AS Quantity,
                price AS Price,
                brand AS Brand
            FROM dbo.products
        ";

        private const string GET_PRODUCT_BY_NAME = @"
            SELECT 
                name AS Name,
                quantity AS Quantity,
                price AS Price,
                brand AS Brand
            FROM dbo.products
            WHERE name = @Name
        ";
        #endregion

        public async Task<int> RegisterProduct(Product product)
        {
            var query = INSERT_PRODUCT;

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Name", product.Name, DbType.String);
            parameters.Add("@Quantity", product.Quantity, DbType.Int64);
            parameters.Add("@Price", product.Price, DbType.Decimal);
            parameters.Add("@Brand", product.Brand, DbType.String);

            return await sqlConnection.ExecuteScalarAsync<int>(query, parameters);
        }

        public async Task AddProductImage(int id, string image)
        {
            var query = ADD_IMAGE;

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Int32);
            parameters.Add("@Image", image, DbType.String);

            await sqlConnection.ExecuteAsync(query, parameters);
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            var query = GET_ALL_PRODUCTS;

            return await sqlConnection.QueryAsync<Product>(query);
        }

        public async Task<Product> GetProductByName(string name)
        {
            var query = GET_PRODUCT_BY_NAME;

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Name", name, DbType.String);

            var products = await sqlConnection.QueryAsync<Product>(query, parameters);

            return products.FirstOrDefault();
        }
    }
}
