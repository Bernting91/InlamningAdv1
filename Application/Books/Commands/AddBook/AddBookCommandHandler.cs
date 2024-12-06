using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;

namespace Application.Books.Commands.AddBook
{
    public class AddBookCommandHandler : IRequestHandler<AddBookCommand, OperationResult<Book>>
    {
        private readonly IBookRepository _bookRepository;

        public AddBookCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<OperationResult<Book>> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            if (request.Book == null)
            {
                return OperationResult<Book>.FailureResult("Book cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(request.Book.Title))
            {
                return OperationResult<Book>.FailureResult("Book title cannot be empty.");
            }

            _bookRepository.AddBook(request.Book);
            return OperationResult<Book>.SuccessResult(request.Book);
        }
    }
}