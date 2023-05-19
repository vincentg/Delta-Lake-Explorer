using Azure;
using Azure.Storage.Files.DataLake.Models;

namespace Delta_Lake_Explorer.Core.Contracts.Services.Azure;
public interface IDatalakeService
{
    AsyncPageable<FileSystemItem> GetFileSystems();
    Pageable<PathItem> GetDeltaPaths(FileSystemItem filesystem);
}
