using RestApiCSharp.ConstantsTestingGeneral;
using RestApiCSharp.Models;
using System.Net;

namespace RestApiCSharp.Tests
{
    public class UpdateUsersTests : BaseApiTest
    {
        [SetUp]
        public void Setup()
        {
            var zipCodes = new List<string> 
            { "oz", "oz1", "oz2", "oz3", "oz4", "oz5", "oz6", "oz7", "oz02", "oz12", "oz22", "oz32", "oz42" };

            ApiClientInstance.ExpandZipCodes(ConstantsTesting.WriteScope, zipCodes);
        }

        [Test]
        public void UpdateUserPatch_Return200_UserUpdated_Test()
        {
            var users = new List<User>
            {
                new User { Age = 1, Name = "u", Sex = "FEMALE", ZipCode = "oz"}
            };

            ApiClientInstance.CreateUsersList(ConstantsTesting.WriteScope, users);

            var userUpdate = new UserUpdate
            {
                UserNewValues = new User { Age = 20, Name = "New", Sex = "MALE", ZipCode = "oz02" },
                UserToChange = new User { Age = 1, Name = "u", Sex = "FEMALE", ZipCode = "oz" }
            };

            var response1 = ApiClientInstance.UpdateUsersPatch(ConstantsTesting.WriteScope, userUpdate);
            var response2 = ApiClientInstance.GetUsers(ConstantsTesting.ReadScope);

            Assert.That(response1.StatusCode, Is.EqualTo(HttpStatusCode.OK),
                            $"Status code 200 not returned." +
                            $"Expected status code 200 (OK), but got {response1.StatusCode}. " +
                            $"Response content: {response1.Content}");
            Assert.That(response2.Content, Does.Contain("New"),
                            $"The user 'New' was not added. Response content: {response2.Content}");
        }

        [Test]
        public void UpdateUserPut_Return200_UserUpdated_Test()
        {
            var users = new List<User>
            {
                new User { Age = 1, Name = "u1", Sex = "FEMALE", ZipCode = "oz1"}
            };

            ApiClientInstance.CreateUsersList(ConstantsTesting.WriteScope, users);

            var userUpdate = new UserUpdate
            {
                UserNewValues = new User { Age = 20, Name = "New1", Sex = "MALE", ZipCode = "oz12" },
                UserToChange = new User { Age = 1, Name = "u1", Sex = "FEMALE", ZipCode = "oz1" }
            };

            var response1 = ApiClientInstance.UpdateUsersPut(ConstantsTesting.WriteScope, userUpdate);
            var response2 = ApiClientInstance.GetUsers(ConstantsTesting.ReadScope);

            Assert.That(response1.StatusCode, Is.EqualTo(HttpStatusCode.OK),
                            $"Status code 200 not returned." +
                            $"Expected status code 200 (OK), but got {response1.StatusCode}. " +
                            $"Response content: {response1.Content}");
            Assert.That(response2.Content, Does.Contain("New"),
                            $"The user 'New' was not added. Response content: {response2.Content}");
        }

        [Test]
        public void UpdateUserIncorrectZipCodePatch_Return424_UserNotUpdated_Test()
        {
            var users = new List<User>
            {
                new User { Age = 1, Name = "u2", Sex = "FEMALE", ZipCode = "oz2"}
            };

            ApiClientInstance.CreateUsersList(ConstantsTesting.WriteScope, users);

            var userUpdate = new UserUpdate
            {
                UserNewValues = new User { Age = 20, Name = "New2", Sex = "MALE", ZipCode = "oz222" },
                UserToChange = new User { Age = 1, Name = "u2", Sex = "FEMALE", ZipCode = "oz2" }
            };

            var response1 = ApiClientInstance.UpdateUsersPatch(ConstantsTesting.WriteScope, userUpdate);
            var response2 = ApiClientInstance.GetUsers(ConstantsTesting.ReadScope);

            Assert.That(response1.Content, Does.Contain("FailedDependency"),
                 $"Response content does not indicate FailedDependency. Content: {response1.Content}");
            Assert.That(response2.Content, Does.Not.Contain("New2"),
                $"The user 'New2' was added. Response content: {response2.Content}");
        }

