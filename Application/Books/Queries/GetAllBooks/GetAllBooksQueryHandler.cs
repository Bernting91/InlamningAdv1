using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using System.Security.Cryptography;

namespace Application.Books.Queries.GetAllBooks
{
    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, OperationResult<List<Book>>>
    {
        private readonly IBookRepository _bookRepository;

        public GetAllBooksQueryHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<OperationResult<List<Book>>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            var books = await _bookRepository.GetAllBooks();
            if (books == null || !books.Any())
            {
                return OperationResult<List<Book>>.FailureResult("No books found.");
            }
            return OperationResult<List<Book>>.SuccessResult(books);
        }
    }
}