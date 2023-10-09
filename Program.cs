using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using HotChocolate.Execution;

// var builder = WebApplication.CreateBuilder(args);

// builder.Services
//     .AddGraphQLServer()
//     .InitializeOnStartup()
//     .AddOperationCompilerOptimizer<ProductOfferPreloadOptimizer>()
//     .AddParameterExpressionBuilder(ctx => ctx.GetScopedState<Offer>(ResolverContextExtensions.PreloadedOfferKey))
//     .AddQueryType<OptimizedExecutorQuery>()
//     .AddTypeExtension<OptimizedExecutorProductExtension>();

// var app = builder.Build();

// app.MapGraphQL();

// app.RunWithGraphQLCommands(args);

BenchmarkRunner.Run<PreloadingBenchmark>();

[MemoryDiagnoser]
public class PreloadingBenchmark
{
    private IRequestExecutor _optimizedExecutor = null!;
    private IRequestExecutor _runtimeSelectionSetCheckExecutor = null!;
    private IRequestExecutor _visitorCheckExecutor = null!;

    [GlobalSetup]
    public async Task GlobalSetup()
    {
        _optimizedExecutor = await new ServiceCollection()
            .AddGraphQL()
            .AddOperationCompilerOptimizer<ProductOfferPreloadOptimizer>()
            .AddParameterExpressionBuilder(ctx => ctx.GetScopedState<Offer>(ResolverContextExtensions.PreloadedOfferKey))
            .AddQueryType<OptimizedExecutorQuery>()
            .AddTypeExtension<OptimizedExecutorProductExtension>()
            .BuildRequestExecutorAsync();

        _runtimeSelectionSetCheckExecutor = await new ServiceCollection()
            .AddGraphQL()
            .AddQueryType<RuntimeSelectionSetCheckQuery>()
            .AddTypeExtension<RuntimeSelectionSetCheckProductExtension>()
            .BuildRequestExecutorAsync();

        _visitorCheckExecutor = await new ServiceCollection()
            .AddGraphQL()
            .AddQueryType<VisitorCheckQuery>()
            .AddTypeExtension<VisitorCheckProductExtension>()
            .BuildRequestExecutorAsync();

        // Warmup
        await _optimizedExecutor.ExecuteAsync("{ __typename }");
        await _runtimeSelectionSetCheckExecutor.ExecuteAsync("{ __typename }");
        await _visitorCheckExecutor.ExecuteAsync("{ __typename }");
    }

    [Benchmark]
    public async Task OptimizedExecutor()
    {
        await _optimizedExecutor.ExecuteAsync("{ productById(id: 1) { id offer { id } } }");
    }

    [Benchmark]
    public async Task RuntimeSelectionSetCheck()
    {
        await _runtimeSelectionSetCheckExecutor.ExecuteAsync("{ productById(id: 1) { id offer { id } } }");
    }

    [Benchmark]
    public async Task VisitorCheck()
    {
        await _visitorCheckExecutor.ExecuteAsync("{ productById(id: 1) { id offer { id } } }");
    }
}