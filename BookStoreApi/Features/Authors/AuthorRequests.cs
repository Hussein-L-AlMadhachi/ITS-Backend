namespace BookStoreApi.Features.Authors;

using BookStoreApi.helpers;


public class AuthorRequest
{
    public required string Name { get; set; }
}

public class SortAuthorsRequest : 
    BookStoreApi.Modules.PaginationRequest,
    BookStoreApi.Modules.ISortableRequest<Author>
{
    [AllowedStrings(["name"])]
    public string? sortBy { get; set; }

    [AllowedStrings(["asc","desc"])]
    public string? sortOrder { get; set; }

    public IEnumerable<Author> Sort( List<Author> authors )
    {
        string sort_order = "asc";

        if ( sortOrder is not null)
        {
            sort_order = sortOrder;
        }

        // sorting
        IEnumerable<Author> sorted_books = authors; 

        if ( sort_order == "asc" )
            sorted_books = [.. sorted_books.OrderBy( b => b.Name )];
        else  // AllowedStrings will take care of invalid values
            sorted_books = [.. sorted_books.OrderByDescending( b => b.Name )];

        return sorted_books;
    }

}

public class SearchAuthorsRequest :
    BookStoreApi.Modules.ISearchableRequest<Author>
{

    public required string query { get; set; }

    public Author? Search( List<Author> authors )
    {
        return authors.FirstOrDefault<Author>( a => a.Name.Equals(query, StringComparison.CurrentCultureIgnoreCase) );
    }

}

