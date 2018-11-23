using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JumioForms.Abstractions
{
    public interface IDocumentverification
    {
        void Init(string apiToken, string apiSecret, string dataCenter, IDictionary<string, object> options, IDictionary<string, object> customization = null);
        Task<DocumentResult> VerifyAsync();
    }
}
