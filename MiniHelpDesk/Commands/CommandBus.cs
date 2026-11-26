namespace MiniHelpDesk.Commands;

/*
klasa: CommandBus

opis:
Rozdziela wywołania komend i zapytań od kodu interfejsu, przekazując je do właściwych handlerów.

pola / właściwości:
_add, AddTicketHandler, handler dodawania zgłoszeń
_delete, DeleteTicketHandler, handler usuwania zgłoszeń
_update, UpdateTicketHandler, handler edycji zgłoszeń
_status, ChangeTicketStatusHandler, handler zmiany statusu
_get, GetTicketsHandler, handler zapytania o listę
*/
public sealed class CommandBus(AddTicketHandler add, DeleteTicketHandler delete, UpdateTicketHandler update,
    ChangeTicketStatusHandler status, GetTicketsHandler get)
{
    private readonly AddTicketHandler _add = add;
    private readonly DeleteTicketHandler _delete = delete;
    private readonly UpdateTicketHandler _update = update;
    private readonly ChangeTicketStatusHandler _status = status;
    private readonly GetTicketsHandler _get = get;

    /*
funkcja: Send

opis:
Przekazuje komendę do odpowiedniego handlera i zwraca jego wynik.

parametry:
command, object, jedna z obsługiwanych komend

zwraca:
object, wynik wykonania komendy
*/
    public object Send(object command) => command switch
    {
        AddTicketCommand c => _add.Handle(c),
        DeleteTicketCommand c => _delete.Handle(c),
        UpdateTicketCommand c => _update.Handle(c),
        ChangeTicketStatusCommand c => _status.Handle(c),
        _ => throw new NotSupportedException($"Nieobsługiwana komenda: {command.GetType().Name}")
    };

    /*
funkcja: Ask

opis:
Przekazuje zapytanie odczytujące do właściwego handlera.

parametry:
query, GetTicketsQuery, zapytanie o listę zgłoszeń

zwraca:
IReadOnlyList<MiniHelpDesk.Models.Ticket>, aktualna lista zgłoszeń
*/
    public IReadOnlyList<MiniHelpDesk.Models.Ticket> Ask(GetTicketsQuery query) => _get.Handle(query);
}
