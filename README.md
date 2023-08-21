# Antelcat.Foundation.Server

Server side of code foundation

Reference to :

+ [Antelcat.Foundation.Core](https://github.com/Antelcat/Foundation-Core)
+ [Feast.RequestMapper](https://github.com/feast107/RequestMapper)

## Dependency-Injection


## Authentication  

+ Jwt

    Easily configure jwt authentication by serialize model into claims and back :

    ``` c#
    builder.Services.ConfigureJwt<IdentityModel>(
        configure: static jwt => jwt.Secret = "Your secret key",
        validation: static async (identity,context) => {
            if (identity.Id < 0) context.Fail("Jwt token invalid"); 
        },
        denied: static context => "Your role has no permission",
        failed: static context => "You are an unauthorized audience"
    );
    ```

    when inherit from [BaseController](/src/Antelcat.Server/Controllers/BaseController.cs), controllers can resolve identity like :

    ``` c#
    [ApiController]
    public class IdentityController : BaseController<IdentityController>{
        
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

+ Cookie

    Cookie authentication seems to be less related to identity model but you still need to provide it :

    ``` c#
    builder.Services.ConfigureCookie<IdentityModel>(
        denied: static context => "Your role has no permission",
        failed: static context => "You are an unauthorized audience"
    );
    ```

    when inherit from [BaseController](./Antelcat.Foundation.Server/Antelcat.Foundation.Server/Controllers/BaseController.cs), controllers can resolve identity like :

    ``` c#
    [ApiController]
    public class IdentityController : BaseController<IdentityController>{
        
        [Autowired]
        private JwtConfigure<IdentityModel> configure;

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignInAsync([FromBody]IdentityModel identity){
            base.SignInAsync(identity, "User");
            return "Successfully login";
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> SignOutAsync(){
            await SignOutAsync();
            return "Successfully logout";
        }

        [HttpGet]
        [Authorize]
        public IActionResult WhoAmI(){
            return base.Identity<IdentityModel>();
        }
    }
    ```
