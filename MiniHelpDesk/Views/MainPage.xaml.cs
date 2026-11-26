using MiniHelpDesk.ViewModels;

namespace MiniHelpDesk.Views;

public partial class MainPage : ContentPage
{
    public MainPage(TicketViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        viewModel.MessageRequested += async message => await MainThread.InvokeOnMainThreadAsync(
            () => DisplayAlert("Mini HelpDesk", message, "OK"));
        SizeChanged += (_, _) => AdaptLayout();
        AdaptLayout();
    }

    private void AdaptLayout()
    {
        if (Width <= 0) return;
        var desktop = Width >= 850;
        if (desktop)
        {
            RootGrid.RowDefinitions.Clear();
            RootGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            RootGrid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
            RootGrid.ColumnDefinitions.Clear();
            RootGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(380)));
            RootGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            Grid.SetRow(FormPanel, 1); Grid.SetColumn(FormPanel, 0); Grid.SetColumnSpan(FormPanel, 1);
            Grid.SetRow(ListPanel, 1); Grid.SetColumn(ListPanel, 1); Grid.SetColumnSpan(ListPanel, 1);
        }
        else
        {
            RootGrid.RowDefinitions.Clear();
            RootGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            RootGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            RootGrid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
            RootGrid.ColumnDefinitions.Clear();
            RootGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            Grid.SetRow(FormPanel, 1); Grid.SetColumn(FormPanel, 0);
            Grid.SetRow(ListPanel, 2); Grid.SetColumn(ListPanel, 0); Grid.SetColumnSpan(ListPanel, 1);
        }
    }
}
