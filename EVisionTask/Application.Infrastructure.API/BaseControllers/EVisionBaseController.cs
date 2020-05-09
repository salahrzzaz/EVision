using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Infrastructure.API.BaseResponses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Application.Infrastructure.API.BaseControllers
{
    public abstract class EVisionControllerBase : ControllerBase
    {
        #region Constructor

        protected EVisionControllerBase(ILogger logger)
        {
            Logger = logger;
            InitThreadCulture();
        }

        private void InitThreadCulture()
        {
            try
            {
                // get language from user token
                var language = "en";
                var cultureInfo = CultureInfo.GetCultureInfo(language);
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
            }
            catch (UnauthorizedAccessException)
            {
                var cultureInfo = CultureInfo.GetCultureInfo("en");
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
            }
        }

        protected void ChangeThreadLanguage(string collation)
        {
            var cultureInfo = CultureInfo.GetCultureInfo(collation);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }

        #endregion

        #region Properties

        protected readonly ILogger Logger;

        #endregion

        #region Methods

       




        #endregion

        #region Responses

        protected virtual IActionResult Success(string message, object response,
            HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var result = new EVisionResponse
            {
                Message = message,
                Response = response
            };
            return statusCode == HttpStatusCode.OK
                ? Ok(result)
                : StatusCode((int)statusCode, result);
        }

        protected virtual IActionResult CreatedWithSuccess(string message, object response)
        {
            return Success(message, response, HttpStatusCode.Created);
        }

        protected virtual IActionResult Failure(HttpStatusCode statusCode, string message, object response = null)
        {
            return StatusCode((int)statusCode, new EVisionResponse
            {
                Message = message,
                Response = response
            });
        }

        #endregion
    }
}
