using ErrorOr;
using ExpenseTracker.Application.CategoryFolders.Interface.Infrastructure;
using ExpenseTracker.Domain.CategoryData;
using ExpenseTracker.Domain.Errors.CategoryErrors;
using ExpenseTracker.Domain.Errors.DatabaseErrors;
using ExpenseTracker.Infrastructure.CategoryRepos.Options;
using Npgsql;

namespace ExpenseTracker.Infrastructure.CategoryRepos
{
    internal class CategoryRepository : ICategoryRepository
    {
        private readonly CategoryOptions _options;
        const string ForeignKeyViolation = "20503";
        const string UniqueViolation = "20505";

        public CategoryRepository(CategoryOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        private NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_options.ConnectionString);
        }

        public async Task<ErrorOr<List<Category>>> GetCategoriesAsync(CancellationToken token)
        {

            var categories = new List<Category>();

            try
            {
                using var connection = CreateConnection();
                await connection.OpenAsync(token);

                const string sql = @"
                SELECT categoryID, categoryName, reason
                FROM Category
                ORDER BY categoryID";

                using var command = new NpgsqlCommand(sql, connection);
                command.CommandTimeout = 30;
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
            catch (NpgsqlException ex) when (ex.InnerException is Timeout)
            {
                return DatabaseErrors.Database.Timeout;
            }
            catch (NpgsqlException ex) when (ex.Message.Contains("connection"))
            {
                return DatabaseErrors.Database.ConnectionFailed;
            }
            catch (NpgsqlException)
            {
                return DatabaseErrors.Database.OperationFailed;
            }
            catch (OperationCanceledException)
            {
                return DatabaseErrors.Database.Timeout;
            }
            catch (Exception)
            {
                return DatabaseErrors.Database.OperationFailed;
            }
        }

        public async Task<ErrorOr<Category>> GetCategoryByIdAsync(int categoryId, CancellationToken token)
        {
            try
            {
                using var connection = CreateConnection();
                await connection.OpenAsync(token);

                const string sql = @"
                SELECT categoryID, categoryName, reason
                FROM Category
                WHERE categoryID = @CategoryId";

                using var command = new NpgsqlCommand(sql, connection);
                command.CommandTimeout = 30;
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

                return CategoryErrors.NotFound.Category;
            }
            catch (NpgsqlException ex) when (ex.InnerException is TimeoutException)
            {
                return DatabaseErrors.Database.Timeout;
            }
            catch (NpgsqlException ex) when (ex.Message.Contains("connection"))
            {
                return DatabaseErrors.Database.ConnectionFailed;
            }
            catch (NpgsqlException)
            {
                return DatabaseErrors.Database.OperationFailed;
            }
            catch (OperationCanceledException)
            {
                return DatabaseErrors.Database.Timeout;
            }
            catch (Exception)
            {
                return DatabaseErrors.Database.OperationFailed;
            }
        }

        public async Task<ErrorOr<Category>> CreateCategoryAsync(Category category, CancellationToken token)
        {
            try
            {
                using var connection = CreateConnection();
                await connection.OpenAsync(token);

                const string sql = @"
                INSERT INTO Category (categoryName, reason)
                VALUES (@CategoryName, @Reason)
                RETURNING categoryID";

                using var command = new NpgsqlCommand(sql, connection);
                command.CommandTimeout = 30;
                command.Parameters.AddWithValue("@CategoryName", category.categoryName);
                command.Parameters.AddWithValue("@Reason", category.reason);

                var categoryId = await command.ExecuteScalarAsync(token);
                category.categoryID = Convert.ToInt32(categoryId);

                return category;
            }
            catch (NpgsqlException ex) when (ex.SqlState == UniqueViolation)
            {
                return DatabaseErrors.Database.DuplicateTransaction;
            }
            catch (NpgsqlException ex) when (ex.InnerException is TimeoutException)
            {
                return DatabaseErrors.Database.Timeout;
            }
            catch (NpgsqlException ex) when (ex.Message.Contains("connection"))
            {
                return DatabaseErrors.Database.ConnectionFailed;
            }
            catch (NpgsqlException)
            {
                return DatabaseErrors.Database.OperationFailed;
            }
            catch (OperationCanceledException)
            {
                return DatabaseErrors.Database.Timeout;
            }
            catch (Exception)
            {
                return DatabaseErrors.Database.OperationFailed;
            }
        }

        public async Task<ErrorOr<Updated>> UpdateCategoryAsync(Category category, CancellationToken token)
        {
            try
            {
                using var connection = CreateConnection();
                await connection.OpenAsync(token);

                const string sql = @"
                UPDATE Category
                SET categoryName = @CategoryName,
                    reason = @Reason
                WHERE categoryID = @CategoryId";

                using var command = new NpgsqlCommand(sql, connection);
                command.CommandTimeout = 30;
                command.Parameters.AddWithValue("@CategoryId", category.categoryID);
                command.Parameters.AddWithValue("@CategoryName", category.categoryName);
                command.Parameters.AddWithValue("@Reason", category.reason);

                var rowsAffected = await command.ExecuteNonQueryAsync(token);
                if (rowsAffected == 0)
                {
                    return CategoryErrors.NotFound.Category;
                }

                return Result.Updated;
            }
            catch (NpgsqlException ex) when (ex.SqlState == UniqueViolation)
            {
                return DatabaseErrors.Database.DuplicateTransaction;
            }
            catch (NpgsqlException ex) when (ex.InnerException is TimeoutException)
            {
                return DatabaseErrors.Database.Timeout;
            }
            catch (NpgsqlException ex) when (ex.Message.Contains("connection"))
            {
                return DatabaseErrors.Database.ConnectionFailed;
            }
            catch (NpgsqlException)
            {
                return DatabaseErrors.Database.OperationFailed;
            }
            catch (OperationCanceledException)
            {
                return DatabaseErrors.Database.Timeout;
            }
            catch (Exception)
            {
                return DatabaseErrors.Database.OperationFailed;
            }
        }

        public async Task<ErrorOr<Deleted>> DeleteCategoryAsync(int categoryId, CancellationToken token)
        {
            try
            {
                using var connection = CreateConnection();
                await connection.OpenAsync(token);

                const string sql = "DELETE FROM Category WHERE categoryID = @CategoryId";

                using var command = new NpgsqlCommand(sql, connection);
                command.CommandTimeout = 30;
                command.Parameters.AddWithValue("@CategoryId", categoryId);

                var rowsAffected = await command.ExecuteNonQueryAsync(token);
                if (rowsAffected == 0)
                {
                    return CategoryErrors.NotFound.Category;
                }

                return Result.Deleted;
            }
            catch (NpgsqlException ex) when (ex.SqlState == ForeignKeyViolation)
            {
                return CategoryErrors.Conflict.CategoryInUse;
            }
            catch (NpgsqlException ex) when (ex.InnerException is TimeoutException)
            {
                return DatabaseErrors.Database.Timeout;
            }
            catch (NpgsqlException ex) when (ex.Message.Contains("connection"))
            {
                return DatabaseErrors.Database.ConnectionFailed;
            }
            catch (NpgsqlException)
            {
                return DatabaseErrors.Database.OperationFailed;
            }
            catch (OperationCanceledException)
            {
                return DatabaseErrors.Database.Timeout;
            }
            catch (Exception)
            {
                return DatabaseErrors.Database.OperationFailed;
            }
        }
    }
}
