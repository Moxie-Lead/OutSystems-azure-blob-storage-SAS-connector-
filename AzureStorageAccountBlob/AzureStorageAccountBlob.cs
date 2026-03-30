using OutSystems.NssAzureStorageAccountBlob;

namespace AzureStorageAccountBlob_ODC
{
    public class AzureStorageAccountBlob : IAzureStorageAccountBlob
    {
        AzureStorageAccountProvider aSAP = new AzureStorageAccountProvider();


        /// <summary>
        /// Creates a container 
        /// </summary>
        /// <param name="connectionString">The connection string for the Storage Account</param>
        /// <param name="containerName">The name of the container. The name will be changed to LowerCase if this is not already the case</param>
        /// <param name="isError">Indicates if the operation was successful</param>
        /// <param name="errorMessage">The errormessage if the action wasn't successful</param>
        public void CreateContainer(string connectionString, string containerName, out bool isError, out string errorMessage)
        {
            aSAP.CreateContainer(connectionString, containerName, out isError, out errorMessage);
        }


        /// <summary>
        /// Deletes a container 
        /// </summary>
        /// <param name="connectionString">The connection string for the Storage Account</param>
        /// <param name="containerName">The name of the container. The name will be changed to LowerCase if this is not already the case</param>
        /// <param name="isError">Indicates if the operation was successful</param>
        /// <param name="errorMessage">The errormessage if the action wasn't successful</param>
        public void DeleteContainer(string connectionString, string containerName, out bool isError, out string errorMessage)
        {

            aSAP.DeleteContainer(connectionString, containerName, out isError, out errorMessage);
        }


        /// <summary>
        /// Uploads a blob to a Container
        /// </summary>
        /// <param name="connectionString">The connection string for the Storage Account</param>
        /// <param name="containerName">The name of the container. The name will be changed to LowerCase if this is not already the case</param>
        /// <param name="blobName">The name of the Blob</param>
        /// <param name="blob">The binary contents of the blob</param>
        /// <param name="isError">Indicates if the operation was successful</param>
        /// <param name="errorMessage">The errormessage if the action wasn't successful</param>
        public void UploadBlob(string connectionString, string containerName, string blobName, byte[] blob, out bool isError, out string errorMessage)
        {
            aSAP.UploadBlob(connectionString, containerName, blobName, blob, out isError, out errorMessage);
        }



        /// <summary>
        /// Deletes a from Blob from a Container
        /// </summary>
        /// <param name="connectionString">The connection string for the Storage Account</param>
        /// <param name="containerName">The name of the container. The name will be changed to LowerCase if this is not already the case</param>
        /// <param name="blobName">The name of the blob</param>
        /// <param name="isError">Indicates if the operation was successful</param>
        /// <param name="errorMessage">The errormessage if the action wasn't successful</param>
        public void DeleteBlob(string connectionString, string containerName, string blobName, out bool isError, out string errorMessage)
        {
            aSAP.DeleteBlob(connectionString, containerName, blobName, out isError, out errorMessage);
        }



        /// <summary>
        /// Creates an Uri with a SAS token for a Blob
        /// </summary>
        /// <param name="connectionString">The connection string for the Storage Account</param>
        /// <param name="containerName">The name of the container. The name will be changed to LowerCase if this is not already the case</param>
        /// <param name="blobName">The name of the blob</param>
        /// <param name="mode">The SAS permission mode (e.g. "upload", "read", "uploadRead").</param>
        /// <param name="durationInMinutes">The lifetime of the SAS Token in minutes. The default value is 10 minutes</param>
        /// <param name="uri">The URI that end-users can use to access the blob</param>
        /// <param name="isError">Indicates if the operation was successful</param>
        /// <param name="errorMessage">The errormessage if the action wasn't successful</param>
        public void GetURLForBlobWithSASToken(string connectionString, string containerName, string blobName, string mode, int durationInMinutes, out string uri, out bool isError, out string errorMessage)
        {
            aSAP.GetURLForBlobWithSASToken(connectionString, containerName, blobName, mode, durationInMinutes, out uri, out isError, out errorMessage);
        }


        /// <summary>
        /// Downloads a from Blob from a Container
        /// </summary>
        /// <param name="connectionString">The connection string for the Storage Account</param>
        /// <param name="containerName">The name of the container. The name will be changed to LowerCase if this is not already the case</param>
        /// <param name="blobName">The name of the Blob</param>
        /// <param name="blob"></param>
        /// <param name="isError">Indicates if the operation was successful</param>
        /// <param name="errorMessage">The errormessage if the action wasn't successful</param>
        public void DownloadBlob(string connectionString, string containerName, string blobName, out byte[] blob, out bool isError, out string errorMessage)
        {
            blob = aSAP.DownloadBlob(connectionString, containerName, blobName, out isError, out errorMessage);
        }
    }
}
