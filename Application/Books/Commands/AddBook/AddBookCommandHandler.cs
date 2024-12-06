using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;

namespace Application.Books.Commands.AddBook
{
    public class AddBookCommandHandler : IRequestHandler<AddBookCommand, Book>
    {
        private readonly IBookRepository _bookRepository;

        public AddBookCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
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

            _bookRepository.AddBook(request.Book);
            return Task.FromResult(request.Book);
        }
    }
}