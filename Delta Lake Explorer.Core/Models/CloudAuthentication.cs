using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delta_Lake_Explorer.Core.Models;
public record CloudAuthentication
{

    public bool IsAuthenticated
    {
        get; init;
    }

    public string UserName
    {
        get; init;
    }

    public string ClientId
    {
        get; init;
    }

}
