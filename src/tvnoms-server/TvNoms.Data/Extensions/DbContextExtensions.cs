using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TvNoms.Core.Entities;
using TvNoms.Core.Utilities;

namespace TvNoms.Server.Data.Extensions;

public static class DbContextExtensions {
  public static ModelBuilder ApplyEntities(this ModelBuilder modelBuilder, IEnumerable<Assembly> assemblies,
    Func<Type, bool>? predicate = null) {
    var entityTypes = assemblies
      .SelectMany(_ => _.DefinedTypes)
      .Select(_ => _.AsType())
      .Where(type => type is { IsClass: true, IsAbstract: false, IsGenericType: false } &&
                     (type.IsCompatibleWith(typeof(IEntity)) || type is BaseEntity) &&
                     (predicate?.Invoke(type) ?? true));

    foreach (var entityType in entityTypes) {
      modelBuilder.Entity(entityType);
    }

    return modelBuilder;
  }

  public static ModelBuilder ApplyConfigurations(this ModelBuilder modelBuilder, IEnumerable<Assembly> assemblies,
    Func<Type, bool>? predicate = null) {
    var entityTypeConfigurationTypes = assemblies
      .SelectMany(_ => _.DefinedTypes)
      .Select(_ => _.AsType())
      .Where(type =>
        type is { IsClass: true, IsAbstract: false, IsGenericType: false } &&
        type.IsCompatibleWith(typeof(IEntityTypeConfiguration<>)) &&
        (predicate?.Invoke(type) ?? true));

    var applyEntityConfigurationMethod = typeof(ModelBuilder)
      .GetMethods()
      .Single(
        e =>
          e is { Name: nameof(ModelBuilder.ApplyConfiguration), ContainsGenericParameters: true } &&
          e
            .GetParameters()
            .SingleOrDefault()?.ParameterType
            .GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>));

    foreach (var entityTypeConfigurationType in entityTypeConfigurationTypes) {
      // Only accept types that contain a parameterless constructor,
      // are not abstract and satisfy a predicate if it was used.
      if (entityTypeConfigurationType.GetConstructor(Type.EmptyTypes) == null
          || (!predicate?.Invoke(entityTypeConfigurationType) ?? false)) {
        continue;
      }

      foreach (var @interface in entityTypeConfigurationType.GetInterfaces()) {
        if (!@interface.IsGenericType) {
          continue;
        }

        if (@interface.GetGenericTypeDefinition() != typeof(IEntityTypeConfiguration<>)) {
          continue;
        }

        var target = applyEntityConfigurationMethod
          .MakeGenericMethod(@interface.GenericTypeArguments[0]);
        target.Invoke(modelBuilder, [
          Activator.CreateInstance(entityTypeConfigurationType)
        ]);
      }
    }

    return modelBuilder;
  }
}
