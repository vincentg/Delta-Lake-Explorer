using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using Delta_Lake_Explorer.Contracts.ViewModels;
using Delta_Lake_Explorer.Core.Contracts.Services;
using Delta_Lake_Explorer.Core.Models;

namespace Delta_Lake_Explorer.ViewModels;

public class DataViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;

    public ObservableCollection<SampleOrder> Source { get; } = new ObservableCollection<SampleOrder>();

    public DataViewModel(ISampleDataService sampleDataService)
    {
        _sampleDataService = sampleDataService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        // TODO: Replace with real data.
        var data = await _sampleDataService.GetGridDataAsync();

        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
