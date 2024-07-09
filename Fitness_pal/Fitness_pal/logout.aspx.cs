using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fitness_pal
{
    public partial class logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Response.Cookies["fitness_user"] != null)
            {
                Response.Cookies["fitness_user"].Expires = DateTime.Now.AddDays(-1);
            }
            if (Response.Cookies["fitness_pass"] != null)
            {
                Response.Cookies["fitness_pass"].Expires = DateTime.Now.AddDays(-1);
            }
            Response.Redirect("/");
        }
    }
}