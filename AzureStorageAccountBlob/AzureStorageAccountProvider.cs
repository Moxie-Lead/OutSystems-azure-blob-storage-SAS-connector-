using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Azure;
using Azure.Core;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;

namespace OutSystems.NssAzureStorageAccountBlob
{
    public partial class AzureStorageAccountProvider

    // quickstart: https://docs.microsoft.com/en-us/azure/storage/blobs/storage-quickstart-blobs-dotnet

    {
        /// <summary>
        /// Creates a container 
        /// </summary>
        /// <param name="connectionString">The connection string for the Storage Account</param>
        /// <param name="containerName">The name of the container. The name will be changed to LowerCase if this is not already the case</param>
        /// <param name="isError">Indicates if the operation was successful</param>
        /// <param name="errorMessage">The errormessage if the action wasn't successful</param>
        public bool CreateContainer(string connectionString, string containerName, out bool isError, out string errorMessage)
        {
            isError = false;
            errorMessage = "";

            containerName = containerName.ToLower();
            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                BlobContainerClient containerClient = blobServiceClient.CreateBlobContainer(containerName);
            }
            catch (Exception e)
            {
                isError = true;
                errorMessage = e.Message;
                return false;
            }

            return true;
        }


        /// <summary>
        /// Deletes a container 
        /// </summary>
        /// <param name="connectionString">The connection string for the Storage Account</param>
        /// <param name="containerName">The name of the container. The name will be changed to LowerCase if this is not already the case</param>
        /// <param name="isError">Indicates if the operation was successful</param>
        /// <param name="errorMessage">The errormessage if the action wasn't successful</param>
        public bool DeleteContainer(string connectionString, string containerName, out bool isError, out string errorMessage)
        {
            isError = false;
            errorMessage = "";

            containerName = containerName.ToLower();
            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                blobServiceClient.DeleteBlobContainer(containerName);
            }
            catch (Exception e)
            {
                isError = true;
                errorMessage = e.Message;
                return false;
            }

            return true;
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
        public bool UploadBlob(string connectionString, string containerName, string blobName, byte[] blob, out bool isError, out string errorMessage)
        {
            isError = false;
            errorMessage = "";

            containerName = containerName.ToLower();
            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                
                // get the container
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                
                // if container doens't exist create it
                if (containerClient == null)
                {
                    containerClient = blobServiceClient.CreateBlobContainer(containerName);
                }

                // get a blob reference
                BlobClient blobClient = containerClient.GetBlobClient(blobName);

                // create a memorystream of the blob
                MemoryStream blobStream = new MemoryStream(blob);
                blobClient.Upload(blobStream);
            }
            catch (Exception e)
            {
                isError = true;
                errorMessage = e.Message;
                return false;
            }

