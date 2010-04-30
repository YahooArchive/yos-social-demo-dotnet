using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Text;

using DotNetOpenAuth.ApplicationBlock;
using DotNetOpenAuth.OAuth.Messages;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.RelyingParty;

using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth.ChannelElements;
using DotNetOpenAuth.OpenId.Extensions.OAuth;

using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Xml;


namespace OpenIdRelyingPartyWebForms
{
    public partial class yahoo : System.Web.UI.Page
    {
        private const string YahooOPIdentifier = "yahoo.com";
        private static readonly OpenIdRelyingParty relyingParty = new OpenIdRelyingParty();

        protected void Page_Load(object sender, EventArgs e)
        {

            IAuthenticationResponse authResponse = relyingParty.GetResponse();
            if (authResponse != null)
            {
                switch (authResponse.Status)
                {
                    case AuthenticationStatus.Authenticated:
                        txt1.Text += "Authentication Success\n";

                        State.FetchResponse = authResponse.GetExtension<FetchResponse>();

                        var positiveAuthorization = authResponse.GetExtension<AuthorizationApprovedResponse>();
                        
                        AuthorizedTokenResponse accessToken = Global.YahooWebConsumer.ProcessUserAuthorization(authResponse);
                        if (accessToken != null)
                        {
                            txt1.Text += "Access token: " + accessToken.AccessToken + "\n";
                            State.YahooAccessToken = accessToken.AccessToken;

                            Response.Redirect("~/yql.aspx");

                            //string yql = "select * from social.profile where guid=me";
                            //XDocument result = YahooConsumer.YQL(Global.YahooWebConsumer, State.YahooAccessToken, yql);
                            //txt1.Text = result.ToString();
				
                        }
                        else
                        {
                            txt1.Text += "Authorization Failed\n";
                        }
                        txt1.Text += "Email: \n" + State.FetchResponse.GetAttributeValue("http://axschema.org/contact/email");
                        break;
                    case AuthenticationStatus.Canceled:
                    case AuthenticationStatus.Failed:
                    default:
                        txt1.Text += "Authentication Failed\n";
                        break;
                }
            }
        }

        protected void btn1_Click(object sender, EventArgs e)
        {
            this.GetYahooRequest().RedirectToProvider();
        }

        private IAuthenticationRequest GetYahooRequest()
        {
            var b = new UriBuilder(Request.Url) { Query = "" };
            IAuthenticationRequest authReq = relyingParty.CreateRequest(YahooOPIdentifier, b.Uri, b.Uri);

            // Prepare the OAuth extension
            Global.YahooWebConsumer.AttachAuthorizationRequest(authReq, "");

            // We also want the user's email address
            var fetch = new FetchRequest();
            fetch.Attributes.AddRequired("http://axschema.org/contact/email");
            fetch.Attributes.AddRequired("http://axschema.org/namePerson");
            fetch.Attributes.AddRequired("http://axschema.org/person/gender");
            fetch.Attributes.AddRequired("http://axschema.org/media/image/default");
            fetch.Attributes.AddRequired("http://axschema.org/pref/language");            
            authReq.AddExtension(fetch);

            return authReq;
        }

