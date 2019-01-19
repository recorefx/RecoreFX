using System.Collections.Generic;

namespace System.Net.Http
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