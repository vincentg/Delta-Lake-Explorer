using Delta_Lake_Explorer.Core.Models;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Delta_Lake_Explorer.Views;

public sealed partial class ExplorerDetailControl : UserControl
{
    public SampleOrder? ListDetailsMenuItem
    {
        get => GetValue(ListDetailsMenuItemProperty) as SampleOrder;
        set => SetValue(ListDetailsMenuItemProperty, value);
    }

    public static readonly DependencyProperty ListDetailsMenuItemProperty = DependencyProperty.Register("ListDetailsMenuItem", typeof(SampleOrder), typeof(ExplorerDetailControl), new PropertyMetadata(null, OnListDetailsMenuItemPropertyChanged));

    public ExplorerDetailControl()
    {
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
