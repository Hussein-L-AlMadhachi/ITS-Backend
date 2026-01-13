namespace BookStoreApi.Controllers;


using Microsoft.AspNetCore.Mvc;
using BookStoreApi.Features.Authors;



[ApiController]
[Route("/[controller]")]
public class AuthorsController : ControllerBase
{

    private readonly List<string> allowedCategories = new List<string>()
    {
        "Novel",
        "Documentary"
    };
    public static List<Author> authors = [
        new() { Id = 1, Name = "George Orwell" },
        new() { Id = 2, Name = "Yuval Noah Harari" }
    ];


    [HttpGet]
    public ActionResult<List<Author>> Get( [FromQuery] SortAuthorsRequest req )
    {
        return Ok( req.Paginate(  req.Sort(authors)  ) );
    }

    [HttpPost]
    public ActionResult CreateAuthor([FromBody] AuthorRequest request)
    {
        var newId = authors.Max(a => a.Id) + 1;

        var author_found = authors.Where(a => a.Name.Equals(request.Name, StringComparison.CurrentCultureIgnoreCase)).ToList();
        if ( author_found.Count == 0 )
        {
            return BadRequest("Book with this title already exists");
        }

        var newAuthor = new Author()
        {
            Id = newId,
            Name = request.Name
        };
        authors.Add(newAuthor);
        return Created();
    }


    [HttpGet("search")]
    public ActionResult<Author> Search( [FromQuery] SearchAuthorsRequest req )
    {
        Author? lookup = req.Search( authors );
        if ( lookup is null)    return NotFound("Author not found");

        return lookup;
    }

    [HttpGet("{id:int}")]
    public ActionResult<Author> GetSingleAuthor(int id)
    {
        try {
            return Ok(authors[id]);
        } catch {
            return NotFound("Author not found");
        }
    }

    [HttpPut("{id:int}")]
    public ActionResult UpdateAuthor( int id , [FromBody] AuthorRequest request)
    {

        try {
            var author_found = authors[ id ];
        } catch {
            return NotFound("Author not found");
        }

        if( request.Name is not null )
        {
            if ( request.Name.Length < 3 )
                return BadRequest("Author name must be at least 3 characters long");
            authors[id].Name = request.Name;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteBook(int id)
    {
        var author = authors.FirstOrDefault(a => a.Id == id);
        if (author is null)
            return NotFound();
        authors.Remove(author);
        return NoContent();
    }

}
