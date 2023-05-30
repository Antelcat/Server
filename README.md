# Antelcat.Foundation.Server

Server side of code foundation

Reference to :

+ [Antelcat.Foundation.Core](https://github.com/Antelcat/Foundation-Core)
+ [Feast.RequestMapper](https://github.com/feast107/RequestMapper)

## Dependency-Injection

Expanded extension from [AutowiredServiceProvider](./extern/Foundation-Core/Antelcat.Foundation.Core/Antelcat.Foundation.Core/Implements/Services/AutowiredServiceProvider.cs) of native [.NET dependency injection](https://github.com/dotnet/docs/blob/main/docs/core/extensions/dependency-injection.md) with [Autowired](./extern/Foundation-Core/Antelcat.Foundation.Core/Antelcat.Foundation.Core/Attributes/AutowiredAttribute.cs), provides a way to support properties and fields injection, especially on the server side.

In [ASP.NET Core](https://github.com/dotnet/aspnetcore) :

```c#
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers() //register controllers
                .AddControllersAsServices() // add controllers as services
                .UseAutowiredControllers(); // use auto wired controllers
builder.Host.UseAutowiredServiceProviderFactory(); // autowired services
```

## Authentication  

+ Jwt

    Easily configure jwt authentication by serialize model into claims and back :

    ``` c#
    builder.Services.ConfigureJwt<IdentityModel>(
        configure: static jwt => jwt.Secret = "Your secret key",
        validation: static async (identity,context) => {
            if (identity.Id < 0) context.Fail("Jwt token invalid"); 
        },
        failed: static context => "You are an unauthorized audience"
    );
    ```

    when inherit from [BaseController](./Antelcat.Foundation.Server/Antelcat.Foundation.Server/Controllers/BaseController.cs), controllers can resolve identity like :

    ``` c#
    [ApiController]
    public class IdentityController : BaseController{
        
        [Autowired]
        private JwtConfigure<IdentityModel> configure;

        [HttpPost]
        [AllowAnonymous]
        public IActionResult MyToken([FromBody]IdentityModel identity){
            return configure.CreateToken(identity);
        }

        [HttpGet]
        [Authorize]
        public IActionResult WhoAmI(){
            return base.Identity<IdentityModel>();
        }
    }
    ```