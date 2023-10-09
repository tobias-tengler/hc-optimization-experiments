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

[MeanColumn, MedianColumn, MemoryDiagnoser]
[GroupBenchmarksBy(BenchmarkDotNet.Configs.BenchmarkLogicalGroupRule.ByCategory)]
public class PreloadingBenchmark
{
    private IRequestExecutor _optimizedExecutor = null!;
    private IRequestExecutor _runtimeSelectionSetCheckExecutor = null!;
    private IRequestExecutor _visitorCheckExecutor = null!;

    private const string _smallQuery = """
       {
            productById(id: 1) { 
                id
                a: __typename
                b: __typename
                c: __typename
                d: name
                e: name
                f: name
                offer { 
                    id
                } 
            }
       } 
    """;

    private const string _largeQuery = """
    {
        # Boilerplate to work the Visitor
        a: __schema @skip(if: true) {
            description
            directives {
            args {
                defaultValue
                deprecationReason
                description
                isDeprecated
                name
                type {
                description
                enumValues {
                    deprecationReason
                    isDeprecated
                    description
                    name
                }
                fields {
                    args {
                    defaultValue
                    deprecationReason
                    }
                }
                }
            }
            }
            types {
            kind
            name
            description
            specifiedByURL
            fields {
                name
                description
                args {
                name
                description
                defaultValue
                isDeprecated
                }
                type {
                name
                specifiedByURL
                fields {
                    name
                    description
                    type {
                    kind
                    name
                    }
                }
                }
                isDeprecated
                deprecationReason
            }
            }
        }
        b: __schema @skip(if: true) {
            description
            directives {
            args {
                defaultValue
                deprecationReason
                description
                isDeprecated
                name
                type {
                description
                enumValues {
                    deprecationReason
                    isDeprecated
                    description
                    name
                }
                fields {
                    args {
                    defaultValue
                    deprecationReason
                    }
                }
                }
            }
            }
            types {
            kind
            name
            description
            specifiedByURL
            fields {
                name
                description
                args {
                name
                description
                defaultValue
                isDeprecated
                }
                type {
                name
                specifiedByURL
                fields {
                    name
                    description
                    type {
                    kind
                    name
                    }
                }
                }
                isDeprecated
                deprecationReason
            }
            }
        }
        c: __schema @skip(if: true) {
            description
            directives {
            args {
                defaultValue
                deprecationReason
                description
                isDeprecated
                name
                type {
                description
                enumValues {
                    deprecationReason
                    isDeprecated
                    description
                    name
                }
                fields {
                    args {
                    defaultValue
                    deprecationReason
                    }
                }
                }
            }
            }
            types {
            kind
            name
            description
            specifiedByURL
            fields {
                name
                description
                args {
                name
                description
                defaultValue
                isDeprecated
                }
                type {
                name
                specifiedByURL
                fields {
                    name
                    description
                    type {
                    kind
                    name
                    }
                }
                }
                isDeprecated
                deprecationReason
            }
            }
        }

        # Actual fields we're interested in
        p1: productById(id: 1) { 
            id
            a: __typename
            b: __typename
            c: __typename
            d: name
            e: name
            f: name
            offer { 
                id
            } 
        }
        p2: productById(id: 2) { 
            id
            a: __typename
            b: __typename
            c: __typename
            d: name
            e: name
            f: name
            offer { 
                id
            } 
        }
        p3: productById(id: 3) { 
            id 
            a: __typename
            b: __typename
            c: __typename
            d: name
            e: name
            f: name
            offer { 
                id
            } 
        }
        p4: productById(id: 4) { 
            id
            a: __typename
            b: __typename
            c: __typename
            d: name
            e: name
            f: name
            offer { 
                id
            } 
        }
        p5: productById(id: 5) { 
            id
            a: __typename
            b: __typename
            c: __typename
            d: name
            e: name
            f: name
            offer { 
                id
            } 
        }
        p6: productById(id: 6) { 
            id 
            a: __typename
            b: __typename
            c: __typename
            d: name
            e: name
            f: name
            offer { 
                id
            }
        }
    }
    """;

    [GlobalSetup]
    public async Task GlobalSetup()
    {
        _optimizedExecutor = await new ServiceCollection()
            .AddGraphQL()
            .AddOperationCompilerOptimizer<ProductOfferPreloadOptimizer>()
            // .AddParameterExpressionBuilder(ctx => ctx.GetScopedState<Offer>(ResolverContextExtensions.PreloadedOfferKey))
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

    [Benchmark(Baseline = true), BenchmarkCategory("Small Query")]
    public async Task RuntimeSelectionSetCheck_SmallQuery()
    {
        await _runtimeSelectionSetCheckExecutor.ExecuteAsync(_smallQuery);
    }

    [Benchmark, BenchmarkCategory("Small Query")]
    public async Task OperationCompilerOptimizer_SmallQuery()
    {
        await _optimizedExecutor.ExecuteAsync(_smallQuery);
    }

    [Benchmark, BenchmarkCategory("Small Query")]
    public async Task RuntimeVisitorCheck_SmallQuery()
    {
        await _visitorCheckExecutor.ExecuteAsync(_smallQuery);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Large Query")]
    public async Task RuntimeSelectionSetCheck_LargeQuery()
    {
        await _runtimeSelectionSetCheckExecutor.ExecuteAsync(_largeQuery);
    }

    [Benchmark, BenchmarkCategory("Large Query")]
    public async Task OperationCompilerOptimizer_LargeQuery()
    {
        await _optimizedExecutor.ExecuteAsync(_largeQuery);
    }

    [Benchmark, BenchmarkCategory("Large Query")]
    public async Task RuntimeVisitorCheck_LargeQuery()
    {
        await _visitorCheckExecutor.ExecuteAsync(_largeQuery);
    }
}