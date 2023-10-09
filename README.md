| Method                                |      Mean |     Error |    StdDev |    Median | Ratio | RatioSD |   Gen0 |   Gen1 | Allocated | Alloc Ratio |
| ------------------------------------- | --------: | --------: | --------: | --------: | ----: | ------: | -----: | -----: | --------: | ----------: |
| RuntimeSelectionSetCheck_LargeQuery   | 20.404 μs | 0.0861 μs | 0.4435 μs | 20.220 μs |  1.00 |    0.00 | 0.1221 |      - |  14.86 KB |        1.00 |
| OperationCompilerOptimizer_LargeQuery | 19.764 μs | 0.0267 μs | 0.1386 μs | 19.760 μs |  0.97 |    0.03 | 0.1221 | 0.0305 |  15.28 KB |        1.03 |
| RuntimeVisitorCheck_LargeQuery        | 25.094 μs | 0.0371 μs | 0.1932 μs | 25.073 μs |  1.23 |    0.03 | 0.1526 |      - |  15.94 KB |        1.07 |
|                                       |           |           |           |           |       |         |        |        |           |             |
| RuntimeSelectionSetCheck_SmallQuery   |  5.047 μs | 0.0055 μs | 0.0281 μs |  5.051 μs |  1.00 |    0.00 | 0.0458 | 0.0076 |   5.07 KB |        1.00 |
| OperationCompilerOptimizer_SmallQuery |  5.040 μs | 0.0052 μs | 0.0266 μs |  5.042 μs |  1.00 |    0.01 | 0.0458 |      - |   5.14 KB |        1.01 |
| RuntimeVisitorCheck_SmallQuery        |  5.899 μs | 0.0103 μs | 0.0529 μs |  5.877 μs |  1.17 |    0.02 | 0.0458 | 0.0076 |   5.25 KB |        1.04 |
