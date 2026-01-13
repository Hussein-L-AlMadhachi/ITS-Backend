
namespace BookStoreApi.Modules;


public class PaginationMetadata
{
    public required int TotalCount {set; get;}
    public required int CurrentPage{set; get;}
    public required int TotalPages{set; get;}
}


public class PaginationResponse<T>
{
    public required PaginationMetadata Metadata {set; get;}
    public required List<T> Data {set; get;}
}

public class PaginationRequest
{
    public required int page { get; set; }
    public required int pageSize { get; set; }

    public PaginationResponse<T> Paginate<T>( IEnumerable<T> data_list )
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 50) pageSize = 50; // cap it

        int data_size = data_list.Count();
        int extra_page = (data_size % pageSize) > 0  ? 1 : 0;
        int total_pages = (data_size/pageSize) + extra_page;

        return new PaginationResponse<T>(  ) {
            Metadata = new PaginationMetadata() { TotalCount = data_size, CurrentPage = page, TotalPages=total_pages },
            Data = [.. data_list.Skip((page - 1) * pageSize).Take(pageSize)]
        };
    }
}

