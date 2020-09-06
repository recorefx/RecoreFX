using System;
using System.Text.Json.Serialization;

using Recore.Text.Json.Serialization.Converters;

namespace Recore
{
    /// <summary>
    /// Represents an absolute URI.
    /// </summary>
    [JsonConverter(typeof(AbsoluteUriConverter))]
    public class AbsoluteUri : Uri
    {
        /// <summary>
        /// Initializes a new instance of <see cref="AbsoluteUri"/> with the given URI.
        /// </summary>
        public AbsoluteUri(string uriString) : base(uriString, UriKind.Absolute) { }

        private AbsoluteUri(Uri baseUri, RelativeUri relativeUri) : base(baseUri, relativeUri) { }

        /// <summary>
        /// Appends a relative URI to an absolute URI.
        /// </summary>
        public AbsoluteUri Combine(string relativeUri) => Combine(new RelativeUri(relativeUri));

        /// <summary>
        /// Appends a relative URI to an absolute URI.
        /// </summary>
        public AbsoluteUri Combine(RelativeUri relativeUri) => new AbsoluteUri(this, relativeUri);

        /// <summary>
        /// Creates a new <see cref="AbsoluteUri"/>. Does not throw an exception if the <see cref="AbsoluteUri"/> cannot be created.
        /// </summary>
        public static bool TryCreate(string uriString, out AbsoluteUri result)
        {
            result = null;
            if (TryCreate(uriString, UriKind.Absolute, out Uri value))
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
    [JsonConverter(typeof(RelativeUriConverter))]
    public class RelativeUri : Uri
    {
        /// <summary>
        /// Initializes a new instance of <see cref="RelativeUri"/> with the given URI.
        /// </summary>
        public RelativeUri(string uriString) : base(uriString, UriKind.Relative) { }

        /// <summary>
        /// Creates a new <see cref="RelativeUri"/>. Does not throw an exception if the <see cref="RelativeUri"/> cannot be created.
        /// </summary>
        public static bool TryCreate(string uriString, out RelativeUri result)
        {
            result = null;
            if (TryCreate(uriString, UriKind.Relative, out Uri value))
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