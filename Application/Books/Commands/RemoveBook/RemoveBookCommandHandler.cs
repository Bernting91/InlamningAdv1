using Domain;
using Infrastructure.Database;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Books.Commands.RemoveBook
{
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
                throw new ArgumentNullException(nameof(request.Book), "Book cannot be null");
            }

            if (_fakeDatabase.Books.Remove(request.Book))
            {
                return Task.FromResult<Book?>(request.Book);
            }
            else
            {
                return Task.FromResult<Book?>(null);
            }
        }
    }
}