using CourseLibrary.API.Entities;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseLibrary.WebApp.Components
{
    public class FavouriteAuthorSummary : ViewComponent
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IServiceProvider _serviceProvider;

        public FavouriteAuthorSummary(ICourseLibraryRepository courseLibraryRepository,
            IServiceProvider services)
        {
            _courseLibraryRepository = courseLibraryRepository ??
                throw new ArgumentNullException(nameof(courseLibraryRepository));
            _serviceProvider = services ??
                throw new ArgumentNullException(nameof(services));
            //_courseLibraryRepository.RestoreDataStore();
        }

        public IViewComponentResult Invoke()
        {
           int lintAuthorsCount = _courseLibraryRepository.GetMyFavouriteAuthorsCount(_serviceProvider);
            return View(lintAuthorsCount);
        }
    }
}
