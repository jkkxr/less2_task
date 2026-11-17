namespace less2_task.Services
{
    public interface IDataProvider<T>
    {
        public Task<ICollection<T>> GetAllAsync();

        public Task SaveChanges(ICollection<T> items);
    }
}
