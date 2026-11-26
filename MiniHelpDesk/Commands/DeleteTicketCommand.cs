namespace MiniHelpDesk.Commands;

/*
klasa: DeleteTicketCommand

opis:
Przenosi identyfikator zgłoszenia przeznaczonego do usunięcia.

pola / właściwości:
Id, Guid, identyfikator usuwanego zgłoszenia
*/
public sealed record DeleteTicketCommand(Guid Id);
