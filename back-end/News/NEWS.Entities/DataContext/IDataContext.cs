namespace NEWS.Entities.DataContext
{
    public interface IDataContext : IDisposable
    {
        int SaveChanges();
    }
}
