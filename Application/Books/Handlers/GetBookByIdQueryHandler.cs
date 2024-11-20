using Application.Books.Queries;
using Domain;
using Infrastructure.Database;
using MediatR;

namespace Application.Books.Handlers;
public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, Book?>
{
    private readonly FakeDatabase _fakeDatabase;

    public GetBookByIdQueryHandler(FakeDatabase fakeDatabase)
    {
        _fakeDatabase = fakeDatabase;
    }

    public Task<Book?> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.Id <= 0)
        {
            throw new ArgumentException("Id must be greater than zero.", nameof(request.Id));
        }
        return Task.FromResult(_fakeDatabase.GetBookById(request.Id));
    }
}