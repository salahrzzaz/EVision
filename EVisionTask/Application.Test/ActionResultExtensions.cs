using System;
using System.Net;
using Application.Infrastructure.API.BaseResponses;
using Microsoft.AspNetCore.Mvc;

namespace Application.Test
{
    public static class ActionResultExtensions
    {
        public static Tuple<HttpStatusCode, EVisionResponse> ToEvisionResponse(this IActionResult actionResult)
        {
            var objectResult = actionResult as ObjectResult;
            if (objectResult?.StatusCode == null)
            {
                return null;
            }

            var statusCode = (HttpStatusCode)objectResult.StatusCode.Value;
            var dasaResponse = objectResult.Value as EVisionResponse;
            return new Tuple<HttpStatusCode, EVisionResponse>(statusCode, dasaResponse);
        }
    }
}
