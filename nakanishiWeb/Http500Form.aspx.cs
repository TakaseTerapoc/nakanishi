using nakanishiWeb.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace nakanishiWeb
{
    public partial class Http500Form : System.Web.UI.Page
    {
        public string errorMessage; 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Funcs.IsNotNullObject(Request.QueryString["errorMessage"]))
            {
                errorMessage = Request.QueryString["errorMessage"].ToString();
            }
            else
            {
                errorMessage = "";
            }
            Session.RemoveAll();
        }
    }
}