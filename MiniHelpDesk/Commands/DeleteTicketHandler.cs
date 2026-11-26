using MiniHelpDesk.Services;

namespace MiniHelpDesk.Commands;

/*
klasa: DeleteTicketHandler

opis:
Obsługuje usuwanie zgłoszenia z repozytorium.

pola / właściwości:
_repository, ITicketRepository, repozytorium zgłoszeń
*/
public sealed class DeleteTicketHandler(ITicketRepository repository)
{
    private readonly ITicketRepository _repository = repository;

    /*
funkcja: Handle

opis:
Usuwa zgłoszenie o identyfikatorze podanym w komendzie.

parametry:
command, DeleteTicketCommand, identyfikator usuwanego zgłoszenia

zwraca:
bool, informacja czy zgłoszenie zostało usunięte
*/
    public bool Handle(DeleteTicketCommand command) => _repository.Delete(command.Id);
}
