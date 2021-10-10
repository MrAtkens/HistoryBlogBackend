namespace GeekBlog.DataAccess.Models.Interfaces
{
    public interface IObserver
    {
        void Update(Blog newBlog);
    }
}
