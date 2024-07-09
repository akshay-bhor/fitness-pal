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
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Is_loggedin())
            {
                //GET USERNAME
                string uname = Request.Cookies["fitness_user"].Value;

                sidebaruname.InnerHtml = "<span class=\"brand l\">" + uname + "</span><span class=\"close r\"><a id = \"menu-close\"> &times;</a></span>";

                sidebarnav.InnerHtml = "<ul class=\"sidebar-nav\"><li class=\"wave\"><a href=\"/\"><span class=\"glyphicon glyphicon-home\"></span> Home</a></li>" +
                    "<li class=\"wave\"><a href=\"/diary\"><span class=\"glyphicon glyphicon-book\"></span> Diary</a></li>" +
                    "<li class=\"wave\"><a href=\"/goal\"><span class=\"glyphicon glyphicon-eye-open\"></span> Goal</a></li>" +
                    "<li class=\"wave\"><a href=\"/nutrition\"><span class=\"glyphicon glyphicon-tasks\"></span> Nutrition</a></li>" +
                    "<li class=\"wave\"><a href=\"/progress\"><span class=\"glyphicon glyphicon-stats\"></span> Progress</a></li>" +
                    "<li class=\"wave\"><a href=\"/food\"><span class=\"glyphicon glyphicon-dashboard\"></span> Calorie Counter</a></li>" +
                    "<li class=\"wave\"><a href=\"/account\"><span class=\"glyphicon glyphicon-user\"></span> Account</a></li>" +
                    "<li class=\"wave\"><a href=\"/logout\"><span class=\"glyphicon glyphicon-log-out\"></span> Logout</a></li></ul>";
            }
            else {
                sidebaruname.InnerHtml = "<span class=\"brand l\">Fitness Pal</span><span class=\"close r\"><a id = \"menu-close\"> &times;</a></span>";
                sidebarnav.InnerHtml = "<ul class=\"sidebar-nav\"><li class=\"wave\"><a href=\"/login\"><span class=\"glyphicon glyphicon-log-in\"></span> Login</a></li>" +
                    "<li class=\"wave\"><a href=\"/register\"><span class=\"glyphicon glyphicon-user\"></span> Register</a></li></ul>";
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
        private string Getdb()
        {
            //CONNECTION STRING
            string connStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=fitness;Integrated Security=True;Pooling=False";
            return connStr;
        }
    }
}