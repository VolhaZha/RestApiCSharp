using Allure.NUnit;
using Allure.NUnit.Attributes;
using RestApiCSharp.ConstantsTestingGeneral;
using System.Net;

namespace RestApiCSharp.Tests
{
    [AllureNUnit]
    public class UploadUsersTests : BaseApiTest
    {
        [SetUp]
        public void Setup()
        {
            var zipCodes = new List<string> 
            { "oz", "oz1", "oz2", "oz3", "oz4", "oz5", "oz6", "oz7" };

            ApiClientInstance.ExpandZipCodes(ConstantsTesting.WriteScope, zipCodes);
        }

        [Test]
        [AllureStep("Upload users file and verify 201 Created response")]
        public void UploadFile_Return201_Test()
        {
            var filePathOk = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestFiles", "UploadTask70.txt");
            var uploadUsers = ApiClientInstance.UploadFile(ConstantsTesting.WriteScope, filePathOk);

            Assert.That(uploadUsers.StatusCode, Is.EqualTo(HttpStatusCode.Created),
                $"Status code 201 not returned." +
                $"Expected status code 201 (Created), but got {uploadUsers.StatusCode}. " +
                $"Response content: {uploadUsers.Content}");
        }

        [Test]
        [AllureStep("Upload users file and verify users are successfully uploaded")]
        public void UploadFile_UsersUploaded_Test()
        {
            var filePathOk = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestFiles", "UploadTask70.txt");
            
            ApiClientInstance.UploadFile(ConstantsTesting.WriteScope, filePathOk);

            var getUsersResponse = ApiClientInstance.GetUsers(ConstantsTesting.ReadScope);

            Assert.That(getUsersResponse.Content, Does.Contain("uploaduser1"),
                            $"The user 'uploaduser1' was not uploaded. Response content: {getUsersResponse.Content}");
        }

        [Test]
        [AllureIssue("BUG_UploadUser_2")]
        [AllureStep("Upload users file with incorrect zip code and verify FailedDependency response")]
        public void UploadFileWrongZip_Return424_Test()
        {
            var filePathZipBad = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestFiles", "UploadWrongZipTask70.txt");
            var uploadUsers = ApiClientInstance.UploadFile(ConstantsTesting.WriteScope, filePathZipBad);

            Assert.That(uploadUsers.Content, Does.Contain("FailedDependency"),
                 $"Response content does not indicate FailedDependency. Content: {uploadUsers.Content}");
        }

        [Test]
        [AllureStep("Upload users file with incorrect zip code and verify users are not uploaded")]
        public void UploadFileWrongZip_UsersNotUploaded_Test()
        {
            var filePathZipBad = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestFiles", "UploadWrongZipTask70.txt");

            ApiClientInstance.UploadFile(ConstantsTesting.WriteScope, filePathZipBad);

            var getUsersResponse = ApiClientInstance.GetUsers(ConstantsTesting.ReadScope);

            Assert.That(getUsersResponse.Content, Does.Not.Contain("uploaduser2"),
                $"The user 'uploaduser2' was uploaded. Response content: {getUsersResponse.Content}");
        }

        [Test]
        [AllureIssue("BUG_UploadUser_1")]
        [AllureStep("Upload users file with missing required fields and verify Conflict response")]
        public void UploadFileMissedField_Return409_Test()
        {
            var filePathMissReqField = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestFiles", "UploadMissFieldTask70.txt");
            var uploadUsers = ApiClientInstance.UploadFile(ConstantsTesting.WriteScope, filePathMissReqField);

            Assert.That(uploadUsers.Content, Does.Contain("Conflict"),
                 $"Response content does not indicate Conflict. Content: {uploadUsers.Content}");
        }

        [Test]
        [AllureStep("Upload users file with missing required fields and verify users are not uploaded")]
        public void UploadFileMissedField_UsersNotUploaded_Test()
        {
            var filePathMissReqField = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestFiles", "UploadMissFieldTask70.txt");

            ApiClientInstance.UploadFile(ConstantsTesting.WriteScope, filePathMissReqField);

            var getUsersResponse = ApiClientInstance.GetUsers(ConstantsTesting.ReadScope);

            Assert.That(getUsersResponse.Content, Does.Not.Contain("uploaduser3"),
                $"The user 'uploaduser3' was uploaded. Response content: {getUsersResponse.Content}");
        }
    }
}
