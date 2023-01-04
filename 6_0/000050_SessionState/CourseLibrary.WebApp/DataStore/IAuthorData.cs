using CourseLibrary.API.Entities;
using System.Collections.Generic;

namespace CourseLibrary.API.DataStore
{
    public interface IAuthorData
    {
        IEnumerable<Author> GetAuthors();
        void RestoreDataStore();
        
        List<string> AddMyFavouriteAuthors(IServiceProvider services, Guid authorId);
    }
}