            return true;
        }


        /// <summary>
        /// Downloads a from Blob from a Container
        /// </summary>
        /// <param name="connectionString">The connection string for the Storage Account</param>
        /// <param name="containerName">The name of the container. The name will be changed to LowerCase if this is not already the case</param>
        /// <param name="blobName">The name of the Blob</param>
        /// <param name="isError">Indicates if the operation was successful</param>
        /// <param name="errorMessage">The errormessage if the action wasn't successful</param>
        public byte[] DownloadBlob(string connectionString, string containerName, string blobName, out bool isError, out string errorMessage)
        {
            isError = false;
            errorMessage = "";

            containerName = containerName.ToLower();
            MemoryStream downloadFileStream = new MemoryStream();

            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

                // get the container
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                // get a blob reference
                BlobClient blobClient = containerClient.GetBlobClient(blobName);
                // download blob
                BlobDownloadInfo blob = blobClient.Download();
                downloadFileStream = new MemoryStream();
                blob.Content.CopyTo(downloadFileStream);
            }
            catch (Exception e)
            {
                isError = true;
                errorMessage = e.Message;
            }

            // write blob to byte array and dispose of the MemoryStream object
            byte[] blobContent = downloadFileStream.ToArray();
            downloadFileStream.Dispose();

            // return the downloaded blob
            return blobContent;
        }


        /// <summary>
        /// Copies a blob to a new destination. The source blob needs to be provided via a URL with a SAS Token. Use the action GetURLForBlobWithSASToken for getting a URL with a SAS Token.
        /// </summary>
        /// <param name="connectionString">The connection string of the destination Storage Account</param>
        /// <param name="containerName">The name of the destination Container in which the target Blob will be copied to</param>
        /// <param name="blobName">The name of the destinaion Blob</param>
        /// <param name="SASUri">The SAS key of the origin blob</param>
        /// <param name="isError">Indicates if the operation was successful</param>
        /// <param name="errorMessage">The errormessage if the action wasn't successful</param>
        public void CopyBlobViaSAS(string connectionString, string containerName, string blobName, string SASUri, out bool isError, out string errorMessage)
        {
            isError = false;
            errorMessage = "";

            containerName = containerName.ToLower();

            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

                // get the containers
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                if (containerClient == null)
                {
                    isError = true;
                    errorMessage = "The destination Container is not found.";
                    return;
                }

                // get a blob reference
                BlobClient blobClient = containerClient.GetBlobClient(blobName);

                // check if the blob exists
                if (blobClient.Exists())
                {
                    isError = true;
                    errorMessage = "The destination Blob already exists. The copy action has be stopped.";
                    return;
                }
                
                // Start the copy action
                blobClient.StartCopyFromUri(new Uri(SASUri));
            }
            catch (Exception e)
            {
                isError = true;
                errorMessage = e.Message;
                return;
            }
        }


        /// <summary>
        /// Appends a Blob to another Blob
        /// </summary>
        /// <param name="connectionString">The connection string for the Storage Account</param>
        /// <param name="containerName">The name of the Container in which the target Blob (will) reside(s). The name will be changed to LowerCase if this is not already the case</param>
        /// <param name="blobName">The name of the target Blob (ie, the blob that will get data append to)</param>
        /// <param name="sourceSASUrl">The URL containing a SAS key for the blob that will be appended to</param>
        /// <param name="isError">Indicates if the operation was successful</param>
        /// <param name="errorMessage">The errormessage if the action wasn't successful</param>
        public bool AppendBlobToBlobViaSASUrl(string connectionString, string containerName, string blobName, string sourceSASUrl, out bool isError, out string errorMessage)
        {
            isError = false;
            errorMessage = "";

            containerName = containerName.ToLower();

            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

                // get the containers
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                // get the blobs
                AppendBlobClient appendBlob = containerClient.GetAppendBlobClient(blobName);

                // create the appendBlob if needed
                appendBlob.CreateIfNotExists();

                // append the data to the blob
                appendBlob.AppendBlockFromUri(new Uri(sourceSASUrl));

                return true;
            }
            catch (Exception e)
            {
                isError = true;
                errorMessage = e.Message;
                return false;
            }
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
            isError = false;
            errorMessage = "";

            containerName = containerName.ToLower();
            MemoryStream downloadFileStream = new MemoryStream();

            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

                // get the container
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                // get a blob reference
                BlobClient blobClient = containerClient.GetBlobClient(blobName);
                // delete blob
                blobClient.Delete();
            }
            catch (Exception e)
            {
                isError = true;
                errorMessage = e.Message;
            }

            // return the downloaded blob
            return;
        }


        // https://docs.microsoft.com/en-us/azure/storage/blobs/storage-blob-user-delegation-sas-create-dotnet

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
            isError = false;
            errorMessage = "";
            uri = "";
            Regex accountNameRegex = new Regex("(?<=AccountName=)[a-z0-9]+");
            Regex accountKeyRegex = new Regex("(?<=AccountKey=).*;");
            string accountName = accountNameRegex.Match(connectionString).Value;
            string accountKey = accountKeyRegex.Match(connectionString).Value;
            accountKey = accountKey.Remove(accountKey.Length - 1); // remove the last char (;)

            if (accountName == "")
            {
                isError = true;
                errorMessage = "Accountname not found in connectionString";
            }    

            containerName = containerName.ToLower();
            MemoryStream downloadFileStream = new MemoryStream();

            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

                var permissions = MapPermissions(mode);

                BlobSasBuilder sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = containerName,
                    BlobName = blobName,
                    Resource = "b",
                    StartsOn = DateTimeOffset.UtcNow,
                    ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(durationInMinutes)
                };

                // Specify read permissions for the SAS.
                sasBuilder.SetPermissions(permissions);

                // Use the key to get the SAS token.
                StorageSharedKeyCredential cred = new StorageSharedKeyCredential(accountName, accountKey);
                string sasToken = sasBuilder.ToSasQueryParameters(cred).ToString();

                // Construct the full URI, including the SAS token.
                UriBuilder fullUri = new UriBuilder()
                {
                    Scheme = "https",
                    Host = string.Format("{0}.blob.core.windows.net", accountName),
                    Path = string.Format("{0}/{1}", containerName, blobName),
                    Query = sasToken
                };

                uri = fullUri.ToString();
            }
            catch (Exception e)
            {
                isError = true;
                errorMessage = e.Message;
            }

            // return the downloaded blob
            return;
        }

        private static BlobSasPermissions MapPermissions(string mode)
        {
            return mode.ToLower() switch
            {
                "upload" => BlobSasPermissions.Create | BlobSasPermissions.Write,
                "read" => BlobSasPermissions.Read,
                "uploadread" => BlobSasPermissions.Create | BlobSasPermissions.Write | BlobSasPermissions.Read,

                // default safe fallback
                _ => throw new ArgumentException("Invalid SAS mode")
            };
        }


        // TODO GetSASTokenForContainer
        public void GetSASTokenForContainer()
        {

        }
    }
}
