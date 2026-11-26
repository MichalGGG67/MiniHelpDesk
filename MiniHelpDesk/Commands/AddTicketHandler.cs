using MiniHelpDesk.Models;
using MiniHelpDesk.Services;

namespace MiniHelpDesk.Commands;

/*
klasa: AddTicketHandler

opis:
Waliduje i dodaje nowe zgłoszenie do repozytorium.

pola / właściwości:
_repository, ITicketRepository, repozytorium zgłoszeń
_validator, TicketValidator, walidator danych zgłoszenia
*/
public sealed class AddTicketHandler(ITicketRepository repository, TicketValidator validator)
{
    private readonly ITicketRepository _repository = repository;
    private readonly TicketValidator _validator = validator;

    /*
funkcja: Handle

opis:
Sprawdza dane, tworzy zgłoszenie ze statusem Nowe i zapisuje je w repozytorium.

parametry:
command, AddTicketCommand, dane nowego zgłoszenia

zwraca:
Ticket, utworzone zgłoszenie
*/
    public Ticket Handle(AddTicketCommand command)
    {
        var errors = _validator.Validate(command.Name, command.Description, command.Priority);
        if (errors.Count > 0) throw new ArgumentException(string.Join(" ", errors.Values));

        var ticket = new Ticket
        {
            Name = command.Name.Trim(),
            Description = command.Description.Trim(),
            Priority = command.Priority,
            Status = "Nowe"
        };
        _repository.Add(ticket);
        return ticket;
    }
}
