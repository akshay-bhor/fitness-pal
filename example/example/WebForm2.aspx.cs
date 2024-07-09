using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace example
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Connstring connstring = new Connstring();
            string connStr = connstring.conn();
            SqlConnection conn = new SqlConnection(connStr);
            try
            {
             Response.Write("Connecting to SQL...");
             conn.Open();
             Response.Write("HERE");
             string sql = "SELECT NDB_no, food_name FROM food_nutrition";
             SqlCommand cmd = new SqlCommand(sql, conn);
             SqlDataReader result = cmd.ExecuteReader();
            while (result.Read())
             {
                 Response.Write("<br>");
                 Response.Write(result[0]);
                 Response.Write("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
                 Response.Write(result[1]);
                 Response.Write("<br>");
            }

             }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }

            conn.Close();
            }
    }
    public class Connstring {
        public String conn()
        {
            string connStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=fitness;Integrated Security=True;Pooling=False";
            return connStr;
        }
    }
}