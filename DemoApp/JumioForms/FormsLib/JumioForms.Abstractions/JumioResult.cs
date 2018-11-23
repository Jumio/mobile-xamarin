using System;
using System.Collections.Generic;
namespace JumioForms.Abstractions
{
    public class JumioResult
    {
        public JumioResult(string message, IDictionary<string, object> result)
        {
            Message = message;
            Result = result;
        }

        public string Message { get; }

        public IDictionary<string, object> Result { get; }
    }
}
