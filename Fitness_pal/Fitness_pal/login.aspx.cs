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
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // CHECK IF ALREADY LOGGED IN
            if (Is_loggedin())
            {
                Response.Redirect("/diary");
            }
            if (IsPostBack)
            {
                string uname = Request.Form["uname"];
                string pwd = Request.Form["pass"];

                if (uname != null && uname.Length > 2 && pwd.Length > 5)
                {
                    //CHECK FOR SPECIAL CHARACTERS IN USERNAME
                    Regex regexItem = new Regex("^[a-zA-Z0-9 ]*$");
                    if (regexItem.IsMatch(uname))
                    {
                        err.InnerHtml = "";

                        //CONVER PASSWORD TO MD5
                        string pass = GetMD5HashData(pwd);

                        //CREATE PUBLIC CLASS INSTANCE
                        pub_class pc = new pub_class();

                        string conn = pc.Getdb();
                        SqlConnection db = new SqlConnection(conn);
                        db.Open();
                        SqlCommand sql = new SqlCommand("SELECT COUNT(id) FROM users WHERE username = @user AND pass = @pass", db);
                        sql.Parameters.AddWithValue("@user", uname);
                        sql.Parameters.AddWithValue("@pass", pass);
                        SqlDataReader result = sql.ExecuteReader();
                        result.Read();
                        int count = (int)result[0];
                        if (count == 1)
                        {
                            //CHECK IF USER IS BANNED
                            SqlCommand asql = new SqlCommand("SELECT status FROM users WHERE username = @user AND pass = @pass", db);
                            asql.Parameters.AddWithValue("@user", uname);
                            asql.Parameters.AddWithValue("@pass", pass);
                            SqlDataReader ares = asql.ExecuteReader();
                            ares.Read();
                            int ustatus = (int)ares[0];

                            if (ustatus != 1)
                            {
                                err.InnerHtml = "<div class='alert alert-danger'>You Are Banned From This Site.</div>";
                            }
                            else
                            {
                                //SET AUTHENTICATION COOKIES
                                Response.Cookies["fitness_user"].Value = uname;
                                Response.Cookies["fitness_user"].Expires = DateTime.Now.AddDays(365);
                                Response.Cookies["fitness_pass"].Value = pass;
                                Response.Cookies["fitness_pass"].Expires = DateTime.Now.AddDays(365);
                                Response.Redirect("/diary");
                            }
                        }
                        else
                        {
                            err.InnerHtml = "<div class='alert alert-danger'>Incorrect Username or Password.</div>";
                        }
                    }
                    else
                    {
                        err.InnerHtml = "<div class='alert alert-danger'>Special Character Not Allowed in Username.</div>";
                    }
                }
                else
                {
                    err.InnerHtml = "<div class='alert alert-danger'>Incorrect Username or Password.</div>";
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