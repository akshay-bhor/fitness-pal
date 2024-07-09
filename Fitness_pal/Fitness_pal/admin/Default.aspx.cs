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
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //ACCESS PUBLIC CLASSES
            pub_class pc = new pub_class();

            //GET AUTHENTICATION COOKIES
            string user = Request.Cookies["fitness_user"].Value;
            string pass = Request.Cookies["fitness_pass"].Value;

            //CHECK FOR LOGIN
            if (!pc.Is_admin(user, pass))
            {
                Response.Redirect("/");
            }
        }
    }
}