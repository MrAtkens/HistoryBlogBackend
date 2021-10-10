using GeekBlog.DataAccess.Models.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace GeekBlog.DataAccess.Models.Patterns
{
    public class BlogsObservable : IObservable
    {
        public List<IObserver> observers = new List<IObserver>();
        public List<Blog> blogs = new List<Blog>();

        public void AddBlog(Blog blog)
        {
            blogs.Add(blog);
            notifyObservers();
        }
        public void addObserver(IObserver o)
        {
            observers.Add(o);
        }
        public void removeObserver(IObserver o)
        {
            observers.Remove(o);
        }

        public void notifyObservers()
        {
            foreach(IObserver observer in observers)
            {
                observer.Update(blogs.Last());
            }
        } 

    }
}
