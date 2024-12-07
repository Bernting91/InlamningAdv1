using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

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
                if (request.Book == null)
                {
                    return OperationResult<Book?>.FailureResult("Book cannot be null.");
                }

                if (request.Book.Id == Guid.Empty)
                {
                    return OperationResult<Book?>.FailureResult("Id cannot be empty.");
                }

                if (string.IsNullOrWhiteSpace(request.Book.Title))
                {
                    return OperationResult<Book?>.FailureResult("Book title cannot be empty.");
                }

                if (request.Book.Author == null)
                {
                    return OperationResult<Book?>.FailureResult("Author cannot be null.");
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