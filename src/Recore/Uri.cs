using System;

namespace Recore
{
    /// <summary>
    /// Represents an absolute URI.
    /// </summary>
    public class AbsoluteUri : Uri
    {
        /// <summary>
        /// Initializes a new instance of <c cref="AbsoluteUri">AbsoluteUri</c> with the given URI.
        /// </summary>
        public AbsoluteUri(string uriString) : base(uriString, UriKind.Absolute) { }

        /// <summary>
        /// Initializes a new instance of <c cref="AbsoluteUri">AbsoluteUri</c>
        /// with the given base URI and relative URI.
        /// </summary>
        public AbsoluteUri(Uri baseUri, string relativeUri) : base(baseUri, relativeUri) { }

        /// <summary>
        /// Initializes a new instance of <c cref="AbsoluteUri">AbsoluteUri</c>
        /// with the given base URI and relative URI.
        /// </summary>
        public AbsoluteUri(Uri baseUri, Uri relativeUri) : base(baseUri, relativeUri) { }

        /// <summary>
        /// Initializes a new instance of <c cref="AbsoluteUri">AbsoluteUri</c>
        /// with the given base URI and relative URI.
        /// </summary>
        public AbsoluteUri(Uri baseUri, RelativeUri relativeUri) : base(baseUri, relativeUri) { }

        // Obsolete
        //public Uri(string uriString, bool dontEscape);
        //public Uri(Uri baseUri, string relativeUri, bool dontEscape);
    }

    /// <summary>
    /// Represents a relative URI.
    /// </summary>
    public class RelativeUri : Uri
    {
        /// <summary>
        /// Initializes a new instance of <c cref="RelativeUri">RelativeUri</c> with the given URI.
        /// </summary>
        public RelativeUri(string uriString) : base(uriString, UriKind.Relative) { }

        // Obsolete
        //public Uri(string uriString, bool dontEscape);
    }
}