using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Books.Commands.RemoveBook
{
    public class RemoveBookCommandHandler : IRequestHandler<RemoveBookCommand, OperationResult<Book?>>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<RemoveBookCommandHandler> _logger;

        public RemoveBookCommandHandler(IBookRepository bookRepository, ILogger<RemoveBookCommandHandler> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<OperationResult<Book?>> Handle(RemoveBookCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling RemoveBookCommand for Book Id: {BookId}", request.Id);

            try
            {
                if (request.Id == Guid.Empty)
                {
                    _logger.LogWarning("RemoveBookCommand received with empty Id.");
                    return OperationResult<Book?>.FailureResult("Id cannot be empty.");
                }

                var book = await _bookRepository.GetBookById(request.Id);
                if (book == null)
                {
                    _logger.LogWarning("RemoveBookCommand received for non-existent Book with Id: {BookId}", request.Id);
                    return OperationResult<Book?>.FailureResult("Book not found.");
                }

                await _bookRepository.DeleteBookById(request.Id);
                _logger.LogInformation("Book with Id: {BookId} removed successfully.", request.Id);
                return OperationResult<Book?>.SuccessResult(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while removing the book with Id: {BookId}", request.Id);
                return OperationResult<Book?>.FailureResult($"An error occurred while removing the book: {ex.Message}");
            }
        }
    }
}