using NUnit.Framework;
using RestSharp;
using RestSharp.Serialization.Json;
using System.Collections.Generic;
using System.Net;

namespace TaskBoard_System_Automated_API_Tests
{
    public class TestsForTheApp
    {
        const string baseUrl = "https://taskboard-1.stoyaniv.repl.co";

        [Test]
        public void ListAllTask()
        {
            //Arange
            var cliant = new RestClient(baseUrl + "/api/tasks");
            cliant.Timeout = 3000;

            //Act
            var request = new RestRequest(Method.GET);
            var response = cliant.Execute(request);
            var allTask = new JsonDeserializer().Deserialize<List<Response>>(response);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            foreach (var task in allTask)
            {
                if (task.board[0].name == "Done")
                {
                    Assert.AreEqual(task.title, "Project skeleton");
                    break;
                }

            }
        }
        [Test]
        public void FindTaskByKeyword()
        {
            //Arange
            var client = new RestClient(baseUrl + "/api/tasks/search/home");
            client.Timeout = 3000;

            //Act
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);
            var allTask = new JsonDeserializer().Deserialize<List<Response>>(response);
            var firstResTitle = allTask[0].title;

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(firstResTitle, "Home page");
        }
        [Test]
        public void FindTaskByInvalidKeyword()
        {
            //Arange
            var client = new RestClient(baseUrl + "/api/tasks/search/invalidkeyword");
            client.Timeout = 3000;

            //Act
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);
            var allTask = new JsonDeserializer().Deserialize<List<Response>>(response);

            //Assert
            Assert.IsEmpty(allTask);
        }

        [Test]
        public void CreatTaskByInvalidData()
        {
            //Arange
            var client = new RestClient(baseUrl + "/api/tasks");
            client.Timeout = 3000;

            //Act
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new
            {
                id = "7",
                title = "",
                description = "",
                dateCreated = "2021-03-09T12:40:46.407Z",
                dateModified = "2021-03-09T12:40:46.407Z"
            });
            var response = client.Execute(request);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Test]
        public void CreatTaskByValidData()
        {
            //Arange
            var client = new RestClient(baseUrl + "/api/tasks");
            client.Timeout = 3000;

            //Act
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new
            {
                id = "7",
                title = "Commit the code in Github",
                description = "Add all files and solution in repo",
                dateCreated = "2021-03-09T12",
                dateModified = "2021-03-09T12"
            });
            var response = client.Execute(request);

            //Assert new task is added
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            //Arange
            var getcliant = new RestClient(baseUrl + "/api/tasks");
            getcliant.Timeout = 3000;

            //Act
            var getrequest = new RestRequest(Method.GET);
            var getresponse = getcliant.Execute(getrequest);
            var allTask = new JsonDeserializer().Deserialize<List<Response>>(getresponse);

            //Assert new task is last one
            var lastTask = allTask[allTask.Count - 1];
            Assert.AreEqual(lastTask.description, "Add all files and solution in repo");
            Assert.AreEqual(lastTask.title, "Commit the code in Github");

            }
        }
}