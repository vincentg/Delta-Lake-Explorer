using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Storage;
using Delta_Lake_Explorer.Core.Contracts.Services;
using Delta_Lake_Explorer.Core.Contracts.Services.Azure;
using Delta_Lake_Explorer.Core.Models.Azure;

namespace Delta_Lake_Explorer.Core.Services.Azure;
public class ARMService : IARMService
{
    private readonly IAuthenticationService _authenticationService;
    private ArmClient _armClient;
    private SubscriptionResource _defaultSubscription;

    public ARMService(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    private async Task<ArmClient> GetArmClientAsync()
    {
        if (_armClient == null)
        {
            var authentication = (AzureAuthentication) await _authenticationService.GetAuthenticationAsync();
            if (authentication.IsAuthenticated)
                _armClient = new ArmClient(authentication.TokenCredential);
        }
        return _armClient;
    }

    // TODO Make this API better and avoid returning nulls
    public Task<SubscriptionResource> GetDefaultSubscriptionAsync()
    {
        if (_defaultSubscription is not null)
        {
            return Task.FromResult(_defaultSubscription);
        }
        if (GetArmClientAsync() is not null)
        {
             _defaultSubscription = _armClient.GetDefaultSubscription();
            return Task.FromResult(_defaultSubscription);
        }
        return null;
    }

    public Task<IEnumerable<string>> GetResourceGroupsAsync(string subscriptionId) => throw new NotImplementedException();
    public Task<IEnumerable<ResourceGroupResource>> GetResourceGroupsAsync() => throw new NotImplementedException();
    public Task<IEnumerable<string>> GetStorageAccountsAsync(string subscriptionId, string resourceGroup) => throw new NotImplementedException();
    public Task<StorageAccountCollection> GetStorageAccountsAsync(ResourceGroupResource resourceGroup) => throw new NotImplementedException();
    public Task<IEnumerable<string>> GetStorageContainersAsync(string subscriptionId, string resourceGroup, string storageAccount) => throw new NotImplementedException();
    public Task<BlobContainerCollection> GetStorageContainersAsync(StorageAccountResource storageAccount) => throw new NotImplementedException();
    public Task<IEnumerable<string>> GetStorageFileContentAsync(string subscriptionId, string resourceGroup, string storageAccount, string storageContainer, string storageFile) => throw new NotImplementedException();
    public Task<IEnumerable<string>> GetStorageFilesAsync(string subscriptionId, string resourceGroup, string storageAccount, string storageContainer) => throw new NotImplementedException();
    public Task SetDefaultSubscriptionAsync(SubscriptionResource subscription)
    {
        _defaultSubscription = subscription;
        return Task.CompletedTask;
    }
    public Task<IEnumerable<SubscriptionResource>> GetSubscriptionsAsync()
    {
        if (GetArmClientAsync().Result is not null)
        {
            return Task.FromResult<IEnumerable<SubscriptionResource>>(_armClient.GetSubscriptions());
        }
        return Task.FromResult(Enumerable.Empty<SubscriptionResource>());
    }
}


