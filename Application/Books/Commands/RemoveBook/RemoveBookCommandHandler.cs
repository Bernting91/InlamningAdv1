using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Books.Commands.RemoveBook
{
    public class RemoveBookCommandHandler : IRequestHandler<RemoveBookCommand, OperationResult<Book?>>
    {
        private readonly IBookRepository _bookRepository;

        public RemoveBookCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<OperationResult<Book?>> Handle(RemoveBookCommand request, CancellationToken cancellationToken)
        {
            try
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

                await _bookRepository.DeleteBookById(request.Id);
                return OperationResult<Book?>.SuccessResult(book);
            }
            catch (Exception ex)
            {
                return OperationResult<Book?>.FailureResult($"An error occurred while removing the book: {ex.Message}");
            }
        }
    }
}