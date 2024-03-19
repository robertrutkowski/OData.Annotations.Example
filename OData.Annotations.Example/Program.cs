using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Formatter.Serialization;
using OData.Annotations.Example;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddOData(opt => opt.AddRouteComponents("odata", EdmModelBuilder.CreateEdmModel(), 
            builder => builder.AddSingleton<IODataSerializerProvider, CustomODataSerializerProvider>()).EnableQueryFeatures());

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
