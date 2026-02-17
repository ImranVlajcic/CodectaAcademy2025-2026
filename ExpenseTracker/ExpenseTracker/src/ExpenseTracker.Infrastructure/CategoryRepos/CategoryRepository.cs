using ExpenseTracker.Application.CategoryFolders.Interface.Infrastructure;
using ExpenseTracker.Domain.CategoryData;
using ExpenseTracker.Infrastructure.CategoryRepos.Options;
using Npgsql;

namespace ExpenseTracker.Infrastructure.CategoryRepos
{
    internal class CategoryRepository : ICategoryRepository
    {
        private readonly CategoryOptions _options;

        public CategoryRepository(CategoryOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        private NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_options.ConnectionString);
        }

        public async Task<List<Category>> GetCategoriesAsync(CancellationToken token)
        {

            var categories = new List<Category>();

            using var connection = CreateConnection();
            await connection.OpenAsync(token);

            const string sql = @"
                SELECT categoryID, categoryName, reason
                FROM Category
                ORDER BY categoryID";

            using var command = new NpgsqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync(token);

            while (await reader.ReadAsync(token))
            {
                categories.Add(new Category
                {
                    categoryID = reader.GetInt32(0),
                    categoryName = reader.GetString(1),
                    reason = reader.GetString(2)
                });
            }

            return categories;
        }

        public async Task<Category?> GetCategoryByIdAsync(int categoryId, CancellationToken token)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync(token);

            const string sql = @"
                SELECT categoryID, categoryName, reason
                FROM Category
                WHERE categoryID = @CategoryId";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@CategoryId", categoryId);

            using var reader = await command.ExecuteReaderAsync(token);

            if (await reader.ReadAsync(token))
            {
                return new Category
                {
                    categoryID = reader.GetInt32(0),
                    categoryName = reader.GetString(1),
                    reason = reader.GetString(2)
                };
            }

            return null;
        }

        public async Task<Category> CreateCategoryAsync(Category category, CancellationToken token)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync(token);

            const string sql = @"
                INSERT INTO Category (categoryName, reason)
                VALUES (@CategoryName, @Reason)
                RETURNING categoryID";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@CategoryName", category.categoryName);
            command.Parameters.AddWithValue("@Reason", category.reason);

            var categoryId = await command.ExecuteScalarAsync(token);
            category.categoryID = Convert.ToInt32(categoryId);

            return category;
        }

        public async Task<bool> UpdateCategoryAsync(Category category, CancellationToken token)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync(token);

            const string sql = @"
                UPDATE Category
                SET categoryName = @CategoryName,
                    reason = @Reason
                WHERE categoryID = @CategoryId";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@CategoryId", category.categoryID);
            command.Parameters.AddWithValue("@CategoryName", category.categoryName);
            command.Parameters.AddWithValue("@Reason", category.reason);

            var rowsAffected = await command.ExecuteNonQueryAsync(token);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId, CancellationToken token)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync(token);

            const string sql = "DELETE FROM Category WHERE categoryID = @CategoryId";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@CategoryId", categoryId);

            var rowsAffected = await command.ExecuteNonQueryAsync(token);
            return rowsAffected > 0;
        }
    }
}
