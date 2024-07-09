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
    public partial class diary : System.Web.UI.Page
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
                cal_info_goal.InnerHtml = "0";
                cal_info_food.InnerHtml = "0";
                cal_info_rem.InnerHtml = "0";
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
                        day_head.InnerHtml = "Diary For - <span id='jqdate'>" + yesterday + "</span>";
                        dt_chosen = yesterday;
                    }
                    if (n_day == "Next")
                    {
                        day_head.InnerHtml = "Diary For - <span id='jqdate'>" + tomorrow + "</span>";
                        dt_chosen = tomorrow;
                    }
                }
                else
                {
                    string now = today.ToString("yyyy-MM-dd");
                    day_head.InnerHtml = "Diary For - <span id='jqdate'>" + now + "</span>";
                    dt_chosen = now;
                }

                //GET DIARY INFO OF USER
                //CHECK FOR DATE ENTRY
                int count = 0;
                string conn = pc.Getdb();
                SqlConnection db = new SqlConnection(conn);
                db.Open();
                SqlCommand sql = new SqlCommand("SELECT COUNT(id) as c FROM diary WHERE uid = @uid AND date = @date", db);
                sql.Parameters.AddWithValue("@uid", userid);
                sql.Parameters.AddWithValue("@date", dt_chosen);
                SqlDataReader result = sql.ExecuteReader();
                if(result.Read()) {
                    count = (int)result["c"];
                }
                db.Close();

                if(count > 0)
                {
                    //GET GOAL CALORIES FOR USER
                    int cal_need = Cal_need(userid);

                    //GET EATEN CALORIES
                    db.Open();
                    SqlCommand ecsql = new SqlCommand("SELECT SUM(calories) AS fcal FROM diary WHERE uid = @uid AND date = @date", db);
                    ecsql.Parameters.AddWithValue("@uid", userid);
                    ecsql.Parameters.AddWithValue("@date", dt_chosen);
                    SqlDataReader fcalresult = ecsql.ExecuteReader();
                    fcalresult.Read();
                    int fcal_taken = (int)fcalresult["fcal"];
                    int cal_rem = (cal_need - fcal_taken);
                    cal_info_goal.InnerHtml = cal_need.ToString();
                    cal_info_food.InnerHtml = fcal_taken.ToString();
                    cal_info_rem.InnerHtml = cal_rem.ToString();

                    if(fcal_taken > cal_need)
                    {
                        info.InnerHtml = "<div class='alert alert-success alert-dismissible'>" +
                            "<a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a>" +
                            "<strong>Congratulations!</strong> You have completed your daily goal." +
                            "</div>";
                    }
                    else
                    {
                        info.InnerHtml = "";
                    }

                    //GET TOTAL CAL EATEN PER TYPE
                    int bftcal = Get_cal(userid, dt_chosen, 1);
                    int lftcal = Get_cal(userid, dt_chosen, 2);
                    int dftcal = Get_cal(userid, dt_chosen, 3);
                    int sftcal = Get_cal(userid, dt_chosen, 4);

                    //ASSIGN CALORIES FOR EACH FOOD TYPE
                    btcal.InnerHtml = bftcal.ToString();
                    ltcal.InnerHtml = lftcal.ToString();
                    dtcal.InnerHtml = dftcal.ToString();
                    stcal.InnerHtml = sftcal.ToString();

                    //GET EATEN FOOD INFO PER TYPE
                    string btmp = Get_food(userid, dt_chosen, 1);
                    string ltmp = Get_food(userid, dt_chosen, 2);
                    string dtmp = Get_food(userid, dt_chosen, 3);
                    string stmp = Get_food(userid, dt_chosen, 4);

                    if(btmp != "0")
                    {
                        bfcontent.InnerHtml = Get_food(userid, dt_chosen, 1);
                    }
                    else
                    {
                        bfcontent.InnerHtml = "";
                    }
                    if (ltmp != "0")
                    {
                        lfcontent.InnerHtml = Get_food(userid, dt_chosen, 2);
                    }
                    else
                    {
                        lfcontent.InnerHtml = "";
                    }
                    if (dtmp != "0")
                    {
                        dfcontent.InnerHtml = Get_food(userid, dt_chosen, 3);
                    }
                    else
                    {
                        dfcontent.InnerHtml = "";
                    }
                    if (stmp != "0")
                    {
                        sfcontent.InnerHtml = Get_food(userid, dt_chosen, 4);
                    }
                    else
                    {
                        sfcontent.InnerHtml = "";
                    }
                    
                }
                else
                {
                    int cal_need = Cal_need(userid);
                    int cal_rem = (cal_need - 0);
                    cal_info_goal.InnerHtml = cal_need.ToString();
                    cal_info_food.InnerHtml = "0";
                    cal_info_rem.InnerHtml = cal_rem.ToString();

                    //ASSIGN CALORIES FOR EACH FOOD TYPE
                    btcal.InnerHtml = "0";
                    ltcal.InnerHtml = "0";
                    dtcal.InnerHtml = "0";
                    stcal.InnerHtml = "0";

                    //CLEAR OUT OLDER CONTENT
                    bfcontent.InnerHtml = "";
                    lfcontent.InnerHtml = "";
                    dfcontent.InnerHtml = "";
                    sfcontent.InnerHtml = "";

                    //CLEAR INFO
                    info.InnerHtml = "";
                }
            }
        }
        public int Cal_need(int uid)
        {
            //GET DB CONNECTION STRING
            pub_class pc = new pub_class();
            string conn = pc.Getdb();
            SqlConnection db = new SqlConnection(conn);

            //GET GOAL CALORIES FOR USER
            db.Open();
            SqlCommand gsql = new SqlCommand("SELECT calorie_need FROM goals WHERE uid = @uid", db);
            gsql.Parameters.AddWithValue("@uid", uid);
            SqlDataReader gresult = gsql.ExecuteReader();
            gresult.Read();
            int cal_need = (int)gresult["calorie_need"];
            db.Close();
            return cal_need;
        }
        public int Get_cal(int uid, string date, int type)
        {
            //GET DB CONNECTION STRING
            pub_class pc = new pub_class();
            string conn = pc.Getdb();
            SqlConnection db = new SqlConnection(conn);

            //GET CALORIES FOR FOOD TYPE
            db.Open();
            SqlCommand sql = new SqlCommand("SELECT COUNT(id) AS c, SUM(calories) AS tcal FROM diary WHERE uid = @uid AND date = @date AND food_type = @type", db);
            sql.Parameters.AddWithValue("@uid", uid);
            sql.Parameters.AddWithValue("@date", date);
            sql.Parameters.AddWithValue("@type", type);
            SqlDataReader result = sql.ExecuteReader();
            while(result.Read())
            {
                int c = (int)result["c"];
                if (c > 0)
                {
                    int tcal = (int)result["tcal"];
                    db.Close();
                    return tcal;
                }
                else
                {
                    return 0;
                }
            }
            db.Close();
            return 0;
        }
        public string Get_food(int uid, string date, int type)
        {
            //GET DB CONNECTION STRING
            pub_class pc = new pub_class();
            string conn = pc.Getdb();
            SqlConnection db = new SqlConnection(conn);

            //SET RETURN STRING
            string r = null;

            //GET COUNT
            db.Open();
            SqlCommand asql = new SqlCommand("SELECT COUNT(id) as c FROM diary WHERE uid = @uid AND date = @date AND food_type = @type", db);
            asql.Parameters.AddWithValue("@uid", uid);
            asql.Parameters.AddWithValue("@date", date);
            asql.Parameters.AddWithValue("@type", type);
            SqlDataReader aresult = asql.ExecuteReader();
            aresult.Read();
            int c = (int)aresult["c"];
            db.Close();

            //GET FOOD
            db.Open();
            SqlCommand sql = new SqlCommand("SELECT fid, food_name, unit, calories, servings FROM diary WHERE uid = @uid AND date = @date AND food_type = @type", db);
            sql.Parameters.AddWithValue("@uid", uid);
            sql.Parameters.AddWithValue("@date", date);
            sql.Parameters.AddWithValue("@type", type);
            SqlDataReader result = sql.ExecuteReader();
            while(result.Read())
            {
                if(c > 0)
                {
                    int fid = (int)result["fid"];
                    string fname = (string)result["food_name"];
                    string unit = (string)result["unit"];
                    int cal = (int)result["calories"];
                    int serv = (int)result["servings"];
                    r += "<div class='block fresult-row bold'>" +
                        "<div class='block'><div class='grid-8 block l tblue'><a href='/calories?food_id=" + fid + "' target='_blank'>" + fname + "</a></div><div class='grid-2 block r text-right'>" + cal + "</div></div>" +
                        "<div class='block'><div class='grid-8 block l'>" + unit + " (" + serv + ")</div><div class='grid-2 block r text-right'><span class='glyphicon glyphicon-trash small delete-c pointer' onclick='delfood(" + fid + "," + type + ");'></span></div></div>" +
                        "</div>";
                }
            }
            db.Close();
            if(string.IsNullOrEmpty(r))
            {
                r = "0";
            }
            return r;
        }
    }
}