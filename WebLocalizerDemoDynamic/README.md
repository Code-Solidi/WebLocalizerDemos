# WebLocalizerDemoDynamic

**WebLocalizer** demo project. Download, build and run after creating an account in www.weblocalizer.eu. 

Demonstrates *dynamic* objects property localization. *Dynamic* are CLR objects which take part in the HTML rendered by ASP.NET MVC and are usually stored to and retrieved from an *external* source, e.g. database.

It is important to understand how properties are stored back in their original source. The *property localizer* (```MovieJsonPropertyLocalizer```) and *localized factory* (```LocalizedCacheFactory```) which creates the localizer on demand are responsible for this. Look at the included sources (```LocalizedCacheFactory.cs``` and ```MovieJsonPropertyLocalizer.cs```).

Replace the texts within '<' and '>' in appsettings.json:

    "WebLocalizer": {
        "UserEmail": "<your email here>",  <-- your email
        "Password": "<your password here>" <-- your password
    }
 
**NB:** Despite just a demo project read the following <a target="_blank" href="https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows">article</a> if you want to use *ASP.NET Core Secrets* to store your credentials. 

There's a video in <a target="_blank" href="https://www.youtube.com/watch?v=RFJMKX8FULM">youtube.com</a> you may wish to watch.

Enjoy!
