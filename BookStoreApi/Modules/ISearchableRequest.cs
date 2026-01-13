namespace BookStoreApi.Modules;


public interface ISearchableRequest<T>
{
    public string query { get; set; }
    public T? Search( List<T> data_list );
}