using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace fsm_api.Common
{
    public class DapperDataService
    {
        private readonly string _connectionString;

        public DapperDataService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["fsmConnection"].ConnectionString;
        }

        private IDbConnection CreateConnection()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        public async Task<T> GetAsync<T>(string sql, object parameters = null)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
                }
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(string sql, object parameters = null)
        {
            using (var connection = CreateConnection())
            {
                return await connection.QueryAsync<T>(sql, parameters);
            }
        }

        public async Task<int> ExecuteAsync(string sql, object parameters = null)
        {
            using (var connection = CreateConnection())
            {
                return await connection.ExecuteAsync(sql, parameters);
            }
        }

        public async Task<SqlMapper.GridReader> QueryMultipleAsync(
            string sql,
            object parameters = null,
            CommandType commandType = CommandType.StoredProcedure)
        {
            var connection = CreateConnection();

            try
            {
                return await connection.QueryMultipleAsync(sql, parameters, commandType: commandType);
            }
            catch
            {
                connection.Close();
                connection.Dispose();
                throw;
            }
        }


        public async Task<T> ExecuteScalarAsync<T>(
            string sql,
            object parameters = null,
            CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    return await connection.QuerySingleAsync<T>(
                        sql,
                        parameters,
                        commandType: commandType
                    );
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

    }
}