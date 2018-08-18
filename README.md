# MessagePack Formatters for ASP.NET Core MVC

Allows to use [MessagePack format](http://msgpack.org/) with ASP.NET Core MVC. 

This library leverages [MessagePack-CSharp](https://github.com/neuecc/MessagePack-CSharp) library by Yoshifumi Kawai (a.k.a. neuecc).

# Installation


```
Install-Packagge Alphacloud.MessagePack.AspNetCore.Formatters
```

# Usage

## Default configuration

Default configuration uses `application/x-msgpack` media type and `ContractlessStandardResolver`.

### Full MVC
Add `AddMsgPackFormatters` to your `Startup.cs / ConfigureServices` 
```
public void ConfigureServices(IServiceCollection services)
{
  services.AddMvc()
    .AddMessagePackFormatters()
    ;
}
```

### Core MVC

When using minimal MVC configuration (e.g. in WebAPI service) only base services are added by default. 
You are responsible for configuring each of the service you are going to use.

**Note:** order of formatters is important - in the example below Json will be default serializer, 
unless MVC is configured to reject requests with unsupported media type.

```
public void ConfigureServices(IServiceCollection services)
{
public void ConfigureServices(IServiceCollection services)
{
  services.AddMvcCore(options =>
    {
        // ...
    })
    .AddDataAnnotations()
    .AddJsonOptions(options =>
    {
        options.SerializerSettings.Culture = CultureInfo.InvariantCulture;
        options.SerializerSettings.Formatting = Formatting.None;
    })
    .AddFormatterMappings()
    .AddJsonFormatters()
    .AddXmlSerializerFormatters()
    .AddMsgPackFormatters()
    ;
}        
```

## Custom configuration

Default configuration can be changed by providing callback to `AddMessagePackFormatters` method.

Available settings:
* `MediaTypes` - allows to specify additional media handled by MessagePack formatters. Default is `application/x-msgpack`.
* `FormatterResolver` - allows to customize serialization, see https://github.com/neuecc/MessagePack-CSharp/blob/master/README.md#object-serialization for details.

```
services.AddMvc()
  .AddMessagePackFormatters(opt =>
  {
      opt.MediaTypes.Clear();
      opt.MediaTypes.Add("application/x-messagepack");
      opt.FormatterResolver = MessagePack.Resolvers.ContractlessStandardResolverAllowPrivate.Instance;
  })

```

# Samples

Sample application is located at `src/samples/NetCoreWebApi`.
Sample requests can be found at `src/samples/MessagePack.postman_collection.json`. To post MsgPack load `SingleModel.msgpack` located under `src/samples`


# References
* MessagePack format https://msgpack.org/index.html
* MessagePack-CSharp https://github.com/neuecc/MessagePack-CSharp
* AddMVC vs AddMvcCore
  * https://www.stevejgordon.co.uk/aspnetcore-anatomy-deep-dive-index 
  * https://offering.solutions/blog/articles/2017/02/07/difference-between-addmvc-addmvcore/ 
* Content negotiation in ASP.NET Core MVC https://code-maze.com/content-negotiation-dotnet-core/
