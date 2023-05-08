using CommunityToolkit.WinUI.UI.Controls;

using Delta_Lake_Explorer.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace Delta_Lake_Explorer.Views;

public sealed partial class ExplorerPage : Page
{
    public ExplorerViewModel ViewModel
    {
        get;
    }

    public ExplorerPage()
    {
        ViewModel = App.GetService<ExplorerViewModel>();
        InitializeComponent();
    }

    private void OnViewStateChanged(object sender, ListDetailsViewState e)
    {
        if (e == ListDetailsViewState.Both)
        {
            ViewModel.EnsureItemSelected();
        }
    }
}
