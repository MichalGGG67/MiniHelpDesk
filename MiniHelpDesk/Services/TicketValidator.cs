namespace MiniHelpDesk.Services;

/*
klasa: TicketValidator

opis:
Centralizuje walidację danych zgłoszenia używaną przy dodawaniu i edycji.

pola / właściwości:
AllowedPriorities, IReadOnlyList<string>, dozwolone wartości priorytetu
*/
public sealed class TicketValidator
{
    public static IReadOnlyList<string> AllowedPriorities { get; } = new[] { "Niski", "Średni", "Wysoki" };

    /*
funkcja: ValidateName

opis:
Sprawdza, czy nazwa po usunięciu spacji na brzegach ma co najmniej trzy znaki.

parametry:
name, string, nazwa zgłoszenia

zwraca:
string?, komunikat błędu lub null, gdy nazwa jest poprawna
*/
    public string? ValidateName(string? name) => string.IsNullOrWhiteSpace(name) || name.Trim().Length < 3
        ? "Nazwa musi mieć co najmniej 3 znaki." : null;

    /*
funkcja: ValidateDescription

opis:
Sprawdza, czy opis po usunięciu spacji na brzegach ma co najmniej dziesięć znaków.

parametry:
description, string, opis zgłoszenia

zwraca:
string?, komunikat błędu lub null, gdy opis jest poprawny
*/
    public string? ValidateDescription(string? description) => string.IsNullOrWhiteSpace(description) || description.Trim().Length < 10
        ? "Opis musi mieć co najmniej 10 znaków." : null;

    /*
funkcja: ValidatePriority

opis:
Sprawdza, czy wybrany priorytet należy do dozwolonego zestawu.

parametry:
priority, string, wybrany priorytet

zwraca:
string?, komunikat błędu lub null, gdy priorytet jest poprawny
*/
    public string? ValidatePriority(string? priority) => !AllowedPriorities.Contains(priority ?? string.Empty)
        ? "Wybierz priorytet zgłoszenia." : null;

    /*
funkcja: Validate

opis:
Zwraca komplet błędów walidacji dla danych zgłoszenia.

parametry:
name, string, nazwa zgłoszenia
description, string, opis zgłoszenia
priority, string, priorytet zgłoszenia

zwraca:
IReadOnlyDictionary<string, string>, słownik błędów przypisanych do pól
*/
    public IReadOnlyDictionary<string, string> Validate(string? name, string? description, string? priority)
    {
        var errors = new Dictionary<string, string>();
        var nameError = ValidateName(name);
        var descriptionError = ValidateDescription(description);
        var priorityError = ValidatePriority(priority);
        if (nameError is not null) errors[nameof(name)] = nameError;
        if (descriptionError is not null) errors[nameof(description)] = descriptionError;
        if (priorityError is not null) errors[nameof(priority)] = priorityError;
        return errors;
    }
}
