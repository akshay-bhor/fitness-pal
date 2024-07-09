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
    public partial class progress : System.Web.UI.Page
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
                err.InnerHtml = "";
                //GET DATES FOR PAST 3 MOTHS
                DateTime prdate = DateTime.Today;
                DateTime ptdate = DateTime.Today.AddDays(-90);

                //GET 6 VALUES FORM 90 DAYS
                DateTime[] date_var = new DateTime[7];
                date_var[0] = ptdate;
                date_var[1] = ptdate.AddDays(15);
                date_var[2] = ptdate.AddDays(30);
                date_var[3] = ptdate.AddDays(45);
                date_var[4] = ptdate.AddDays(60);
                date_var[5] = ptdate.AddDays(75);
                date_var[6] = prdate;

                err.InnerHtml += "<script type='text/javascript'>" +
                    "google.charts.load('current', { 'packages':['corechart']" +
                    "});" +
                    "google.charts.setOnLoadCallback(drawChart);" +

                    "function drawChart()" +
                    "{" +
                    "var data = google.visualization.arrayToDataTable([" +
                    "['Date', 'Weight'],";
                
                string conn = pc.Getdb();
                SqlConnection db = new SqlConnection(conn);
                //GET COUNT OF WEIGHT ENTRIES TO CREATE ARRAY
                db.Open();
                SqlCommand csql = new SqlCommand("SELECT COUNT(id) AS c FROM usr_wt WHERE uid = @uid AND date BETWEEN @past AND @present", db);
                csql.Parameters.AddWithValue("@uid", userid);
                csql.Parameters.AddWithValue("@past", ptdate);
                csql.Parameters.AddWithValue("@present", prdate);
                SqlDataReader cres = csql.ExecuteReader();
                cres.Read();
                int c = (int)cres["c"];
                db.Close();

                DateTime[] dt_arr = new DateTime[c];
                int[] wt_arr = new int[c];

                //GET WEIGHT DATA FOR USERID
                db.Open();
                SqlCommand sql = new SqlCommand("SELECT wt, date FROM usr_wt WHERE uid = @uid AND date BETWEEN @past AND @present ORDER BY date ASC", db);
                sql.Parameters.AddWithValue("@uid", userid);
                sql.Parameters.AddWithValue("@past", ptdate);
                sql.Parameters.AddWithValue("@present", prdate);
                SqlDataReader res = sql.ExecuteReader();
                int i = 0; int j = 0; int key = 0;
                while(res.Read())
                {
                    DateTime dt = (DateTime)res["date"];
                    string date = dt.ToString("dd-MM");
                    string wdate = dt.ToString("MMM d, yyyy");
                    int wt = (int)res["wt"];

                    dt_arr[key] = dt;
                    wt_arr[key] = wt;

                    //if (i < 7)
                    //{
                        //foreach (DateTime dte in date_var)
                        //{
                            //if (dte < dt)
                            //{
                                //string tmp = dte.ToString("dd-MM");
                                //err.InnerHtml += "['" + tmp + "'," + wt + "],";
                                //i++;
                                //Response.Write(i + "<br>");
                                //date_var = date_var.Where(val => val != dte).ToArray();
                            //}
                        //}
                        //j++;
                        //Response.Write(j + "j<br>");
                    //}
                    key++;

                    //err.InnerHtml += "['" + date + "'," + wt + "],";
                    wt_entries.InnerHtml += "<div class='fresult-row block'><span class='l'>" + wdate + "</span><span class='r'>" + wt + " kg</span></div>";
                }
                db.Close();
                //CREATE ENTREIS FOR DATES
                int k = 0;
                for(i = 0; i < dt_arr.Length; i++)
                {
                    if(k < 7)
                    {
                        foreach (DateTime dte in date_var)
                        {
                            if (dte < dt_arr[i])
                            {
                                string tmp = dte.ToString("dd-MM");
                                if (i == 0) {
                                err.InnerHtml += "['" + tmp + "'," + wt_arr[i] + "],";
                                }
                                else
                                {
                                    err.InnerHtml += "['" + tmp + "'," + wt_arr[i-1] + "],";
                                }
                                date_var = date_var.Where(val => val != dte).ToArray();
                            }
                        }
                        k++;
                    }
                }

                //LAST ENTRY
                string temp = date_var[0].ToString("dd-MM");
                err.InnerHtml += "['" + temp + "'," + wt_arr[c - 1] + "],";

                err.InnerHtml += "]);" +

                    "var options = {" +
                    "title: 'Weight Chart'," +
                    "curveType: 'function'," +
                    "legend: { position: 'bottom' }" +
                    "};" +

                    "var chart = new google.visualization.LineChart(document.getElementById('chart_div'));" +

                    "chart.draw(data, options);" +
                    "}" +
                    "</script>";
            }
        }
    }
}