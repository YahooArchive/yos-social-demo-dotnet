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
    public partial class member : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(State.YahooAccessToken))
            {
                string yql = "select * from social.profile where guid=me";
                XDocument result = YahooConsumer.YQL(Global.YahooWebConsumer, State.YahooAccessToken, yql);

                XNamespace n1 = "http://www.yahooapis.com/v1/base.rng";
                XNamespace n2 = "http://social.yahooapis.com/v1/schema.rng";
         
                lname.Text = result.Root.Element("results").Element(n2 + "profile").Element(n2 + "nickname").Value;
                //lname.Text = result.Element("//query/results/profile/nickname").Value;


            }
            else
            {
                Response.Redirect("~/yahoo.aspx");
            }
        }
    }
}