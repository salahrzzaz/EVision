using System;
using System.Collections.Generic;

namespace Application.Infrastructure.API.BaseResponses
{
    public class EVisionResponse
    {
        public string Message { get; set; }
        public object Response { get; set; }
        public IEnumerable<string> ValidationErrors { get; set; }
    }
}
