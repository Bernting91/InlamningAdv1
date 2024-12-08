using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Books.Queries.GetbookbyID
{
    public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, OperationResult<Book?>>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<GetBookByIdQueryHandler> _logger;

        public GetBookByIdQueryHandler(IBookRepository bookRepository, ILogger<GetBookByIdQueryHandler> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<OperationResult<Book?>> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetBookByIdQuery for Book Id: {BookId}", request.Id);

            if (request.Id == Guid.Empty)
            {
                _logger.LogWarning("GetBookByIdQuery received with empty Id.");
                return OperationResult<Book?>.FailureResult("Id cannot be empty.");
            }

            var book = await _bookRepository.GetBookById(request.Id);
            if (book == null)
            {
                _logger.LogWarning("GetBookByIdQuery received for non-existent Book with Id: {BookId}", request.Id);
                return OperationResult<Book?>.FailureResult("Book not found.");
            }

            _logger.LogInformation("Book with Id: {BookId} found.", request.Id);
            return OperationResult<Book?>.SuccessResult(book);
        }
    }
}