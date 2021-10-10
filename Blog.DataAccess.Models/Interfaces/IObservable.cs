namespace GeekBlog.DataAccess.Models.Interfaces
{
    public interface IObservable
    {
        void addObserver(IObserver o);
        void removeObserver(IObserver o);
        void notifyObservers();
    }

}
