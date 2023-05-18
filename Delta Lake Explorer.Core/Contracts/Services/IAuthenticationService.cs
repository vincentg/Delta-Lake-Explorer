using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Delta_Lake_Explorer.Core.Models;

namespace Delta_Lake_Explorer.Core.Contracts.Services;
public interface IAuthenticationService
{
    Task<CloudAuthentication> AuthenticateAsync(InteractiveBrowserCredential
                                                customCredential = null);
    Task<CloudAuthentication> GetAuthenticationAsync();
    Task<bool> IsAuthenticatedAsync();
    Task<bool> LogoutAsync();

}
