using Application.Books.Commands;
using Domain;
using Infrastructure.Database;
using MediatR;

namespace Application.Books.Handlers;
public class AddBookCommandHandler : IRequestHandler<AddBookCommand, Book>
{
    private readonly FakeDatabase _fakeDatabase;

    public AddBookCommandHandler(FakeDatabase fakeDatabase)
    {
        _fakeDatabase = fakeDatabase;
    }

    public Task<Book> Handle(AddBookCommand request, CancellationToken cancellationToken)
    {
        if (request.Book == null)
        {
            throw new ArgumentNullException(nameof(request.Book), "Book cannot be null.");
        }
        if (string.IsNullOrWhiteSpace(request.Book.Title))
        {
            throw new ArgumentException("Title cannot be empty.", nameof(request.Book));
        }
        return Task.FromResult(_fakeDatabase.AddNewBook(request.Book));
    }
}