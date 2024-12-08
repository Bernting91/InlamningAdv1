using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Books.Commands.AddBook
{
    public class AddBookCommandHandler : IRequestHandler<AddBookCommand, OperationResult<Book>>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<AddBookCommandHandler> _logger;

        public AddBookCommandHandler(IBookRepository bookRepository, ILogger<AddBookCommandHandler> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<OperationResult<Book>> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling AddBookCommand");

            if (request.Book == null)
            {
                _logger.LogWarning("AddBookCommand received with null Book.");
                return OperationResult<Book>.FailureResult("Book cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(request.Book.Title))
            {
                _logger.LogWarning("AddBookCommand received with empty Book title.");
                return OperationResult<Book>.FailureResult("Book title cannot be empty.");
            }
            if (request.Book.Author == null)
            {
                _logger.LogWarning("AddBookCommand received with null Author.");
                return OperationResult<Book>.FailureResult("Author cannot be null.");
            }

            var addedBook = await _bookRepository.AddBook(request.Book);
            _logger.LogInformation("Book with Id: {BookId} added successfully.", addedBook.Id);
            return OperationResult<Book>.SuccessResult(addedBook);
        }
    }
}