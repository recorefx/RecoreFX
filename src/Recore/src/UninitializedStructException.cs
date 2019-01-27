using System;

namespace Recore
{
    public class UninitializedStructException<T> : InvalidOperationException where T : struct
    {
        public UninitializedStructException()
            : base(string.Format(Strings.UnitializedStruct, typeof(T).Name))
        {
        }
    }
}
