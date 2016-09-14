# HttpClientGoodies.NET

[![Build status](https://ci.appveyor.com/api/projects/status/v8bx4kl22po40vso?svg=true)](https://ci.appveyor.com/project/jeffijoe/httpclientgoodies-net)
[![NuGet](https://img.shields.io/nuget/v/HttpClientGoodies.svg?maxAge=2592000)](http://nuget.org/packages/HttpClientGoodies)

A set of useful utilities for the .NET HttpClient.

# Installation

```
Install-Package HttpClientGoodies
```

# What's it for?

These small but useful utilities will make your HttpClient life worth living.

## Fastest way to profit

Here's an example of sending JSON content and reading JSON content in the least amount of code possible, while also providing Basic Authentication.

```csharp
var dataToSend = new Todo {
    Text = 'Install this package'
};

var createdTodo = RequestBuilder.Post('http://api.todos.com/todos/{id}')
    .BasicAuthentication("username", "password")
    .AddUrlSegment("id", 123)
    .JsonContent(dataToSend)
    .SendAsync()
    .AsJson<Todo>();

Console.WriteLine(createdTodo.Text);
```

## That's cool, what else can it do?

Quite a lot! **All methods that build the request are chainable!** Let's asume the following for each snippet:

```csharp
var builder = new RequestBuilder();
```

* **Headers**:

  ```csharp
  builder.AddHeader("X-MyHeader", "cool value");
  ```

* **Query parameters**

  ```csharp
  builder.AddQuery("searchText", "how do i get my girlfriend to tape her fingers together like a dinosaur");
  ```

* **Base URI + Resource URI separately**

  ```csharp
  builder.BaseUri("http://todos.com")
         .ResourceUri("api/todos");
  ```

* **URL segments**

  ```csharp
  builder.BaseUri("http://todos.com")
         .ResourceUri("api/todos/{id}")
         .AddUrlSegment("id", 123);
  ```

* **Authentication**

  ```csharp
  builder.Authentication("Basic", "<some base64 here>");

  // Basic auth shortcut (will base64 for you!)
  builder.BasicAuthentication("username", "password");
  ```

* **Setting content**

  ```csharp
  builder.Content(new HttpStringContent("hehehe"));

  // Want to send JSON?
  builder.Content(new JsonContent(new Todo()));

  // Can do you one better!
  builder.JsonContent(new Todo());
  ```


* **HTTP method**

  ```csharp
  builder.Method(HttpMethod.Get);
  ```

* **Getting the `HttpRequestMessage` to send**

  ```csharp
  var message = builder.ToHttpRequestMessage();
  await someClient.SendAsync(message);
  ```

* **Sending the request without manually calling `ToHttpRequestMessage`**

  ```csharp
  var response = await builder.SendAsync();
  // if you have a client instance you can pass it in.
  var response = await builder.SendAsync(client);
  ```

* **Reading content as JSON**

  ```csharp
  var response = await builder.SendAsync();
  var todo = await response.Content.ReadAsJsonAsync<Todo>();

  // Even better...
  var todo = await builder.SendAsync().AsJson<Todo>();

  // Custom settings? No problemo!
  var settings = new JsonSerializerSettings();
  var todo = await response.Content.ReadAsJsonAsync<Todo>(settings);
  var todo = await builder.SendAsync().AsJson<Todo>(settings);
  ```

* **Shortcuts for methods**

  The following **static methods** are available: `Get`, `Post`, `Put`, `Patch`, `Delete`, `Head`, `Options`, `Trace`.

  ```csharp
  var todo = await RequestBuilder
    .Get('http://todos.com/api/todos/123')
    .SendAsync()
    .AsJson<Todo>();
  ```

# Author

Jeff Hansen - [@Jeffijoe](https://twitter.com/Jeffijoe)
