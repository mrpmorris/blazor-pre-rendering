# blazor-pre-rendering

Based on an [article](https://andrewlock.net/enabling-prerendering-for-blazor-webassembly-apps/) by Andrew Lock.

You can see the diff between a standard ASP.NET hosted Blazor WASM app
and a WasmPreRendered app [here](https://github.com/mrpmorris/blazor-pre-rendering/commits/master).

1. Create a new ASP.NET hosted Blazor WASM app
2. Edit Server\Program.cs and change `app.MapFallbackToFile("index.html");` to

```c#
app.MapFallbackToPage("/_Host");
```

3. Create a new Razor Page `_Host.cshtml` in `Server\Pages`
4. Copy the contents of `Client\wwwroot\index.html`
5. Paste the contents beneath `@page` in `Server\Pages\_Host.cshtml`
6. Under the `@page` line on line 1, add

```
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
```

7. Replace `<div id="app">Loading...</div>`

```html
<component
    type="typeof(NameOfYourProject.Client.App)"
    render-mode="WebAssemblyPrerendered" />
```

8. Edit `Client\Program.cs` and delete the line

`builder.RootComponents.Add<App>("#app");`

9. In `Server\Program.cs` add the following code beneath `builder.Services.AddRazorPages();`

```c#
services.AddSingleton<HttpClient>(sp =>
    {
        // Get the address that the app is currently running at
        var server = sp.GetRequiredService<IServer>();
        var addressFeature = server.Features.Get<IServerAddressesFeature>();
        string baseAddress = addressFeature.Addresses.First();
        return new HttpClient { BaseAddress = new Uri(baseAddress) };
    });
```

10. To ensure both client and server have shared services, create a file `Client\ServiceRegistration.cs`
11. Add the following class

```c#
public static class ServiceRegistration
{
    public static void Register(IServiceCollection services)
    {
        // Register your client services here
    }
}
```

12. Call that method from the server in `Program.cs`
13. Call that method from the client in `Client.cs`

