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

        public AuthorsController(ICourseLibraryRepository courseLibraryRepository)
        {
            _courseLibraryRepository = courseLibraryRepository ??
                throw new ArgumentNullException(nameof(courseLibraryRepository));
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
                authorDtos.Add(new AuthorDto
                {
                    Id = aurthorModel.Id,
                    MainCategory = aurthorModel.MainCategory,
                    Name = string.Concat(aurthorModel.FirstName, " ", aurthorModel.LastName),
                    Age = aurthorModel.DateOfBirth.GetCurrentAge()
                }); ;
            }

            return View(authorDtos);

            //THE FOLLOING RESPONSES FOR WEBAPI
            //////JsonResult is an action result which formats the given object as JSON.
            //return new JsonResult(authorsFromRepo);
            //return Ok(authorsFromRepo);
        }
    }
}
