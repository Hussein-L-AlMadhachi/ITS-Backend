namespace BookStoreApi.Features.Books;



public class Book
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Category { get; set; }
}
