using CourseLibrary.API.Entities;
using CourseLibrary.API.Helpers;
using CourseLibrary.API.Models;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseLibrary.API.Controllers
{
    //////We could also inherit it from Controller, but by doing so, we'd also add support for views, which isn't needed when building an API.
    public class AuthorsController : Controller
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IServiceProvider _serviceProvider;

        public AuthorsController(ICourseLibraryRepository courseLibraryRepository, IServiceProvider serviceProvider)
        {
            _courseLibraryRepository = courseLibraryRepository ??
                throw new ArgumentNullException(nameof(courseLibraryRepository));
            _serviceProvider = serviceProvider ??
              throw new ArgumentNullException(nameof(serviceProvider));
            _courseLibraryRepository.RestoreDataStore();
        }

        //////IActionResult defines a contract that represents the results of an action method.
        public IActionResult List()
        {
            var authorsFromRepo = _courseLibraryRepository.GetAuthors();
            ViewBag.WhatIsThisList = "This is list of Authors";
            
            List<AuthorDto> authorDtos= new List<AuthorDto>();
            
            foreach (var aurthorModel in authorsFromRepo)
            {
                authorDtos.Add(CreateAuthorViewModelFromAuthorEntityModel(aurthorModel));
            }

            return View(authorDtos);

            //THE FOLLOING RESPONSES FOR WEBAPI
            //////JsonResult is an action result which formats the given object as JSON.
            //return new JsonResult(authorsFromRepo);
            //return Ok(authorsFromRepo);
        }

        
        public IActionResult Details(Guid authorId)
        {
            var authorFromRepo = _courseLibraryRepository.GetAuthor(authorId);

            if (authorFromRepo == null)
            {
                return NotFound();
            }

            return View(CreateAuthorViewModelFromAuthorEntityModel(authorFromRepo));
        }


        public IActionResult AddtoMyFavouriteAuthor(Guid authorId)
        {
            var authorFromRepo = _courseLibraryRepository.AddMyFavouriteAuthors(_serviceProvider, authorId);
            string lstrAuthors = string.Empty;
            foreach (var author in authorFromRepo)
            {
                lstrAuthors = string.Concat(lstrAuthors, author.Id.ToString(), ", ");

            }
            //NOTE:    ViewBag.MyFavouriteAuthors (or) ViewData["MyFavouriteAuthors"]  doesn't work here
            TempData["MyFavouriteAuthors"] = lstrAuthors.Trim().TrimEnd(',');
            return RedirectToAction("List");
        }

        private AuthorDto CreateAuthorViewModelFromAuthorEntityModel(Author aurthorModel)
        {
            AuthorDto authorDto = new AuthorDto
            {
                Id = aurthorModel.Id,
                MainCategory = aurthorModel.MainCategory,
                Name = string.Concat(aurthorModel.FirstName, " ", aurthorModel.LastName),
                Age = aurthorModel.DateOfBirth.GetCurrentAge(),
                CourseDtos = new List<CourseDto>()
            };

            foreach (var courseModel in aurthorModel.Courses)
            {
                authorDto.CourseDtos.Add(new CourseDto
                {
                    Id = courseModel.Id,
                    AuthorId = courseModel.AuthorId,
                    Description = courseModel.Description,
                    Title = courseModel.Title
                });
            }

            return authorDto;
        }

    }
}
