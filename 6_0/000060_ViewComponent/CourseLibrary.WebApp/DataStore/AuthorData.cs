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


        public List<string> GetMyFavouriteAuthors(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session;

            string myFavouriteAuthorIds = session?.GetString("MyFavouriteAuthorIds") ?? string.Empty;

            session?.SetString("MyFavouriteAuthorIds", myFavouriteAuthorIds);

            return myFavouriteAuthorIds.Split(',').ToList();
        }

        public List<string> AddMyFavouriteAuthors(IServiceProvider services, Guid authorId)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session;

            string myFavouriteAuthorIds = session?.GetString("MyFavouriteAuthorIds") ?? string.Empty;

            if (string.IsNullOrWhiteSpace(myFavouriteAuthorIds))
            {
                myFavouriteAuthorIds = authorId.ToString();
            }
            else
            {
                if(!myFavouriteAuthorIds.Split(',').Contains(authorId.ToString()))
                {
                    myFavouriteAuthorIds += "," + authorId.ToString();
                }
            }
            session?.SetString("MyFavouriteAuthorIds", myFavouriteAuthorIds);


            return myFavouriteAuthorIds.Split(',').ToList();
        }

    }
}
