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
    public class pub_class
    {
        public bool Is_loggedin(string user, string pass)
        {
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
        public int Get_userid(string user, string pass)
        {
            if (Is_loggedin(user, pass))
            {
                if (user != null && pass != null)
                {
                    String conn = Getdb();
                    SqlConnection db = new SqlConnection(conn);
                    db.Open();

                    SqlCommand sql = new SqlCommand("SELECT id FROM users WHERE username = @user AND pass = @pass AND status = 1", db);
                    sql.Parameters.AddWithValue("@user", user);
                    sql.Parameters.AddWithValue("@pass", pass);
                    SqlDataReader result = sql.ExecuteReader();
                    result.Read();
                    int userid = (int)result["id"];

                    return userid;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        public bool Is_goalset(int userid)
        {
            string conn = Getdb();
            SqlConnection db = new SqlConnection(conn);
            db.Open();
            SqlCommand sql = new SqlCommand("SELECT goal FROM users WHERE id = @uid", db);
            sql.Parameters.AddWithValue("@uid", userid);
            SqlDataReader result = sql.ExecuteReader();
            result.Read();
            int goal = (int)result["goal"];
            if(goal == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Is_admin(string user, string pass)
        {
            if (Is_loggedin(user, pass))
            {
                String conn = Getdb();
                SqlConnection db = new SqlConnection(conn);
                db.Open();

                SqlCommand sql = new SqlCommand("SELECT rank FROM users WHERE username = @user AND pass = @pass AND status = 1", db);
                sql.Parameters.AddWithValue("@user", user);
                sql.Parameters.AddWithValue("@pass", pass);
                SqlDataReader result = sql.ExecuteReader();
                result.Read();
                int rank = (int)result["rank"];
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
            else
            {
                return false;
            }
        }
        public string GetMD5HashData(string data)
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
        public string Getdb()
        {
            //CONNECTION STRING
            string connStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=fitness;MultipleActiveResultSets=true;Integrated Security=True;Pooling=False";
            return connStr;
        }
    }
}