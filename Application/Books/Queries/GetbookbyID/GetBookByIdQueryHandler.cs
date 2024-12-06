using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;


namespace Application.Books.Queries.GetbookbyID
{
    public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, Book?>
    {
        private readonly IBookRepository _bookRepository;

        public GetBookByIdQueryHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public Task<Book?> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                throw new System.ArgumentException("Id cannot be empty.", nameof(request.Id));
            }
            return _bookRepository.GetBookById(request.Id);
        }
    }
}