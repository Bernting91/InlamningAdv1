using Application.Books.Commands.AddBook;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

public class AddBookCommandHandler : IRequestHandler<AddBookCommand, OperationResult<Book>>
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly ILogger<AddBookCommandHandler> _logger;

    public AddBookCommandHandler(IBookRepository bookRepository, IAuthorRepository authorRepository, ILogger<AddBookCommandHandler> logger)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _logger = logger;
    }

    public async Task<OperationResult<Book>> Handle(AddBookCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling AddBookCommand");

        if (request.Book == null)
        {
            _logger.LogWarning("AddBookCommand received with null Book.");
            return OperationResult<Book>.FailureResult("Book cannot be null.");
        }
        if (string.IsNullOrWhiteSpace(request.Book.Title))
        {
            _logger.LogWarning("AddBookCommand received with empty Book title.");
            return OperationResult<Book>.FailureResult("Book title cannot be empty.");
        }
        if (request.Book.Author == null)
        {
            _logger.LogWarning("AddBookCommand received with null Author.");
            return OperationResult<Book>.FailureResult("Author cannot be null.");
        }

        var existingAuthor = await _authorRepository.GetAuthorById(request.Book.Author.Id);
        if (existingAuthor == null)
        {
            await _authorRepository.AddAuthor(request.Book.Author);
        }
        else
        {
            request.Book.Author = existingAuthor;
        }

        var addedBook = await _bookRepository.AddBook(request.Book);
        _logger.LogInformation("Book with Id: {BookId} added successfully.", addedBook.Id);
        return OperationResult<Book>.SuccessResult(addedBook);
    }
}