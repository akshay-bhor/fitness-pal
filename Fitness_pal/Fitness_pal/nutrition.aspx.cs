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
    public partial class nutrition : System.Web.UI.Page
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

            //CHECK FOR LOGIN
            if (!pc.Is_loggedin(user, pass))
            {
                Response.Redirect("/login");
            }

            //GET USERID
            userid = pc.Get_userid(user, pass);

            //CHECK IF GOAL IS SET
            if (!pc.Is_goalset(userid))
            {
                err.InnerHtml = "<div class='alert alert-danger'>You have not set any goal, <a href='/goal'>SET NOW</a></div>";
            }
            else
            {
                string dt_chosen = Request.Form["date_chosen"];
                if (string.IsNullOrEmpty(dt_chosen))
                {
                    dt_chosen = DateTime.Now.ToString("yyyy-MM-dd");
                }
                string p_day = Request.Form["prv"];
                string n_day = Request.Form["nxt"];
                var today = DateTime.Parse(dt_chosen);
                if (!string.IsNullOrEmpty(p_day) || !string.IsNullOrEmpty(n_day))
                {
                    string tomorrow = today.AddDays(1).ToString("yyyy-MM-dd");
                    string yesterday = today.AddDays(-1).ToString("yyyy-MM-dd");
                    if (p_day == "Prev")
                    {
                        day_head.InnerHtml = "Nutrition For - <span id='jqdate'>" + yesterday + "</span>";
                        dt_chosen = yesterday;
                    }
                    if (n_day == "Next")
                    {
                        day_head.InnerHtml = "Nutrition For - <span id='jqdate'>" + tomorrow + "</span>";
                        dt_chosen = tomorrow;
                    }
                }
                else
                {
                    string now = today.ToString("yyyy-MM-dd");
                    day_head.InnerHtml = "Nutrition Info For - <span id='jqdate'>" + now + "</span>";
                    dt_chosen = now;
                }

                //CLEAR PREVIOUS RECORDS
                nutri_bar.InnerHtml = "";

                //GET GOAL INFO FROM USER GOALS
                //SETUP DATABASE CONNECTION
                string conn = pc.Getdb();
                SqlConnection db = new SqlConnection(conn);
                db.Open();
                SqlCommand sql = new SqlCommand("SELECT wt, gender, calorie_need FROM goals WHERE uid = @uid", db);
                sql.Parameters.AddWithValue("@uid", userid);
                SqlDataReader result = sql.ExecuteReader();
                result.Read();
                int wt = (int)result["wt"];
                int gender = (int)result["gender"];
                int cal_need = (int)result["calorie_need"];
                db.Close();

                //SET INITIAL VALUE OF PRIMARY NUTRIENTS
                int tcal = 0;
                int tpro = 0;
                int tcarbs = 0;

                //SET INITIAL VALUE OF SUMS
                int tfiber = 0;
                int tsugar = 0;
                int tsodium = 0;
                int tpotassium = 0;
                int tcalcium = 0;
                int tiron = 0;
                int tvitc = 0;

                //CHECK FOR THAT DAYS ENTRY
                if (Entry_exist(dt_chosen, userid))
                {
                    //GET TOTAL CAL, PROTEIN, AND CARBS
                    db.Open();
                    SqlCommand asql = new SqlCommand("SELECT SUM(calories) AS tcal, SUM(protein) AS tpro, SUM(carbs) AS tcarbs FROM diary WHERE uid = @uid AND date = @date", db);
                    asql.Parameters.AddWithValue("@uid", userid);
                    asql.Parameters.AddWithValue("@date", dt_chosen);
                    SqlDataReader aresult = asql.ExecuteReader();
                    if (aresult.Read())
                    {
                        tcal = (int)aresult["tcal"];
                        tpro = (int)(decimal)aresult["tpro"];
                        tcarbs = (int)(decimal)aresult["tcarbs"];
                    }
                    db.Close();

                    //SET INITIAL VALUES FOR FID AND SERVINGS

                    //GET REMAINING FOOD NUTRITION
                    db.Open();
                    SqlCommand bsql = new SqlCommand("SELECT fid, servings FROM diary WHERE uid = @uid AND date = @date", db);
                    bsql.Parameters.AddWithValue("@uid", userid);
                    bsql.Parameters.AddWithValue("@date", dt_chosen);
                    SqlDataReader bresult = bsql.ExecuteReader();
                    while (bresult.Read())
                    {
                        int fid = (int)bresult["fid"];
                        int serv = (int)bresult["servings"];

                        //GET NUTRITION VALUE FOR THAT FOOD ID
                        SqlCommand csql = new SqlCommand("SELECT [fiber_(g)], [sugar_(g)], [sodium_(mg)], [potassium_(mg)], [calcium_(mg)], [iron_(mg)] FROM food_nutrition WHERE id = @fid", db);
                        csql.Parameters.AddWithValue("@fid", fid);
                        SqlDataReader cres = csql.ExecuteReader();
                        cres.Read();
                        double fiber = Convert.ToDouble(cres["fiber_(g)"]);
                        double sugar = Convert.ToDouble(cres["sugar_(g)"]);
                        double sodium = Convert.ToDouble(cres["sodium_(mg)"]);
                        double potassium = Convert.ToDouble(cres["potassium_(mg)"]);
                        double calcium = Convert.ToDouble(cres["calcium_(mg)"]);
                        double iron = Convert.ToDouble(cres["iron_(mg)"]);

                        tfiber += Gtot(fiber, serv);
                        tsugar += Gtot(sugar, serv);
                        tsodium += Gtot(sodium, serv);
                        tpotassium += Gtot(potassium, serv);
                        tcalcium += Gtot(calcium, serv);
                        tiron += Gtot(iron, serv);
                    }
                    db.Close();
                }

                //CODE FOR NUTRITION PROGRESS BAR
                //CALORIES
                int cal_perc = Get_perc(cal_need, tcal);
                int cal_left = cal_need - tcal;
                nutri_bar.InnerHtml += "<div class='block fresult-row'><div class='block'><div class='grid-4 l'>Calories</div><div class='grid-2 l text-right'>" + tcal + "</div><div class='grid-2 l text-right'>" + cal_need + "</div><div class='grid-2 l text-right'>" + cal_left + " Kcal</div></div>" +
                    "<div class='block tpad-5'><div class='progress'>" +
                    "<div class='progress-bar progress-bar-striped progress-bar-success active' role='progressbar' aria-valuenow='" + cal_perc + "' aria-valuemin='0' aria-valuemax='100' style='width:" + cal_perc + "%'>" + 
                        cal_perc + "%" +
                     "</div>" +
                    "</div></div></div>";
                //PROTEIN
                int prot_need = (int)((float)wt * 2.2f);
                int prot_perc = Get_perc(prot_need, tpro);
                int prot_left = (prot_need - tpro);
                nutri_bar.InnerHtml += "<div class='block fresult-row'><div class='block'><div class='grid-4 l'>Protein</div><div class='grid-2 l text-right'>" + tpro + "</div><div class='grid-2 l text-right'>" + prot_need + "</div><div class='grid-2 l text-right'>" + prot_left + " g</div></div>" +
                    "<div class='block tpad-5'><div class='progress'>" +
                    "<div class='progress-bar progress-bar-striped progress-bar-success active' role='progressbar' aria-valuenow='" + prot_perc + "' aria-valuemin='0' aria-valuemax='100' style='width:" + prot_perc + "%'>" +
                        prot_perc + "%" +
                     "</div>" +
                    "</div></div></div>";
                //CARBS
                int carb_need = (int)((((float)cal_need / 4f) / 100f) * 50);
                int carb_perc = Get_perc(carb_need, tcarbs);
                int carb_left = (carb_need - tcarbs);
                nutri_bar.InnerHtml += "<div class='block fresult-row'><div class='block'><div class='grid-4 l'>Carbohydrates</div><div class='grid-2 l text-right'>" + tcarbs + "</div><div class='grid-2 l text-right'>" + carb_need + "</div><div class='grid-2 l text-right'>" + carb_left + " g</div></div>" +
                   "<div class='block tpad-5'><div class='progress'>" +
                   "<div class='progress-bar progress-bar-striped progress-bar-success active' role='progressbar' aria-valuenow='" + carb_perc + "' aria-valuemin='0' aria-valuemax='100' style='width:" + carb_perc + "%'>" +
                       carb_perc + "%" +
                    "</div>" +
                   "</div></div></div>";
                //FIBER
                int fneed = 0;
                if(gender == 1)
                {
                    fneed = 38;
                }
                else
                {
                    fneed = 25;
                }
                int fleft = (fneed - tfiber);
                nutri_bar.InnerHtml += "<div class='block fresult-row'>" +
                    "<div class='block'>" +
                    "<div class='grid-4 l'>Fiber</div><div class='grid-2 l text-right'>" + tfiber + "</div><div class='grid-2 l text-right'>" + fneed +"</div><div class='grid-2 l text-right'>" + fleft +" g</div>" +
                    "</div>" + 
                    "</div>";
                //SUGAR
                int sneed = 0;
                if (gender == 1)
                {
                    sneed = 95;
                }
                else
                {
                    sneed = 80;
                }
                int sleft = (sneed - tsugar);
                nutri_bar.InnerHtml += "<div class='block fresult-row'>" +
                    "<div class='block'>" +
                    "<div class='grid-4 l'>Sugar</div><div class='grid-2 l text-right'>" + tsugar + "</div><div class='grid-2 l text-right'>" + sneed + "</div><div class='grid-2 l text-right'>" + sleft + " g</div>" +
                    "</div>" +
                    "</div>";
                //SODIUM
                int sod_need = 2300;
                int sod_left = (sod_need - tsodium);
                nutri_bar.InnerHtml += "<div class='block fresult-row'>" +
                    "<div class='block'>" +
                    "<div class='grid-4 l'>Sodium</div><div class='grid-2 l text-right'>" + tsodium + "</div><div class='grid-2 l text-right'>" + sod_need + "</div><div class='grid-2 l text-right'>" + sod_left + " mg</div>" +
                    "</div>" +
                    "</div>";
                //POTASSIUM
                int pot_need = 3500;
                int pot_left = (pot_need - tpotassium);
                nutri_bar.InnerHtml += "<div class='block fresult-row'>" +
                    "<div class='block'>" +
                    "<div class='grid-4 l'>Potassium</div><div class='grid-2 l text-right'>" + tpotassium + "</div><div class='grid-2 l text-right'>" + pot_need + "</div><div class='grid-2 l text-right'>" + pot_left + " mg</div>" +
                    "</div>" +
                    "</div>";
                //CALCIUM
                int calcium_need = 1000;
                int calcium_left = (calcium_need - tcalcium);
                nutri_bar.InnerHtml += "<div class='block fresult-row'>" +
                    "<div class='block'>" +
                    "<div class='grid-4 l'>Calcium</div><div class='grid-2 l text-right'>" + tcalcium + "</div><div class='grid-2 l text-right'>" + calcium_need + "</div><div class='grid-2 l text-right'>" + calcium_left + " mg</div>" +
                    "</div>" +
                    "</div>";
                //IRON
                int iron_need = 0;
                if (gender == 1)
                {
                    iron_need = 10;
                }
                else
                {
                    iron_need = 15;
                }
                int iron_left = (iron_need - tiron);
                nutri_bar.InnerHtml += "<div class='block fresult-row'>" +
                    "<div class='block'>" +
                    "<div class='grid-4 l'>Iron</div><div class='grid-2 l text-right'>" + tiron + "</div><div class='grid-2 l text-right'>" + iron_need + "</div><div class='grid-2 l text-right'>" + iron_left + " mg</div>" +
                    "</div>" +
                    "</div>";

            }
        }
        public int Gtot(double amt, int serv)
        {
            int tot = (int)((double)amt * (int)serv);
            return tot;
        }
        public bool Entry_exist(string date, int uid)
        {
            pub_class pc = new pub_class();
            string conn = pc.Getdb();
            SqlConnection db = new SqlConnection(conn);
            db.Open();
            SqlCommand sql = new SqlCommand("SELECT COUNT(id) AS c FROM diary WHERE date = @date AND uid = @uid", db);
            sql.Parameters.AddWithValue("@date", date);
            sql.Parameters.AddWithValue("@uid", uid);
            SqlDataReader res = sql.ExecuteReader();
            res.Read();
            int c = (int)res["c"];
            if(c > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int Get_perc(int total, int now)
        {
            if(now >= total)
            {
                return 100;
            }
            else
            {
                int perc = (int)(((float)now / (float)total) * 100);
                return perc;
            }
        }
    }
}