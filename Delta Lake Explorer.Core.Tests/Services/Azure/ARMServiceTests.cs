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

    /* Turns out it is extremelly difficult to mock StorageAccountCollection
     * I will leave this here for future reference, way out it migrate from
     * xUnit to nUnit and use the Azure SDK Test Framework
     * 
        [Fact()]
        public void GetStorageAccountsAsyncTest()
        {
            var result = new Mock<ResourceGroupResource>();
            // Returns 2 Storage account, one Data-Lake the other not,
            // only the datalake should be returned
            var storageaccount1 = new Mock<StorageAccountResource>();
            var storageaccount1Data = new Mock<StorageAccountData>();
            storageaccount1Data.Setup(x => x.IsHnsEnabled).Returns(false);
            storageaccount1.Setup(x=> x.Data).Returns(storageaccount1Data.Object);
            var storageaccount2 = new Mock<StorageAccountResource>();
            var storageaccount2Data = new Mock<StorageAccountData>();
            storageaccount2Data.Setup(x => x.IsHnsEnabled).Returns(true);
            storageaccount2.Setup(x => x.Data).Returns(storageaccount2Data.Object);
            var storageaccounts = new List<StorageAccountResource>() { storageaccount1.Object, storageaccount2.Object };
            var pageValues = new[] { storageaccounts };

            var armservice = new ARMService(_authMock.Object,
                           _armclientMock.Object);
            armservice.SetDefaultResourceGroup(result.Object);
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

        // Should be empty enumerable, no Auth
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

}