namespace Dhruvarth.TeamVision.PustakParab.DbService
{
    public interface ISqlDbConn
    {
        public string ConnectionString { get; }
    }
    public class SqlDbConn : ISqlDbConn
    {
        public string ConnectionString { get; }

        public SqlDbConn(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
