using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MiniHelpDesk.Commands;
using MiniHelpDesk.Models;
using MiniHelpDesk.Services;

namespace MiniHelpDesk.ViewModels;

/*
klasa: TicketViewModel

opis:
Łączy widok z logiką aplikacji, obsługuje formularz, walidację, komendy i liczniki zgłoszeń.

pola / właściwości:
Tickets, ObservableCollection<Ticket>, obserwowalna kolekcja zgłoszeń wyświetlana w CollectionView
Name, string, edytowana nazwa zgłoszenia
Description, string, edytowany opis zgłoszenia
Priority, string, wybrany priorytet
NameError, string, komunikat walidacji nazwy
DescriptionError, string, komunikat walidacji opisu
PriorityError, string, komunikat walidacji priorytetu
SelectedTicket, Ticket, aktualnie wybrane zgłoszenie
IsEditing, bool, informacja czy formularz edytuje istniejące zgłoszenie
TotalCount, int, liczba wszystkich zgłoszeń
OpenCount, int, liczba zgłoszeń niezamkniętych
*/
public sealed class TicketViewModel : INotifyPropertyChanged
{
    private readonly CommandBus _bus;
    private readonly TicketValidator _validator;
    private string _name = "";
    private string _description = "";
    private string _priority = "";
    private string _nameError = "";
    private string _descriptionError = "";
    private string _priorityError = "";
    private Ticket? _selectedTicket;
    private bool _isEditing;

    public ObservableCollection<Ticket> Tickets { get; } = new();
    public IReadOnlyList<string> Priorities => TicketValidator.AllowedPriorities;
    public IReadOnlyList<string> Statuses => ChangeTicketStatusHandler.AllowedStatuses;
    public string Name { get => _name; set { if (Set(ref _name, value)) ValidateForm(); } }
    public string Description { get => _description; set { if (Set(ref _description, value)) ValidateForm(); } }
    public string Priority { get => _priority; set { if (Set(ref _priority, value)) ValidateForm(); } }
    public string NameError { get => _nameError; private set => Set(ref _nameError, value); }
    public string DescriptionError { get => _descriptionError; private set => Set(ref _descriptionError, value); }
    public string PriorityError { get => _priorityError; private set => Set(ref _priorityError, value); }
    public Ticket? SelectedTicket { get => _selectedTicket; private set => Set(ref _selectedTicket, value); }
    public bool IsEditing { get => _isEditing; private set { if (Set(ref _isEditing, value)) OnPropertyChanged(nameof(FormTitle)); } }
    public string FormTitle => IsEditing ? "Edytuj zgłoszenie" : "Nowe zgłoszenie";
    public int TotalCount => Tickets.Count;
    public int OpenCount => Tickets.Count(t => t.Status != "Zamknięte");
    public string SaveButtonText => IsEditing ? "Zapisz zmiany" : "Dodaj zgłoszenie";

    public ICommand SaveTicketCommand { get; }
    public ICommand ClearFormCommand { get; }
    public ICommand EditTicketCommand { get; }
    public ICommand DeleteTicketCommand { get; }
    public ICommand ChangeTicketStatusCommand { get; }
    public event PropertyChangedEventHandler? PropertyChanged;
    public event Action<string>? MessageRequested;

    public TicketViewModel(CommandBus bus, TicketValidator validator)
    {
        _bus = bus;
        _validator = validator;
        SaveTicketCommand = new RelayCommand(SaveTicket);
        ClearFormCommand = new RelayCommand(ClearForm);
        EditTicketCommand = new RelayCommand<Ticket>(EditTicket);
        DeleteTicketCommand = new RelayCommand<Ticket>(DeleteTicket);
        ChangeTicketStatusCommand = new RelayCommand<Ticket>(ChangeStatus);
        RefreshTickets();
    }

    /*
funkcja: SaveTicket

opis:
Waliduje formularz, a następnie wysyła komendę dodania lub aktualizacji zgłoszenia.

parametry:
brak

zwraca:
void, nic nie zwraca
*/
    public void SaveTicket()
    {
        ValidateForm();
        if (HasErrors) { MessageRequested?.Invoke("Popraw wskazane pola przed zapisaniem zgłoszenia."); return; }
        try
        {
            var wasEditing = IsEditing;
            if (IsEditing && SelectedTicket is not null)
                _bus.Send(new UpdateTicketCommand(SelectedTicket.Id, Name, Description, Priority));
            else
                _bus.Send(new AddTicketCommand(Name, Description, Priority));
            RefreshTickets();
            ClearForm();
            MessageRequested?.Invoke(wasEditing ? "Zgłoszenie zostało zaktualizowane." : "Zgłoszenie zostało dodane.");
        }
        catch (Exception ex) { MessageRequested?.Invoke(ex.Message); }
    }

