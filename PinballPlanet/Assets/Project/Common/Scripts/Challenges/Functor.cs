using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class Functor
{
    public static Func<T, T, bool> Greater<T>()
       where T : IComparable<T>
    {
        return delegate(T lhs, T rhs) { return lhs.CompareTo(rhs) > 0; };
    }

    public static Func<T, T, bool> Less<T>()
       where T : IComparable<T>
    {
        return delegate(T lhs, T rhs) { return lhs.CompareTo(rhs) < 0; };
    }

    public static Func<T, T, bool> Equal<T>()
       where T : IComparable<T>
    {
        return delegate(T lhs, T rhs) { return lhs.CompareTo(rhs) == 0; };
    }
}