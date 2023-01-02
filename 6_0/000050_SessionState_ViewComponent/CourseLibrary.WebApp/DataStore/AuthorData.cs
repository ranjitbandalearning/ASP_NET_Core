using CourseLibrary.API.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.DataStore
{
    public class AuthorData : IAuthorData
    {
        public AuthorData()
        {
            RestoreDataStore();
        }
        public IEnumerable<Author> GetAuthors()
        {
            var authors = JsonConvert.DeserializeObject<IEnumerable<Author>>(File.ReadAllText(@"DataStore\Authors.json"));
            return authors;
        }

        public void RestoreDataStore()
        {
            File.Copy(@"DataStore\Authors-master.json", @"DataStore\Authors.json", true);
        }


        public int GetMyFavouriteAuthors(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session;

            string myFavouriteAuthorIds = session?.GetString("MyFavouriteAuthorIds") ?? string.Empty;

            session?.SetString("MyFavouriteAuthorIds", myFavouriteAuthorIds);

            return myFavouriteAuthorIds.Split(',').Count();
        }

        public int AddMyFavouriteAuthors(IServiceProvider services, Guid authorId)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session;

            string myFavouriteAuthorIds = session?.GetString("MyFavouriteAuthorIds") ?? string.Empty;
            myFavouriteAuthorIds = string.Concat(myFavouriteAuthorIds, ",", authorId);
            session?.SetString("MyFavouriteAuthorIds", myFavouriteAuthorIds);
            
            return myFavouriteAuthorIds.Split(',').Count();
        }

    }
}
