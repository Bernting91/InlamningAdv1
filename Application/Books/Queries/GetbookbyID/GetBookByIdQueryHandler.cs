using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;


namespace Application.Books.Queries.GetbookbyID
{
    public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, OperationResult<Book?>>
    {
        private readonly IBookRepository _bookRepository;

        public GetBookByIdQueryHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<OperationResult<Book?>> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                return OperationResult<Book?>.FailureResult("Id cannot be empty.");
            }
            var book = await _bookRepository.GetBookById(request.Id);
            if (book == null)
            {
                return OperationResult<Book?>.FailureResult("Book not found.");
            }
            return OperationResult<Book?>.SuccessResult(book);
        }
    }
}