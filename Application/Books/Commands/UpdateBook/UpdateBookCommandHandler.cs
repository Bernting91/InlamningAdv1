using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;

namespace Application.Books.Commands.UpdateBook
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, OperationResult<Book?>>
    {
        private readonly IBookRepository _bookRepository;

        public UpdateBookCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<OperationResult<Book?>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Book.Id == Guid.Empty)
                {
                    return OperationResult<Book?>.FailureResult("Id cannot be empty.");
                }

                if (string.IsNullOrWhiteSpace(request.Book.Title))
                {
                    return OperationResult<Book?>.FailureResult("Book title cannot be empty.");
                }

                var existingBook = await _bookRepository.GetBookById(request.Book.Id);
                if (existingBook == null)
                {
                    return OperationResult<Book?>.FailureResult("Book with that ID does not exist.");
                }

                var updatedBook = await _bookRepository.UpdateBook(request.Book.Id, request.Book);
                return OperationResult<Book?>.SuccessResult(updatedBook);
            }
            catch (Exception ex)
            {
                return OperationResult<Book?>.FailureResult($"An error occurred while updating the book: {ex.Message}");
            }
        }
    }
}