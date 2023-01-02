using CourseLibrary.API.DataStore;
using CourseLibrary.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICourseLibraryRepository, CourseLibraryRepository>();
builder.Services.AddSingleton<IAuthorData, AuthorData>();

//The below extension method enables MVC in this project.
builder.Services.AddControllersWithViews();

var app = builder.Build();

//The below MapGet - middleware component listens to the root of the
//application and return "Hello World!"
//app.MapGet("/", () => "Hello World!");

//The below UseStaticFiles - middleware component brings in the support for returning static files.
app.UseStaticFiles();

//The below UseDeveloperExceptionPage - middleware component helps to see errors inside of my executing application.
//This will show developer exception page with internal details so conditional statement is required.
if(app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

//The below  - middleware component helps to navigate our pages.
//So that ASP NET CORE handle incoming requests correctly
//This sets some defaults in MVC to route to the views that we are going to have 
app.MapDefaultControllerRoute();

app.Run();
