using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MiniHelpDesk.Models;

/*
klasa: Ticket

opis:
Przechowuje dane pojedynczego zgłoszenia i powiadamia interfejs o zmianach właściwości.

pola / właściwości:
Id, Guid, unikalny identyfikator zgłoszenia
Name, string, nazwa problemu
Description, string, opis problemu
Priority, string, priorytet zgłoszenia
Status, string, aktualny status zgłoszenia
PropertyChanged, event, informuje obserwatorów o zmianie właściwości
*/
public sealed class Ticket : INotifyPropertyChanged
{
    private string _name = string.Empty;
    private string _description = string.Empty;
    private string _priority = "Średni";
    private string _status = "Nowe";

    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get => _name; set => SetField(ref _name, value); }
    public string Description { get => _description; set => SetField(ref _description, value); }
    public string Priority { get => _priority; set => SetField(ref _priority, value); }
    public string Status { get => _status; set => SetField(ref _status, value); }

    public event PropertyChangedEventHandler? PropertyChanged;

    /*
funkcja: SetField

opis:
Ustawia wartość pola i zgłasza zdarzenie PropertyChanged, gdy wartość uległa zmianie.

parametry:
field, T, referencja do pola przechowującego wartość
value, T, nowa wartość pola
propertyName, string, nazwa właściwości zgłaszana obserwatorom

zwraca:
bool, informacja czy wartość została zmieniona
*/
    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        return true;
    }
}
