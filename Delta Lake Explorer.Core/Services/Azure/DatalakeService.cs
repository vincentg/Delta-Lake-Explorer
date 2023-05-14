using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.ResourceManager.Storage;
using Azure.Storage.Files.DataLake.Models;
using Delta_Lake_Explorer.Core.Contracts.Services;
using Delta_Lake_Explorer.Core.Contracts.Services.Azure;

namespace Delta_Lake_Explorer.Core.Services.Azure;
public class DatalakeService : IDatalakeService
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IARMService _armService;

    public DatalakeService(IAuthenticationService authenticationService,
        IARMService armService)
    {
        _authenticationService = authenticationService;
        _armService = armService;
    }

    public IEnumerable<FileSystemItem> GetFileSystems()
    {
        // return empty object for now
        return Enumerable.Empty<FileSystemItem>();
    }

    public IEnumerable<PathItem> GetDeltaPaths(FileSystemItem filesystem) => throw new NotImplementedException();
}
