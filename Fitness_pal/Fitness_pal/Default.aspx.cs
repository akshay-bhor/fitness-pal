using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fitness_pal
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(IsPostBack)
            {
                string search_param = Request.Form["food_name"];
                if(!string.IsNullOrEmpty(search_param))
                {
                    Response.Redirect("/food?food_name=" + search_param);
                }
                else { }
            }
        }
    }
}