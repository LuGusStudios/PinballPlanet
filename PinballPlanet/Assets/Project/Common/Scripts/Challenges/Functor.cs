using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class Functor
{
	// GREATER

    public static Func<T, T, bool> Greater<T>()
       where T : IComparable<T>
    {
        return delegate(T lhs, T rhs) { return lhs.CompareTo(rhs) > 0; };
    }

	public static Func<int, int, bool> GreaterInt()
	{
		return delegate(int lhs, int rhs) { return lhs.CompareTo(rhs) > 0; };
	}

	public static Func<float, float, bool> GreaterFloat()
	{
		return delegate(float lhs, float rhs) { return lhs.CompareTo(rhs) > 0; };
	}

	public static Func<double, double, bool> GreaterDouble()
	{
		return delegate(double lhs, double rhs) { return lhs.CompareTo(rhs) > 0; };
	}

	public static Func<string, string, bool> GreaterString()
	{
		return delegate(string lhs, string rhs) { return lhs.CompareTo(rhs) > 0; };
	}

	// LESS

    public static Func<T, T, bool> Less<T>()
       where T : IComparable<T>
    {
        return delegate(T lhs, T rhs) { return lhs.CompareTo(rhs) < 0; };
    }

	public static Func<int, int, bool> LessInt()
	{
		return delegate(int lhs, int rhs) { return lhs.CompareTo(rhs) < 0; };
	}
	
	public static Func<float, float, bool> LessFloat()
	{
		return delegate(float lhs, float rhs) { return lhs.CompareTo(rhs) < 0; };
	}
	
	public static Func<double, double, bool> LessDouble()
	{
		return delegate(double lhs, double rhs) { return lhs.CompareTo(rhs) < 0; };
	}
	
	public static Func<string, string, bool> LessString()
	{
		return delegate(string lhs, string rhs) { return lhs.CompareTo(rhs) < 0; };
	}

	// EQUAL

    public static Func<T, T, bool> Equal<T>()
       where T : IComparable<T>
    {
        return delegate(T lhs, T rhs) { return lhs.CompareTo(rhs) == 0; };
    }

	public static Func<int, int, bool> EqualInt()
	{
		return delegate(int lhs, int rhs) { return lhs.CompareTo(rhs) == 0; };
	}
	
	public static Func<float, float, bool> EqualFloat()
	{
		return delegate(float lhs, float rhs) { return lhs.CompareTo(rhs) == 0; };
	}
	
	public static Func<double, double, bool> EqualDouble()
	{
		return delegate(double lhs, double rhs) { return lhs.CompareTo(rhs) == 0; };
	}
	
	public static Func<string, string, bool> EqualString()
	{
		return delegate(string lhs, string rhs) { return lhs.CompareTo(rhs) == 0; };
	}

	// UNEQUAL

    public static Func<T, T, bool> Unequal<T>()
       where T : IComparable<T>
    {
        return delegate(T lhs, T rhs) { return lhs.CompareTo(rhs) != 0; };
    }

	public static Func<int, int, bool> UnequalInt()
	{
		return delegate(int lhs, int rhs) { return lhs.CompareTo(rhs) != 0; };
	}
	
	public static Func<float, float, bool> UnequalFloat()
	{
		return delegate(float lhs, float rhs) { return lhs.CompareTo(rhs) != 0; };
	}
	
	public static Func<double, double, bool> UnequalDouble()
	{
		return delegate(double lhs, double rhs) { return lhs.CompareTo(rhs) != 0; };
	}
	
	public static Func<string, string, bool> UnequalString()
	{
		return delegate(string lhs, string rhs) { return lhs.CompareTo(rhs) != 0; };
	}
	
}