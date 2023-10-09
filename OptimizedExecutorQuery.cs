using HotChocolate.Execution.Processing;
using HotChocolate.Language;
using HotChocolate.Resolvers;
using static HotChocolate.Execution.Processing.Selection;

public class OptimizedExecutorQuery
{
  public Product? GetProductById(int id, IResolverContext context)
  {
    if (ResolverContextExtensions.ShouldFetchOffer_Optimizer(context))
    {
      var preloadedOffer = new Offer(3, "Preloaded Offer");

      context.SetPreloadedOffer(preloadedOffer);
    }

    return new Product(id, "iPhone");
  }
}

[ExtendObjectType(typeof(Product))]
public class OptimizedExecutorProductExtension
{
  [PreloadedOffer]
  public Offer GetOffer(Offer? preloadedOffer)
  {
    if (preloadedOffer is not null)
    {
      return preloadedOffer;
    }

    return new Offer(2, "Apple");
  }
}

public class ProductOfferPreloadOptimizer : IOperationOptimizer
{
  public void OptimizeOperation(OperationOptimizerContext context)
  {
    var operation = context.CreateOperation();

    if (operation.Type != OperationType.Query)
    {
      return;
    }

    var rootSelections = operation.RootSelectionSet.Selections;
    foreach (var rootSelection in rootSelections)
    {
      ProcessSelection(operation, rootSelection, context);
    }
  }

  private void ProcessSelection(IOperation operation, ISelection selection, OperationOptimizerContext context)
  {
    var returnType = selection.Type.NamedType();

    if (returnType.Name != "Product")
    {
      return;
    }

    OptimizeProductResolution(operation, selection);

    if (selection.SelectionSet is null)
    {
      return;
    }

    var possibleTypes = operation.GetPossibleTypes(selection);
    foreach (var type in possibleTypes)
    {
      var selections = operation.GetSelectionSet(selection, type).Selections;
      foreach (var childSelection in selections)
      {
        ProcessSelection(operation, childSelection, context);
      }
    }
  }

  private void OptimizeProductResolution(IOperation operation, ISelection selection)
  {
    if (selection.SelectionSet is null || selection.Type is not IObjectType productType)
    {
      return;
    }

    var selectionSetOnType = operation.GetSelectionSet(selection, productType);

    var offerSelection = selectionSetOnType.Selections.FirstOrDefault(s => s.Field.Name == "offer");

    if (offerSelection is null)
    {
      return;
    }

    if (selection is Selection typedSelection)
    {
      typedSelection.SetOption(CustomOptionsFlags.Option1);
    }
  }
}
