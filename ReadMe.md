ASP.NET Core Technology
1. appsettings.json
	- Application Level Configuration
		- Connection String
		- Logging
		- Tokens Settings
	- IConfiguration Interface Contract
		- Using Which ASP.NET Core Reads the appsettings.json 
2. Program.cs
	- The File that is responsible to start the ASP.NET Core Hosting process in dotnet.exe
	- Host class and its CreateDefaultBuilder() method
		- Returns IHostBuilder and initialize the ASP.NET Core App
		- Uses the 'Kestratl' Hosting Engine
			- Manages all Application Dependencies
				- Sessios
				- Security
				- Request Processing
				- CORS
				- Additional External Dependencies
3. Startup.cs
	- The object dependency and server configuration startup file, contains 'StartUp' class, that is 
		instantiated by the Program class
	- Constructor() injected with IConfiguration interface using the Host class 
	- CongureServices(IServiceCollection) method with IServiceCollection interface
		- IServiceCollection provides the Dependency Container to register all services those are
			needed by ASP.NET Core Application
		- The IServiceCollection interface uses 'ServiceDescriptor' class that will manage 
				the instance serach for all the objects which we want to retrive from the dependencies 
		- The 'ServiceDescriptor' class uses LifeCycle methods to define the scope aka Lifetime of
			the object when the request want to uses it.
			- Singleton
				- Object is live throughout the lifetime of the application
				- Global Object for entire app across all sessions
			- Scopped
				- Object is live for a specific session
				- Global for a specific session aka statefull
			- Transient
				- Object is live only for a request
				- Stateless
	- Configure(IApplicationBuilder, IWebHostEnvironment) method
		- This method is used to manage the Http Pipeline for Request Processing
		- IWebHostEnvironment
			- Interface that detect the type of Host envi,
				- IIS, NginX, Apache, Docker
			- Based on Dev. Mode and Production Mode, decides the execution
		- IApplicationBuilder
			- Configure the Middlewares(?) in the Http Pipeline during the request processing
			- Middlewares, the replacement for HttpHandler and HttpModules
				- Exception, HttpRedirection, CORS, Routig, Session, 
				- Authentication, Authorization, Custom Middlewares

===================================================================================================
Programming With ASP.NET Core
1. Model Layer
	- Entity Classes
	- Data Access Layer
2. Repositories
	- Classes thoses are used to access Data Access Layer
	- These classes will be registered in DI Container
	- used for the support of default repository pattern used by ASP.NET Core
		- USed to isolate Data Access from the Controllers
3. Controllers
	- Inject the Repository in Controllers
		- The Base class is 'Controller', 
			- Further Derived from 'ControllerBase' class
				- This is a common base class for MVC Controllers and API Controllers
				- This class is used for Request Processing
					- Manages Routing
					- Manages Action Filters
					- Security
					- Input Model Validation using HttpPost request  
					- WEB API Execution Method (?)
			- implments IActionFilter for Creating Action Filters e.g. Exception, Logging, etc.
			- The 'Controller' class contains methods for
				- Returning 'MVC View' using View() method
					- Uses 'ViewResult'
						- Class to Compile and Execute view
				- Returning 'Partial View' using PartialView() method, Partial View is like User Control
					- Uses 'PartialViewResult'
						- Class to Compile and Execute partial view
				- Returns JsonResult with Json() method
					- Serialze the Result in JSON format
				- OnActionExecuting() anf OnActionExecuted() methods for Action Method Execution
					- Thes methods are used to execute Action Methods for the controller
				- All Action Methods are by default HttpGet methods
				- Decorate methods as HttpPost to execute them with Post request anf Post model data 
					with request
					- HttpPost methods must validate the input posted model 
	- Action Methods
		- HttpGet and HttpPost
4. Views
	- Razor Views
		- Tag Helpers
		- Partial Views
	- Views will be scaffolded (geberated) based on the Models are known as Stringly typed veiws 
	- View Templates
		- List, will accept IEnumerable of model class to generate View Elements
			- e.g. if model class is Category, then model for view will be
				- IEnumerable<Category>
		- Crate, will accept and empty object as model class
		- Edit, will accept the model class which is to be edited
		- Delete, will accept the   model class to be deleted
	- The RazorPage<TModel> is the base class for the Razor View
		- TModel is the model class that is passed to veiw while scaffolding the view
			- The type of TModel will be decided based on the View Template selected
				- e.g. if View template is 'List' and Model class is 'Category' then
					the TModel will be IEnumerable<Category>
		- The 'Model' property of RazorPage<TModel> class is of the Type TModel, it will be used 
			to load and execute the model class when view is executed
		- The 'ViewData' property is used to pass additional data to the view from the controller
	- The view uses HTML helper methods to shoe labels for the properties from the model class and 
		 show data of the model class
	- The ASP.NET Core Views uses 'TagHelpers' (?) to perform actions
		- Tag helpers are 'server-side' attributes applied on HTML elements on view
			- asp-action: makes Http request to action method from the controller
			- asp-controller: makes Http request to controller
			- asp-items: execute for loop on collection to generate the HTML elements
			- asp-validation-message: validations
			- asp-for: Bind the model property to HTML element to read/write values
		- The Microsoft.AspNetCore.Mvc.TagHelpers assembly defines Tag Helpers
5. Acton Filters
6. Sessions
7. Security
8. WEB API Programming
	- Http Methods
	- JWT Token Security
9. Middlewares
10. Deployment


==================================================================================================
Ex 1: Create Product Repository with CRUD methods and register it in DI
EX 2: Complete Edit/Delete Method of the Category Controller by using Edit/Delete views
Ex 3: Create ProductController with necessary DI
Ex 4: Add Action method to perform CRUD operations for Products using all views.