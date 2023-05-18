using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Delta_Lake_Explorer.Core.Contracts.Services;
using Delta_Lake_Explorer.Core.Models;
using Delta_Lake_Explorer.Core.Models.Azure;

namespace Delta_Lake_Explorer.Core.Services.Azure;
public class AzureAuthenticationService : IAuthenticationService
{
    private AzureAuthentication _azureAuthentication;

    public AzureAuthenticationService()
    {
        _azureAuthentication = new AzureAuthentication
        {
            IsAuthenticated = false
        };
    }
    public async Task<CloudAuthentication> AuthenticateAsync(InteractiveBrowserCredential
                                                            customCredential = null)
    {
        var opt = new InteractiveBrowserCredentialOptions();
        opt.DisableAutomaticAuthentication = true;

        var credential = customCredential ?? new InteractiveBrowserCredential(opt);
        try
        {
            var authrecord = await credential.AuthenticateAsync();
            _azureAuthentication = new AzureAuthentication
            {
                IsAuthenticated = true,
                UserName = authrecord.Username,
                ClientId = authrecord.ClientId,
                TenantId = authrecord.TenantId,
                TokenCredential = credential
            };
            return _azureAuthentication;
        }
        catch (AuthenticationFailedException e)
        {
            return new AzureAuthentication
            {
                IsAuthenticated = false,
                AuthenticationError = e.Message,
            };
        }
    }
    public Task<CloudAuthentication> GetAuthenticationAsync() => Task.FromResult<CloudAuthentication>(_azureAuthentication);
    public Task<bool> IsAuthenticatedAsync() => Task.FromResult(_azureAuthentication.IsAuthenticated);
    public Task<bool> LogoutAsync()
    {
        throw new NotImplementedException();
    }



}
