using Xunit;
using Delta_Lake_Explorer.Core.Services.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.ResourceManager;
using Delta_Lake_Explorer.Core.Contracts.Services;
using Moq;
using Delta_Lake_Explorer.Core.Contracts.Services.Azure;
using Delta_Lake_Explorer.Core.Models.Azure;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Storage;
using Azure;

namespace Delta_Lake_Explorer.Core.Services.Azure.Tests;

public class ARMServiceTests
{
    private readonly Mock<ArmClient> _armclientMock = new();
    private readonly Mock<IAuthenticationService> _authMock = new();

    public ARMServiceTests()
    {

    }

    [Fact()]
    public void SetDefaultResourceGroupTest()
    {
        var result = new Mock<ResourceGroupResource>();
        var armservice = new ARMService(_authMock.Object,
                       _armclientMock.Object);
        armservice.SetDefaultResourceGroup(result.Object);
    }

    /*  I tried hard to test this method, but Azure SDK make it difficult to mock
     *  RG.GetStorageAccounts() 
    [Fact()]
    public void GetStorageAccountsAsyncTest()
    {
        var defaultrg = new Mock<ResourceGroupResource>();
        var storagecollection = new Mock<StorageAccountCollection>();
        
        // Returns 2 Storage account, one Data-Lake the other not,
        // only the datalake should be returned
        var storageaccount1 = new Mock<StorageAccountResource>();
        var storageaccount2 = new Mock<StorageAccountResource>();


        var storageaccount1Data = new StorageAccountData(location: "US West");
        storageaccount1Data.IsHnsEnabled = false;
        var storageaccount2Data = new StorageAccountData(location: "US West");
        storageaccount2Data.IsHnsEnabled = false;

        storageaccount1.Setup(x => x.Data).Returns(storageaccount1Data);
        storageaccount2.Setup(x => x.Data).Returns(storageaccount2Data);

        var pagelist = new List<Page<StorageAccountResource>> {
              Page<StorageAccountResource>.FromValues(new[] { storageaccount1.Object, storageaccount2.Object  }, continuationToken: null, new Mock<Response>().Object)
             };

        storagecollection.Setup(x => x.GetAll(new CancellationToken())).Returns(Pageable<StorageAccountResource>.FromPages(pagelist));
        // This line is not executable :( , GetStorageAccounts() is not virtual 
        defaultrg.Setup(x => x.GetStorageAccounts()).Returns(storagecollection.Object);

        var armservice = new ARMService(_authMock.Object,
                       _armclientMock.Object);
        armservice.SetDefaultResourceGroup(defaultrg.Object);
        var result = armservice.GetStorageAccountsAsync().Result;
        // only the storage account with HNS enabled should be returned
        Assert.Single(result);
        Assert.Equal(storageaccount1.Object, result.First());
    }
    */

    [Fact()]
    public void GetStorageAccountsEmptyTest()
    {
        var armservice = new ARMService(_authMock.Object,
            _armclientMock.Object);
        // Should be empty enumerable, no Resource Group set
        var norg = armservice.GetStorageAccountsAsync();
        Assert.False(norg.Result.Any());
    }

    [Fact()]
    public void GetDefaultSubscriptionAsyncTestNoAuth()
    {
        AzureAuthentication mockAuthResult = new AzureAuthentication();
        _authMock.Setup(x => x.GetAuthenticationAsync())
            .ReturnsAsync(mockAuthResult);
        var armservice = new ARMService(_authMock.Object,
            _armclientMock.Object);
        // Should be empty enumerable, no Auth
        var noauth = armservice.GetDefaultSubscriptionAsync();
        Assert.Null(noauth.Result);
    }
    [Fact()]
    public void GetDefaultSubscriptionAsyncTestAz()
    {
        var result = new Mock<SubscriptionResource>();
        AzureAuthentication mockAuthResult =
            new AzureAuthentication() { IsAuthenticated = true };
        _authMock.Setup(x => x.GetAuthenticationAsync())
            .ReturnsAsync(mockAuthResult);
        // Test default subscription from AZ
        _armclientMock.Setup(x => x.GetDefaultSubscription(new CancellationToken()))
            .Returns(result.Object);

        var armservice = new ARMService(_authMock.Object,
            _armclientMock.Object);

        // Should be same subscription resource from Mock
        var fromAz = armservice.GetDefaultSubscriptionAsync();
        Assert.Equal(result.Object, fromAz.Result);
    }

    [Fact()]
    public void SetDefaultSubscriptionAsyncTest()
    {
        var armservice = new ARMService(_authMock.Object,
    _armclientMock.Object);
        var result = new Mock<SubscriptionResource>();

        armservice.SetDefaultSubscriptionAsync(result.Object);
        Assert.Equal(result.Object,
            armservice.GetDefaultSubscriptionAsync().Result);
    }

    [Fact()]
    public void GetDefaultStorageAccountTest()
    {

        var armservice = new ARMService(_authMock.Object);
        var result = new Mock<StorageAccountResource>();

        armservice.SetDefaultStorageAccount(result.Object);
        Assert.Equal(armservice.GetDefaultStorageAccount(), result.Object);

    }

    [Fact()]
    public void GetSubscriptionsAsyncTest()
    {
        var subscription = new Mock<SubscriptionResource>();
        var subcollection = new Mock<SubscriptionCollection>();
        var pagelist = new List<Page<SubscriptionResource>> {
            Page<SubscriptionResource>.FromValues(new[] { subscription.Object }, continuationToken: null, new Mock<Response>().Object)
           };

        subcollection.Setup(x => x.GetAll(new CancellationToken())).Returns(Pageable<SubscriptionResource>.FromPages(pagelist));
        _armclientMock.Setup(x => x.GetSubscriptions()).Returns(subcollection.Object);
        var armservice = new ARMService(_authMock.Object, _armclientMock.Object);

        var subs = armservice.GetSubscriptionsAsync();
        Assert.Equal(new List<SubscriptionResource> { subscription.Object }, subs.Result);

        // Test cache invalidation , should

        _armclientMock.Reset();
        // This should still get the cached value, even if mock is reseted
        subs = armservice.GetSubscriptionsAsync();
        Assert.Equal(new List<SubscriptionResource> { subscription.Object }, subs.Result);

        // Now invalidate cache, then the Mock reset will take effect and return null
        armservice.InvalidateSubscriptionsCache();
        subs = armservice.GetSubscriptionsAsync();

        Assert.Null(subs.Result);



    }
}