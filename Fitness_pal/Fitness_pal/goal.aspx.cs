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
    public partial class goal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //CHECK FOR LOGIN
            if (!Is_loggedin())
            {
                Response.Redirect("/login");
            }
            if (IsPostBack) {
                //GET POSTED DATA
                int wt = Int32.Parse(htmlwt.Value);
                int ht = Int32.Parse(htmlht.Value);
                int age = Int32.Parse(htmlage.Value);
                int gender = Int32.Parse(htmlgender.Value);
                int act_lvl = Int32.Parse(htmlactlvl.Value);
                int goal = Int32.Parse(htmlgoal.Value);


                //GET USERID
                //GET COOKIES VALUE
                string user = Request.Cookies["fitness_user"].Value;
                string pass = Request.Cookies["fitness_pass"].Value;

                string conn = Getdb();
                SqlConnection db = new SqlConnection(conn);
                db.Open();
                SqlCommand sql = new SqlCommand("SELECT id, goal FROM users WHERE username  = @user AND pass = @pass", db);
                sql.Parameters.AddWithValue("@user", user);
                sql.Parameters.AddWithValue("@pass", pass);
                SqlDataReader result = sql.ExecuteReader();
                result.Read();
                int userid = (int)result[0];
                int is_goal_set = (int)result[1];
                db.Close();

                //CHECK IF GOAL IS SET
                if (is_goal_set == 1)
                {
                    //THEN UPDATE GOAL
                    bool update = Update_goal(userid, wt, ht, age, gender, act_lvl, goal, 1);
                    if(update)
                    {
                        err.InnerHtml = "<script>alert(\"Goal Updated Successfully.\");var delay = 200;" + 
                            "setTimeout(function(){ window.location = \"/goal\"; }, delay);</script>";
                    }
                    else
                    {
                        err.InnerHtml = "<script>alert(\"Error Updating Goal.\");var delay = 200;" + 
                            "setTimeout(function(){ window.location = \"/goal\"; }, delay);</script>";
                    }
                }
                else {
                    //INSERT GOAL
                    bool update = Update_goal(userid, wt, ht, age, gender, act_lvl, goal, 0);
                    if (update)
                    {
                        err.InnerHtml = "<script>alert(\"Goal Set Successfully.\");var delay = 200;" +
                            "setTimeout(function(){ window.location = \"/goal\"; }, delay);</script>";
                    }
                    else
                    {
                        err.InnerHtml = "<script>alert(\"Error Setting Goal.\");var delay = 200;" +
                            "setTimeout(function(){ window.location = \"/goal\"; }, delay);</script>";
                    }
                }

            }
            else
            {
                //CHECK IF GOAL IS SETUP

                //GET COOKIES VALUE
                string user = Request.Cookies["fitness_user"].Value;
                string pass = Request.Cookies["fitness_pass"].Value;

                string conn = Getdb();
                SqlConnection db = new SqlConnection(conn);
                db.Open();
                SqlCommand sql = new SqlCommand("SELECT id, goal FROM users WHERE username  = @user AND pass = @pass", db);
                sql.Parameters.AddWithValue("@user", user);
                sql.Parameters.AddWithValue("@pass", pass);
                SqlDataReader result = sql.ExecuteReader();
                result.Read();
                int userid = (int)result[0];
                int goal = (int)result[1];
                db.Close();
                if (goal == 0) {
                    err.InnerHtml = "<div class='alert alert-danger'>You have not set any goal, set goal now.</div>";
                    sbtn.Value = "SAVE";
                }
                else
                {
                    //FETCH GOAL DATA FROM DATABASE
                    db.Open();
                    SqlCommand gsql = new SqlCommand("SELECT wt, ht, age, gender, goal, bf, bmr, act_lvl, calorie_need FROM goals WHERE uid = @userid", db);
                    gsql.Parameters.AddWithValue("@userid", userid);
                    SqlDataReader gresult = gsql.ExecuteReader();
                    gresult.Read();
                    int uwt = (int)gresult[0];
                    int uht = (int)gresult[1];
                    int uage = (int)gresult[2];
                    int ugender = (int)gresult[3];
                    int ugoal = (int)gresult[4];
                    int ubmi = (int)gresult[5];
                    int ubmr = (int)gresult[6];
                    int uact_lvl = (int)gresult[7];
                    int ucal_need = (int)gresult[8];

                    //SET GOAL VALUES
                    string usr_goal = null;
                    if (ugoal == 1)
                    {
                        usr_goal = "Gain weight (0.5 kg/w)";
                    }
                    if (ugoal == 2)
                    {
                        usr_goal = "Keep Weight";
                    }
                    if (ugoal == 3)
                    {
                        usr_goal = "Lose Weight (0.5 kg/w)";
                    }

                    //ADD VALUES TO THE INPUT BOXES
                    htmlwt.Value = uwt.ToString();
                    htmlht.Value = uht.ToString();
                    htmlgender.Value = ugender.ToString();
                    htmlage.Value = uage.ToString();
                    htmlactlvl.Value = uact_lvl.ToString();
                    htmlgoal.Value = ugoal.ToString();
                    sbtn.Value = "EDIT";

                    //SHOW FITNESS INFO
                    ginfo.InnerHtml = "<div class='list-group card'><div class='list-group-item block'>" +
                        "<h2 class='tpad'>Goal Info</h2>" + 
                        "<div class='l'><b>BMI: </b></div><div class='r'>" + ubmi + "</div></div>" +
                        "<div class='list-group-item block'><div class='l'><b>BMR: (Basal Metabolic Rate)</b></div><div class='r'>" + ubmr + " Kcal</div></div>" +
                        "<div class='list-group-item block'><div class='l'><b>Goal: </b></div><div class='r'>" + usr_goal + "</div></div>" +
                        "<div class='list-group-item block'><div class='l'><b>Daily Calorie Needs: </b></div><div class='r'>" + ucal_need + " Kcal</div></div></div>";
                }
            }
        }
        private bool Is_loggedin()
        {
            //CHECK FOR COOKIES
            var user_c = Request.Cookies["fitness_user"];
            var pass_c = Request.Cookies["fitness_pass"];
            if (user_c != null && pass_c != null)
            {
                //GET COOKIES VALUE
                string user = Request.Cookies["fitness_user"].Value;
                string pass = Request.Cookies["fitness_pass"].Value;

                if (user != null && pass != null)
                {
                    String conn = Getdb();
                    SqlConnection db = new SqlConnection(conn);
                    db.Open();

                    SqlCommand sql = new SqlCommand("SELECT COUNT(id) FROM users WHERE username = @user AND pass = @pass AND status = 1", db);
                    sql.Parameters.AddWithValue("@user", user);
                    sql.Parameters.AddWithValue("@pass", pass);
                    SqlDataReader result = sql.ExecuteReader();
                    result.Read();
                    int count = (int)result[0];
                    if (count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        private bool Update_goal(int userid, int wt, int ht, int age, int gender, int act_lvl, int goal, int update) {

            //CALCULATE BODY FAT
            int bf = (int)(((float)wt / (float)ht / (float)ht) * 10000);

            //GET LEAN FACTOR MULTIPLIER
            string conn = Getdb();
            SqlConnection db = new SqlConnection(conn);
            db.Open();
            SqlCommand sql = new SqlCommand("SELECT multiplier FROM lean_factor WHERE bf_min <= @bf AND bf_max >= @bf AND gender = @gender", db);
            sql.Parameters.AddWithValue("@bf", bf);
            sql.Parameters.AddWithValue("@gender", gender);
            SqlDataReader result = sql.ExecuteReader();
            result.Read();
            var lfmultiplier = (double)result[0];
            db.Close();
            
            //GET ACTIVITY LEVEL MULTIPLIER
            db.Open();
            SqlCommand asql = new SqlCommand("SELECT multiplier FROM act_lvl WHERE id = @act_lvl", db);
            asql.Parameters.AddWithValue("@act_lvl", act_lvl);
            SqlDataReader aresult = asql.ExecuteReader();
            aresult.Read();
            var almultiplier = (double)aresult[0];
            db.Close();

            float cal = 0;
            if (gender == 1) {
                cal = (float)wt;
            }
            else {
                cal = (float)wt * 0.9f;
            }
            
            //MULTIPLY BY 24
            cal = (float)cal * 24f;
            //MULTIPLY BY LEAN FACTOR
            cal = (float)cal * (float)lfmultiplier;
            //GOT BASAL METABOLIC RATE
            int bmr = (int)cal;
            //MULTIPLY BY ACTIVITY LEVEL MULTIPLIER
            cal = (float)cal * (float)almultiplier;
            //GOT DAILY CALORIE  NEEDS
            //CONVERT IT TO INT
            int cal_need = (int)cal;

            //UPDATE CALORIE NEED ACCORDING TO GOAL
            if(goal == 1)
            {
                cal_need = cal_need + 500;
            }
            if(goal == 2) { }
            if(goal == 3)
            {
                cal_need = cal_need - 500;
            }

            //SET EXEXUTION FLAG
            int exe = 0;

            if (update == 1)
            {
                //UPDATE YOUR GOAL
                db.Open();
                SqlCommand upsql = new SqlCommand("UPDATE goals SET wt = @wt, ht = @ht, age = @age, bf = @bf, bmr = @bmr, gender = @gender, goal = @goal, act_lvl = @act_lvl, calorie_need = @cal_need WHERE uid = @uid", db);
                upsql.Parameters.AddWithValue("@wt", wt);
                upsql.Parameters.AddWithValue("@ht", ht);
                upsql.Parameters.AddWithValue("@age", age);
                upsql.Parameters.AddWithValue("@bf", bf);
                upsql.Parameters.AddWithValue("@bmr", bmr);
                upsql.Parameters.AddWithValue("@gender", gender);
                upsql.Parameters.AddWithValue("@goal", goal);
                upsql.Parameters.AddWithValue("@act_lvl", act_lvl);
                upsql.Parameters.AddWithValue("@cal_need", cal_need);
                upsql.Parameters.AddWithValue("@uid", userid);
                exe = upsql.ExecuteNonQuery();
                db.Close();
            }
            if (update == 0) {
                db.Open();
                SqlCommand insql = new SqlCommand("INSERT INTO goals(wt, ht, age, bf, bmr, gender, goal, act_lvl, calorie_need, uid) VALUES (@wt, @ht, @age, @bf, @bmr, @gender, @goal, @act_lvl, @cal_need, @uid)", db);
                insql.Parameters.AddWithValue("@wt", wt);
                insql.Parameters.AddWithValue("@ht", ht);
                insql.Parameters.AddWithValue("@age", age);
                insql.Parameters.AddWithValue("@bf", bf);
                insql.Parameters.AddWithValue("@bmr", bmr);
                insql.Parameters.AddWithValue("@gender", gender);
                insql.Parameters.AddWithValue("@goal", goal);
                insql.Parameters.AddWithValue("@act_lvl", act_lvl);
                insql.Parameters.AddWithValue("@cal_need", cal_need);
                insql.Parameters.AddWithValue("@uid", userid);
                exe = insql.ExecuteNonQuery();
                //SET GOAL TRUE IN USER TABLE
                SqlCommand upsql = new SqlCommand("UPDATE users SET goal = 1 WHERE id = @userid", db);
                upsql.Parameters.AddWithValue("@userid", userid);
                upsql.ExecuteNonQuery();
                db.Close();
            }

            if (exe != 0) {
                bool upwt = Up_wt(userid, wt);
                if (upwt) {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        private bool Up_wt(int userid, int wt) {

            DateTime today = DateTime.Today;

            //CHECK FOR TODAY'S ENTRY
            string conn = Getdb();
            SqlConnection db = new SqlConnection(conn);
            db.Open();
            SqlCommand check = new SqlCommand("SELECT COUNT(id) as c FROM usr_wt WHERE uid = @uid AND date = @today", db);
            check.Parameters.AddWithValue("@uid", userid);
            check.Parameters.AddWithValue("@today", today);
            SqlDataReader result = check.ExecuteReader();
            result.Read();
            int exist = (int)result[0];
            db.Close();

            //SET EXECUTION FLAG
            int exe = 0;

            if(exist != 0)
            {
                //UPDATE
                db.Open();
                SqlCommand up = new SqlCommand("UPDATE usr_wt SET wt = @wt WHERE uid = @uid AND date = @today", db);
                up.Parameters.AddWithValue("@wt", wt);
                up.Parameters.AddWithValue("@uid", userid);
                up.Parameters.AddWithValue("@today", today);
                exe = up.ExecuteNonQuery();
                db.Close();
                if(exe != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                //INSERT
                db.Open();
                SqlCommand ins = new SqlCommand("INSERT INTO usr_wt (uid, wt, date) VALUES (@uid, @wt, @date)", db);
                ins.Parameters.AddWithValue("@uid", userid);
                ins.Parameters.AddWithValue("@wt", wt);
                ins.Parameters.AddWithValue("@date", today);
                exe = ins.ExecuteNonQuery();
                db.Close();
                if (exe != 0) {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        private string GetMD5HashData(string data)
        {
            //create new instance of md5
            MD5 md5 = MD5.Create();

            //convert the input text to array of bytes
            byte[] hashData = md5.ComputeHash(Encoding.Default.GetBytes(data));

            //create new instance of StringBuilder to save hashed data
            StringBuilder returnValue = new StringBuilder();

            //loop for each byte and add it to StringBuilder
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }

            // return hexadecimal string
            return returnValue.ToString();

        }
        private string Getdb()
        {
            //CONNECTION STRING
            string connStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=fitness;Integrated Security=True;Pooling=False";
            return connStr;
        }
    }
}