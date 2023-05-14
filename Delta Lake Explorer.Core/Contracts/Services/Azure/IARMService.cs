using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Storage;
using Delta_Lake_Explorer.Core.Models.Azure;

namespace Delta_Lake_Explorer.Core.Contracts.Services.Azure;

/* 
 * This Interface will represent azure resources (subscriptions, resource groups, etc.)
 */
public interface IARMService
{
    Task<IEnumerable<SubscriptionResource>> GetSubscriptionsAsync();
    //Get Default Subscription
    Task<SubscriptionResource> GetDefaultSubscriptionAsync();
    //Set Default Subscription
    Task SetDefaultSubscriptionAsync(SubscriptionResource subscription);

    void SetDefaultResourceGroup(ResourceGroupResource resourceGroup);

    void SetDefaultStorageAccount(StorageAccountResource storageAccount);
    StorageAccountResource GetDefaultStorageAccount();

    Task<IEnumerable<ResourceGroupResource>> GetResourceGroupsAsync();
    Task<IEnumerable<StorageAccountResource>> GetStorageAccountsAsync();
    public void InvalidateSubscriptionsCache();
}
