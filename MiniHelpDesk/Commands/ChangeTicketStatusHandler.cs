using MiniHelpDesk.Models;
using MiniHelpDesk.Services;

namespace MiniHelpDesk.Commands;

/*
klasa: ChangeTicketStatusHandler

opis:
Sprawdza i zmienia status istniejącego zgłoszenia.

pola / właściwości:
_repository, ITicketRepository, repozytorium zgłoszeń
AllowedStatuses, IReadOnlyList<string>, dozwolone statusy zgłoszenia
*/
public sealed class ChangeTicketStatusHandler(ITicketRepository repository)
{
    private readonly ITicketRepository _repository = repository;
    public static IReadOnlyList<string> AllowedStatuses { get; } = new[] { "Nowe", "W trakcie", "Zamknięte" };

    /*
funkcja: Handle

opis:
Weryfikuje nowy status i ustawia go na istniejącym obiekcie zgłoszenia.

parametry:
command, ChangeTicketStatusCommand, identyfikator zgłoszenia i nowy status

zwraca:
Ticket, zmienione zgłoszenie
*/
    public Ticket Handle(ChangeTicketStatusCommand command)
    {
        if (!AllowedStatuses.Contains(command.Status))
            throw new ArgumentException("Nieprawidłowy status.");

        var ticket = _repository.GetById(command.Id)
            ?? throw new KeyNotFoundException("Nie znaleziono zgłoszenia.");
        ticket.Status = command.Status;
        return ticket;
    }
}
