# WebLocalizerDemoDynamic

**WebLocalizer** demo project. Download, build and run after creating an account in www.weblocalizer.eu. 

Demonstrates *dynamic* objects property localization. *Dynamic* are CLR objects which take part in the HTML rendered by ASP.NET MVC and are usually stored to and retrieved from an *external* source, e.g. database.

It is important to understand how properties are stored back in their original source. The *property localizer* (```MovieJsonPropertyLocalizer```) and *localized factory* (```LocalizedCacheFactory```) which creates the localizer on demand are responsible for this. Look at the included sources (```LocalizedCacheFactory.cs``` and ```MovieJsonPropertyLocalizer.cs```).

Replace the demo credentials with "real" ones in appsettings.json:

    "WebLocalizer": {
        "UserEmail": "demo@weblocallizer.eu",  <-- replace with your email
        "Password": "Password-2021" <--  replace with your password
    }
 
**NB:** Despite just a demo project read the following <a target="_blank" href="https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows">article</a> if you want to use *ASP.NET Core Secrets* to store your credentials. 

There are some <a target="_blank" href="https://www.weblocalizer.eu#videos">videos</a> you may wish to watch.

Enjoy!
