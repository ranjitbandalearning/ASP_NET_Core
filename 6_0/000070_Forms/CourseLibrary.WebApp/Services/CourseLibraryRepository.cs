﻿using CourseLibrary.API.DataStore;
using CourseLibrary.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CourseLibrary.API.Services
{
    public class CourseLibraryRepository : ICourseLibraryRepository
    {
        private readonly IAuthorData _authorData;

        public CourseLibraryRepository(IAuthorData authorData)
        {
            _authorData = authorData ??
                throw new ArgumentNullException(nameof(authorData));
        }

        public IEnumerable<Author> GetAuthors()
        {
            return _authorData.GetAuthors();
            //return _context.Authors.ToList<Author>();
        }

      

        public void RestoreDataStore()
        {
            _authorData.RestoreDataStore();
        }


        public Author GetAuthor(Guid authorId)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            return _authorData.GetAuthors().FirstOrDefault(a => a.Id == authorId);
        }

        public IEnumerable<Author> GetMyFavouriteAuthors(IServiceProvider services)
        {
            List<string> lcolFavouriteAurthors = _authorData.GetMyFavouriteAuthors(services);
            return _authorData.GetAuthors().Where(x => lcolFavouriteAurthors.Contains(x.Id.ToString().ToLower()));
        }

        public IEnumerable<Author> AddMyFavouriteAuthors(IServiceProvider services, Guid authorId)
        {
            List<string> lcolFavouriteAurthors = _authorData.AddMyFavouriteAuthors(services, authorId);
            return _authorData.GetAuthors().Where(x => lcolFavouriteAurthors.Contains(x.Id.ToString().ToLower()));
        }

        public int GetMyFavouriteAuthorsCount(IServiceProvider services)
        {
            List<string> lcolAuthorIds = _authorData.GetMyFavouriteAuthors(services);
            
            //Fix for the empty collection
            return (lcolAuthorIds.Count == 1 ? (lcolAuthorIds[0].Trim().Length > 0 ? 1 : 0) : lcolAuthorIds.Count);
        }

        public void AddAuthor(Author author)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            // the repository fills the id (instead of using identity columns)
            author.Id = Guid.NewGuid();

            foreach (var course in author.Courses)
            {
                course.Id = Guid.NewGuid();
                course.AuthorId = author.Id;
            }

            _authorData.Authors.Add(author);

        }

        public bool Save()
        {
            return (_authorData.SaveChanges() >= 0);
        }

        public void AddCourse(Guid authorId, Course course)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }
            // always set the AuthorId to the passed-in authorId
            course.AuthorId = authorId;
            course.Id = Guid.NewGuid();
            _authorData.Authors.First(x => x.Id == authorId).Courses.Add(course);
        }

        ////// public IEnumerable<Author> GetAuthors(IEnumerable<Guid> authorIds)
        //////{
        //////    //if (authorIds == null)
        //////    //{
        //////    //    throw new ArgumentNullException(nameof(authorIds));
        //////    //}

        //////    //return _context.Authors.Where(a => authorIds.Contains(a.Id))
        //////    //    .OrderBy(a => a.FirstName)
        //////    //    .OrderBy(a => a.LastName)
        //////    //    .ToList();
        //////    return _authorData.GetAuthors();

        //public void DeleteCourse(Course course)
        //{
        //    _context.Courses.Remove(course);
        //}

        //public Course GetCourse(Guid authorId, Guid courseId)
        //{
        //    if (authorId == Guid.Empty)
        //    {
        //        throw new ArgumentNullException(nameof(authorId));
        //    }

        //    if (courseId == Guid.Empty)
        //    {
        //        throw new ArgumentNullException(nameof(courseId));
        //    }

        //    return _context.Courses
        //      .Where(c => c.AuthorId == authorId && c.Id == courseId).FirstOrDefault();
        //}

        //public IEnumerable<Course> GetCourses(Guid authorId)
        //{
        //    if (authorId == Guid.Empty)
        //    {
        //        throw new ArgumentNullException(nameof(authorId));
        //    }

        //    return _context.Courses
        //                .Where(c => c.AuthorId == authorId)
        //                .OrderBy(c => c.Title).ToList();
        //}

        //public void UpdateCourse(Course course)
        //{
        //    // no code in this implementation
        //}

        //public void AddAuthor(Author author)
        //{
        //    if (author == null)
        //    {
        //        throw new ArgumentNullException(nameof(author));
        //    }

        //    // the repository fills the id (instead of using identity columns)
        //    author.Id = Guid.NewGuid();

        //    foreach (var course in author.Courses)
        //    {
        //        course.Id = Guid.NewGuid();
        //    }

        //    _context.Authors.Add(author);
        //}

        //public bool AuthorExists(Guid authorId)
        //{
        //    if (authorId == Guid.Empty)
        //    {
        //        throw new ArgumentNullException(nameof(authorId));
        //    }

        //    return _context.Authors.Any(a => a.Id == authorId);
        //}

        //public void DeleteAuthor(Author author)
        //{
        //    if (author == null)
        //    {
        //        throw new ArgumentNullException(nameof(author));
        //    }

        //    _context.Authors.Remove(author);
        //}


        //public void UpdateAuthor(Author author)
        //{
        //    // no code in this implementation
        //}

        //public bool Save()
        //{
        //    return (_context.SaveChanges() >= 0);
        //}

        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

        //protected virtual void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //       // dispose resources when needed
        //    }
        //}
    }
}
