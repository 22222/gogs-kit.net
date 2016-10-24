A .NET client library for the [Gogs](https://github.com/gogits/gogs) API.

This library is not complete.  So far it's mostly focused on the user, organization, and team API endpoints.

The design is inspired by the [Octokit.net library](https://github.com/octokit/octokit.net).


Installation
============
For now, you'll have to build this library on your own.  There will probably be a NuGet package at some point.


Getting Started
===============
Here's a minimal example that uses the API client:

```c#
var gogsClient = new GogsKit.GogsClient("https://try.gogs.io/api/v1/");
var user = await gogsClient.Users.GetAsync("test");
```

Assuming the Gogs demo site hasn't changed since this was written, that example should just work.  It makes an anonymous connection to the API and parses the result into a .NET object (a [UserResult](GogsKit/Models/Results/UserResult.cs) in this case).

Two types of authentication are supported: by username/password and by access token.  Here's an example that uses both:

```c#
var credentials = new Credentials("username", "password");
var gogsClient = new GogsClient("https://try.gogs.io/api/v1/", credentials);

var tokens = await gogsClient.Users.GetTokensAsync("username");
var token = tokens.FirstOrDefault();
if (!tokens.Any())
{
	token = await gogsClient.Users.CreateTokenAsync("username", "token_name");
}

var tokenCredentials = new Credentials(token.Sha1);
var tokenGogsClient = new GogsClient("https://try.gogs.io/api/v1/", tokenCredentials);
var userOrgs = tokenGogsClient.User.GetOrganizationsAsync();
```

See the [Gogs API documentation](https://github.com/gogits/go-gogs-client/wiki) for more details on the available API endpoints.