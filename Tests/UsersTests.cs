using Allure.NUnit;
using Allure.NUnit.Attributes;
using Newtonsoft.Json;
using RestApiCSharp.ConstantsTestingGeneral;
using RestSharp;
using System.Net;

namespace RestApiCSharp.Tests
{
    [AllureNUnit]
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
        [AllureStep("Create user with all fields and verify 201 Created response")]
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
        [AllureStep("Verify user created with all fields is present in users list")]
        public void AddUserAllFields_UserAdded_Test()
        {
            RestResponse response = ApiClientInstance.GetUsers(ConstantsTesting.ReadScope);

            Assert.That(response.Content, Does.Contain("u1"),
                $"The user 'u1' was not added. Response content: {response.Content}");
        }

        [Test]
        [AllureIssue("BUG_AddUser_1")]
        [AllureStep("Verify zip code is removed after creating user with all fields")]
        public void AddUserAllFields_ZipCodeRemoved_Test()
        {
            RestResponse response = ApiClientInstance.GetZipCodes(ConstantsTesting.ReadScope);

            Assert.That(response.Content, Does.Not.Contain("oz1"),
                $"The zip code 'oz1' was not removed. Response content: {response.Content}");
        }

        [Test]
        [AllureStep("Create user with required fields only and verify 201 Created response")]
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
        [AllureStep("Verify user created with required fields only is present in users list")]
        public void AddUserReqFields_UserAdded_Test()
        {
            RestResponse response = ApiClientInstance.GetUsers(ConstantsTesting.ReadScope);

            Assert.That(response.Content, Does.Contain("u3"),
                $"The user 'u3' was not added. Response content: {response.Content}");
        }

        [Test]
        [AllureStep("Create user with incorrect zip code and verify FailedDependency response")]
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
        [AllureStep("Verify user is not created when incorrect zip code is provided")]
        public void AddUserIncorrectZipCode_UserNotAdded_Test()
        {
            RestResponse response = ApiClientInstance.GetUsers(ConstantsTesting.ReadScope);

            Assert.That(response.Content, Does.Not.Contain("u4"),
                $"The user 'u4' was not added. Response content: {response.Content}");
        }

        [Test]
        [AllureIssue("BUG_AddUser_2")]
        [AllureStep("Create duplicate user and verify 400 BadRequest response")]
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
        [AllureIssue("BUG_AddUser_3")]
        [AllureStep("Verify duplicate user is not added to users list")]
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
