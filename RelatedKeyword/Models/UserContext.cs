using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace RelatedKeyword.Models
{
    public class UserContext :DbContext
    {
        public string ConnectionString { get; set; }

        public UserContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        /// <summary>
        /// by nuget "MySql.Data"
        /// </summary>
        /// <returns></returns>
        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
    }
}
