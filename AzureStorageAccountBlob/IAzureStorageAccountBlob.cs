using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using OutSystems.ExternalLibraries.SDK;

namespace AzureStorageAccountBlob_ODC
{
    [OSInterface(
    Name = "AzureStorageAccount_Blob",
    IconResourceName = "AzureStorageAccountBlob.Logo.png",
    Description = "This is an implementation of the latest Azure Storage SDK. It supports a wide variety of features with the most notable the Append option and the Pagination option. This component currently supports the most used capabilities of the Azure Storage SDK and more will be added in the near future. To use the demo you need to have an Azure Storage Account with its Connection String available. The demo shows you how to use most features of the component. Please use this as a guideline for creating your own applications."
    )]
    public interface IAzureStorageAccountBlob
    {
        /// <summary>
        /// Creates a container 
        /// </summary>
        /// <param name="connectionString">The connection string for the Storage Account</param>
        /// <param name="containerName">The name of the container. The name will be changed to LowerCase if this is not already the case</param>
        /// <param name="isError">Indicates if the operation was successful</param>
        /// <param name="errorMessage">The errormessage if the action wasn't successful</param>
        public void CreateContainer(string connectionString, string containerName, out bool isError, out string errorMessage);



        /// <summary>
        /// Deletes a container 
        /// </summary>
        /// <param name="connectionString">The connection string for the Storage Account</param>
        /// <param name="containerName">The name of the container. The name will be changed to LowerCase if this is not already the case</param>
        /// <param name="isError">Indicates if the operation was successful</param>
        /// <param name="errorMessage">The errormessage if the action wasn't successful</param>
        public void DeleteContainer(string connectionString, string containerName, out bool isError, out string errorMessage);



        /// <summary>
        /// Uploads a blob to a Container
        /// </summary>
        /// <param name="connectionString">The connection string for the Storage Account</param>
        /// <param name="containerName">The name of the container. The name will be changed to LowerCase if this is not already the case</param>
        /// <param name="blobName">The name of the Blob</param>
        /// <param name="blob">The binary contents of the blob</param>
        /// <param name="isError">Indicates if the operation was successful</param>
        /// <param name="errorMessage">The errormessage if the action wasn't successful</param>
        public void UploadBlob(string connectionString, string containerName, string blobName, byte[] blob, out bool isError, out string errorMessage);



        /// <summary>
        /// Downloads a from Blob from a Container
        /// </summary>
        /// <param name="connectionString">The connection string for the Storage Account</param>
        /// <param name="containerName">The name of the container. The name will be changed to LowerCase if this is not already the case</param>
        /// <param name="blobName">The name of the Blob</param>
        /// <param name="blob"></param>
        /// <param name="isError">Indicates if the operation was successful</param>
        /// <param name="errorMessage">The errormessage if the action wasn't successful</param>
        public void DownloadBlob(string connectionString, string containerName, string blobName, out byte[] blob, out bool isError, out string errorMessage);



        /// <summary>
        /// Deletes a from Blob from a Container
        /// </summary>
        /// <param name="connectionString">The connection string for the Storage Account</param>
        /// <param name="containerName">The name of the container. The name will be changed to LowerCase if this is not already the case</param>
        /// <param name="blobName">The name of the blob</param>
        /// <param name="isError">Indicates if the operation was successful</param>
        /// <param name="errorMessage">The errormessage if the action wasn't successful</param>
        public void DeleteBlob(string connectionString, string containerName, string blobName, out bool isError, out string errorMessage);



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
        public void GetURLForBlobWithSASToken(string connectionString, string containerName, string blobName, string mode, int durationInMinutes, out string uri, out bool isError, out string errorMessage);

    }
}
