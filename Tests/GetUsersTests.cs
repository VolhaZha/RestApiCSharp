using RestApiCSharp.ConstantsTestingGeneral;
using RestSharp;
using System.Net;

namespace RestApiCSharp.Tests
{
    public class GetUsersTests : BaseApiTest
    {

        [SetUp]
        public void Setup()
        {
            var users = new List<User>
            {
                new User { Age = 5, Name = "uGetUsersTests1", Sex = "FEMALE" },
                new User { Age = 15, Name = "uGetUsersTests2", Sex = "FEMALE" },
                new User { Age = 30, Name = "uGetUsersTests3", Sex = "MALE" },
                new User { Age = 50, Name = "uGetUsersTests4", Sex = "MALE" }
            };

            ApiClientInstance.CreateUsersList(ConstantsTesting.WriteScope, users);
        }

        [Test]
        public void GetUsers_Return200_Test()
        {
            RestResponse response = ApiClientInstance.GetUsers(ConstantsTesting.ReadScope);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK),
                $"Status code 200 not returned." +
                $"Expected status code 200 (OK), but got {response.StatusCode}. " +
                $"Response content: {response.Content}");
        }

        [Test]
        public void GetUsers_UserReceived_Test()
        {
            RestResponse response = ApiClientInstance.GetUsers(ConstantsTesting.ReadScope);

            var expectedUsers = new[] { "uGetUsersTests1", "uGetUsersTests2", "uGetUsersTests3", "uGetUsersTests4" };

            foreach (var user in expectedUsers)
            {
                Assert.That(response.Content, Does.Contain(user),
                    $"The user '{user}' was not received. Response content: {response.Content}");
            }
        }

        [Test]
        public void GetUsersOlderThan_Return200_Test()
        {
            RestResponse response = ApiClientInstance.GetUsers(ConstantsTesting.ReadScope, olderThan: "6");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK),
                $"Status code 200 not returned." +
                $"Expected status code 200 (OK), but got {response.StatusCode}. " +
                $"Response content: {response.Content}");
        }

        [Test]
        public void GetUsersOlderThan_UserAdded_Test()
        {
            RestResponse response = ApiClientInstance.GetUsers(ConstantsTesting.ReadScope, olderThan: "6");

            var expectedUsers = new[] { "uGetUsersTests2", "uGetUsersTests3", "uGetUsersTests4" };

            foreach (var user in expectedUsers)
            {
                Assert.That(response.Content, Does.Contain(user),
                    $"The user '{user}' was not received. Response content: {response.Content}");
            }
            Assert.That(response.Content, Does.Not.Contain("uGetUsersTests1"),
            $"The user '{"uGetUsersTests1"}' was received, though should not be. Response content: {response.Content}");
        }

        [Test]
        public void GetUsersYoungerThan_Return200_Test()
        {
            RestResponse response = ApiClientInstance.GetUsers(ConstantsTesting.ReadScope, youngerThan: "6");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK),
                $"Status code 200 not returned." +
                $"Expected status code 200 (OK), but got {response.StatusCode}. " +
                $"Response content: {response.Content}");
        }

        [Test]
        public void GetUsersYoungerThan_UserAdded_Test()
        {
            RestResponse response = ApiClientInstance.GetUsers(ConstantsTesting.ReadScope, youngerThan: "6");

            var expectedUsers = new[] { "uGetUsersTests1" };

            foreach (var user in expectedUsers)
            {
                Assert.That(response.Content, Does.Contain(user),
                    $"The user '{user}' was not received. Response content: {response.Content}");
            }
            Assert.That(response.Content, Does.Not.Contain("uGetUsersTests2"),
            $"The user '{"uGetUsersTests2"}' was received, though should not be. Response content: {response.Content}");
        }

        [Test]
        public void GetUsersSex_Return200_Test()
        {
            RestResponse response = ApiClientInstance.GetUsers(ConstantsTesting.ReadScope, sex: "MALE");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK),
                $"Status code 200 not returned." +
                $"Expected status code 200 (OK), but got {response.StatusCode}. " +
                $"Response content: {response.Content}");
        }

        [Test]
        public void GetUsersSex_UserAdded_Test()
        {
            RestResponse response = ApiClientInstance.GetUsers(ConstantsTesting.ReadScope, sex: "MALE");

            var expectedUsers = new[] { "uGetUsersTests3", "uGetUsersTests4" };

            foreach (var user in expectedUsers)
            {
                Assert.That(response.Content, Does.Contain(user),
                    $"The user '{user}' was not received. Response content: {response.Content}");
            }
            Assert.That(response.Content, Does.Not.Contain("uGetUsersTests1"),
            $"The user '{"uGetUsersTests1"}' was received, though should not be. Response content: {response.Content}");
        }
    }
}
