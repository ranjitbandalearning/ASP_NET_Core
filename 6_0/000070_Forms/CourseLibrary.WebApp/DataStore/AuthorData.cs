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
        bool isRestoredDatabaseCalled = false;
        List<Author> _authors = null;
        string path = @"DataStore\Authors.json";

        public List<Author> Authors
        {
            get
            {
                if (_authors == null || !_authors.Any())
                {
                    _authors = GetAuthors().ToList();
                }
                return _authors;
            }
            //set
            //{
            //    if(value != null)
            //    {
            //        _authors.AddRange(value);
            //    }
            //}
        }


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
            if(isRestoredDatabaseCalled == false)
            {
                File.Copy(@"DataStore\Authors-master.json", @"DataStore\Authors.json", true);

                isRestoredDatabaseCalled = true;
            }
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

        public int SaveChanges()
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(this.Authors));
            return 0;
        }

    }
}
