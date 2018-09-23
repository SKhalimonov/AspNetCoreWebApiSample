using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApiSample.Services.Exceptions;

namespace WebApiSample.Controllers.Base
{
    public abstract class BaseApiController : Controller
    {
        protected BaseApiController(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        protected ILogger Logger { get; }

        protected async Task<object> ExecuteSafely(Func<object> func)
        {
            try
            {
                if (ModelState.Values.Any(x => x.Errors.Any()))
                {
                    return BadRequest(ModelState);
                }

                return await Task.Run(func);
            }
            catch (ValidationException ex)
            {
                AddModelStateError(ex.FieldName, ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        protected void AddModelStateError(string field, string message)
        {
            ModelState.AddModelError(field, message);
        }

        protected object CombineValidationError()
        {
            return new
            {
                message = "The request is invalid.",
                error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
            };
        }
    }
}
