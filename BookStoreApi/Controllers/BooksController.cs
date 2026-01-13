namespace BookStoreApi.Controllers;


using Microsoft.AspNetCore.Mvc;
using BookStoreApi.Features.Books;
using BookStoreApi.Modules;



[ApiController]
[Route("/[controller]")]
public class BooksController : ControllerBase
{

    private readonly List<string> allowedCategories = new List<string>()
    {
        "Novel",
        "Documentary"
    };
    public static List<Book> Books = new()
    {
        new Book() { Id = 1, Title = "Xyz", Category = "Novel" },
        new Book() { Id = 2, Title = "Abc", Category = "Documentary" },
    };



    [HttpPost]
    public ActionResult CreateBook([FromBody] BookRequest request)
    {
        var newId = Books.Max(b => b.Id) + 1;
        if (!allowedCategories.Contains(request.Category))
            return BadRequest("Category is invalid");

        var book_found = Books.Where(b => b.Title.Equals(request.Title, StringComparison.CurrentCultureIgnoreCase)).ToList();
        if ( book_found.Count == 0 )
        {
            return BadRequest("Book with this title already exists");
        }

        var newBook = new Book()
        {
            Title = request.Title,
            Category = request.Category,
            Id = newId
        };
        Books.Add(newBook);
        return Created();
    }

    [HttpGet]
    public ActionResult<PaginationResponse<Book> > Get( [FromQuery] SortBooksRequest req )
    {
        return Ok( req.Paginate(  req.Sort(Books)  ) );
    }

    [HttpGet("{id:int}")]
    public ActionResult<Book> GetSingleBook(int id)
    {
        return Books[id];
    }

    [HttpGet("search")]
    public ActionResult<Book> Search( [FromQuery] SearchBooksRequest req )
    {
        Book? lookup = req.Search( Books );
        if ( lookup is null)    return NotFound("Book not found");

        return lookup;
    }

    [HttpPut("/{id:int}")]
    public ActionResult UpdateBook( int id , [FromBody] BookRequest request)
    {
        try {
            var book_found = Books[ id ];
        } catch
        {
            return NotFound("Book not found");
        }

        if( request.Category is not null )
        {
            if( !allowedCategories.Contains(request.Category))
            {
                return BadRequest("Catagory is invalid");
            }
            Books[id].Category = request.Category;
        }

        if( request.Title is not null)
        {
            Books[id].Title = request.Title;
        }

        return NoContent();
    }

    [HttpGet("/catagory/{catagory}")]
    public ActionResult<List<Book>> GetByCatagory( string catagory )
    {
        var filtered = Books.Where(b => b.Category.Equals(catagory, StringComparison.CurrentCultureIgnoreCase)).ToList();
        return Ok( filtered );
    }


    [HttpDelete("{id}")]
    public ActionResult DeleteBook(int id)
    {
        var book = Books.FirstOrDefault(b => b.Id == id);
        if (book is null)
            return NotFound();
        Books.Remove(book);
        return NoContent();
    }

}
