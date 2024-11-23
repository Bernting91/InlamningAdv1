using Domain;
using Infrastructure.Database;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Books.Commands.UpdateBook
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Book?>
    {
        private readonly FakeDatabase _fakeDatabase;

        public UpdateBookCommandHandler(FakeDatabase fakeDatabase)
        {
            _fakeDatabase = fakeDatabase;
        }

        public Task<Book?> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            if (request.Book == null)
            {
                throw new ArgumentNullException(nameof(request.Book), "Book cannot be null");
            }

            Book? bookToUpdate = _fakeDatabase.Books.Find(book => book.Id == request.Book.Id);
            if (bookToUpdate != null)
            {
                bookToUpdate.Title = request.Book.Title;
                bookToUpdate.Description = request.Book.Description;
            }
            return Task.FromResult(bookToUpdate);
        }
    }
}