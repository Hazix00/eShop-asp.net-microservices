using Dapper;
using Discount.API.Entities;
using Npgsql;

namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;
        private string _connectionString;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _connectionString = _configuration.GetValue<string>("DatabaseSettings:ConnectionString");
        }
        public async Task<Coupon> GetDiscount(string ProductName)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("SELECT * FROM Coupon WHERE ProductName = @ProductName;", new {ProductName});
            if(coupon is null)
            {
                return new Coupon { ProductName = "No Discount", Description = "No Discount Desc", Amount = 0 };
            }
            return coupon;
        }
        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            return await ExcecuteCUDOperation("INSERT INTO Coupon VALUES(@ProductName, @Description, @Amount);", coupon);
        }
        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            return await ExcecuteCUDOperation("UPDATE Coupon SET ProductName = @ProductName, Description = @Description, Amount = @Amount WHERE ID = @ID;", coupon);
        }
        public async Task<bool> DeleteDiscount(string ProductName)
        {
            return await ExcecuteCUDOperation("DELETE Coupon WHERE ProductName = @ProductName;", new { ProductName });
        }

        private async Task<bool> ExcecuteCUDOperation(string query, object param)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var affected = await connection.ExecuteAsync(query, param);
            return affected > 0;
        }

    }
}
