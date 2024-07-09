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
    public partial class food_search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string search_param = Request.QueryString["food"];
            string food_type = Request.QueryString["type"];

            //GET DB CONNECTION
            pub_class pc = new pub_class();
            string conn = pc.Getdb();
            SqlConnection db = new SqlConnection(conn);

            if (!string.IsNullOrEmpty(search_param))
            {
                //GET FOOD DATA FROM TABLE
                db.Open();
                SqlCommand sql = new SqlCommand("SELECT TOP(20) id, food_name, [energy_(Kcal)], unit_1, unit_2 FROM food_nutrition WHERE food_name LIKE @fname", db);
                sql.Parameters.AddWithValue("@fname", "%" + search_param + "%");
                SqlDataReader result = sql.ExecuteReader();
                while (result.Read())
                {
                    int id = (int)result["id"];
                    string fname = (string)result["food_name"];
                    int fcal = (int)result["energy_(Kcal)"];
                    string funit = (string)result["unit_1"];
                    string funit2 = (string)result["unit_2"];

                    if (string.IsNullOrEmpty(funit))
                    {
                        funit = funit2;
                    }

                    Response.Write("<div class='block aj-food-result'>" +
                        "<div class='block'><a href='/calories?food_id=" + id + "' target='_blank'><div class='grid-8 block l'>" + fname + "</div></a><div class='grid-2 block text-right r'>" + fcal + "</div></div>" +
                        "<div class='block'><div class='grid-5 block l'>" + funit + "</div>" +
                        "<div class='grid-5 block text-right r'><div class='grid-2 block r'><span class='glyphicon glyphicon-plus tblue pointer' onclick='addfood(" + id + ", " + food_type + ");'></span></div>" +
                        "<div class='grid-2 block r'><input type='number' name='serv_size' id='" + id + "' class='form-control' value='1' min='1' /></div></div></div>" + 
                        "</div>");
                }
            }
        }
    }
}