using MySql.Data.MySqlClient;

namespace ApiRest.Repository
{
    public abstract class AbstractRepositoryMySql
    {
        protected string ConnectionString;
        protected ILogger log;

        public AbstractRepositoryMySql(DataAccess connectionInfo, ILogger log)
        {
            ConnectionString = connectionInfo.ConnectionString;
            this.log = log; 
        }


        protected MySqlConnection Connect()
        {
            return new MySqlConnection(ConnectionString);
        }

        protected void L(string message)
        {
            log.LogInformation(message);
        }

        protected void E(string message)
        {
            log.LogError(message);
        }

        protected void E(Exception exception) => log.LogError(exception, message: null);
    }
}
