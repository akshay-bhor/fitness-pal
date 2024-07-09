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
    public partial class WebForm1 : System.Web.UI.Page
    {
        string connStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=fitness;Integrated Security=True;Pooling=False";
        protected void Page_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(connStr);

           // try
           // {
              //  Response.Write("Connecting to SQL...");
               // conn.Open();
              //  Response.Write("HERE");
              //  string sql = "SELECT NDB_no, food_name FROM food_nutrition";
              //  SqlCommand cmd = new SqlCommand(sql, conn);
              //  SqlDataReader result = cmd.ExecuteReader();
               // while (result.Read())
              //  {
              //      Response.Write("<br>");
              //      Response.Write(result[0]);
              //      Response.Write("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
              //      Response.Write(result[1]);
              //      Response.Write("<br>");
               // }

           // }
            //catch (Exception ex)
            //{
            //    Response.Write(ex.ToString());
            //}

            //conn.Close();
            //Response.Write("Done.");

            try
            {
                DataTable data = new DataTable();
                SqlDataAdapter ad = new SqlDataAdapter("SELECT id, food_name FROM food_nutrition", conn);
                ad.Fill(data);

                DropDownList1.DataSource = data;
                DropDownList1.DataTextField = "food_name";
                DropDownList1.DataValueField = "id";
                DropDownList1.DataBind();

                DropDownList1.Items.Insert(0, new ListItem("<select>", "0"));
            }
            catch (Exception ex) {
                Response.Write(ex.ToString());
            }
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
         

        }
    }
}