using Application.Books.Commands;
using Domain;
using Infrastructure.Database;
using MediatR;

namespace Application.Books.Handlers;
public class RemoveBookCommandHandler : IRequestHandler<RemoveBookCommand, Book?>
{
    private readonly FakeDatabase _fakeDatabase;

    public RemoveBookCommandHandler(FakeDatabase fakeDatabase)
    {
        _fakeDatabase = fakeDatabase;
    }

    public Task<Book?> Handle(RemoveBookCommand request, CancellationToken cancellationToken)
    {
        if (request.Book == null)
        {
            throw new ArgumentNullException(nameof(request.Book), "Book cannot be null.");
        }
        return Task.FromResult(_fakeDatabase.RemoveBook(request.Book));
    }
}