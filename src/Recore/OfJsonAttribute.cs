using System;
using System.Text.Json.Serialization;

using Recore.Text.Json.Serialization.Converters;

namespace Recore
{
    /// <summary>
    /// Place on an subtype of <seealso cref="Of{T}"/> to serialize it to JSON as its underlying type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class OfJsonAttribute : JsonConverterAttribute
    {
        /// <summary>
        /// Initializes a new instance of <see cref="OfJsonAttribute"/>.
        /// </summary>
        public OfJsonAttribute(Type thisType, Type innerType)
            : base(typeof(OfConverter<,>).MakeGenericType(new[] { thisType, innerType }))
        {
        }
    }
}
