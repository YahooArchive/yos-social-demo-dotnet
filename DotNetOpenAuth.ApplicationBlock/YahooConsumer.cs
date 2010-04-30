
namespace DotNetOpenAuth.ApplicationBlock
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml;
    using System.Xml.Linq;
    using System.Web;
    using DotNetOpenAuth.Messaging;
    using DotNetOpenAuth.OAuth;
    using DotNetOpenAuth.OAuth.ChannelElements;

    /// <summary>
    /// A consumer capable of communicating with Google Data APIs.
    /// </summary>
    public static class YahooConsumer
    {
        /// <summary>
        /// The Consumer to use for accessing Google data APIs.
        /// </summary>
        public static readonly ServiceProviderDescription ServiceDescription = new ServiceProviderDescription
        {
            //RequestTokenEndpoint = new MessageReceivingEndpoint("https://open.login.yahoo.apis.com/openid/op/auth", HttpDeliveryMethods.AuthorizationHeaderRequest | HttpDeliveryMethods.GetRequest),
            RequestTokenEndpoint = new MessageReceivingEndpoint("https://api.login.yahoo.com/oauth/v2/get_request_token", HttpDeliveryMethods.AuthorizationHeaderRequest | HttpDeliveryMethods.GetRequest),
            UserAuthorizationEndpoint = new MessageReceivingEndpoint("https://api.login.yahoo.com/oauth/v2/request_auth", HttpDeliveryMethods.AuthorizationHeaderRequest | HttpDeliveryMethods.GetRequest),
            AccessTokenEndpoint = new MessageReceivingEndpoint("https://api.login.yahoo.com/oauth/v2/get_token", HttpDeliveryMethods.AuthorizationHeaderRequest | HttpDeliveryMethods.GetRequest),
            TamperProtectionElements = new ITamperProtectionChannelBindingElement[] { new HmacSha1SigningBindingElement() },
        };


        /// <summary>
        /// The service description to use for accessing Google data APIs using an X509 certificate.
        /// </summary>
        /// <param name="signingCertificate">The signing certificate.</param>
        /// <returns>A service description that can be used to create an instance of
        /// <see cref="DesktopConsumer"/> or <see cref="WebConsumer"/>. </returns>
        public static ServiceProviderDescription CreateRsaSha1ServiceDescription(X509Certificate2 signingCertificate)
        {
            if (signingCertificate == null)
            {
                throw new ArgumentNullException("signingCertificate");
            }

            return new ServiceProviderDescription
            {
                RequestTokenEndpoint = new MessageReceivingEndpoint("https://api.login.yahoo.com/oauth/v2/get_request_token", HttpDeliveryMethods.AuthorizationHeaderRequest | HttpDeliveryMethods.GetRequest),
                UserAuthorizationEndpoint = new MessageReceivingEndpoint("https://api.login.yahoo.com/oauth/v2/request_auth", HttpDeliveryMethods.AuthorizationHeaderRequest | HttpDeliveryMethods.GetRequest),
                AccessTokenEndpoint = new MessageReceivingEndpoint("https://api.login.yahoo.com/oauth/v2/get_token", HttpDeliveryMethods.AuthorizationHeaderRequest | HttpDeliveryMethods.GetRequest),
                TamperProtectionElements = new ITamperProtectionChannelBindingElement[] { new RsaSha1SigningBindingElement(signingCertificate) },
            };
        }

        /// <summary>
        /// Requests authorization from Google to access data from a set of Google applications.
        /// </summary>
        /// <param name="consumer">The Google consumer previously constructed using <see cref="CreateWebConsumer"/> or <see cref="CreateDesktopConsumer"/>.</param>
        /// <param name="requestedAccessScope">The requested access scope.</param>
        public static void RequestAuthorization(WebConsumer consumer)
        {
            if (consumer == null)
            {
                throw new ArgumentNullException("consumer");
            }

            Uri callback = Util.GetCallbackUrlFromContext();
            var request = consumer.PrepareRequestUserAuthorization(callback, null, null);
            consumer.Channel.Send(request);
        }

        /// <summary>
        /// Requests authorization from Google to access data from a set of Google applications.
        /// </summary>
        /// <param name="consumer">The Google consumer previously constructed using <see cref="CreateWebConsumer"/> or <see cref="CreateDesktopConsumer"/>.</param>
        /// <param name="requestedAccessScope">The requested access scope.</param>
        /// <param name="requestToken">The unauthorized request token assigned by Google.</param>
        /// <returns>The request token</returns>
        public static Uri RequestAuthorization(DesktopConsumer consumer,  out string requestToken)
        {
            if (consumer == null)
            {
                throw new ArgumentNullException("consumer");
            }

            return consumer.RequestUserAuthorization(null, null, out requestToken);
        }

        public static XDocument YQL(ConsumerBase consumer, string accessToken, string yql)
        {
            if (consumer == null)
            {
                throw new ArgumentNullException("consumer");
            }


            yql = yql.Replace("'", "\"");
            string uri = "http://query.yahooapis.com/v1/yql?q=" + HttpUtility.UrlEncode(yql).Replace("+", "%20").Replace("*", "%2A").Replace("(", "%28").Replace(")", "%29");
            
            MessageReceivingEndpoint endPoint = new MessageReceivingEndpoint(uri, HttpDeliveryMethods.AuthorizationHeaderRequest | HttpDeliveryMethods.GetRequest);
            
            var request = consumer.PrepareAuthorizedRequest(endPoint, accessToken);
            string header = request.Headers.GetValues(0)[0];
            header = header.Replace("OAuth", "").Replace("\"", "").Replace(",", "&").Trim();
            uri += "&" + header;

            HttpWebRequest req = (HttpWebRequest) WebRequest.Create(uri);
            req.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.9b5) Gecko/2008032620 Firefox/3.0b5";
            WebResponse resp = req.GetResponse();         
            Encoding enc = System.Text.Encoding.GetEncoding(1252);
            StreamReader loResponseStream = new
            StreamReader(resp.GetResponseStream(), enc);
            string body = loResponseStream.ReadToEnd();

            return XDocument.Parse(body);
        }


        /*
        public static string postYQL(ConsumerBase consumer, string accessToken, string yql)
        {
            if (consumer == null)
            {
                throw new ArgumentNullException("consumer");
            }

            string data = "q=select * from social.profile where guid=me";
            string uri = "http://query.yahooapis.com/v1/public/yql";
            uri = "http://kaget.info/home.php";

            MessageReceivingEndpoint endPoint = new MessageReceivingEndpoint(uri, HttpDeliveryMethods.AuthorizationHeaderRequest | HttpDeliveryMethods.PostRequest);

            WebRequest request = consumer.PrepareAuthorizedRequest(endPoint, accessToken);

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
            StreamReader(response.GetResponseStream(),enc);
            string body = loResponseStream.ReadToEnd();
            loResponseStream.Close();
            response.Close();

            return body;
        }
*/

    }
}
