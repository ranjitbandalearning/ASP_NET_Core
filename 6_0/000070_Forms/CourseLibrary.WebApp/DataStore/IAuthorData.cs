using CourseLibrary.API.Entities;
using System.Collections.Generic;

namespace CourseLibrary.API.DataStore
{
    public interface IAuthorData
    {
        IEnumerable<Author> GetAuthors();
        void RestoreDataStore();
        List<string> GetMyFavouriteAuthors(IServiceProvider services);
        List<string> AddMyFavouriteAuthors(IServiceProvider services, Guid authorId);
        
        List<Author> Authors { get; }
        int SaveChanges();
    }
}