    public bool HasErrors => !string.IsNullOrEmpty(NameError) || !string.IsNullOrEmpty(DescriptionError) || !string.IsNullOrEmpty(PriorityError);

    /*
funkcja: ValidateForm

opis:
Aktualizuje komunikaty walidacji na podstawie bieżącej zawartości formularza.

parametry:
brak

zwraca:
void, nic nie zwraca
*/
    public void ValidateForm()
    {
        NameError = _validator.ValidateName(Name) ?? "";
        DescriptionError = _validator.ValidateDescription(Description) ?? "";
        PriorityError = _validator.ValidatePriority(Priority) ?? "";
        OnPropertyChanged(nameof(HasErrors));
    }

    /*
funkcja: RefreshTickets

opis:
Wykonuje zapytanie CQRS i synchronizuje obserwowalną kolekcję z repozytorium.

parametry:
brak

zwraca:
void, nic nie zwraca
*/
    public void RefreshTickets()
    {
        var items = _bus.Ask(new GetTicketsQuery());
        Tickets.Clear();
        foreach (var ticket in items) Tickets.Add(ticket);
        OnPropertyChanged(nameof(TotalCount));
        OnPropertyChanged(nameof(OpenCount));
    }

    /*
funkcja: EditTicket

opis:
Wczytuje dane wybranego zgłoszenia do formularza edycji.

parametry:
ticket, Ticket, zgłoszenie przeznaczone do edycji

zwraca:
void, nic nie zwraca
*/
    public void EditTicket(Ticket ticket)
    {
        SelectedTicket = ticket;
        IsEditing = true;
        Name = ticket.Name;
        Description = ticket.Description;
        Priority = ticket.Priority;
        OnPropertyChanged(nameof(SaveButtonText));
        ValidateForm();
    }

    /*
funkcja: DeleteTicket

opis:
Wysyła komendę usunięcia zgłoszenia i odświeża listę oraz liczniki.

parametry:
ticket, Ticket, zgłoszenie przeznaczone do usunięcia

zwraca:
void, nic nie zwraca
*/
    public void DeleteTicket(Ticket ticket)
    {
        _bus.Send(new DeleteTicketCommand(ticket.Id));
        if (SelectedTicket?.Id == ticket.Id) ClearForm();
        RefreshTickets();
    }

    /*
funkcja: ChangeStatus

opis:
Przechodzi do kolejnego statusu za pomocą komendy i odświeża liczniki.

parametry:
ticket, Ticket, zgłoszenie, którego status ma zostać zmieniony

zwraca:
void, nic nie zwraca
*/
    public void ChangeStatus(Ticket ticket)
    {
        var index = Array.IndexOf(Statuses.ToArray(), ticket.Status);
        var next = Statuses[(index + 1) % Statuses.Count];
        _bus.Send(new ChangeTicketStatusCommand(ticket.Id, next));
        OnPropertyChanged(nameof(OpenCount));
    }

    /*
funkcja: ClearForm

opis:
Czyści formularz i przełącza go z powrotem w tryb dodawania.

parametry:
brak

zwraca:
void, nic nie zwraca
*/
    public void ClearForm()
    {
        SelectedTicket = null;
        IsEditing = false;
        Name = "";
        Description = "";
        Priority = "";
        OnPropertyChanged(nameof(SaveButtonText));
        ValidateForm();
    }



    private bool Set<T>(ref T field, T value, [CallerMemberName] string? name = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(name);
        return true;
    }
    private void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

/*
klasa: RelayCommand<T>

opis:
Udostępnia metodę obiektu jako komendę możliwą do powiązania z kontrolką XAML.

pola / właściwości:
execute, Action<T>, metoda wykonywana po uruchomieniu komendy
*/
public sealed class RelayCommand<T>(Action<T> execute) : ICommand
{
    public event EventHandler? CanExecuteChanged;
    public bool CanExecute(object? parameter) => parameter is T;
    public void Execute(object? parameter) { if (parameter is T value) execute(value); }
}
