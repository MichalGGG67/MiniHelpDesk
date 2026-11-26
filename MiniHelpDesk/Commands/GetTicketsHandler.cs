using MiniHelpDesk.Models;
using MiniHelpDesk.Services;

namespace MiniHelpDesk.Commands;

/*
klasa: GetTicketsHandler

opis:
Realizuje zapytanie odczytujące wszystkie zgłoszenia.

pola / właściwości:
_repository, ITicketRepository, repozytorium zgłoszeń
*/
public sealed class GetTicketsHandler(ITicketRepository repository)
{
    private readonly ITicketRepository _repository = repository;

    /*
funkcja: Handle

opis:
Pobiera listę zgłoszeń bez zmieniania danych.

parametry:
query, GetTicketsQuery, zapytanie o wszystkie zgłoszenia

zwraca:
IReadOnlyList<Ticket>, lista zgłoszeń
*/
    public IReadOnlyList<Ticket> Handle(GetTicketsQuery query) => _repository.GetAll();
}
