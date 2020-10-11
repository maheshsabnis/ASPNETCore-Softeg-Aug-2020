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
				- Session
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
	- A View can accept 'Only-One' model object during scaffolding
		- displaying data from more than one moedl in View
		- Use ViewBag or ViewDataDictyionary to send data from multiple models to view
5. Acton Filters
	- Error Handling
		- Error Hadling at Model Level (Recommended approach)
		- Custom Model validations aka Domain specific model data validation
			- Creat a custom classs derived from ValidationAttribuete class
				- Override IsValid() method with logic of validation
		- Error Handling for Process Based Execution aka while executing the business logic
			- USe Exception Handling
	- Action Filters getes executed during the MVC and API Controller Request Processing
		- Gloabl FIlter->Controller Filter->Action Filter (OnActionExecuting)
		- (OnActionExecuted) ActionFilter -> Controller Filter -> Global Filter 
			- Custom Filters for
					- Logging
					- Exception Management (Recommended to use the Middlewares)	
			- IActionFilter Interface must be implemented by the custom filter class
			- ActionFilterAttribute, the abatrsct base class or custom filters to override the 
				Result execution
6. Sessions
	- Add the InMemory Distributed Case Service to store sessions
	- Use AddSession () method for session configuration
	- By default only primptive types are stored in Sessions e.g. Number, String, Date, boolean, etc
	- To store a CLR object in session use Custom Session Provider
7. Security
	- Secure Access to the controller
		- User Credentials security, simple User based authentication (default)
		- Role based security, simple Role based authorization
		- Policy Based Authentication, enhancements in RBS, we group roles to define access policies
			- e.g. Read/Write, etc.
		- Cloud based OpenId Aothentication aks OpenIdConnect using Cloud AD e.g. Azure Active Directory
		- Token Based Authentication, used for API Securty
			- Third Party client apps using API
	- Microsoft.AspNetCore.Identity, package with
		- UserManager<IdentityUser>
			- Used for User and Role Management
			- Add Role to User
		- RoleManager<IdentityRole>
			- Class used for Role Management
		- SingInManager<IdentityUser>
			- Manage the User SignIn	
		- Identity Middleware
			- AddDefaultIdentity<IdentityUser>()
				- Service method to Provide User Based Authentication
			- AddIdentity<IdentityUser, IdentityRole>()
				- Service Method to provide Role Based Authentication and Authorization
			- AddAuthentication(),
				- Serice method to provide USer Based Auth using EntityFraeworkCore
					- Microsoft.AspNetCore.Identity.EntityFrameworkCore package
				- Define scehams for Authentication (default is Basic)
				- Can define schemas for Token Based Authentication
			- AddAuthorization()
				- Service method to manage Roles and authorization using 
					- Microsoft.AspNetCore.Identity.EntityFrameworkCore package
				- Configured for Policy Based Authentications
			- UseAuthentication()
				- Middleware  that provides the User Based Auth, for HttpContext and HttpRequest in it
			- UserAuthorization()
				- Middleware that is used to execute Httprequest in HttpContext based on Role-Based-Security		
		- Identity Scaffolder as Razor UI Library to provides Views for Identity
			- All these veiws are Razor WebForms
	- Adding Roles by creating a new RoleController and its action methods for creating roles and vies
		to create and list roles.
			- RoeManager<IdentityRole>
				- CreateAsync() method to create role
				- The 'Roles' an IEnumerable<IdentityRole>
	- Assign tole to user while creating user.
	- Policy Based  Authentication
		- A new approach for clubing multiple Roles together inso single policy
		- Define the policies using PolicyBuilder class in 'AddAuthorization()' service method

