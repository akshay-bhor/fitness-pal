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
    public partial class account : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //CHECK FOR LOGIN
            if (!Is_loggedin())
            {
                Response.Redirect("/login");
            }

            //CHECK IF GOAL IS SETUP

            //GET COOKIES VALUE
            string user = Request.Cookies["fitness_user"].Value;
            string pass = Request.Cookies["fitness_pass"].Value;

            string conn = Getdb();
            SqlConnection db = new SqlConnection(conn);
            db.Open();
            SqlCommand sql = new SqlCommand("SELECT id, username, mail, goal, reg FROM users WHERE username  = @user AND pass = @pass", db);
            sql.Parameters.AddWithValue("@user", user);
            sql.Parameters.AddWithValue("@pass", pass);
            SqlDataReader result = sql.ExecuteReader();
            result.Read();
            int userid = (int)result["id"];
            int goal = (int)result["goal"];
            string username = (string)result["username"];
            string mail = (string)result["mail"];
            string regon = (string)result["reg"];
            db.Close();

            //CHANGE PASSWORD SCRIPT
            if(IsPostBack)
            {
                string form_pass = Request.Form["pass"];
                string form_rpass = Request.Form["rpass"];
                //CHECK IF BOTH STRINGS ARE EQUAL 
                if(form_pass == form_rpass)
                {
                    //GET MD5 HASH
                    string pass_hash = GetMD5HashData(form_pass);

                    //UPDATE PASSWORD
                    db.Open();
                    SqlCommand csql = new SqlCommand("UPDATE users SET pass = @pass WHERE id = @uid" ,db);
                    csql.Parameters.AddWithValue("@pass", pass_hash);
                    csql.Parameters.AddWithValue("@uid", userid);
                    int exe = csql.ExecuteNonQuery();
                    db.Close();
                    if(exe != 0)
                    {
                        //LOGOUT USER
                        err.InnerHtml = "<script>alert(\"Password Changed Successfully\");window.location = \"/logout\"</script>";
                    }
                    else
                    {
                        err.InnerHtml = "<div class='alert alert-danger'>Error Updating Password, Try Again.</div>";
                    }
                }
                else
                {
                    err.InnerHtml = "<div class='alert alert-danger'>Passwords Do Not Match, Try Again.</div>";
                }
            }

            //SET GLOBAL CONTEXT STRING
            string usr_goal = null;
            int ucal_need = 0;

            if (goal == 0)
            {
                err.InnerHtml = "<div class='alert alert-danger'>You have not set any goal, <a href='/goal'>set goal</a> now.</div>";
            }
            else
            {
                //FETCH GOAL DATA FROM DATABASE
                db.Open();
                SqlCommand gsql = new SqlCommand("SELECT goal, calorie_need FROM goals WHERE uid = @userid", db);
                gsql.Parameters.AddWithValue("@userid", userid);
                SqlDataReader gresult = gsql.ExecuteReader();
                gresult.Read();
                int ugoal = (int)gresult["goal"];
                ucal_need = (int)gresult["calorie_need"];
                db.Close();

                //SET GOAL VALUES
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
            }

            if(usr_goal == null)
            {
                usr_goal = "<span class='tred'>Not Set</span>";
            }

            httpunmaeinfo.InnerHtml = "<span class='tblue'>" + username + "</span>";
            httpusreg.InnerHtml = "<b class='tblue'>Reg on: </b><small>" + regon + "</small>";
            httpusrgoal.InnerHtml = "<b class='tblue'>Goal: </b><small>" + usr_goal + "</small>";
            httpuname.InnerHtml = username;
            httpemail.InnerHtml = mail;
            httpgoal.InnerHtml = usr_goal;
            httpcalneed.InnerHtml = ucal_need.ToString() + " Kcal";
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