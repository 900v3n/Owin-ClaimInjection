# Owin-ClaimInjection
Injecting user claims for testing purpose.

# How to use

Refer to the sample below to register local authentication module in the pipeline.
``` csharp
if(runOnLocal == true)
{
    var roleMappings = new Dictionary<GroupType, string>()
    {
        { GroupType.MasterAdministrator, "0f0c15cd-5c79-4726-866f-df23b43aa0c0" },
        { GroupType.Administrator, "d23900ca-8073-4edf-8ae7-d76fd95fae17" },
        { GroupType.InternalUser, "3dd1671c-cf5b-406a-8e62-f35578bfe500" }
    };

    var users = new List<ClaimInjection.Model.User>
    {
        new User { Name = "Master Admin", Email = "masteradmin@example.com", Group = GroupType.MasterAdministrator },
        new User { Name = "Administrator", Email = "administrator@example.com", Group = GroupType.Administrator },
        new User { Name = "Internal User", Email = "internaluser@example.com", Group = GroupType.InternalUser }
    };

    app.UseLocalAuthentication(roleMappings, users);
}
else
{
    // Register actual authentication.
}

```

To trigger local authentication from test code, navigate to the site with following query string.

``` csharp
http://sitename.example.com/?user=internaluser@example.com

```