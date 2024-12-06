namespace Domain
{
    public class Author
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Author(Guid id, string name)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Id must be a valid GUID.", nameof(id));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            }

            Id = id;
            Name = name;
        }
    }
}