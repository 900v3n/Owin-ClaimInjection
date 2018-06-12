# Owin-ClaimInjection
Injecting user claims for testing purpose.

# How to use

Refer to the sample below to register local authentication module in the pipeline.
``` csharp
if(runOnLocal == true)
{
    // Create local users.
    var users = new List<User>(){
        new User{ Name = "Internal User", Role = "InternalUser", Email = "internaluser@example.com" }
    };

    app.UseLocalAuthentication(new LocalAuthenticationOptions(users))
}
else
{
    // Register actual authentication.
}

```

To trigger local authentication, navigate to the site with following query string.

``` csharp
http://sitename.example.com/?user=internaluser@example.com

```