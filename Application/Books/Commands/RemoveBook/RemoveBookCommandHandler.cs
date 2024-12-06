using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Books.Commands.RemoveBook
{
    public class RemoveBookCommandHandler : IRequestHandler<RemoveBookCommand, Book?>
    {
        private readonly IBookRepository _bookRepository;

        public RemoveBookCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<Book?> Handle(RemoveBookCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(request.Id), "Book ID cannot be null or empty.");
            }

            var book = await _bookRepository.GetBookById(request.Id);
            if (book == null)
            {
                throw new KeyNotFoundException("Book not found.");
            }

            await _bookRepository.DeleteBookById(request.Id);
            return book;
        }
    }
}