using ApiRest.Model;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;

namespace ApiRest.Repository
{
    public class RepoMySqlUsers : AbstractRepositoryMySql, IRepositoryUser
    {
        public RepoMySqlUsers(DataAccess connectionInfo, ILogger<RepoMySqlUsers> log) : 
            base(connectionInfo, log){}

        public async Task<User?> LoginAsync(LoginAPI api)
        {
            MySqlConnection connection = Connect();
            MySqlCommand? cmd= null;
            User? result = null;

            try
            {
                connection.Open();
                cmd = connection.CreateCommand();

                cmd.CommandText = "api_rest.read_user";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add( "email", MySqlDbType.VarChar, 500 ).Value = api.Email;
                cmd.Parameters.Add("user_password", MySqlDbType.VarChar, 100).Value = api.Password;
                DbDataReader data = await cmd.ExecuteReaderAsync();

                while( data.Read() )
                {
                    result = new User
                    {
                        Name = data["name"].ToString(),
                        Email = data["email"].ToString(),
                    };
                }
            }
            catch(Exception e)
            {
                E(e);
                throw new Exception("Error while trying to read user: " + e.Message );
            }
            finally
            {
                cmd?.Dispose();
                connection.Close();
                connection.Dispose();
            }

            return result;
        }
    }
}
