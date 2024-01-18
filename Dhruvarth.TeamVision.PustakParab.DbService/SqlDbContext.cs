using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace Dhruvarth.TeamVision.PustakParab.DbService
{
    public interface ISqlDbContext<T> where T : class
    {
        T Get(string query, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = null);
        int GetCount(string query, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = null);
        IEnumerable<T> GetAll(string query);
        IEnumerable<T> GetAll(string query, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = null);
        int Execute(string query, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = null);
        int Execute(string query, DataTable parms, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = null);
        T ExecuteScalar(string query);
        T ExecuteScalar(string query, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = null);

        Task<T> GetAsync(string query, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = null);
        Task<IEnumerable<T>> GetAllAsync(string query, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = null);

        Task<T> ExecuteScalarAsync(string query, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = null);
    }

    /// <summary>
    /// Implemented generic Dapper to communicate with SQL DataBase
    /// </summary>
    public class SqlDbContext<T> : ISqlDbContext<T> where T : class
    {
        #region Private Fields
        private readonly string sqlConnection;
        #endregion

        #region Public methods
        public SqlDbContext(ISqlDbConn _sqlConn)
        {
            sqlConnection = _sqlConn.ConnectionString;
        }

        public int Execute(string query, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = null)
        {
            using (IDbConnection db = new SqlConnection(sqlConnection))
            {
                return db.Execute(query, parms, commandType: commandType, commandTimeout: commandTimeout);
            }
        }

        public int Execute(string query, DataTable parms, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = null)
        {
            SqlCommand command = null;
            using (SqlConnection db = new SqlConnection(sqlConnection))
            {
                db.Open();
                command = new SqlCommand(query, db);
                command.CommandType = commandType;
                command.Parameters.Add("@TempTableUpdate", SqlDbType.Structured).Value = parms;
                return command.ExecuteNonQuery();
            }
        }

        public T ExecuteScalar(string query)
        {
            using (IDbConnection db = new SqlConnection(sqlConnection))
            {
                return db.ExecuteScalar<T>(query);
            }
        }

        public T ExecuteScalar(string query, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = null)
        {
            using (IDbConnection db = new SqlConnection(sqlConnection))
            {
                return db.ExecuteScalar<T>(query, parms, commandType: commandType, commandTimeout: commandTimeout);
            }
        }

        public T Get(string query, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = null)
        {
            using (IDbConnection db = new SqlConnection(sqlConnection))
            {
                return db.QueryFirstOrDefault<T>(query, parms, commandType: commandType, commandTimeout: commandTimeout);
            }
        }

        public int GetCount(string query, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = null)
        {
            int count = 0;
            using (IDbConnection db = new SqlConnection(sqlConnection))
            {
                count = db.QueryFirst<int>(query, parms, commandType: commandType, commandTimeout: commandTimeout);
            }
            return count;

        }

        public IEnumerable<T> GetAll(string query)
        {
            using (IDbConnection db = new SqlConnection(sqlConnection))
            {
                return db.Query<T>(query);
            }
        }

        public IEnumerable<T> GetAll(string query, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = null)
        {
            using (IDbConnection db = new SqlConnection(sqlConnection))
            {
                return db.Query<T>(query, parms, commandType: commandType, commandTimeout: commandTimeout);
            }
        }


        #endregion

        #region Async Methods
        public async Task<IEnumerable<T>> GetAllAsync(string query, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = null)
        {
            using (IDbConnection db = new SqlConnection(sqlConnection))
            {
                return await db.QueryAsync<T>(query, parms, commandType: commandType, commandTimeout: commandTimeout);
            }
        }
        public async Task<T> GetAsync(string query, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = null)
        {
            using (IDbConnection db = new SqlConnection(sqlConnection))
            {
                return await db.QueryFirstOrDefaultAsync<T>(query, parms, commandType: commandType, commandTimeout: commandTimeout);
            }

        }

        public async Task<T> ExecuteScalarAsync(string query, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = null)
        {
            using (IDbConnection db = new SqlConnection(sqlConnection))
            {
                return await db.ExecuteScalarAsync<T>(query, parms, commandType: commandType, commandTimeout: commandTimeout);
            }
        }
        #endregion
    }
}
