using Domain;
using Infrastructure.Database;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Books.Queries.GetbookbyID
{
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
                throw new ArgumentException("Invalid book ID");
            }

            Book? book = _fakeDatabase.Books.Find(book => book.Id == request.Id);
            return Task.FromResult(book);
        }
    }
}