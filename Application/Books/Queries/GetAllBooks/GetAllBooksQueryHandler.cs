using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Books.Queries.GetAllBooks
{
    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, OperationResult<List<Book>>>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<GetAllBooksQueryHandler> _logger;

        public GetAllBooksQueryHandler(IBookRepository bookRepository, ILogger<GetAllBooksQueryHandler> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<OperationResult<List<Book>>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllBooksQuery");

            var books = await _bookRepository.GetAllBooks();
            if (books == null || !books.Any())
            {
                _logger.LogWarning("No books found.");
                return OperationResult<List<Book>>.FailureResult("No books found.");
            }

            _logger.LogInformation("Found {BookCount} books.", books.Count);
            return OperationResult<List<Book>>.SuccessResult(books);
        }
    }
}