        [Test]
        public void UpdateUserIncorrectZipCodePut_Return424_UserNotUpdated_Test()
        {
            var users = new List<User>
            {
                new User { Age = 1, Name = "u3", Sex = "FEMALE", ZipCode = "oz3"}
            };

            ApiClientInstance.CreateUsersList(ConstantsTesting.WriteScope, users);

            var userUpdate = new UserUpdate
            {
                UserNewValues = new User { Age = 20, Name = "New3", Sex = "MALE", ZipCode = "oz332" },
                UserToChange = new User { Age = 1, Name = "u3", Sex = "FEMALE", ZipCode = "oz3" }
            };

            var response1 = ApiClientInstance.UpdateUsersPut(ConstantsTesting.WriteScope, userUpdate);
            var response2 = ApiClientInstance.GetUsers(ConstantsTesting.ReadScope);

            Assert.That(response1.Content, Does.Contain("FailedDependency"),
                 $"Response content does not indicate FailedDependency. Content: {response1.Content}");
            Assert.That(response2.Content, Does.Not.Contain("New3"),
                $"The user 'New3' was added. Response content: {response2.Content}");
        }

        [Test]
        public void UpdateUserNotAllReqFieldsPatch_Return409_Test()
        {
            var users = new List<User>
            {
                new User { Age = 1, Name = "u4", Sex = "FEMALE", ZipCode = "oz4"}
            };

            ApiClientInstance.CreateUsersList(ConstantsTesting.WriteScope, users);

            var userUpdate = new UserUpdate
            {
                UserNewValues = new User { Age = 20, Name = "New4", ZipCode = "oz42" },
                UserToChange = new User { Age = 1, Name = "u4", Sex = "FEMALE", ZipCode = "oz4" }
            };

            var response1 = ApiClientInstance.UpdateUsersPatch(ConstantsTesting.WriteScope, userUpdate);

            Assert.That(response1.Content, Does.Contain("Conflict"),
                 $"Response content does not indicate Conflict. Content: {response1.Content}");
        }

        [Test]
        public void UpdateUserNotAllReqFieldsPatch_UserNotUpdated_Test()
        {
            var users = new List<User>
            {
                new User { Age = 1, Name = "u5", Sex = "FEMALE", ZipCode = "oz5"}
            };

            ApiClientInstance.CreateUsersList(ConstantsTesting.WriteScope, users);

            var userUpdate = new UserUpdate
            {
                UserNewValues = new User { Age = 20, Name = "New4", ZipCode = "oz42" },
                UserToChange = new User { Age = 1, Name = "u5", Sex = "FEMALE", ZipCode = "oz5" }
            };

            var response1 = ApiClientInstance.UpdateUsersPatch(ConstantsTesting.WriteScope, userUpdate);
            var response2 = ApiClientInstance.GetUsers(ConstantsTesting.ReadScope);

            Assert.That(response2.Content, Does.Not.Contain("New4"),
                $"The user 'New4' was added. Response content: {response2.Content}");
            Assert.That(response2.Content, Does.Contain("u5"),
                $"The user 'u5' was removed. Response content: {response2.Content}");
        }

        [Test]
        public void UpdateUserNotAllReqFieldsPut_Return409_Test()
        {
            var users = new List<User>
            {
                new User { Age = 1, Name = "u6", Sex = "FEMALE", ZipCode = "oz6"}
            };

            ApiClientInstance.CreateUsersList(ConstantsTesting.WriteScope, users);

            var userUpdate = new UserUpdate
            {
                UserNewValues = new User { Age = 20, Name = "New4", ZipCode = "oz42" },
                UserToChange = new User { Age = 1, Name = "u6", Sex = "FEMALE", ZipCode = "oz6" }
            };

            var response1 = ApiClientInstance.UpdateUsersPut(ConstantsTesting.WriteScope, userUpdate);
            var response2 = ApiClientInstance.GetUsers(ConstantsTesting.ReadScope);

            Assert.That(response1.Content, Does.Contain("Conflict"),
                 $"Response content does not indicate Conflict. Content: {response1.Content}");
        }

        [Test]
        public void UpdateUserNotAllReqFieldsPut_UserNotUpdated_Test()
        {
            var users = new List<User>
            {
                new User { Age = 1, Name = "u7", Sex = "FEMALE", ZipCode = "oz7"}
            };

            ApiClientInstance.CreateUsersList(ConstantsTesting.WriteScope, users);

            var userUpdate = new UserUpdate
            {
                UserNewValues = new User { Age = 20, Name = "New4", ZipCode = "oz42" },
                UserToChange = new User { Age = 1, Name = "u7", Sex = "FEMALE", ZipCode = "oz7" }
            };

            var response1 = ApiClientInstance.UpdateUsersPut(ConstantsTesting.WriteScope, userUpdate);
            var response2 = ApiClientInstance.GetUsers(ConstantsTesting.ReadScope);

            Assert.That(response2.Content, Does.Not.Contain("New4"),
                $"The user 'New4' was added. Response content: {response2.Content}");
            Assert.That(response2.Content, Does.Contain("u7"),
                $"The user 'u7' was removed. Response content: {response2.Content}");
        }
    }
}
