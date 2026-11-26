using MiniHelpDesk.Commands;
using MiniHelpDesk.Services;
using MiniHelpDesk.ViewModels;
using MiniHelpDesk.Views;

namespace MiniHelpDesk;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();
        builder.Services.AddSingleton<ITicketRepository, InMemoryTicketRepository>();
        builder.Services.AddSingleton<TicketValidator>();
        builder.Services.AddSingleton<CommandBus>();
        builder.Services.AddSingleton<AddTicketHandler>();
        builder.Services.AddSingleton<DeleteTicketHandler>();
        builder.Services.AddSingleton<UpdateTicketHandler>();
        builder.Services.AddSingleton<ChangeTicketStatusHandler>();
        builder.Services.AddSingleton<GetTicketsHandler>();
        builder.Services.AddSingleton<TicketViewModel>();
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<AppShell>();
        return builder.Build();
    }
}
