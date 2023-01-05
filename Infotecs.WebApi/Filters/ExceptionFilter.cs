using System.Net;
using Infotecs.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Infotecs.WebApi.Filters;

public class ExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        context.Result = context.Exception switch
        {
            CsvParseException => new BadRequestObjectResult(context.Exception.Message),
            _ => new StatusCodeResult((int)HttpStatusCode.InternalServerError)
        };
    }
}