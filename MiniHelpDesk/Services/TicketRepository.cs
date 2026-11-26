using MiniHelpDesk.Models;

namespace MiniHelpDesk.Services;

public interface ITicketRepository
{
    /*
funkcja: GetAll

opis:
Pobiera wszystkie zgłoszenia zapisane w repozytorium.

parametry:
brak

zwraca:
IReadOnlyList<Ticket>, kopia listy zgłoszeń
*/
    IReadOnlyList<Ticket> GetAll();

    /*
funkcja: GetById

opis:
Wyszukuje zgłoszenie według unikalnego identyfikatora.

parametry:
id, Guid, identyfikator zgłoszenia

zwraca:
Ticket?, znalezione zgłoszenie lub null
*/
    Ticket? GetById(Guid id);

    /*
funkcja: Add

opis:
Dodaje zgłoszenie do repozytorium.

parametry:
ticket, Ticket, zgłoszenie do zapisania

zwraca:
void, nic nie zwraca
*/
    void Add(Ticket ticket);

    /*
funkcja: Delete

opis:
Usuwa zgłoszenie o wskazanym identyfikatorze.

parametry:
id, Guid, identyfikator zgłoszenia

zwraca:
bool, informacja czy zgłoszenie zostało usunięte
*/
    bool Delete(Guid id);
}

/*
klasa: InMemoryTicketRepository

opis:
Implementuje repozytorium zgłoszeń przechowywanych w pamięci aplikacji.

pola / właściwości:
_tickets, List<Ticket>, wewnętrzna lista zgłoszeń
*/
public sealed class InMemoryTicketRepository : ITicketRepository
{
    private readonly List<Ticket> _tickets = new();
    public IReadOnlyList<Ticket> GetAll() => _tickets.ToList();
    public Ticket? GetById(Guid id) => _tickets.FirstOrDefault(t => t.Id == id);
    public void Add(Ticket ticket) => _tickets.Add(ticket);
    public bool Delete(Guid id)
    {
        var ticket = GetById(id);
        return ticket is not null && _tickets.Remove(ticket);
    }
}
