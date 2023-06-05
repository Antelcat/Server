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