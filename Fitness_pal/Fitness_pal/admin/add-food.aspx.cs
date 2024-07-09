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

namespace Fitness_pal.admin
{
    public partial class add_food : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //SET GLOBAL VARIABLES
            int userid = 0;

            //ACCESS PUBLIC CLASSES
            pub_class pc = new pub_class();

            //GET AUTHENTICATION COOKIES
            string user = Request.Cookies["fitness_user"].Value;
            string pass = Request.Cookies["fitness_pass"].Value;

            //CREATE DATABASE CONNECTION
            string conn = pc.Getdb();
            SqlConnection db = new SqlConnection(conn);

            if (pc.Is_admin(user, pass))
            {
                if(IsPostBack)
                {
                    //GET POSTED VALUES
                    string fname = Request.Form["fname"];
                    decimal water = Convert.ToDecimal(Request.Form["water"]);
                    int cal = Convert.ToInt32(Request.Form["energy"]);
                    decimal protein = Convert.ToDecimal(Request.Form["protein"]);
                    decimal lipid = Convert.ToDecimal(Request.Form["lipid"]);
                    decimal ash = Convert.ToDecimal(Request.Form["ash"]);
                    decimal carbs = Convert.ToDecimal(Request.Form["carbs"]);
                    decimal fiber = Convert.ToDecimal(Request.Form["fiber"]);
                    decimal sugar = Convert.ToDecimal(Request.Form["sugar"]);
                    decimal calcium = Convert.ToDecimal(Request.Form["calcium"]);
                    decimal iron = Convert.ToDecimal(Request.Form["iron"]);
                    decimal magnesium = Convert.ToDecimal(Request.Form["magnesium"]); 
                    decimal phosphorus = Convert.ToDecimal(Request.Form["phosphorus"]);
                    decimal potassium = Convert.ToDecimal(Request.Form["potassium"]);
                    decimal sodium = Convert.ToDecimal(Request.Form["sodium"]);
                    decimal zinc = Convert.ToDecimal(Request.Form["zinc"]);
                    decimal copper = Convert.ToDecimal(Request.Form["copper"]);
                    decimal manganese = Convert.ToDecimal(Request.Form["manganese"]);
                    decimal vitc = Convert.ToDecimal(Request.Form["vitc"]);
                    decimal vitb6 = Convert.ToDecimal(Request.Form["vitb6"]);
                    decimal vite = Convert.ToDecimal(Request.Form["vite"]);
                    string unit = Request.Form["unit"];
                    string tunit = "NA";

                    //INSERT
                    db.Open();
                    SqlCommand sql = new SqlCommand("INSERT INTO food_nutrition ([food_name], [water_(g)], [energy_(Kcal)], [protein_(g)], [lipid_(g)], [ash_(g)], [carbohydrate_(g)], [fiber_(g)], [sugar_(g)], [calcium_(mg)], [iron_(mg)], [magnesium_(mg)], [phosphorus_(mg)], [potassium_(mg)], [sodium_(mg)], [zinc_(mg)], [copper_(mg)], [manganese_(mg)], [vit_C_(mg)], [vit_B6_(mg)], [vit_E_(mg)], [unit_1], [unit_2]) VALUES (@fname, @water, @cal, @protein, @lipid, @ash, @carbs, @fiber, @sugar, @calcium, @iron, @magnesium, @phosphorus, @potassium, @sodium, @zinc, @copper, @manganese, @vitc, @vitb6, @vite, @unit, @tunit)", db);
                    sql.Parameters.AddWithValue("@fname", fname);
                    sql.Parameters.AddWithValue("@water", water);
                    sql.Parameters.AddWithValue("@cal", cal);
                    sql.Parameters.AddWithValue("@protein", protein);
                    sql.Parameters.AddWithValue("@lipid", lipid);
                    sql.Parameters.AddWithValue("@ash", ash);
                    sql.Parameters.AddWithValue("@carbs", carbs);
                    sql.Parameters.AddWithValue("@fiber", fiber);
                    sql.Parameters.AddWithValue("@sugar", sugar);
                    sql.Parameters.AddWithValue("@calcium", calcium);
                    sql.Parameters.AddWithValue("@iron", iron);
                    sql.Parameters.AddWithValue("@magnesium", magnesium);
                    sql.Parameters.AddWithValue("@phosphorus", phosphorus);
                    sql.Parameters.AddWithValue("@potassium", potassium);
                    sql.Parameters.AddWithValue("@sodium", sodium);
                    sql.Parameters.AddWithValue("@zinc", zinc);
                    sql.Parameters.AddWithValue("@copper", copper);
                    sql.Parameters.AddWithValue("@manganese", manganese);
                    sql.Parameters.AddWithValue("@vitc", vitc);
                    sql.Parameters.AddWithValue("@vitb6", vitb6);
                    sql.Parameters.AddWithValue("@vite", vite);
                    sql.Parameters.AddWithValue("@unit", unit);
                    sql.Parameters.AddWithValue("@tunit", tunit);
                    int exe = 0;
                    exe = sql.ExecuteNonQuery();
                    db.Close();
                    if(exe != 0)
                    {
                        err.InnerHtml = "<div class='alert alert-success'>Inserted Successfully.</div>";
                    }
                    else
                    {
                        err.InnerHtml = "<div class='alert alert-danger'>Error Inserting Record, Try Again.</div>";
                    }
                }
            }
            else
            {
                Response.Redirect("/");
            }
        }
    }
}