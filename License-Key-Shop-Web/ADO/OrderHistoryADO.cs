using License_Key_Shop_Web.Models;
using Microsoft.Data.SqlClient;

namespace License_Key_Shop_Web.ADO
{
    public class OrderHistoryADO
    {
        public static string getConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).

                AddJsonFile("appsettings.json", optional: false);

            IConfiguration con = builder.Build();

            return con.GetValue<string>("ConnectionStrings:DBContext");
        }

        static public void Insert(string username)
        {
            using (SqlConnection con = new SqlConnection(getConnectionString()))
            {
                string sql = @"insert into OrderHistory_HE173252 values (@name)";
                con.Open();
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.AddWithValue("@name", username);
                com.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}
