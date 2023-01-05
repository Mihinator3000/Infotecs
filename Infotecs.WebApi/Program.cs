using Infotecs.Abstractions.DataAccess;
using Infotecs.Core.Extensions;
using Infotecs.DataAccess;
using Infotecs.Dto.Extensions;
using Infotecs.WebApi.Filters;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;

services.AddDbContext<IInfotecsDatabaseContext, InfotecsDatabaseContext>(o => o
    .UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")))
    .AddMappingConfiguration()
    .AddProviders()
    .AddServices();

services.AddControllers(o =>
    o.Filters.Add<ExceptionFilter>()
);

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<InfotecsDatabaseContext>();
await context.Database.EnsureCreatedAsync();

app.Run();