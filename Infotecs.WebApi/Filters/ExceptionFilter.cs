using System.Net;
using Infotecs.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace Infotecs.WebApi.Filters;

public class ExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is CsvParseException)
        {
            context.Result = new BadRequestObjectResult(context.Exception.Message);
            return;
        }

        Log.Error(context.Exception, "Unhandled Exception:");

        context.Result = new StatusCodeResult((int)HttpStatusCode.InternalServerError);
    }
}