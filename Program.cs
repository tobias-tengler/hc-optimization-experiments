using BenchmarkDotNet.Running;
using BenchmarkDotNet.Attributes;
using HotChocolate.Execution;
using BenchmarkDotNet.Configs;

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
        var result = await _visitorCheckExecutor.ExecuteAsync("{ productById(id: 1) { id offer { id } } }");

        var json = result.ToJson();
    }
}