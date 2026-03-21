namespace UrlShorten.Models.Commons
{
    public interface IEntityId<TKey>
    {
        TKey Id { get; set; }
    }
}
