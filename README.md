| Method                                |      Mean |     Error |    StdDev |    Median | Ratio |   Gen0 |   Gen1 | Allocated | Alloc Ratio |
| ------------------------------------- | --------: | --------: | --------: | --------: | ----: | -----: | -----: | --------: | ----------: |
| RuntimeSelectionSetCheck_LargeQuery   | 19.980 μs | 0.0884 μs | 0.0827 μs | 19.959 μs |  1.00 | 0.1221 |      - |  14.86 KB |        1.00 |
| OperationCompilerOptimizer_LargeQuery | 18.358 μs | 0.0822 μs | 0.0729 μs | 18.338 μs |  0.92 | 0.1221 |      - |  14.48 KB |        0.97 |
| RuntimeVisitorCheck_LargeQuery        | 24.529 μs | 0.1547 μs | 0.1447 μs | 24.478 μs |  1.23 | 0.1526 |      - |  15.94 KB |        1.07 |
|                                       |           |           |           |           |       |        |        |           |             |
| RuntimeSelectionSetCheck_SmallQuery   |  4.997 μs | 0.0326 μs | 0.0305 μs |  5.000 μs |  1.00 | 0.0458 |      - |   5.07 KB |        1.00 |
| OperationCompilerOptimizer_SmallQuery |  4.737 μs | 0.0224 μs | 0.0210 μs |  4.738 μs |  0.95 | 0.0458 | 0.0076 |   5.01 KB |        0.99 |
| RuntimeVisitorCheck_SmallQuery        |  5.966 μs | 0.0231 μs | 0.0216 μs |  5.964 μs |  1.19 | 0.0458 |      - |   5.25 KB |        1.04 |
