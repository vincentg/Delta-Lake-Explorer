﻿using Azure.ResourceManager.Resources;
using Delta_Lake_Explorer.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Delta_Lake_Explorer.Views;

public sealed partial class ExplorerDetailControl : UserControl
{
    public ExplorerDetailViewModel ViewModel
    {
        get;
    }
    public ResourceGroupResource? ListDetailsMenuItem
    {
        get => GetValue(ListDetailsMenuItemProperty) as ResourceGroupResource;
        set => SetValue(ListDetailsMenuItemProperty, value);
    }

    public static readonly DependencyProperty ListDetailsMenuItemProperty = DependencyProperty.Register("ListDetailsMenuItem", typeof(ResourceGroupResource), typeof(ExplorerDetailControl), new PropertyMetadata(null, OnListDetailsMenuItemPropertyChanged));

    public ExplorerDetailControl()
    {
        ViewModel = App.GetService<ExplorerDetailViewModel>();
        InitializeComponent();
    }

    private static void OnListDetailsMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ExplorerDetailControl control)
        {
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
