namespace ApiRest.Repository
{
    public class DataAccess
    {
        private string connectionString;

        public string ConnectionString { get => connectionString; }

        public DataAccess(string ConnectionString)
        {
            this.connectionString = ConnectionString;
        }
    }
}
