using System;

namespace Blazor.Fluxor
{
	internal static class TypeHelper
	{
		internal static Type[] GetGenericParametersForSpecificGenericType(Type implementingType, Type requiredGenericType)
		{
			while (implementingType != null)
			{
				if (implementingType.IsGenericType && implementingType.GetGenericTypeDefinition() == requiredGenericType)
					return implementingType.GetGenericArguments();
				implementingType = implementingType.BaseType;
			}
			return null;
		}

	}
}
