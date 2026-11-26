using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniHelpDesk.Commands;
using MiniHelpDesk.Models;
using MiniHelpDesk.Services;
using System.ComponentModel;

namespace MiniHelpDesk.Tests;

[TestClass]
public sealed class TicketLogicTests
{
    [TestMethod]
    public void NameValidationRejectsTooShortName()
    {
        var validator = new TicketValidator();
        Assert.IsNotNull(validator.ValidateName(" ab "));
        Assert.IsNull(validator.ValidateName(" abc "));
    }

    [TestMethod]
    public void DescriptionValidationRequiresTenTrimmedCharacters()
    {
        var validator = new TicketValidator();
        Assert.IsNotNull(validator.ValidateDescription(" 123456789 "));
        Assert.IsNull(validator.ValidateDescription(" 1234567890 "));
    }

    [TestMethod]
    public void PriorityValidationAcceptsOnlyDefinedValues()
    {
        var validator = new TicketValidator();
        Assert.IsNotNull(validator.ValidatePriority("Pilny"));
        Assert.IsNull(validator.ValidatePriority("Wysoki"));
    }

    [TestMethod]
    public void AddHandlerCreatesTicketWithNewStatusAndTrimmedFields()
    {
        var repo = new InMemoryTicketRepository();
        var handler = new AddTicketHandler(repo, new TicketValidator());
        var ticket = handler.Handle(new AddTicketCommand("  Drukarka  ", "  Nie drukuje dokumentów  ", "Średni"));
        Assert.AreEqual("Drukarka", ticket.Name);
        Assert.AreEqual("Nowe", ticket.Status);
        Assert.AreEqual(1, repo.GetAll().Count);
    }

    [TestMethod]
    public void AddHandlerDoesNotSaveInvalidTicket()
    {
        var repo = new InMemoryTicketRepository();
        var handler = new AddTicketHandler(repo, new TicketValidator());
        Assert.ThrowsException<ArgumentException>(() => handler.Handle(new AddTicketCommand("x", "krótki", "Średni")));
        Assert.AreEqual(0, repo.GetAll().Count);
    }

    [TestMethod]
    public void DeleteHandlerRemovesTicket()
    {
        var repo = new InMemoryTicketRepository();
        var ticket = new Ticket { Name = "Awaria", Description = "Opis problemu technicznego", Priority = "Niski" };
        repo.Add(ticket);
        Assert.IsTrue(new DeleteTicketHandler(repo).Handle(new DeleteTicketCommand(ticket.Id)));
        Assert.AreEqual(0, repo.GetAll().Count);
    }

    [TestMethod]
    public void StatusChangeRaisesPropertyChanged()
    {
        var repo = new InMemoryTicketRepository();
        var ticket = new Ticket { Name = "Awaria", Description = "Opis problemu technicznego", Priority = "Niski" };
        repo.Add(ticket);
        string? changed = null;
        ticket.PropertyChanged += (_, e) => changed = e.PropertyName;
        new ChangeTicketStatusHandler(repo).Handle(new ChangeTicketStatusCommand(ticket.Id, "W trakcie"));
        Assert.AreEqual("Status", changed);
        Assert.AreEqual("W trakcie", ticket.Status);
    }

    [TestMethod]
    public void UpdateHandlerChangesFieldsAndKeepsTicketIdentity()
    {
        var repo = new InMemoryTicketRepository();
        var ticket = new Ticket { Name = "Awaria", Description = "Opis problemu technicznego", Priority = "Niski" };
        repo.Add(ticket);
        var updated = new UpdateTicketHandler(repo, new TicketValidator())
            .Handle(new UpdateTicketCommand(ticket.Id, "Nowa nazwa", "Dłuższy opis problemu", "Wysoki"));
        Assert.AreSame(ticket, updated);
        Assert.AreEqual("Nowa nazwa", ticket.Name);
        Assert.AreEqual("Wysoki", ticket.Priority);
    }
}