8. WEB API Programming
	- Http Methods
		- HttpGet / HttpGet("<Template Expression to read values from Headers>")
		- HttpPost/ HttpPost("<Template Expression to read values from Headers>")
		- HttpPut / ----//----
		- HttpDelete / ----//----
		- HttpPatch / ----//----
	- Define the JSON options for serailications for serializing CLR object in original
	property names instead of camel case
	- ApiControllerAttribute is a class applied on API Controller class. This is used to map 
	   the HTTP Request BODY  from HTTP  POST and PUT Request  to the CLR Object passed to the
	   action method.
	   - Parameter Model Binders
		- FromBody
			- Read data from HTTP Request Body and map with CLR Object
		- FromRoute
			- Read data from HTTP Router Parameters and   map with CLR Object
		- FromQuery
			- Read data from HTTP Query String and  map with CLR Object
		- FromForm
			- REad data from HTML FormModel (HTML 5) and  map with CLR Object
	- JWT Token Security 
		- Token is a Randomly generated values that contains identity information (aka Claim), thst is 
		used to verify the client (Authentication) with its access rights (Authorization) and proides
		access of the API 
			- Json Web Tokens (JWT)
				- W3C standard corss platform mechanism of Authenticating the client App.
				- 3 Secitions of JWT
					- Header
						- Algorithm to encrypt the information of identity
					- Payload
						- The Identity Claim used by the client to access the API
							- UserId or RoleId
							- UserId and RoleId
					- Signeture
						- The value to check an integrity of the Token aka Crypto Signeture
			- Security Access Markup Language (SAML) Tokens
				- Used by the Active Directory to provides secure idneityt to client  
			- AUTHORIZATION 'Bearer <TOKEN>'
9. Middlewares
	- CReate a class that is ctor injected with RequestDelegat
		- this delegate accepts HttpContext as input parameter 
	- The class will have InvokeAsync() method, that accepts HttpContext as input parameter
		- this method contains logic for Middleware
	- Create an externsion class with extension methid for IApplicationBuilder to register
		the middleware

10. The Cross-Origin-Resource-Sharing (Thee CORS Policy)
	- ust be defined by the service so that Browser CLients (aka JavaScript) clients from different
		origin are able to access teh APIs.
			- services.AddCors(); --> to Set CORS policies
			- app.UseCors(); --> To apply the CORS middleware
11. Deployment
	- On-Premises Deployment
		- Self Host
		- IIS 
			- COnfigure Database Server having IIS Access Rights
	- Cloud
		- Microsoft Azure
			- Migrate Databases to Azure
			- Modify the COnnection String in Application
			- Deploye the App on Azure App Service
	- DOcker
		- Recommended only in case of Microservices


==================================================================================================
Ex 1: Create Product Repository with CRUD methods and register it in DI
EX 2: Complete Edit/Delete Method of the Category Controller by using Edit/Delete views
Ex 3: Create ProductController with necessary DI
Ex 4: Add Action method to perform CRUD operations for Products using all views.


===========================================================================================
Ex 4: Date 30-08-2020
When the navigation takes place from Error View to the Controller, for Create View, 
show the data thast has cause exception
===========================================================================================
Ex 5: Date 06-09-2020
Modify the Custom Exception Filter to Log the Exception along with request details e.g. Model posted
error message in database. The database must have table called as RequestLog, that will log all messagess
along with exception message using following structure
	- Request Id
	- ControllerName
	- ActionName
	- DateTime
	- ExceutionStatus (Successs/excepion)
	- ExceptionType e.g. Database exception /custom exception etc.
	- ExceptionMessage


Date 13-09-2020

Ex 6: Create a default 'Administrator' role in Application Startup and admin@myuser.com as default user.
Add admin@myuser.com to the Administrator Role. (Write this logic in 'Configure()' method)
1. RoleController, can be accessed only by Administrator Role users
2. Modify the RoleController to assign Role to User


EX 7: CReate a Custom Middleare that will log the information of  Each Expcption Request  into 
database with excpeiton information as follows
	- User Name (Read this from the Request Header)
		- HttpCOntext.Request.Headers.Authorization
			- Provide the USer Name
		- Exception Message
		- The Model Data Passed as Request.
			- Write a custom Exception class that will read all the Model Errors
				- ModelState
Mail the solution on sabnis_m@hotmail.com
