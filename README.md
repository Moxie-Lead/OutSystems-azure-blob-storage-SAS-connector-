# Azure Blob Storage SAS Connector (OutSystems ODC)

## Overview

The **Azure Blob Storage SAS Connector** enables seamless integration between OutSystems Developer Cloud (ODC) applications and Azure Blob Storage using Shared Access Signatures (SAS).

Built on top of the official Azure SDK for .NET, this connector not only allows interaction with blob storage but also supports dynamic generation of SAS tokens, eliminating the need to pre-generate them externally.

---

## Features

* Upload files (blobs) to Azure Blob Storage
* Download blobs
* Delete blobs
* Generate SAS tokens programmatically using Azure SDK
* Secure access without exposing storage account keys
* Built using Azure SDK for improved reliability and performance

---

## Architecture

This connector leverages the Azure SDK for .NET to interact with Azure Blob Storage and generate SAS tokens on demand.

Instead of relying on pre-generated SAS tokens, the connector can create them dynamically based on:

Required permissions
Expiration time
Target resource (container/blob)

This approach simplifies integration and improves flexibility in access control.

---

## Requirements

* OutSystems Developer Cloud (ODC)
* **.NET 8.0 runtime**
* Azure Storage Account

⚠️ Note: To generate SAS tokens programmatically, the connector requires access to storage account credentials (e.g., connection string) in a secure server-side context.

---

## Setup

### 1. Configure Connector

Set the following parameters in your ODC module:

* Connection string
* Container Name

### 2. SAS Token Generation

Use the provided actions to generate SAS tokens dynamically by specifying:

Permissions
Expiration time
Target container

### 3. Use Actions

Call the exposed actions to interact with Azure Blob Storage:

* GetURLForBlobWithSASToken
* UploadBlob
* DownloadBlob
* DeleteBlob
* CreateContainer
* DeleteContainer

---

## Usage Example

### Generate URL for upload

1. Call `GetURLForBlobWithSASToken` with SAS permissions, duration, etc
2. Upload via JS for client-side use using the previous URL

### Download File

1. Provide blob name
2. Call `DownloadBlob`
3. Handle returned binary

---

## Security Considerations

* Never expose SAS tokens in client-side logic
* Use short-lived SAS tokens whenever possible
* Restrict permissions to the minimum required

---

## Contributing

Contributions are welcome. Feel free to open issues or submit pull requests.

---