using System.Collections.ObjectModel;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Storage;
using Azure.Storage.Files.DataLake.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Delta_Lake_Explorer.Contracts.ViewModels;
using Delta_Lake_Explorer.Core.Contracts.Services.Azure;
using Delta_Lake_Explorer.Helpers;

namespace Delta_Lake_Explorer.ViewModels;

public class ExplorerViewModel : ObservableRecipient, INavigationAware
{
    private readonly IARMService _armService;
    private readonly IDatalakeService _datalakeService;

    private ResourceGroupResource? _selected;
    private StorageAccountResource? _selectedStorage;
    private string? _activeSubscriptionText;
    
    private ObservableCollection<StorageAccountResource> _storageAccounts = new();
    public ObservableCollection<ResourceGroupResource> ResourceGroups { get; private set; } = new();

    public ObservableCollection<StorageAccountResource> StorageAccounts
    {
        get => _storageAccounts;
        set => SetProperty(ref _storageAccounts, value);
    }

    public ResourceGroupResource? Selected
    {
        get => _selected;
        set
        {
            SetProperty(ref _selected, value);
            _armService.SetDefaultResourceGroup(value);
            StorageAccounts = new ObservableCollection<StorageAccountResource>(_armService.GetStorageAccountsAsync().Result);
        }
    }

    public StorageAccountResource? SelectedStorage
    {

        get => _selectedStorage;
        set
        {
            SetProperty(ref _selectedStorage, value);
            _armService.SetDefaultStorageAccount(value);
            // TEST CODE
            IEnumerable<FileSystemItem> a = _datalakeService.GetFileSystems().ToBlockingEnumerable();
            //var c = a.Where(x => x.Name == "curateddata").First();
            List<PathItem> b = _datalakeService.GetDeltaPaths(a.First()).ToList();
            var d = new List<PathItem>();
            foreach (var x in a)
                Console.WriteLine(x);
            foreach (var x in b)
                d.Append(x);

            Console.WriteLine();    

        }
    }

    public string ActiveSubscriptionText
    {
        get => _activeSubscriptionText;
        set => SetProperty(ref _activeSubscriptionText, value);
    }

    public ExplorerViewModel(IARMService armService, IDatalakeService datalakeService)
    {
        _armService = armService;
        _datalakeService = datalakeService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        setSubscriptionText();
        ResourceGroups = new ObservableCollection<ResourceGroupResource>(
            (await _armService.GetResourceGroupsAsync())
            .OrderBy(i => i.Data.Name));

        StorageAccounts = new ObservableCollection<StorageAccountResource>(
            await _armService.GetStorageAccountsAsync());

        // TODO: Replace with real data.
        //var data = await _sampleDataService.GetListDetailsDataAsync();

        ///foreach (var item in data)
        //{
        //    SampleItems.Add(item);
        // }
    }

    public void OnNavigatedFrom()
    {
    }

    public void EnsureItemSelected()
    {
        //Selected ??= SampleItems.First();
    }

    private async void setSubscriptionText()
    {
        var defaultSubscription = await _armService.GetDefaultSubscriptionAsync();
        if (defaultSubscription != null)
        {
            ActiveSubscriptionText = "Explorer_ActiveSubscription".GetLocalized() + $": {defaultSubscription.Data.DisplayName} ({defaultSubscription.Data.SubscriptionId})";
        }

        else
        {
            ActiveSubscriptionText = "Explorer_PleaseAuthenticate".GetLocalized();
        }
    }


}
