using Xunit;
using Delta_Lake_Explorer.Core.Services.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Azure.Identity;
using Delta_Lake_Explorer.Core.Models.Azure;
using System.Reflection;

namespace Delta_Lake_Explorer.Core.Services.Azure.Tests;

public class AzureAuthenticationServiceTests
{
    [Fact()]
    public void AuthenticateAsyncTestSuccess()
    {
        var service = new AzureAuthenticationService();
        var azMock = new Mock<InteractiveBrowserCredential>();

        var username = "test@test.com";
        var clientId = "0000";
        var tenantId = "1111";
        // Use reflexion to use an internal constructor since mock did not work
        var authRecord = CreateInstance<AuthenticationRecord>(username, "test", "test", tenantId, clientId);

        azMock.Setup(x => x.AuthenticateAsync(new CancellationToken()))
                     .ReturnsAsync(authRecord);
        var result = (AzureAuthentication) service.AuthenticateAsync(azMock.Object).Result;
        Assert.True(result.IsAuthenticated);
        Assert.Equal(username, result.UserName);
        Assert.Equal(clientId, result.ClientId);
        Assert.Equal(tenantId, result.TenantId);
        Assert.Equal(azMock.Object, result.TokenCredential);
        Assert.True(service.IsAuthenticatedAsync().Result);
    }
    [Fact()]
    public void AuthenticateAsyncTestExc()
    {
        var service = new AzureAuthenticationService();
        var azMock = new Mock<InteractiveBrowserCredential>();
        azMock.Setup(x => x.AuthenticateAsync(new CancellationToken()))
                     .ThrowsAsync(new AuthenticationFailedException("test"));
        var result = (AzureAuthentication) service.AuthenticateAsync(azMock.Object).Result;
        Assert.False(result.IsAuthenticated);
        Assert.Equal("test",result.AuthenticationError);
        Assert.False(service.IsAuthenticatedAsync().Result);
    }

    private static T CreateInstance<T>(params object[] args)
    {
        var type = typeof(T);
        var instance = type.Assembly.CreateInstance(
            type.FullName, false,
            BindingFlags.Instance | BindingFlags.NonPublic,
            null, args, null, null);
        return (T)instance;
    }

}