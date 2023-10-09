using HotChocolate.Execution.Processing;
using HotChocolate.Resolvers;

public static class ResolverContextExtensions
{
  public const string ShouldFetchOfferKey = "shouldFetchOffer";
  public const string PreloadedOfferKey = "preloadedOffer";

  public static bool ShouldFetchOffer_Optimizer(this IResolverContext context)
    => context.Selection is Selection typedSelection &&
      typedSelection.CustomOptions.HasFlag(Selection.CustomOptionsFlags.Option1);


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