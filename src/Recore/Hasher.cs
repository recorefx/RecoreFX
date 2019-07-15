namespace Recore
{
    /// <summary>
    /// Computes hash codes for types.
    /// </summary>
    public static class Hasher
    {
        /// <summary>
        /// Combines the hash codes of one or more objects into a single hash code.
        /// </summary>
        /// <remarks>
        /// This is useful for computing the hash code for a type from the hash codes of all its members.
        /// It is superseded in .NET Core 3.0 by the <c>System.HashCode</c> type.
        /// </remarks>
        public static int GetHashCode(int seed1=17, int seed2=23, int nullHash = 0, params object[] fields)
        {
            unchecked
            {
                int hash = seed1;
                foreach (var field in fields)
                {
                    hash *= seed2;
                    hash += field?.GetHashCode() ?? nullHash;
                }

                return hash;
            }
        }
    }
}