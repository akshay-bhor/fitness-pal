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
    public partial class del_food : System.Web.UI.Page
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
                string date = Request.QueryString["date"];

                //DELETE CORRESPONDING ENTRY
                string conn = pc.Getdb();
                SqlConnection db = new SqlConnection(conn);
                db.Open();
                SqlCommand sql = new SqlCommand("DELETE TOP(1) FROM diary WHERE uid = @uid AND food_type = @ftype AND fid = @fid AND date = @date", db);
                sql.Parameters.AddWithValue("@uid", userid);
                sql.Parameters.AddWithValue("@ftype", type);
                sql.Parameters.AddWithValue("@fid", food_id);
                sql.Parameters.AddWithValue("@date", date);
                exe = sql.ExecuteNonQuery();
                db.Close();
                if(exe != 0)
                {
                    Response.Write("<script>alert(\"Entry deleted successfuly\");</script>");
                }
                else
                {
                    Response.Write("<script>alert(\"Error deleting entry, try again\");</script>");
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