using System;
using System.ComponentModel;

namespace AllGreen.Lib.Core.Engine.Test
{
    public interface IFluent
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        Boolean Equals(Object obj);

        [EditorBrowsable(EditorBrowsableState.Never)]
        Int32 GetHashCode();

        [EditorBrowsable(EditorBrowsableState.Never)]
        String ToString();

        [EditorBrowsable(EditorBrowsableState.Never)]
        Type GetType();
    }
}