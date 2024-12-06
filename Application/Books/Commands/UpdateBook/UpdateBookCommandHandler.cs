using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;

namespace Application.Books.Commands.UpdateBook
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Book?>
    {
        private readonly IBookRepository _bookRepository;

        public UpdateBookCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<Book?> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            if (request.Book.Id == Guid.Empty)
            {
                throw new System.ArgumentException("Id cannot be empty.", nameof(request.Book.Id));
            }
            if (string.IsNullOrWhiteSpace(request.Book.Title))
            {
                throw new System.ArgumentException("Title cannot be empty.", nameof(request.Book.Title));
            }
            var book = await _bookRepository.UpdateBook(request.id, request.Book);
            return book;
        }
    }
}