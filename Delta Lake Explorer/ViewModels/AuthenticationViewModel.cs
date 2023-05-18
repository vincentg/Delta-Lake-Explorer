using System.Collections.ObjectModel;
using System.Windows.Input;
using Azure.ResourceManager.Resources;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Delta_Lake_Explorer.Core.Contracts.Services;
using Delta_Lake_Explorer.Core.Contracts.Services.Azure;
using Delta_Lake_Explorer.Core.Models.Azure;
using Delta_Lake_Explorer.Core.Services.Azure;
using Delta_Lake_Explorer.Helpers;
using Delta_Lake_Explorer.Models;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.UI.Xaml.Controls;

namespace Delta_Lake_Explorer.ViewModels;

public class AuthenticationViewModel : ObservableRecipient
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IARMService _armService;
    private AzureAuthentication? _azureAuthentication;
    private bool? _isAuthenticated;
    private string? _buttonText;
    private string? _labelText;
    private ObservableCollection<SubscriptionData> _subscriptionList;
    private ObservableCollection<SubscriptionData> _fullSubscriptionList;
    // TODO, the Service can likely maintain that map between Id and SubscriptionResouce
    // and have the Set DefaultSubscription accept the ID?
    private Dictionary<string, SubscriptionResource> _subscriptionsDictionnary;


    public AuthenticationViewModel(IAuthenticationService authenticationService, IARMService armService)
    {
        _authenticationService = authenticationService;
        _armService = armService;
        _azureAuthentication = (AzureAuthentication)_authenticationService.GetAuthenticationAsync().Result;
        _isAuthenticated = _azureAuthentication.IsAuthenticated;
        _buttonText = (bool)_isAuthenticated ? "Authentication_LoggedIn".GetLocalized() : "Authentication_ClickToLogin".GetLocalized();
        _labelText = generateLabelText();
        refreshSubscriptions();

    }
    public bool? IsAuthenticated
    {
        get => _isAuthenticated;
        set => SetProperty(ref _isAuthenticated, value);
    }

    public string ButtonText
    {
        get => _buttonText;
        set => SetProperty(ref _buttonText, value);
    }
    public ObservableCollection<SubscriptionData> SubscriptionList
    {

        get => _subscriptionList;
        set => SetProperty(ref _subscriptionList, value);
    }

    public string LabelText
    {

        get => _labelText;
        set => SetProperty(ref _labelText, value);
    }

    public void FilterChanged(object sender, TextChangedEventArgs args)
    {
        var text = ((TextBox)sender).Text.ToLower();
        
        if (text == "")
        {
            SubscriptionList = _fullSubscriptionList;
        }
        else
        {
            SubscriptionList = new ObservableCollection<SubscriptionData>(
                _fullSubscriptionList.Where(i => i.DisplayName.ToLower().Contains(text) || i.SubscriptionId.ToLower().Contains(text)));
        }
    }

    public ICommand LoginCommand => new RelayCommand(async () =>
       {
           if (IsAuthenticated == true)
           {
               //IsAuthenticated = await _authenticationService.LogoutAsync();
               //ButtonText = ResourceExtensions.GetLocalized("Authentication_ClickToLogin.Text");
           }
           else
           {
               _azureAuthentication = (AzureAuthentication) await _authenticationService.AuthenticateAsync();
               IsAuthenticated = _azureAuthentication.IsAuthenticated;
               ButtonText = "Authentication_LoggedIn".GetLocalized();
               LabelText = generateLabelText();
               refreshSubscriptions();
           }
       });

    public ICommand ReloadSubscriptionCommand => new RelayCommand(() =>
    {
        refreshSubscriptions();
    });


    public void SubscriptionSelected(object sender, SelectionChangedEventArgs args)
    {
        // TODO May need a try catch here
        var subscription = (SubscriptionData)args.AddedItems[0];

        _armService.SetDefaultSubscriptionAsync(_subscriptionsDictionnary[subscription.SubscriptionId]);
    }

    private void refreshSubscriptions()
    {
        IEnumerable<SubscriptionResource> subscriptions;
        subscriptions = _armService.GetSubscriptionsAsync().Result;
        SubscriptionList = new ObservableCollection<SubscriptionData>(subscriptions.Select(i => i.Data).OrderBy(i => i.DisplayName));
        _fullSubscriptionList = _subscriptionList;
        _subscriptionsDictionnary = subscriptions.ToDictionary(i => i.Data.SubscriptionId, i => i);
    }

    private string generateLabelText()
    {
        if (_isAuthenticated == true)
        {
            return "Authentication_Username".GetLocalized() + $": {_azureAuthentication.UserName}\r\n" +
                   "Authentication_ClientId".GetLocalized() + $": {_azureAuthentication.ClientId}\r\n" +
                   "Authentication_TenantId".GetLocalized() + $": {_azureAuthentication.TenantId}\r\n\r\n" +
                   "Authentication_SubscriptionSelect".GetLocalized() + "\r\n";
        }
        else
        {
            return "Authentication_NotLoggedIn".GetLocalized();
        }
    }
}
