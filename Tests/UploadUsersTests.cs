using RestApiCSharp.ConstantsTestingGeneral;
using System.Net;

namespace RestApiCSharp.Tests
{
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
        public void UploadFile_UsersUploaded_Test()
        {
            var filePathOk = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestFiles", "UploadTask70.txt");
            
            ApiClientInstance.UploadFile(ConstantsTesting.WriteScope, filePathOk);

            var getUsersResponse = ApiClientInstance.GetUsers(ConstantsTesting.ReadScope);

            Assert.That(getUsersResponse.Content, Does.Contain("uploaduser1"),
                            $"The user 'uploaduser1' was not uploaded. Response content: {getUsersResponse.Content}");
        }

        [Test]
        public void UploadFileWrongZip_Return424_Test()
        {
            var filePathZipBad = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestFiles", "UploadWrongZipTask70.txt");
            var uploadUsers = ApiClientInstance.UploadFile(ConstantsTesting.WriteScope, filePathZipBad);

            Assert.That(uploadUsers.Content, Does.Contain("FailedDependency"),
                 $"Response content does not indicate FailedDependency. Content: {uploadUsers.Content}");
        }

        [Test]
        public void UploadFileWrongZip_UsersNotUploaded_Test()
        {
            var filePathZipBad = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestFiles", "UploadWrongZipTask70.txt");

            ApiClientInstance.UploadFile(ConstantsTesting.WriteScope, filePathZipBad);

            var getUsersResponse = ApiClientInstance.GetUsers(ConstantsTesting.ReadScope);

            Assert.That(getUsersResponse.Content, Does.Not.Contain("uploaduser2"),
                $"The user 'uploaduser2' was uploaded. Response content: {getUsersResponse.Content}");
        }

        [Test]
        public void UploadFileMissedField_Return409_Test()
        {
            var filePathMissReqField = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestFiles", "UploadMissFieldTask70.txt");
            var uploadUsers = ApiClientInstance.UploadFile(ConstantsTesting.WriteScope, filePathMissReqField);

            Assert.That(uploadUsers.Content, Does.Contain("Conflict"),
                 $"Response content does not indicate Conflict. Content: {uploadUsers.Content}");
        }

        [Test]
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
