namespace MiniHelpDesk.Commands;

/*
klasa: ChangeTicketStatusCommand

opis:
Przenosi identyfikator zgłoszenia i status, który ma zostać ustawiony.

pola / właściwości:
Id, Guid, identyfikator zgłoszenia
Status, string, nowy status
*/
public sealed record ChangeTicketStatusCommand(Guid Id, string Status);
