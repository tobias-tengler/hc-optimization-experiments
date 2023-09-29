using HotChocolate.Resolvers;
using Microsoft.Diagnostics.Tracing.Parsers.Clr;

public static class ResolverContextExtensions
{
  public const string ShouldFetchOfferKey = "shouldFetchOffer";
  public const string PreloadedOfferKey = "preloadedOffer";

  public static bool ShouldFetchOffer_Optimizer(this IResolverContext context)
    => context.GetLocalStateOrDefault<bool>(ShouldFetchOfferKey);

  public static bool ShouldFetchOffer_Runtime(this IResolverContext context)
  {
    if (context.Selection.Type.NamedType() is not ObjectType objectType)
    {
      return false;
    }

    var selections = context.GetSelections(objectType);

    return selections.Any(s => s.Field.Name == "offer");
  }

  public static void SetPreloadedOffer(this IResolverContext context, Offer offer)
    => context.SetScopedState(PreloadedOfferKey, offer);
}