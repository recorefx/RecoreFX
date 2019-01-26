using System.Collections.Generic;
using System.Net.Http;

namespace Recore.Net.Http
{
    public class UrlEncodedForm
    {
        public UrlEncodedForm(IEnumerable<KeyValuePair<string, string>> values)
        {
            // TODO
        }

        public StringContent StringContent { get; }
    }
}