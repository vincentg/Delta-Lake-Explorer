using Delta_Lake_Explorer.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace Delta_Lake_Explorer.Views;

// TODO: Change the grid as appropriate for your app. Adjust the column definitions on DataGridPage.xaml.
// For more details, see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid.
public sealed partial class DataPage : Page
{
    public DataViewModel ViewModel
    {
        get;
    }

    public DataPage()
    {
        ViewModel = App.GetService<DataViewModel>();
        InitializeComponent();
    }
}
