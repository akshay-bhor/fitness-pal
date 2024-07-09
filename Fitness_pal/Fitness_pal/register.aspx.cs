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
    public partial class register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //CHECK IF ALREADY LOGGED IN
            if (Is_loggedin()) {
                Response.Redirect("/diary");
            }

            if (IsPostBack) {
                string uname = Request.Form["uname"];
                string email = Request.Form["email"];
                string pwd = Request.Form["pass"];

                if (uname != null && uname.Length > 2 && email != null && pwd.Length > 5)
                {
                    //CHECK FOR USERNAME AND EMAIL EXIATENCE
                    if (Not_exist_usr(uname) == "true" && Not_exist_mail(email) == "true")
                    {
                        //CHECK FOR SPECIAL CHARACTERS IN USERNAME
                        Regex regexItem = new Regex("^[a-zA-Z0-9 ]*$");
                        if (regexItem.IsMatch(uname))
                        {
                            err.InnerHtml = "";

                            //CONVER PASSWORD TO MD5
                            string pass = GetMD5HashData(pwd);

                            //IF ALL IS WELL THEN INSERT DATA IN USERS TABLE
                            DateTime today = DateTime.Today;
                            String conn = Getdb();
                            SqlConnection db = new SqlConnection(conn);
                            db.Open();
                            SqlCommand sql = new SqlCommand("INSERT INTO users(username, mail, pass, reg) VALUES(@uname, @email, @pass, @today)", db);
                            sql.Parameters.AddWithValue("@uname", uname);
                            sql.Parameters.AddWithValue("@email", email);
                            sql.Parameters.AddWithValue("@pass", pass);
                            sql.Parameters.AddWithValue("@today", today);
                            int exe = sql.ExecuteNonQuery();
                            db.Close();
                            if (exe != 0)
                            {
                                //SET AUTHENTICATION COOKIES
                                Response.Cookies["fitness_user"].Value = uname;
                                Response.Cookies["fitness_user"].Expires = DateTime.Now.AddDays(365);
                                Response.Cookies["fitness_pass"].Value = pass;
                                Response.Cookies["fitness_pass"].Expires = DateTime.Now.AddDays(365);

                                err.InnerHtml = "<div class='alert alert-success'>Registration Successfull.</div><script>window.location.href = \"/\";</script>";
                            }
                            else
                            {
                                err.InnerHtml = "<div class='alert alert-danger'>Unknown Error Occured While Registering, Try Again.</div>";
                            }
                        }
                        else
                        {
                            err.InnerHtml = "<div class='alert alert-danger'>Special Character Not Allowed in Username.</div>";
                        }
                    }
                    else {
                        err.InnerHtml = "<div class='alert alert-danger'>Username or Email already Exist.</div>";
                    }
                }
                else {
                    err.InnerHtml = "<div class='alert alert-danger'>Please fill all fields.</div>";
                }
            }
            else {  }
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
        private string Not_exist_usr(string uname) {
            String conn = Getdb();
            SqlConnection db = new SqlConnection(conn);
            db.Open();
            SqlCommand sql = new SqlCommand("SELECT COUNT(id) as c FROM users WHERE username = @uname", db);
            sql.Parameters.AddWithValue("@uname", uname);
            SqlDataReader result = sql.ExecuteReader();
            result.Read();
            int count = (int)result[0];
            db.Close();
            if (count != 0) {
                return "false";
            }
            else {
                return "true";
            }
        }
        private string Not_exist_mail(string email)
        {
            String conn = Getdb();
            SqlConnection db = new SqlConnection(conn);
            db.Open();
            SqlCommand sql = new SqlCommand("SELECT COUNT(mail) as c FROM users WHERE mail = @email", db);
            sql.Parameters.AddWithValue("@email", email);
            SqlDataReader result = sql.ExecuteReader();
            result.Read();
            int count = (int)result[0];
            db.Close();
            if (count != 0)
            {
                return "false";
            }
            else
            {
                return "true";
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