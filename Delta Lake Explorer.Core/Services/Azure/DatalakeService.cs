using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.ResourceManager.Storage;
using Azure.Storage.Files.DataLake;
using Azure.Storage.Files.DataLake.Models;
using Delta_Lake_Explorer.Core.Contracts.Services;
using Delta_Lake_Explorer.Core.Contracts.Services.Azure;
using Delta_Lake_Explorer.Core.Models.Azure;

namespace Delta_Lake_Explorer.Core.Services.Azure;
public class DatalakeService : IDatalakeService
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IARMService _armService;
    private StorageAccountResource? _defaultStorageAccount;
    private DataLakeServiceClient? dataLakeServiceClient;


    public DatalakeService(IAuthenticationService authenticationService,
        IARMService armService)
    {
        _authenticationService = authenticationService;
        _armService = armService;
    }

    private async Task<DataLakeServiceClient> GetDataLakeServiceClientAsync()
    {
        if (dataLakeServiceClient is not null)
        {
            return dataLakeServiceClient;
        }
        else
        {
            var authentication = (AzureAuthentication)await _authenticationService.GetAuthenticationAsync();
            if (authentication.IsAuthenticated)
            {
                _defaultStorageAccount = _armService.GetDefaultStorageAccount();
                if (_defaultStorageAccount is not null)
                {
                    var dfsURI = new System.Uri($"https://{_defaultStorageAccount.Data.Name}.dfs.core.windows.net");
                    dataLakeServiceClient = new DataLakeServiceClient(dfsURI, authentication.TokenCredential);
                    return dataLakeServiceClient;
                }
            }
            return null;
        }
    }

    public AsyncPageable<FileSystemItem> GetFileSystems()
    {
        if (GetDataLakeServiceClientAsync().Result is not null)
        {
            return dataLakeServiceClient.GetFileSystemsAsync();
        }
        return null;
    }

    // TODO MOVE TO Async asap
    public Pageable<PathItem> GetDeltaPaths(FileSystemItem filesystem)
    {
        if (GetDataLakeServiceClientAsync().Result is not null)
        {
            var authentication = (AzureAuthentication)_authenticationService.GetAuthenticationAsync().Result;
            if (authentication.IsAuthenticated)
            {
                _defaultStorageAccount = _armService.GetDefaultStorageAccount();
                if (_defaultStorageAccount is not null)
                {
                    var dfsURI = new System.Uri($"https://{_defaultStorageAccount.Data.Name}.dfs.core.windows.net/curateddata");
                    var filesystemClient = new DataLakeFileSystemClient(dfsURI, authentication.TokenCredential);
                    //var filesystemClient = dataLakeServiceClient.GetFileSystemClient(filesystem.Name);
                    // Need to add filter for delta path (using await foreach?)
                    return filesystemClient.GetPaths(recursive: false);
                }
            }

            return null;
        }
        return null;
    }
}