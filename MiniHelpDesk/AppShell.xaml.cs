using MiniHelpDesk.Views;

namespace MiniHelpDesk;

public partial class AppShell : Shell
{
    public AppShell(MainPage mainPage)
    {
        InitializeComponent();
        Items.Add(new ShellContent { Title = "Mini HelpDesk", Content = mainPage });
    }
}
