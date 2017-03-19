Single Sign-On your asp.net web app

1.  Add user to the Active Directory

![C:\\Users\\HardikMistry\\Desktop\\single sign on\\add user to active directtory.png](media/f542c078a18d2c2cdd3cf921ae35fa1e.png)

Give a username

![C:\\Users\\HardikMistry\\Desktop\\single sign on\\give a username.png](media/5a7e6087928bc4e076d49a532b8e22aa.png)

Fill in other details of the user you are adding, currently we are adding a user
with User Role

![C:\\Users\\HardikMistry\\Desktop\\single sign on\\set other details of the user.png](media/273a9eb99bd1b609ce9229b6c4c936e8.png)

Copy and save the temporary password

![C:\\Users\\HardikMistry\\Desktop\\single sign on\\save temporary password.png](media/7c7035681534810cf529a44929f00739.png)

Create a new ASP.NET MVC Web Application

![C:\\Users\\HardikMistry\\Desktop\\single sign on\\add web app - set project name.png](media/65b212048887371a75e89e6e4ce7e5d9.png)

![C:\\Users\\HardikMistry\\Desktop\\single sign on\\create mvc app - no authentication.png](media/d9919648af820add41fd174f88bdba75.png)

![C:\\Users\\HardikMistry\\Desktop\\single sign on\\ensure - no authentication.png](media/d871746fa67912a39c36fa515593c832.png)

Add the following packages using the Nuget Package Manager

-   Microsoft.Owin.IdentityModels.Protocol.Extensions

-   System.IdentityModel.Tokens.Jwt **(Please use v4.0.x of this package,
    upgrading to v5.0.x breaks the code)**

-   Microsoft.Owin.Security.OpenIdConnect

-   Microsoft.Owin.Security.Cookies

-   Microsoft.Owin.Host.SystemWeb

Right click App\_Start folder and Add a class, name it Startup.Auth.cs

![C:\\Users\\HardikMistry\\Desktop\\single sign on\\create Startup.Auth.cs class in App\_Start folder.png](media/79dcaf711905910a665d72d7e4470ed2.png)

**Code:**

using System;

using System.Collections.Generic;

using System.Linq;

using System.Text;

using System.Threading.Tasks;

using Owin;

using Microsoft.Owin.Security;

using Microsoft.Owin.Security.Cookies;

using Microsoft.Owin.Security.OpenIdConnect;

using System.Configuration;

using System.Globalization;

namespace binaryfactory.azuread //replace this namespace with your project name

{

    public partial class Startup

    {

        private static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];

        private static string aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];

        private static string tenant = ConfigurationManager.AppSettings["ida:Tenant"];

        private static string postLogoutRedirectUri = ConfigurationManager.AppSettings["ida:PostLogoutRedirectUri"];

        string authority = String.Format(CultureInfo.InvariantCulture, aadInstance, tenant);

        public void ConfigureAuth(IAppBuilder app)

        {

            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseOpenIdConnectAuthentication(

                new OpenIdConnectAuthenticationOptions

                {

                    ClientId = clientId,

                    Authority = authority,

                    PostLogoutRedirectUri = postLogoutRedirectUri,

                    Notifications = new OpenIdConnectAuthenticationNotifications

                    {

                        AuthenticationFailed = context =\>

                        {

                            context.HandleResponse();

                            context.Response.Redirect("/Error?message=" + context.Exception.Message);

                            return Task.FromResult(0);

                        }

                    }

                });

        }

    }

}

Right click the project and add a OWIN Startup Class and name is Startup.cs and
add invoke ConfigureAuth(app) as shown

![C:\\Users\\HardikMistry\\Desktop\\single sign on\\add Owin Startup class and call it Startup.png](media/9f6671d63f6496872919127c5e2a97c2.png)

Right click the controllers folder and name it AccountController as shown

![C:\\Users\\HardikMistry\\Desktop\\single sign on\\add Empty MVC controller.png](media/9b612e167c2ab3a9e9ca782132c594d8.png)

![C:\\Users\\HardikMistry\\Desktop\\single sign on\\add AccountController.png](media/5232ce4ae1e34058c08f5dcd163080d0.png)

**Code: // copy paste the following as illustrated above**

