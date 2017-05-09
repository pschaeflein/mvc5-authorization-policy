# mvc5-authorization-policy
Authorization concepts from ASP.NET Core back-ported to ASP.NET 4.6 / MVC 5

##ASP.NET Core Security
Changes in security for ASP.NET core MVC include a policy-based scheme for authorizing requests. This project is a back-port of the source of this scheme for ASP.NET 5 (.Net Fx 4.6.1)
The ASP.NET core source code is available on GitHub at [https://github.com/aspnet/Security](https://github.com/aspnet/Security).

##Usage
Following the convention documented for [ASP.NET core Claims-based Authorization](https://docs.asp.net/en/latest/security/authorization/claims.html), The mvc5-authorization-policy project includes a custom [AuthorizeAttribute] for declaring a policy.

```
[Schaeflein.Community.MVC5AuthZPolicy.Authorize(Policy="LocalIssuer")]
public class ManageController : Controller
{
  // your actions here
}
```

A policy must be registered with the Authorization Service. This registration can be accomplished via a base class or via OWIN middleware.

###Base Class
```
public ControllerBase()
{
  var options = new AuthorizationOptions();
  options.AddPolicy("Users", new AuthorizationPolicyBuilder()
                                  .RequireAuthenticatedUser()
                                  .AddRequirements(new WindowsLocalUserPolicyRequirement())
                                  .Build());

  authorizationService = new AuthorizationService(
    new AuthorizationPolicyProvider(options),
    new IAuthorizationHandler[] { new PassThroughAuthorizationHandler() });
}
```

###OWIN Middleware
```
public partial class Startup
{
  // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
  public void ConfigureAuth(IAppBuilder app)
  {
    var options = new AuthorizationOptions();
    options.AddPolicy("LocalIssuer", new AuthorizationPolicyBuilder()
                                           .AddRequirements(new LocalIssuerRequirement())
                                           .Build());

  app.UseMVC5AuthZPolicy(options); //You may also pass a Func<IAuthorizationService> to use as a factory, this is required if you want to inject requirements handlers
  // additional setup
}
```

## Policies
Policies have a name, and are implemented via a Requirement and Handler. Requirements are expressed via the AuthenticationOptions that are passed to the service. 
Handlers that are passed to the service are invoked during the AuthorizeAsync method. The provided PassThroughAuthorizationHandler will invoke handler methods that exist on the Requirement classes.

###Sample Requirement/Handler
```
public class LocalIssuerRequirement : AuthorizationHandler<LocalIssuerRequirement>, IAuthorizationRequirement
{
  public LocalIssuerRequirement() { }

  public string Issuer
  {
    get
    {
      return "ASP.NET Identity";
    }
  }
  public string ClaimType
  {
	  get
    {
      return "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider";
    }
  }

  protected override void Handle(AuthorizationContext context, LocalIssuerRequirement requirement)
  {
    Claim issuerClaim = context.User.FindFirst(c => c.Type == ClaimType);

    if (issuerClaim.Value == Issuer)
    {
      context.Succeed(requirement);
    }
  }
}
``` 