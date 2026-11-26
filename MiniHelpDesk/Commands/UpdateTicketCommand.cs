namespace MiniHelpDesk.Commands;

/*
klasa: UpdateTicketCommand

opis:
Przenosi identyfikator i nowe dane edytowanego zgłoszenia.

pola / właściwości:
Id, Guid, identyfikator edytowanego zgłoszenia
Name, string, nowa nazwa
Description, string, nowy opis
Priority, string, nowy priorytet
*/
public sealed record UpdateTicketCommand(Guid Id, string Name, string Description, string Priority);
