﻿using System.Collections.ObjectModel;
using Azure.ResourceManager.Resources;
using CommunityToolkit.Mvvm.ComponentModel;
using Delta_Lake_Explorer.Contracts.ViewModels;
using Delta_Lake_Explorer.Core.Contracts.Services;
using Delta_Lake_Explorer.Core.Contracts.Services.Azure;
using Delta_Lake_Explorer.Core.Models;
using Delta_Lake_Explorer.Helpers;

namespace Delta_Lake_Explorer.ViewModels;

public class ExplorerViewModel : ObservableRecipient, INavigationAware
{
    private readonly IARMService _armService;
    private SampleOrder? _selected;
    private string? _activeSubscriptionText;


    public SampleOrder? Selected
    {
        get => _selected;
        set => SetProperty(ref _selected, value);
    }

    public string ActiveSubscriptionText
    {
        get => _activeSubscriptionText;
        set => SetProperty(ref _activeSubscriptionText, value);
    }

    public ObservableCollection<SampleOrder> SampleItems { get; private set; } = new ObservableCollection<SampleOrder>();
    public ObservableCollection<ResourceGroupResource> ResourceGroups { get; private set; } = new ObservableCollection<ResourceGroupResource>();

    public ExplorerViewModel(IARMService armService)
    {
        _armService = armService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        SampleItems.Clear();
        setSubscriptionText();
        ResourceGroups = new ObservableCollection<ResourceGroupResource>(
            (await _armService.GetResourceGroupsAsync())
            .OrderBy(i => i.Data.Name));

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
