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
    public partial class food : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String search_param = null;
            Regex regexItem = new Regex("^[a-zA-Z0-9 ]*$");
            if (IsPostBack)
            {
                search_param = Request.Form["food_name"];
                if (regexItem.IsMatch(search_param))
                {
                    Response.Redirect("/food?food_name=" + search_param);
                }
                else
                {
                    err.InnerHtml = "<script>alert(\"Special Characters Not Allowed\");</script>";
                    Response.Redirect("/food");
                }
            }
            else
            {
                search_param = Request.QueryString["food_name"];
                if (search_param != null)
                {
                    if (regexItem.IsMatch(search_param)) { }
                    else
                    {
                        err.InnerHtml = "<script>alert(\"Special Characters Not Allowed\");</script>";
                        Response.Redirect("/food");
                    }
                }
            }
            if(search_param != null)
            {
                //GET FOOD LIST 
                pub_class pc = new pub_class();
                string conn = pc.Getdb();
                SqlConnection db = new SqlConnection(conn);
                db.Open();
                SqlCommand sql = new SqlCommand("SELECT TOP(20) id, food_name, [energy_(Kcal)], [protein_(g)], [carbohydrate_(g)], [sugar_(g)], unit_1, unit_2 FROM food_nutrition WHERE food_name LIKE @fname", db);
                sql.Parameters.AddWithValue("@fname", "%" + search_param + "%");
                SqlDataReader result = sql.ExecuteReader();

                //MENTION SEARCH RESULT PARAMETER
                fresult_holder.InnerHtml = "<h2 class='tpad'>Search Result For: " + search_param + "</h2>";

                while (result.Read()) {
                    int fid = (int)result["id"];
                    string fname = (string)result["food_name"];
                    int fcal = (int)result["energy_(Kcal)"];
                    decimal fprotein = (decimal)result["protein_(g)"];
                    decimal fcarbs = (decimal)result["carbohydrate_(g)"];
                    string fsugar = (string)result["sugar_(g)"];
                    string funit = (string)result["unit_1"];
                    string funit2 = (string)result["unit_2"];

                    //CHECK IF SUGAR IS EMPTY
                    if(string.IsNullOrEmpty(fsugar))
                    {
                        fsugar = "NA";
                    }
                    else
                    {
                        fsugar = fsugar + "g";
                    }

                    if(string.IsNullOrEmpty(funit))
                    {
                        funit = funit2;
                    }

                    fresult_holder.InnerHtml += "<div class='fresult-row block'><div class='iblock grid-8 l'>" + 
                        "<span class='block tblue bold big'><a href='/calories?food_id=" + fid + "'>"+ fname +"</a></span>" +
                        "<span class='block tpad-5'>"+ funit +"</span>" +
                        "<span class='block tpad-5'><span class='glyphicon glyphicon-triangle-right small'></span> Calories: "+ fcal +
                        " <span class='glyphicon glyphicon-triangle-right small'></span> Carbs: " + fcarbs +"g " +
                        " <span class='glyphicon glyphicon-triangle-right small'></span> Sugar: " + fsugar +
                        " <span class='glyphicon glyphicon-triangle-right small'></span> Protein: " + fprotein +"g " +
                        "</span>" + 
                        "</div><div class='iblock grid-2 r'><div class='circle-div'>"+ fcal +"</div></div></div>";
                }
                db.Close();
            }
            else
            {
                fresult_holder.InnerHtml = "<h2 class='text-center'>Food Contents</h2>" +
                    "<div class='block text-center tpad'>Understand how the food you’re eating contributes to your daily calories, macronutrients, and micronutrients.</div>";
            }
        }
    }
}