public void SignIn()

        {

            // Send an OpenID Connect sign-in request.

            if (!Request.IsAuthenticated)

            {

                HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "/" }, OpenIdConnectAuthenticationDefaults.AuthenticationType);

            }

        }

        public void SignOut()

        {

            // Send an OpenID Connect sign-out request.

            HttpContext.GetOwinContext().Authentication.SignOut(

                OpenIdConnectAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);

        }

Right click the View/Shared folder and add a MVC Partial page and name it
\_LoginPartial.cshtml as shown and copy the following code

![C:\\Users\\HardikMistry\\Desktop\\single sign on\\add mvc partial layout.png](media/7a1b46e4f1b33ac6ee60f225f264f455.png)

![C:\\Users\\HardikMistry\\Desktop\\single sign on\\add code to login partial.png](media/9b92fa7a4e3936602f230b3a3c074342.png)

**Code:**

\@if (Request.IsAuthenticated)

{

    \<text\>

        \<ul class="nav navbar-nav navbar-right"\>

            \<li class="navbar-text"\>

                Hello, \@User.Identity.Name!

            \</li\>

            \<li\>

                \@Html.ActionLink("Sign out", "SignOut", "Account")

            \</li\>

        \</ul\>

    \</text\>

}

else

{

    \<ul class="nav navbar-nav navbar-right"\>

        \<li\>\@Html.ActionLink("Sign in", "SignIn", "Account", routeValues: null, htmlAttributes: new { id = "loginLink" })\</li\>

    \</ul\>

}

Open the \_Layout.cshtml and modified as shown below

![C:\\Users\\HardikMistry\\Desktop\\single sign on\\add partial page to layout dot cshtml.png](media/60844827e6ecde3a5cb4a99009fbe7fd.png)

Enable SSL to the project, to do so, left click the project and press F4 on the
keyboard and enable SSL and copy paste the url

![C:\\Users\\HardikMistry\\Desktop\\single sign on\\press F4 to invoke properties of the project.png](media/2fdd9f283d5bd6460c54953c4870009d.png)

![C:\\Users\\HardikMistry\\Desktop\\single sign on\\enable ssl for the project and copy the https url.png](media/a9aaa0c70539a7ba6bd38ea2028004e8.png)

Copy the url and replace the url from web iis express runtime

![C:\\Users\\HardikMistry\\Desktop\\single sign on\\set https url to the web property of the project.png](media/bcadea07081f6a79805776cc8d427945.png)

Navigate to your active directory and now register your application to support
single sign on

![C:\\Users\\HardikMistry\\Desktop\\single sign on\\register your asp web app.png](media/fa4b3cc9e0000aa55da02a7c70129ecb.png)

![C:\\Users\\HardikMistry\\Desktop\\single sign on\\add an app organisation is developing.png](media/809a9e6e0342199f116a8bc5c741abee.png)

![C:\\Users\\HardikMistry\\Desktop\\single sign on\\give a name of the app to register.png](media/0e1e4a30ec573322d4b4bc30434ca2a8.png)

![C:\\Users\\HardikMistry\\Desktop\\single sign on\\give sign on url and app id uri.png](media/999e400329551c670586627c129783ec.png)

Copy the client id

![C:\\Users\\HardikMistry\\Desktop\\single sign on\\click configure and scroll down to obtain client id.png](media/a7eeac0f6636aa50742f93737bfd17f4.png)

![C:\\Users\\HardikMistry\\Desktop\\single sign on\\copy the client id.png](media/be067b4f48f743dac3e25d51440c3b60.png)

Update the web.config of your root project as shown

![C:\\Users\\HardikMistry\\Desktop\\single sign on\\add the following to web.config in root of the project.png](media/3ad1f0268641288eb8d8d488feca04e4.png)

Run the app

![C:\\Users\\HardikMistry\\Desktop\\single sign on\\run the app and click on sign in.png](media/afc4a33309690e56b58ddf653074fc2d.png)

![C:\\Users\\HardikMistry\\Desktop\\single sign on\\login with user credentials.png](media/6085e512e7d0b1040a849024190051e3.png)

![C:\\Users\\HardikMistry\\Desktop\\single sign on\\will ask to update password.png](media/ed9baa27b0a7ee395ea4b493b72e73fb.png)

![C:\\Users\\HardikMistry\\Desktop\\single sign on\\verify the identiy of the user and sign out.png](media/468e37ca6e3f5c11c6843816d8f068ca.png)
