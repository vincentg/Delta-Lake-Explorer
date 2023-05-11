using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Delta_Lake_Explorer.Core.Contracts.Services.Azure;

namespace Delta_Lake_Explorer.ViewModels;
public class ExplorerDetailViewModel : ObservableRecipient
{
    private readonly IARMService _armService;
    private ObservableCollection<StorageAccountResource>? _storageAccounts;

    public ExplorerDetailViewModel(IARMService armService)
    {
        _armService = armService;
           // i =>  new ObservableCollection<StorageAccountResource>(i.Result));
    }

  

    public ObservableCollection<StorageAccountResource> StorageAccounts
    {
        get => _storageAccounts;
        set => SetProperty(ref _storageAccounts, value);
    }

    public ICommand WACommand => new RelayCommand(async () =>
    {
        StorageAccounts = new ObservableCollection<StorageAccountResource>(await _armService.GetStorageAccountsAsync());
    });

}
