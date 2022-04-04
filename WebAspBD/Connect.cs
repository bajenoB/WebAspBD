using System.Data.SqlClient;

namespace WebAspBD
{
    public class Connection_DataBase
    {
        public static SqlConnection conn { get; set; }
        public static bool isConnect { get; set; }
        public Connection_DataBase()
        {
        }
        public static bool Connect()
        {
            if (conn == null)
            {
                conn = new SqlConnection("Data Source=savel.site4now.net;Initial Catalog=db_a838e6_bajenob;User Id=db_a838e6_bajenob_admin;Password=Savelstan123");
            }
            isConnect = false;
            try
            {
                if (!isConnect)
                {
                    conn.Open();

                }
                isConnect = true;
            }
            catch (System.Exception)
            {
                isConnect = false;
            }
            return isConnect;
        }
    }
}
