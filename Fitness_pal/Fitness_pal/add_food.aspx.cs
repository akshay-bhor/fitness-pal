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
    public partial class add_food : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //SET GLOBAL VARIABLES
            int userid = 0;
            int guest = 0;
            int goal = 1;
            int exe = 0;

            //ACCESS PUBLIC CLASSES
            pub_class pc = new pub_class();

            //GET AUTHENTICATION COOKIES
            string user = Request.Cookies["fitness_user"].Value;
            string pass = Request.Cookies["fitness_pass"].Value;

            //CHECK FOR LOGIN
            if (!pc.Is_loggedin(user, pass))
            {
                guest = 1;
            }

            //GET USERID
            userid = pc.Get_userid(user, pass);

            //CHECK IF GOAL IS SET
            if (!pc.Is_goalset(userid))
            {
                goal = 0;
            }

            if (goal == 1 && guest == 0)
            {
                //GET POSTED VARIABLES
                int food_id = int.Parse(Request.QueryString["id"]);
                int type = int.Parse(Request.QueryString["type"]);
                int servings = int.Parse(Request.QueryString["servings"]);
                string date = Request.QueryString["date"];

                //GET FOOD NUTRITION INFO
                string conn = pc.Getdb();
                SqlConnection db = new SqlConnection(conn);
                db.Open();
                SqlCommand sql = new SqlCommand("SELECT id, food_name, [energy_(Kcal)], [protein_(g)], [carbohydrate_(g)], unit_1, unit_2 FROM food_nutrition WHERE id = @id", db);
                sql.Parameters.AddWithValue("@id", food_id);
                SqlDataReader result = sql.ExecuteReader();
                result.Read();
                int fid = (int)result["id"];
                string food_name = (string)result["food_name"];
                int fcal = (int)result["energy_(Kcal)"];
                decimal protein = (decimal)result["protein_(g)"];
                decimal carbs = (decimal)result["carbohydrate_(g)"];
                string unit = (string)result["unit_1"];
                string unit2 = (string)result["unit_2"];
                db.Close();
                if (string.IsNullOrEmpty(unit))
                {
                    unit = unit2;
                }

                //MODIFY NUTRITION ACCORDING TO NUMBER OF SERVINGS
                fcal = (int)(fcal * servings);
                protein = (decimal)((decimal)protein * (decimal)servings);
                carbs = (decimal)((decimal)carbs * (decimal)servings);

                //INSERT INTO DIARY
                db.Open();
                SqlCommand isql = new SqlCommand("INSERT INTO diary (uid, food_type, food_name, unit, calories, protein, carbs, date, servings, fid) VALUES (@uid, @ftype, @fname, @unit, @cal, @protein, @carbs, @date, @serv, @fid)", db);
                isql.Parameters.AddWithValue("@uid", userid);
                isql.Parameters.AddWithValue("@ftype", type);
                isql.Parameters.AddWithValue("@fname", food_name);
                isql.Parameters.AddWithValue("@unit", unit);
                isql.Parameters.AddWithValue("@cal", fcal);
                isql.Parameters.AddWithValue("@protein", protein);
                isql.Parameters.AddWithValue("@carbs", carbs);
                isql.Parameters.AddWithValue("@date", date);
                isql.Parameters.AddWithValue("@serv", servings);
                isql.Parameters.AddWithValue("@fid", fid);
                exe = isql.ExecuteNonQuery();
                db.Close();
                if (exe != 0)
                {
                    Response.Write("<script>$(\"#fresult_holder\").html(\"<div class='alert alert-success'>Food Added Successfully</div>\");</script>");
                }
                else
                {
                    Response.Write("<script>alert(\"Error adding food\");</script>");
                }
            }
            else
            {
                if (goal == 0)
                {
                    Response.Write("<script>alert(\"You have not set any goal, set now\");</script>");
                }
                if (guest == 1)
                {
                    Response.Write("<script>alert(\"Please login to add food\");</script>");
                }
            }
        }
    }
}