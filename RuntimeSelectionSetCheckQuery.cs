using System.Reflection;
using HotChocolate.Resolvers;
using HotChocolate.Types.Descriptors;

public class RuntimeSelectionSetCheckQuery
{
  public Product? GetProductById(int id, IResolverContext context)
  {
    if (ResolverContextExtensions.ShouldFetchOffer_Runtime(context))
    {
      var preloadedOffer = new Offer(3, "Preloaded Offer");

      context.SetPreloadedOffer(preloadedOffer);
    }

    return new Product(id, "iPhone");
  }
}

[ExtendObjectType(typeof(Product))]
public class RuntimeSelectionSetCheckProductExtension
{
  [PreloadedOffer]
  public Offer GetOffer()
  {
    return new Offer(2, "Apple");
  }
}

public class PreloadedOfferAttribute : ObjectFieldDescriptorAttribute
{
  protected override void OnConfigure(IDescriptorContext context, IObjectFieldDescriptor descriptor, MemberInfo member)
  {
    descriptor.Use(next => context =>
    {
      if (context.ScopedContextData.TryGetValue(ResolverContextExtensions.PreloadedOfferKey, out var offer))
      {
        context.Result = offer;
        return ValueTask.CompletedTask;
      }

      return next(context);
    });
  }
}
