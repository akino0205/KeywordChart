using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace RelatedKeyword.Models
{
    public class UserContext :DbContext
    {

        public UserContext(DbContextOptions<UserContext> options)
        : base(options)
        {
        }
        /// <summary>
        /// by nuget "MySql.Data"
        /// </summary>
        /// <returns></returns>
        //private MySqlConnection GetConnection()
        //{
        //    return new MySqlConnection(ConnectionString);
        //}
    }
}
