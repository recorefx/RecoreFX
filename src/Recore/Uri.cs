using System;

namespace Recore
{
    /// <summary>
    /// Represents an absolute URI.
    /// </summary>
    public class AbsoluteUri : Uri
    {
        /// <summary>
        /// Initializes a new instance of <see cref="AbsoluteUri"/> with the given URI.
        /// </summary>
        public AbsoluteUri(string uriString) : base(uriString, UriKind.Absolute) { }

        /// <summary>
        /// Initializes a new instance of <see cref="AbsoluteUri"/>
        /// with the given base URI and relative URI.
        /// </summary>
        public AbsoluteUri(Uri baseUri, string relativeUri) : base(baseUri, relativeUri) { }

        /// <summary>
        /// Initializes a new instance of <see cref="AbsoluteUri"/>
        /// with the given base URI and relative URI.
        /// </summary>
        public AbsoluteUri(Uri baseUri, Uri relativeUri) : base(baseUri, relativeUri) { }

        /// <summary>
        /// Initializes a new instance of <see cref="AbsoluteUri"/>
        /// with the given base URI and relative URI.
        /// </summary>
        public AbsoluteUri(Uri baseUri, RelativeUri relativeUri) : base(baseUri, relativeUri) { }

        // Obsolete
        //public Uri(string uriString, bool dontEscape);
        //public Uri(Uri baseUri, string relativeUri, bool dontEscape);

        /// <summary>
        /// Appends a path to an absolute URI.
        /// </summary>
        /// <remarks>
        /// This is a strongly typed alternative to the constructor <see cref="AbsoluteUri(Uri, string)"/>.
        /// Also, the constructor will keep the relative part of the base URI
        /// only if it is terminated with a slash.
        /// This operator ensures that the relative part of the base URI is always preserved.
        /// </remarks>
        public AbsoluteUri Combine(string relativeUri) => Combine(new RelativeUri(relativeUri));

        /// <summary>
        /// Appends a path to an absolute URI.
        /// </summary>
        /// <remarks>
        /// This is a strongly typed alternative to the constructor <see cref="AbsoluteUri(Uri, Uri)"/>.
        /// Also, the constructor will keep the relative part of the base URI
        /// only if it is terminated with a slash.
        /// This operator ensures that the relative part of the base URI is always preserved.
        /// </remarks>
        public AbsoluteUri Combine(RelativeUri relativeUri)
        {
            AbsoluteUri baseUri;
            if (AbsolutePath.EndsWith("/"))
            {
                baseUri = this;
            }
            else
            {
                baseUri = new AbsoluteUri(ToString() + "/");
            }

            return new AbsoluteUri(baseUri, relativeUri);
        }

        /// <summary>
        /// Creates a new <see cref="AbsoluteUri"/>. Does not throw an exception if the <see cref="AbsoluteUri"/> cannot be created.
        /// </summary>
        public static bool TryCreate(string uriString, out AbsoluteUri result)
        {
            result = null;
            if (Uri.TryCreate(uriString, UriKind.Absolute, out Uri value))
            {
                result = value.AsAbsoluteUri();
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Represents a relative URI.
    /// </summary>
    public class RelativeUri : Uri
    {
        /// <summary>
        /// Initializes a new instance of <see cref="RelativeUri"/> with the given URI.
        /// </summary>
        public RelativeUri(string uriString) : base(uriString, UriKind.Relative) { }

        // Obsolete
        //public Uri(string uriString, bool dontEscape);

        /// <summary>
        /// Appends a path to a relative URI.
        /// </summary>
        /// <remarks>
        /// The constructor <see cref="Uri(Uri, string)"/> will throw a <see cref="UriFormatException"/> if called with two relative URIs.
        /// </remarks>
        public RelativeUri Combine(string relativeUri) => Combine(new RelativeUri(relativeUri));

        /// <summary>
        /// Appends a path to a relative URI.
        /// </summary>
        /// <remarks>
        /// The constructor <see cref="Uri(Uri, Uri)"/> will throw a <see cref="UriFormatException"/> if called with two relative URIs.
        /// </remarks>
        public RelativeUri Combine(RelativeUri relativeUri)
        {
            var baseUriString = ToString();
            var relativeUriString = relativeUri.ToString();

            // Ensure that the base ends with "/" and the part to append does not start with "/"
            if (!baseUriString.EndsWith("/"))
            {
                baseUriString += "/";
            }

            relativeUriString.TrimStart('/');

            return new RelativeUri(baseUriString + relativeUriString);
        }

        /// <summary>
        /// Creates a new <see cref="RelativeUri"/>. Does not throw an exception if the <see cref="RelativeUri"/> cannot be created.
        /// </summary>
        public static bool TryCreate(string uriString, out RelativeUri result)
        {
            result = null;
            if (Uri.TryCreate(uriString, UriKind.Relative, out Uri value))
            {
                result = value.AsRelativeUri();
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Extension methods for the <see cref="Uri"/> type.
    /// </summary>
    public static class UriExtensions
    {
        /// <summary>
        /// Returns an instance of <see cref="AbsoluteUri"/> with the same value as <paramref name="uri"/> 
        /// if it is absolute, or null if it is relative.
        /// </summary>
        /// <remarks>
        /// Because an instance of <see cref="Uri"/> may be neither <see cref="AbsoluteUri"/> nor <see cref="RelativeUri"/>,
        /// patterns like <c>(AbsoluteUri)uri</c> or <c>uri as AbsoluteUri</c> cannot be used reliably.
        /// <see cref="AsAbsoluteUri(Uri)"/> works as <c>uri as AbsoluteUri</c> would if <see cref="Uri"/> were an abstract base class.
        /// It complements <see cref="Uri.IsAbsoluteUri"/> in this regard.
        /// </remarks>
        public static AbsoluteUri AsAbsoluteUri(this Uri uri)
        {
            if (uri.IsAbsoluteUri)
            {
                return new AbsoluteUri(uri.AbsoluteUri);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns an instance of <see cref="AbsoluteUri"/> with the same value as <paramref name="uri"/> 
        /// if it is absolute, or null if it is relative.
        /// </summary>
        /// <remarks>
        /// Because an instance of <see cref="Uri"/> may be neither <see cref="AbsoluteUri"/> nor <see cref="RelativeUri"/>,
        /// patterns like <c>(AbsoluteUri)uri</c> or <c>uri as AbsoluteUri</c> cannot be used reliably.
        /// <see cref="AsRelativeUri(Uri)"/> works as <c>uri as RelativeUri</c> would if <see cref="Uri"/> were an abstract base class.
        /// </remarks>
        public static RelativeUri AsRelativeUri(this Uri uri)
        {
            if (uri.IsAbsoluteUri)
            {
                return null;
            }
            else
            {
                return new RelativeUri(uri.ToString());
            }
        }
    }
}