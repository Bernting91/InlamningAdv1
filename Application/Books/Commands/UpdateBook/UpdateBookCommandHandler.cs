using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Books.Commands.UpdateBook
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, OperationResult<Book?>>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<UpdateBookCommandHandler> _logger;

        public UpdateBookCommandHandler(IBookRepository bookRepository, ILogger<UpdateBookCommandHandler> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<OperationResult<Book?>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling UpdateBookCommand for Book Id: {BookId}", request.Book?.Id);

            try
            {
                if (request.Book == null)
                {
                    _logger.LogWarning("UpdateBookCommand received with null Book.");
                    return OperationResult<Book?>.FailureResult("Book cannot be null.");
                }

                if (request.Book.Id == Guid.Empty)
                {
                    _logger.LogWarning("UpdateBookCommand received with empty Id.");
                    return OperationResult<Book?>.FailureResult("Id cannot be empty.");
                }

                if (string.IsNullOrWhiteSpace(request.Book.Title))
                {
                    _logger.LogWarning("UpdateBookCommand received with empty Book title.");
                    return OperationResult<Book?>.FailureResult("Book title cannot be empty.");
                }

                if (request.Book.Author == null)
                {
                    _logger.LogWarning("UpdateBookCommand received with null Author.");
                    return OperationResult<Book?>.FailureResult("Author cannot be null.");
                }

                var existingBook = await _bookRepository.GetBookById(request.Book.Id);
                if (existingBook == null)
                {
                    _logger.LogWarning("UpdateBookCommand received for non-existent Book with Id: {BookId}", request.Book.Id);
                    return OperationResult<Book?>.FailureResult("Book with that ID does not exist.");
                }

                var updatedBook = await _bookRepository.UpdateBook(request.Book.Id, request.Book);
                _logger.LogInformation("Book with Id: {BookId} updated successfully.", request.Book.Id);
                return OperationResult<Book?>.SuccessResult(updatedBook);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the book with Id: {BookId}", request.Book?.Id);
                return OperationResult<Book?>.FailureResult($"An error occurred while updating the book: {ex.Message}");
            }
        }
    }
}