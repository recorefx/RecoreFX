namespace Recore
{
    /// <summary>
    /// Compute hash codes for types.
    /// </summary>
    public static class Hasher
    {
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