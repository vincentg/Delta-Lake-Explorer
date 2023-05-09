using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;

namespace Delta_Lake_Explorer.Core.Models;
public record AzureAuthentication : CloudAuthentication
{
    public string TenantId
    {
        get; init;
    }

    public TokenCredential TokenCredential
    {
        get; init;
    }
}
