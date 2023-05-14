using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Files.DataLake.Models;

namespace Delta_Lake_Explorer.Core.Contracts.Services.Azure;
public interface IDatalakeService
{

    IEnumerable<FileSystemItem> GetFileSystems();

    IEnumerable<PathItem> GetDeltaPaths(FileSystemItem filesystem);
}
