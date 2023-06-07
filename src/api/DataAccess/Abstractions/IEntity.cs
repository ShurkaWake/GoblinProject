namespace DataAccess.Abstractions
{
    public interface IEntity<TKey> where TKey : IComparable<TKey>
    {
        public TKey Id { get; }
    }
}
