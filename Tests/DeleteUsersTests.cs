using Allure.NUnit;
using Allure.NUnit.Attributes;
using RestApiCSharp.ConstantsTestingGeneral;
using System.Net;

namespace RestApiCSharp.Tests
{
    [AllureNUnit]
    public class DeleteUsersTests : BaseApiTest
    {
        [SetUp]
        public void Setup()
        {
            var zipCodes = new List<string> 
            { "oz", "oz1", "oz2", "oz3" };

            ApiClientInstance.ExpandZipCodes(ConstantsTesting.WriteScope, zipCodes);
        }

        [Test]
        [AllureIssue("BUG_DeleteUser_1")]
        [AllureStep("Delete user and validate response")]
        public void DeleteUser_AllFields_Return204_UserDeleted_ZipCodeReturned_Test()
        {
            var user = new User
            {
                Age = 0,
                Name = "u",
                Sex = "FEMALE",
                ZipCode = "oz"
            };

            ApiClientInstance.CreateUsers(ConstantsTesting.WriteScope, user);

            var deleteUsersResponse = ApiClientInstance.DeleteUser(ConstantsTesting.WriteScope, user);
            var getUsersResponse = ApiClientInstance.GetUsers(ConstantsTesting.ReadScope);
            var getZipCodesResponse = ApiClientInstance.GetZipCodes(ConstantsTesting.ReadScope);

            Assert.That(deleteUsersResponse.StatusCode, Is.EqualTo(HttpStatusCode.NoContent),
                            $"Status code 204 not returned." +
                            $"Expected status code 204 (No Content), but got {deleteUsersResponse.StatusCode}. " +
                            $"Response content: {deleteUsersResponse.Content}");
            Assert.That(getUsersResponse.Content, Does.Not.Contain("u"),
                            $"The user 'u' was not deleted. Response content: {getUsersResponse.Content}");
            Assert.That(getZipCodesResponse.Content, Does.Contain("oz"),
                            $"Not all available zip codes found. Response content: {getZipCodesResponse.Content}");
        }

        [Test]
        [AllureIssue("BUG_DeleteUser_2")]
        [AllureStep("Delete user with required fields and validate response")]
        public void DeleteUser_RequiredFields_Return204_UserDeleted_ZipCodeReturned_Test()
        {
            var user = new User
            {
                Age = 0,
                Name = "u1",
                Sex = "FEMALE",
                ZipCode = "oz1"
            };

            var userRequiredFields = new User
            {
                Name = "u1",
                Sex = "FEMALE"
            };

            ApiClientInstance.CreateUsers(ConstantsTesting.WriteScope, user);

            var deleteUsersResponse = ApiClientInstance.DeleteUser(ConstantsTesting.WriteScope, userRequiredFields);
            var getUsersResponse = ApiClientInstance.GetUsers(ConstantsTesting.ReadScope);
            var getZipCodesResponse = ApiClientInstance.GetZipCodes(ConstantsTesting.ReadScope);

            Assert.That(deleteUsersResponse.StatusCode, Is.EqualTo(HttpStatusCode.NoContent),
                            $"Status code 204 not returned." +
                            $"Expected status code 204 (No Content), but got {deleteUsersResponse.StatusCode}. " +
                            $"Response content: {deleteUsersResponse.Content}");
            Assert.That(getUsersResponse.Content, Does.Not.Contain("u1"),
                            $"The user 'u1' was not deleted. Response content: {getUsersResponse.Content}");
            Assert.That(getZipCodesResponse.Content, Does.Contain("oz1"),
                            $"Not all available zip codes found. Response content: {getZipCodesResponse.Content}");
        }

        [Test]
        [AllureIssue("BUG_DeleteUser_3")]
        [AllureStep("Delete user and validate response")]
        public void DeleteUser_RequiredFieldsBoth_Return204_UserDeleted_Test()
        {
            var user = new User
            {
                Name = "u2",
                Sex = "FEMALE"
            };

            ApiClientInstance.CreateUsers(ConstantsTesting.WriteScope, user);

            var deleteUsersResponse = ApiClientInstance.DeleteUser(ConstantsTesting.WriteScope, user);
            var getUsersResponse = ApiClientInstance.GetUsers(ConstantsTesting.ReadScope);

            Assert.That(deleteUsersResponse.StatusCode, Is.EqualTo(HttpStatusCode.NoContent),
                            $"Status code 204 not returned." +
                            $"Expected status code 204 (No Content), but got {deleteUsersResponse.StatusCode}. " +
                            $"Response content: {deleteUsersResponse.Content}");
            Assert.That(getUsersResponse.Content, Does.Not.Contain("u2"),
                            $"The user 'u2' was not deleted. Response content: {getUsersResponse.Content}");
        }

        [Test]
        [AllureStep("Delete user with missing required fields and check for conflict")]
        public void DeleteUser_NotAllReqFields_Return409_UserNotDeleted_Test()
        {
            var user = new User
            {
                Age = 0,
                Name = "u3",
                Sex = "FEMALE",
                ZipCode = "oz3"
            };

            var userNotAllReqFields = new User
            {
                Age = 0,
                Name = "u3",
                ZipCode = "oz3"
            };

            ApiClientInstance.CreateUsers(ConstantsTesting.WriteScope, user);

            var deleteUsersResponse = ApiClientInstance.DeleteUser(ConstantsTesting.WriteScope, userNotAllReqFields);
            var getUsersResponse = ApiClientInstance.GetUsers(ConstantsTesting.ReadScope);

            Assert.That(deleteUsersResponse.Content, Does.Contain("Conflict"),
                 $"Response content does not indicate FailedDependency. Content: {deleteUsersResponse.Content}");
            Assert.That(getUsersResponse.Content, Does.Contain("u3"),
                            $"The user 'u1' was not deleted. Response content: {getUsersResponse.Content}");
        }
    }
}
