namespace BookStoreApi.Features.Books;



using System.ComponentModel.DataAnnotations;
using BookStoreApi.helpers;
using BookStoreApi.Modules;



/**
 *  WRITING SECTION
**/

public class BookRequest
{
    [MinLength(3, ErrorMessage = "Title must be at least 3 characters long.")]
    public required string Title { get; set; }
    [AllowedStrings(["Novel", "Documentary"])]
    public required string Category { get; set; }
}



/**
 *  FETCHING SECTION
**/

public class SortBooksRequest : 
    BookStoreApi.Modules.PaginationRequest,
    BookStoreApi.Modules.ISortableRequest<Book>
{

    [AllowedStrings(["title","category"])]
    public string? sortBy { get; set; }

    [AllowedStrings(["asc","desc"])]
    public string? sortOrder { get; set; }

    public IEnumerable<Book> Sort( List<Book> books )
    {
        // parse params
        string sort_by = "title";
        string sort_order = "asc";

        if ( sortBy is not null)
        {
            sort_by = sortBy;
        }

        if ( sortOrder is not null)
        {
            sort_order = sortOrder;
        }

        // sorting
        IEnumerable<Book> sorted_books = books; 

        if ( sort_by == "title")
        {
            if ( sort_order == "asc" )
                sorted_books = [.. sorted_books.OrderBy( b => b.Title )];
            else  // AllowedStrings will take care of invalid values
                sorted_books = [.. sorted_books.OrderByDescending( b => b.Title )];
        }
        else if ( sort_by == "category")
        {
            if ( sort_order == "asc" )
                sorted_books = [.. sorted_books.OrderBy( b => b.Category )];
            else  // AllowedStrings will take care of invalid values
                sorted_books = [.. sorted_books.OrderByDescending( b => b.Category )];
        }

        return sorted_books;
    }

}



public class SearchBooksRequest :
    BookStoreApi.Modules.ISearchableRequest<Book>
{

    public required string query { get; set; }

    public Book? Search( List<Book> books)
    {
        return books.FirstOrDefault<Book>( b=> b.Title == query );
    }

}


