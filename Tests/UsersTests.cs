using Newtonsoft.Json;
using RestApiCSharp.ConstantsTestingGeneral;
using RestSharp;
using System.Net;

namespace RestApiCSharp.Tests
{
    public class UsersTests : BaseApiTest
    {
        [SetUp]
        public void Setup()
        {
            var zipCodes = new List<string> { "oz", "oz1", "oz2", "oz3", "oz4" };

            ApiClientInstance.ExpandZipCodes(ConstantsTesting.WriteScope, zipCodes);

            var users = new List<User>
            {
                new User { Age = 0, Name = "u1", Sex = "FEMALE", ZipCode = "oz1" },
                new User { Name = "u3", Sex = "FEMALE" }
            };

            ApiClientInstance.CreateUsersList(ConstantsTesting.WriteScope, users);
        }

        [Test]
        public void AddUserAllFields_Return201_Test()
        {
            var user = new User
            {
                Age = 0,
                Name = "u",
                Sex = "FEMALE",
                ZipCode = "oz"
            };

            RestResponse response = ApiClientInstance.CreateUsers(ConstantsTesting.WriteScope, user);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created),
                $"Status code 201 not returned." +
                $"Expected status code 201 (Created), but got {response.StatusCode}. " +
                $"Response content: {response.Content}");
        }

        [Test]
        public void AddUserAllFields_UserAdded_Test()
        {
            RestResponse response = ApiClientInstance.GetUsers(ConstantsTesting.ReadScope);

            Assert.That(response.Content, Does.Contain("u1"),
                $"The user 'u1' was not added. Response content: {response.Content}");
        }

        [Test]
        public void AddUserAllFields_ZipCodeRemoved_Test()
        {
            RestResponse response = ApiClientInstance.GetZipCodes(ConstantsTesting.ReadScope);

            Assert.That(response.Content, Does.Not.Contain("oz1"),
                $"The zip code 'oz1' was not removed. Response content: {response.Content}");
        }

        [Test]
        public void AddUserReqFields_Return201_Test()
        {
            var user = new User
            {
                Name = "u2",
                Sex = "FEMALE"
            };

            RestResponse response = ApiClientInstance.CreateUsers(ConstantsTesting.WriteScope, user);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created),
                $"Status code 201 not returned." +
                $"Expected status code 201 (Created), but got {response.StatusCode}. " +
                $"Response content: {response.Content}");
        }

        [Test]
        public void AddUserReqFields_UserAdded_Test()
        {
            RestResponse response = ApiClientInstance.GetUsers(ConstantsTesting.ReadScope);

            Assert.That(response.Content, Does.Contain("u3"),
                $"The user 'u3' was not added. Response content: {response.Content}");
        }

        [Test]
        public void AddUserIncorrectZipCode_Return424_Test()
        {
            var user = new User
            {
                Age = 0,
                Name = "u4",
                Sex = "FEMALE",
                ZipCode = "brfrtr"
            };

            RestResponse response = null;

            response = ApiClientInstance.CreateUsers(ConstantsTesting.WriteScope, user);

            Assert.That(response.Content, Does.Contain("FailedDependency"),
                 $"Response content does not indicate FailedDependency. Content: {response.Content}");
        }

        [Test]
        public void AddUserIncorrectZipCode_UserNotAdded_Test()
        {
            RestResponse response = ApiClientInstance.GetUsers(ConstantsTesting.ReadScope);

            Assert.That(response.Content, Does.Not.Contain("u4"),
                $"The user 'u4' was not added. Response content: {response.Content}");
        }

        [Test]
        public void AddUserDuplicate_Return400_Test()
        {
            var user = new User
            {
                Name = "u1",
                Sex = "FEMALE"
            };

            RestResponse response = null;

            response = ApiClientInstance.CreateUsers(ConstantsTesting.WriteScope, user);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest),
                $"Status code 400 not returned. " +
                $"Expected status code 400 (BadRequest), " +
                $"but got {response.StatusCode}. Response content: {response.Content}");
        }

        [Test]
        public void AddUserDuplicate_UserNotAdded_Test()
        {
            var initialResponse = ApiClientInstance.GetUsers(ConstantsTesting.ReadScope);
            var initialUsers = JsonConvert.DeserializeObject<List<User>>(initialResponse.Content);
            int initialCount = initialUsers!.Count;

            var user = new User
            {
                Name = "u1",
                Sex = "FEMALE"
            }; 
            
            RestResponse response = ApiClientInstance.CreateUsers(ConstantsTesting.WriteScope, user);

            var finalResponse = ApiClientInstance.GetUsers(ConstantsTesting.ReadScope);
            var finalUsers = JsonConvert.DeserializeObject<List<User>>(finalResponse.Content);
            int finalCount = finalUsers!.Count;

            Assert.That(finalCount, Is.EqualTo(initialCount),
                $"Expected no duplicate user to be added. Initial count: {initialCount}, Final count: {finalCount}. " +
                $"Response content: {finalResponse.Content}");
        }
    }
}
