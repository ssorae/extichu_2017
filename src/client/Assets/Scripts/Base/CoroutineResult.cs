using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CoroutineResult<T>
{
	private T _value = default(T);

	public void Set(T value)
		=> _value = value;

	public static implicit operator T(CoroutineResult<T> me)
	{
		return me._value;
	}
}
