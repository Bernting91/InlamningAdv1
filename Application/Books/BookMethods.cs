using Application.Books.Commands;
using Application.Books.Queries;
using Domain;
using Infrastructure.Database;
using MediatR;
using System.Threading.Tasks;

namespace Application.Books
{
    public class BookMethods
    {
        private readonly IMediator _mediator;

        public BookMethods(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Book> AddNewBook(Book book)
        {
            return await _mediator.Send(new AddBookCommand(book));
        }

        public async Task<Book?> RemoveBook(Book book)
        {
            return await _mediator.Send(new RemoveBookCommand(book));
        }

        public async Task<Book?> UpdateBook(Book book)
        {
            return await _mediator.Send(new UpdateBookCommand(book));
        }

        public async Task<Book?> GetBookById(int id)
        {
            return await _mediator.Send(new GetBookByIdQuery(id));
        }
    }
}