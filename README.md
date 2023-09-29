```

BenchmarkDotNet v0.13.8, macOS Sonoma 14.0 (23A344) [Darwin 23.0.0]
Apple M2 Max, 1 CPU, 12 logical and 12 physical cores
.NET SDK 8.0.100-rc.1.23463.5
  [Host]     : .NET 7.0.11 (7.0.1123.42427), Arm64 RyuJIT AdvSIMD
  DefaultJob : .NET 7.0.11 (7.0.1123.42427), Arm64 RyuJIT AdvSIMD


```

| Method                   |     Mean |     Error |    StdDev |   Gen0 |   Gen1 | Allocated |
| ------------------------ | -------: | --------: | --------: | -----: | -----: | --------: |
| OptimizedExecutor        | 3.954 μs | 0.0186 μs | 0.0174 μs | 0.0458 |      - |   4.67 KB |
| RuntimeSelectionSetCheck | 3.941 μs | 0.0227 μs | 0.0212 μs | 0.0381 | 0.0076 |   4.58 KB |
| VisitorCheck             | 4.741 μs | 0.0616 μs | 0.0546 μs | 0.0458 | 0.0076 |   4.76 KB |
