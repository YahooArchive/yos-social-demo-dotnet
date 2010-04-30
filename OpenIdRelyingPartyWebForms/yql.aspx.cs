using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using DotNetOpenAuth.ApplicationBlock;

namespace OpenIdRelyingPartyWebForms
{
    public partial class yql : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void bquery_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(State.YahooAccessToken))
            {
                if (tyql.Text != "")
                {
                    XDocument result = YahooConsumer.YQL(Global.YahooWebConsumer, State.YahooAccessToken, tyql.Text);
                    tresult.Text = result.ToString();
                }
            }
            else
            {
                Response.Redirect("~/yahoo.aspx");
            }
        }
    }
}