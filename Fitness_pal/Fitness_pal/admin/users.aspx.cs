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
    public partial class users : System.Web.UI.Page
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

            //CHECK FOR GET REQUEST
            string ban = Request.QueryString["ban"];
            string unban = Request.QueryString["unban"];
            
            if(ban != null) {
                int ban_uid = Convert.ToInt32(ban);
                if(!Isuid_admin(ban_uid))
                {
                    //BAN
                    int exe = 0;
                    db.Open();
                    SqlCommand bsql = new SqlCommand("UPDATE users SET status = 2 WHERE id = @uid", db);
                    bsql.Parameters.AddWithValue("@uid", ban_uid);
                    exe = bsql.ExecuteNonQuery();
                    if(exe != 0)
                    {
                        err.InnerHtml = "<script>alert(\"Banned Successfully.\");window.location.replace(\"./users\");</script>";
                    }
                    else
                    {
                        err.InnerHtml = "<script>alert(\"Error Banning User, Try Again.\");window.location.replace(\"./users\");</script>";
                    }
                    db.Close();
                }
                else
                {
                    err.InnerHtml = "<script>alert(\"Admin cannot be banned.\");window.location.replace(\"./users\");</script>";
                }
            }

            if (unban != null)
            {
                int unban_uid = Convert.ToInt32(unban);
                if (!Isuid_admin(unban_uid))
                {
                    //BAN
                    int exe = 0;
                    db.Open();
                    SqlCommand usql = new SqlCommand("UPDATE users SET status = 1 WHERE id = @uid", db);
                    usql.Parameters.AddWithValue("@uid", unban_uid);
                    exe = usql.ExecuteNonQuery();
                    if (exe != 0)
                    {
                        err.InnerHtml = "<script>alert(\"UnBanned Successfully.\");window.location.replace(\"./users\");</script>";
                    }
                    else
                    {
                        err.InnerHtml = "<script>alert(\"Error UnBanning User, Try Again.\");window.location.replace(\"./users\");</script>";
                    }
                    db.Close();
                }
                else
                {
                    err.InnerHtml = "<script>alert(\"Error.\");window.location.replace(\"./users\");</script>";
                }
            }

            //CHECK FOR LOGIN
            if (pc.Is_admin(user, pass))
            {
                if(IsPostBack)
                {
                    //CLEAR TABLE CONTENT
                    tbody.InnerHtml = "";

                    string search_param = huname.Value;

                    //SEARCH DATEBASE FOR SEARCH PARAMETER
                    db.Open();
                    SqlCommand asql = new SqlCommand("SELECT TOP(20) * FROM users WHERE username LIKE @search_param ORDER BY id DESC", db);
                    asql.Parameters.AddWithValue("@search_param", search_param);
                    SqlDataReader ares = asql.ExecuteReader();
                    while(ares.Read())
                    {
                        int uid = (int)ares["id"];
                        string uname = (string)ares["username"];
                        string mail = (string)ares["mail"];
                        int status = (int)ares["status"];
                        int rank = (int)ares["rank"];
                        Wtable(uid, uname, mail, status, rank);
                    }
                    db.Close();
                }
                else
                {
                    //CLEAR TABLE CONTENT
                    tbody.InnerHtml = "";

                    //GET INFO OF LATEST USERS
                    db.Open();
                    SqlCommand sql = new SqlCommand("SELECT TOP(20) * FROM users ORDER BY id DESC", db);
                    SqlDataReader res = sql.ExecuteReader();
                    while(res.Read())
                    {
                        int uid = (int)res["id"];
                        string uname = (string)res["username"];
                        string mail = (string)res["mail"];
                        int status = (int)res["status"];
                        int rank = (int)res["rank"];
                        Wtable(uid, uname, mail, status, rank);
                    }
                    db.Close();
                }
            }
            else
            {
                Response.Redirect("/");
            }
        }
        public bool Wtable(int uid, string uname, string mail, int st, int rk)
        {
            //SET RANK
            string rank = null;
            if(rk == 1)
            {
                rank = "<span class='label label-success'>Admin</span>";
            }
            else
            {
                rank = "<span class='label label-default'>User</span>";
            }
            //SET STATUS AND MODIFY
            string status = null;
            string modify = null;
            if (st == 1)
            {
                status = "<span class='label label-success'>Active</span>";
                modify = "<a href='?ban=" + uid + "'><span class='label label-danger'>Ban</span></a>";
            }
            else
            {
                status = "<span class='label label-danger'>Banned</span>";
                modify = "<a href='?unban=" + uid + "'><span class='label label-success'>UnBan</span></a>";
            }

            //WRITE TO TABLE
            tbody.InnerHtml += "<tr class='text-center'>" +
                "<td>" + uname + "</td><td>" + mail + "</td><td>" + status + "</td><td>" + rank + "</td>" +
                "<td>" + modify + "</td>" + 
                "</tr>";

            return true;
        }
        public bool Isuid_admin (int userid)
        {
            //ACCESS PUBLIC CLASSES
            pub_class pc = new pub_class();

            //CREATE DATABASE CONNECTION
            string conn = pc.Getdb();
            SqlConnection db = new SqlConnection(conn);

            db.Open();
            SqlCommand sql = new SqlCommand("SELECT rank FROM users WHERE id = @uid", db);
            sql.Parameters.AddWithValue("@uid", userid);
            SqlDataReader res = sql.ExecuteReader();
            res.Read();
            int rank = (int)res["rank"];
            db.Close();
            if(rank == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}