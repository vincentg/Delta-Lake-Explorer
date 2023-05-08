using Delta_Lake_Explorer.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace Delta_Lake_Explorer.Views;

public sealed partial class AuthenticationPage : Page
{
    public AuthenticationViewModel ViewModel
    {
        get;
    }

    public AuthenticationPage()
    {
        ViewModel = App.GetService<AuthenticationViewModel>();
        InitializeComponent();
    }
}
