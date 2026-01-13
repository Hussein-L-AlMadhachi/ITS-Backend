namespace BookStoreApi.Modules;



public interface ISortableRequest<T>
{
    public string? sortBy { get; set; }
    public string? sortOrder { get; set; }
    public IEnumerable<T> Sort( List<T> data_list );
}