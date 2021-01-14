using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HeadlessModeAndRestSharp
{
    [TestFixture]
    class RestSharp
    {
        RestClient client = new RestClient("https://greencity-user.azurewebsites.net");

        public string getAccessToken()
        {
            JsonObject json1 = new JsonObject();
            json1.Add("email", "xdknxusqvjeovowpfk@awdrt.com");
            json1.Add("password", "Temp#001");
            var request = new RestRequest("/ownSecurity/signIn", Method.POST);
            request.AddParameter("application/json", json1, ParameterType.RequestBody);
            IRestResponse restResponse = client.Execute(request);
            Regex regex = new Regex("\"accessToken\":\"(.*)\",\"refreshToken");
            Match match = regex.Match(restResponse.Content);
            return match.Groups[1].Value;
        }

        [Test]
        public void createReply()
        {
            string accessToken = getAccessToken();
            Console.WriteLine(accessToken);
            JsonObject json2 = new JsonObject();
            json2.Add("parentCommentId", "4992");
            json2.Add("text", "create reply with restSharp");
            var request = new RestRequest("/econews/comments/8759", Method.POST);
            request.AddHeader("Authorization", "Bearer " + accessToken);
            request.AddParameter("application/json", json2, ParameterType.RequestBody);
            IRestResponse restResponse = client.Execute(request);
            Console.WriteLine(restResponse.Content);
        }
    }
}
