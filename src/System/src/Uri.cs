namespace System
{
    public class AbsoluteUri : Uri
    {
        public AbsoluteUri(string uriString) : base(uriString, UriKind.Absolute) { }

        public AbsoluteUri(Uri baseUri, string relativeUri) : base(baseUri, relativeUri) { }

        public AbsoluteUri(Uri baseUri, Uri relativeUri) : base(baseUri, relativeUri) { }

        // Obsolete
        //public Uri(string uriString, bool dontEscape);
        //public Uri(Uri baseUri, string relativeUri, bool dontEscape);
    }

    public class RelativeUri : Uri
    {
        public RelativeUri(string uriString) : base(uriString, UriKind.Relative) { }

        // Obsolete
        //public Uri(string uriString, bool dontEscape);
    }
}