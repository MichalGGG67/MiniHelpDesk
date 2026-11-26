using MiniHelpDesk.Models;
using MiniHelpDesk.Services;

namespace MiniHelpDesk.Commands;

/*
klasa: UpdateTicketHandler

opis:
Waliduje i aktualizuje nazwę, opis oraz priorytet istniejącego zgłoszenia.

pola / właściwości:
_repository, ITicketRepository, repozytorium zgłoszeń
_validator, TicketValidator, walidator danych zgłoszenia
*/
public sealed class UpdateTicketHandler(ITicketRepository repository, TicketValidator validator)
{
    private readonly ITicketRepository _repository = repository;
    private readonly TicketValidator _validator = validator;

    /*
funkcja: Handle

opis:
Sprawdza nowe dane i aktualizuje właściwości istniejącego obiektu zgłoszenia.

parametry:
command, UpdateTicketCommand, identyfikator i nowe dane zgłoszenia

zwraca:
Ticket, zaktualizowane zgłoszenie
*/
    public Ticket Handle(UpdateTicketCommand command)
    {
        var errors = _validator.Validate(command.Name, command.Description, command.Priority);
        if (errors.Count > 0) throw new ArgumentException(string.Join(" ", errors.Values));

        var ticket = _repository.GetById(command.Id)
            ?? throw new KeyNotFoundException("Nie znaleziono zgłoszenia.");

        ticket.Name = command.Name.Trim();
        ticket.Description = command.Description.Trim();
        ticket.Priority = command.Priority;
        return ticket;
    }
}
