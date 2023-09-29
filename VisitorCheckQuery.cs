using System.Reflection;
using HotChocolate.Data.Projections;
using HotChocolate.Execution.Processing;
using HotChocolate.Resolvers;

public class VisitorCheckQuery
{
  public Product? GetProductById(int id, IResolverContext context)
  {
    var visitorResult = ProductVisitor.Visit(context);
    
    if (visitorResult.NeedsOffer)
    {
      var preloadedOffer = new Offer(3, "Preloaded Offer");

      context.SetPreloadedOffer(preloadedOffer);
    }

    return new Product(id, "iPhone");
  }
  
}

[ExtendObjectType(typeof(Product))]
public class VisitorCheckProductExtension
{
  [PreloadedOffer]
  public Offer GetOffer()
  {
    return new Offer(2, "Apple");
  }
}

public class ProductVisitor : SelectionVisitor<ProductVisitorContext>
{
  private static readonly ProductVisitor Instance = new();
  public static IProductVisitorResult Visit(IResolverContext resolverContext)
  {
    var context = new ProductVisitorContext(resolverContext);
    Instance.Visit(context, context.ResolverContext.Selection);
    return context;
  }

  private void Visit(ProductVisitorContext context, ISelection selection)
  {
    context.Selection.Push(selection);
    Visit(selection.Field, context);
  }

  protected override ISelectionVisitorAction Enter(ISelection selection, ProductVisitorContext context)
  {
    if (selection.Field.RuntimeType == typeof(Product))
    {
      return SkipAndLeave;
    }
    
    if (selection.Field.ResolverMember is MethodInfo methodInfo)
    {
      var parameters = methodInfo.GetParameters();
      context.NeedsOffer = parameters.Any(p => p.ParameterType == typeof(Offer));
    }
    if (context is { NeedsOffer: true })
    {
      return Break;
    }
    return base.Enter(selection, context);
  }
}

public interface IProductVisitorResult
{
  bool NeedsOffer { get; }
}

public class ProductVisitorContext : ISelectionVisitorContext, IProductVisitorResult
{
  public ProductVisitorContext(IResolverContext context)
  {
    Selection = new Stack<ISelection>();
    ResolvedType = new Stack<INamedType?>();
    ResolverContext = context;
  }
  public Stack<ISelection> Selection { get; }
  public Stack<INamedType?> ResolvedType { get; }
  public IResolverContext ResolverContext { get; }
  public bool NeedsOffer { get; set; }
}