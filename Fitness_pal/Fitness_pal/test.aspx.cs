using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Fitness_pal
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            pub_class pc = new pub_class();
            string conn = pc.Getdb();
            SqlConnection db = new SqlConnection(conn);

            int uid = 5;
            string date = "2019-03-10";
            int type = 3;
            int tcal = 0;

            //GET CALORIES FOR FOOD TYPE
            db.Open();
            SqlCommand sql = new SqlCommand("SELECT COUNT(id) as c, SUM(calories) AS tcal FROM diary WHERE uid = @uid AND date = @date AND food_type = @type", db);
            sql.Parameters.AddWithValue("@uid", uid);
            sql.Parameters.AddWithValue("@date", date);
            sql.Parameters.AddWithValue("@type", type);
            SqlDataReader result = sql.ExecuteReader();
            while (result.Read())
            {
                int c = (int)result["c"];
                if(c > 0)
                {
                    tcal = (int)result["tcal"];
                }
            }
            Response.Write(tcal);
            db.Close();
        }
    }
}