        private void RenderContacts(XDocument contactsDocument)
        {
            txt1.Text += "\n" + contactsDocument.ToString();
            /*
            var contacts = from entry in contactsDocument.Root.Elements(XName.Get("entry", "http://www.w3.org/2005/Atom"))
                           select new { Name = entry.Element(XName.Get("title", "http://www.w3.org/2005/Atom")).Value, Email = entry.Element(XName.Get("email", "http://schemas.google.com/g/2005")).Attribute("address").Value };
            StringBuilder tableBuilder = new StringBuilder();
            tableBuilder.Append("<table><tr><td>Name</td><td>Email</td></tr>");
            foreach (var contact in contacts)
            {
                tableBuilder.AppendFormat(
                    "<tr><td>{0}</td><td>{1}</td></tr>",
                    HttpUtility.HtmlEncode(contact.Name),
                    HttpUtility.HtmlEncode(contact.Email));
            }
            tableBuilder.Append("</table>");
            this.PlaceHolder1.Controls.Add(new Literal { Text = tableBuilder.ToString() });
             * */
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string header = "<query xmlns:yahoo=\"http://www.yahooapis.com/v1/base.rng\" yahoo:count=\"1\" yahoo:created=\"2010-04-03T12:19:45Z\" yahoo:lang=\"en-US\" yahoo:updated=\"2010-04-03T12:19:45Z\" yahoo:uri=\"http://query.yahooapis.com/v1/yql?q=select+*+from+social.profile+where+guid%3Dme\">" +
  "<diagnostics>" + 
  "  <publiclyCallable>false</publiclyCallable>" + 
  "  <url execution-time=\"21\"><![CDATA[http://social.yahooapis.com/v1/users.guid(V476BWX6PONZQZQSFUG2ZCOMC4)/profile]]></url>" + 
  "  <user-time>24</user-time>" + 
  "  <service-time>21</service-time>" + 
  "  <build-version>5275</build-version>" + 
  "</diagnostics>" + 
  "<results>" + 
  "  <profile xmlns=\"http://social.yahooapis.com/v1/schema.rng\">" + 
  "    <guid>V476BWX6PONZQZQSFUG2ZCOMC4</guid>" + 
  "    <created>2010-03-08T05:05:47Z</created>" + 
  "    <displayAge>30</displayAge>" + 
  "    <gender>M</gender>" + 
  "    <image>" + 
  "      <height>192</height>" + 
  "      <imageUrl>http://a323.yahoofs.com/coreid/4b9485dfi20a7zws133sp2/L6B982Q7cqtoXb9agRNjUQhorZ_5/1/t192.jpeg?ciAAToMB5BaujrdP</imageUrl>" + 
  "      <size>192x192</size>" + 
  "      <width>192</width>" + 
  "    </image>" + 
  "    <nickname>Jimmi</nickname>" + 
  "    <profileUrl>http://profiles.yahoo.com/u/V476BWX6PONZQZQSFUG2ZCOMC4</profileUrl>" + 
  "    <status>" + 
  "      <lastStatusModified>2010-04-03T04:25:28Z</lastStatusModified>" + 
  "      <linkTo />" + 
  "      <message>yea... almost done</message>" + 
  "    </status>" + 
  "    <isConnected>false</isConnected>" + 
  "  </profile>" + 
  "</results>" + 
"</query>";

            XDocument xml = XDocument.Parse(header);
            //XElement xml = XElement.Parse(header);
            XNamespace n1 = "http://www.yahooapis.com/v1/base.rng";
            XNamespace n2 = "http://social.yahooapis.com/v1/schema.rng";
            //txt1.Text += xml.Element("results").Element(n2 + "profile").Element(n2 + "guid").Value;
            //XElement item = new XElement("results", n1 + "profile", n2 + "guid");
            txt1.Text = xml.Root.Element("results").Element(n2 + "profile").Element(n2 + "guid").Value;
            //txt1.Text = item.Value;
            
        }

        public static string postYQL()
        {

            string data = "q=select * from social.profile where guid=me";
            //string uri = "http://query.yahooapis.com/v1/public/yql?q=select * from flickr.photos.recent";
            //string uri = "http://query.yahooapis.com/v1/public/yql?" + data;
            string uri = "http://kaget.info/home.php";

            //MessageReceivingEndpoint endPoint = new MessageReceivingEndpoint(uri, HttpDeliveryMethods.AuthorizationHeaderRequest | HttpDeliveryMethods.PostRequest);

            WebRequest request = WebRequest.Create(uri);

            request.ContentType = "application/atom+xml";
            request.Method = "POST";
            byte[] bytes = Encoding.ASCII.GetBytes(data);
            Stream os = null;
            request.ContentLength = bytes.Length;
            os = request.GetRequestStream();
            os.Write(bytes, 0, bytes.Length);
            os.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Encoding enc = System.Text.Encoding.GetEncoding(1252);
            StreamReader loResponseStream = new
            StreamReader(response.GetResponseStream(), enc);
            string body = loResponseStream.ReadToEnd();
            loResponseStream.Close();
            response.Close();

            return body;
        }



    }
}