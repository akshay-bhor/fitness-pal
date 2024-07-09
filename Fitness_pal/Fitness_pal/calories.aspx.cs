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
    public partial class calories : System.Web.UI.Page
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
                    Response.Redirect("/calories");
                }
            }

            //GET FOOD ID AND INFO
            string food_id = Request.QueryString["food_id"];

            if (!string.IsNullOrEmpty(food_id))
            {

                //GET PUBLIC CLASS OBJECT
                pub_class pc = new pub_class();
                string conn = pc.Getdb();
                SqlConnection db = new SqlConnection(conn);
                db.Open();
                SqlCommand sql = new SqlCommand("SELECT * FROM food_nutrition WHERE id = @food_id", db);
                sql.Parameters.AddWithValue("@food_id", food_id);
                SqlDataReader result = sql.ExecuteReader();
                result.Read();
                string fname = (string)result["food_name"];
                int fcal = (int)result["energy_(Kcal)"];
                decimal fprotein = (decimal)result["protein_(g)"];
                decimal fcarbs = (decimal)result["carbohydrate_(g)"];
                string fsugar = (string)result["sugar_(g)"];
                string fiber = (string)result["fiber_(g)"];
                decimal flipid = (decimal)result["lipid_(g)"];
                string fminerals = (string)result["ash_(g)"];
                string fcalcium = (string)result["calcium_(mg)"];
                string firon = (string)result["iron_(mg)"];
                string fmagnesium = (string)result["magnesium_(mg)"];
                string fpotassium = (string)result["potassium_(mg)"];
                string fsodium = (string)result["sodium_(mg)"];
                string fzinc = (string)result["zinc_(mg)"];
                string fvitc = (string)result["vit_C_(mg)"];
                string fvitb6 = (string)result["vit_B6_(mg)"];
                string fvite = (string)result["vit_E_(mg)"];
                string funit = (string)result["unit_1"];
                string funit2 = (string)result["unit_2"];
                if (string.IsNullOrEmpty(funit))
                {
                    funit = funit2;
                }
                db.Close();

                //SET GLOBAL ATTRIBUTES
                string user = null;
                string pass = null;
                int userid = 0;
                int goal_cal = 2000;
                var user_c = Request.Cookies["fitness_user"];
                var pass_c = Request.Cookies["fitness_pass"];
                if (user_c != null && pass_c != null)
                {
                    //GET COOKIES VALUE
                    user = Request.Cookies["fitness_user"].Value;
                    pass = Request.Cookies["fitness_pass"].Value;
                }
                if (pc.Is_loggedin(user, pass))
                {
                    userid = pc.Get_userid(user, pass);

                    //GET GOAL CALORIES
                    db.Open();
                    SqlCommand gsql = new SqlCommand("SELECT calorie_need FROM goals WHERE uid = @uid", db);
                    gsql.Parameters.AddWithValue("@uid", userid);
                    SqlDataReader gresult = gsql.ExecuteReader();
                    gresult.Read();
                    goal_cal = (int)gresult["calorie_need"];
                    db.Close();
                }

                //CALCULATE REMAINING CALORIES AND PERCENTAGE
                int remcal = goal_cal - fcal;
                int cal_perc = (int)(((float)fcal / (float)goal_cal) * 100);

                //INSERT VALUES TO CORRESPONDING FIELDS
                food_heading.InnerHtml = fname;
                ser_size.InnerHtml = "Serving Size: " + funit;
                fact_chart.InnerHtml = "<div class='grid-3 l text-center'><div class='circle-div circle-div-big'>" + fcal + "<br>Cal</div></div>" +
                    "<div class='grid-2 l text-center'><span class='bold'>" + fcarbs + "g<br>Carbs</span></div>" +
                    "<div class='grid-2 l text-center'><span class='bold'>" + fsugar + "g<br>Sugar</span></div>" +
                    "<div class='grid-2 l text-center'><span class='bold'>" + fprotein + "g<br>Protein</span></div>";
                rem_cal.InnerHtml = remcal + " cal";
                pbar.InnerHtml = "<div class='progress-bar progress-bar-striped active' role='progressbar'" +
                                 "aria-valuenow='" + cal_perc + "' aria-valuemin='0' aria-valuemax='100' style='width:" + cal_perc + "%'>" + cal_perc + "%" + 
                                 "</div>";
                comp_cal.InnerHtml = fcal + "/" + goal_cal + " cal";
                nutri_info.InnerHtml = "<div class='fresult-row block'><span class='iblock grid-8 l'>Carbs</span><span class='iblock grid-2 r txt-right'>" + fcarbs + " g</span></div>" +
                    "<div class='fresult-row block'><span class='iblock grid-8 l'>Sugar</span><span class='iblock grid-2 r txt-right'>" + fsugar + " g</span></div>" +
                    "<div class='fresult-row block'><span class='iblock grid-8 l'>Protein</span><span class='iblock grid-2 r txt-right'>" + fprotein + " g</span></div>" +
                    "<div class='fresult-row block'><span class='iblock grid-8 l'>Fiber</span><span class='iblock grid-2 r txt-right'>" + fiber + " g</span></div>" +
                    "<div class='fresult-row block'><span class='iblock grid-8 l'>Lipid</span><span class='iblock grid-2 r txt-right'>" + flipid + " g</span></div>" +
                    "<div class='fresult-row block'><span class='iblock grid-8 l'>Minerals</span><span class='iblock grid-2 r txt-right'>" + fminerals + " g</span></div>" +
                    "<div class='fresult-row block'><span class='iblock grid-8 l'>Calcium</span><span class='iblock grid-2 r txt-right'>" + fcalcium + " mg</span></div>" +
                    "<div class='fresult-row block'><span class='iblock grid-8 l'>Iron</span><span class='iblock grid-2 r txt-right'>" + firon + " mg</span></div>" +
                    "<div class='fresult-row block'><span class='iblock grid-8 l'>Magnesium</span><span class='iblock grid-2 r txt-right'>" + fmagnesium + " mg</span></div>" +
                    "<div class='fresult-row block'><span class='iblock grid-8 l'>Potassium</span><span class='iblock grid-2 r txt-right'>" + fpotassium + " mg</span></div>" +
                    "<div class='fresult-row block'><span class='iblock grid-8 l'>Sodium</span><span class='iblock grid-2 r txt-right'>" + fsodium + " mg</span></div>" +
                    "<div class='fresult-row block'><span class='iblock grid-8 l'>Zinc</span><span class='iblock grid-2 r txt-right'>" + fzinc + " mg</span></div>" +
                    "<div class='fresult-row block'><span class='iblock grid-8 l'>Vitamin C</span><span class='iblock grid-2 r txt-right'>" + fvitc + " mg</span></div>" +
                    "<div class='fresult-row block'><span class='iblock grid-8 l'>Vitamin B6</span><span class='iblock grid-2 r txt-right'>" + fvitb6 + " mg</span></div>" +
                    "<div class='fresult-row block'><span class='iblock grid-8 l'>Vitamin E</span><span class='iblock grid-2 r txt-right'>" + fvite + " mg</span></div>";
            }
            else
            {
                Response.Redirect("/food");
            }
        }
    }
}