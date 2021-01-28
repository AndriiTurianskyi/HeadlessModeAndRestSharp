﻿using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HeadlessModeAndRestSharp
{
    [TestFixture]
    class RestSharp
    {
        RestClient restClient = new RestClient("https://greencity.azurewebsites.net");
       private string commentId;
        public string getAccessToken()
        {
            RestClient ownSecurityClient = new RestClient("https://greencity-user.azurewebsites.net");
            var request = new RestRequest("/ownSecurity/signIn", Method.POST);
            request.AddParameter("application/json", "{\"email\":\"pruvat@gmail.com\", \"password\":\"1Test@test\"}", ParameterType.RequestBody);
            IRestResponse restResponse = ownSecurityClient.Execute(request);
            Regex regex = new Regex("\"accessToken\":\"(.*)\",\"refreshToken");
            Match match = regex.Match(restResponse.Content);
            return match.Groups[1].Value;
        }

        [Test]
        public void createComment()
        {
            string accessToken = getAccessToken();
            JsonObject commentData = new JsonObject();
            commentData.Add("parentCommentId", "0");
            commentData.Add("text", "create reply by restSharp");
            var request = new RestRequest("/econews/comments/9257", Method.POST);
            request.AddHeader("Authorization", "Bearer " + accessToken);
            request.AddParameter("application/json", commentData, ParameterType.RequestBody);
            IRestResponse restResponse = restClient.Execute(request);
            // get comment id
            Regex regex = new Regex("\"id\":(\\d*),\"name");
            Match match = regex.Match(restResponse.Content);
            commentId = match.Groups[1].Value;
            //
            Assert.AreEqual(HttpStatusCode.Created, restResponse.StatusCode);
        }

        [Test]
        public void deleteComment()
        {
            string accessToken = getAccessToken();
            JsonObject commentData = new JsonObject();
            commentData.Add("id", commentId);
            var request = new RestRequest("/econews/comments", Method.DELETE);
            request.AddHeader("Authorization", "Bearer " + accessToken);
            request.AddParameter("application/json", commentData, ParameterType.RequestBody);
            IRestResponse restResponse = restClient.Execute(request);
            Console.WriteLine(restResponse.Content);
            Console.WriteLine(commentId);
            Assert.AreEqual(HttpStatusCode.OK, restResponse.StatusCode);
        }
    }
}
