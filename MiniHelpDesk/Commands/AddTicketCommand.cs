namespace MiniHelpDesk.Commands;

/*
klasa: AddTicketCommand

opis:
Przenosi dane potrzebne do utworzenia nowego zgłoszenia.

pola / właściwości:
Name, string, nazwa zgłoszenia
Description, string, opis zgłoszenia
Priority, string, wybrany priorytet
*/
public sealed record AddTicketCommand(string Name, string Description, string Priority);
