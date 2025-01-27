﻿namespace TvNoms.Core.Utilities;

public static class ReflectionExtensions {
  public static bool IsCompatibleWith(this Type type, Type otherType) {
    return otherType.IsGenericTypeDefinition
      ? type.IsAssignableToGenericTypeDefinition(otherType)
      : otherType.IsAssignableFrom(type);
  }

  private static bool IsAssignableToGenericTypeDefinition(this Type type, Type genericType) {
    foreach (var interfaceType in type.GetInterfaces()) {
      if (!interfaceType.IsGenericType) {
        continue;
      }

      var genericTypeDefinition = interfaceType.GetGenericTypeDefinition();
      if (genericTypeDefinition == genericType) {
        return true;
      }
    }

    if (type.IsGenericType) {
      var genericTypeDefinition = type.GetGenericTypeDefinition();
      if (genericTypeDefinition == genericType) {
        return true;
      }
    }

    var baseType = type.BaseType;
    return baseType is not null && baseType.IsAssignableToGenericTypeDefinition(genericType);
  }
}
