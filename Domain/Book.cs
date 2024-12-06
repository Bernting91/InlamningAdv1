namespace Domain
{
    public class Book
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Author Author { get; set; }

        public Book(Guid id, string title, string description, Author author)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Id must be a valid GUID.", nameof(id));
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title cannot be null or empty.", nameof(title));
            }

            if (author == null)
            {
                throw new ArgumentNullException(nameof(author), "Author cannot be null.");
            }

            Id = id;
            Title = title;
            Description = description;
            Author = author;
        }
    }
}