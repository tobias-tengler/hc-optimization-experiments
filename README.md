| Method                                |      Mean |     Error |    StdDev |    Median | Ratio |   Gen0 |   Gen1 | Allocated | Alloc Ratio |
| ------------------------------------- | --------: | --------: | --------: | --------: | ----: | -----: | -----: | --------: | ----------: |
| RuntimeSelectionSetCheck_LargeQuery   | 19.882 μs | 0.0939 μs | 0.0878 μs | 19.900 μs |  1.00 | 0.1221 | 0.0305 |  14.86 KB |        1.00 |
| OperationCompilerOptimizer_LargeQuery | 19.574 μs | 0.0915 μs | 0.0856 μs | 19.586 μs |  0.98 | 0.1221 |      - |  15.23 KB |        1.03 |
| RuntimeVisitorCheck_LargeQuery        | 24.809 μs | 0.2722 μs | 0.2546 μs | 24.759 μs |  1.25 | 0.1526 |      - |  15.94 KB |        1.07 |
|                                       |           |           |           |           |       |        |        |           |             |
| RuntimeSelectionSetCheck_SmallQuery   |  4.956 μs | 0.0232 μs | 0.0217 μs |  4.955 μs |  1.00 | 0.0458 | 0.0076 |   5.07 KB |        1.00 |
| OperationCompilerOptimizer_SmallQuery |  4.883 μs | 0.0147 μs | 0.0130 μs |  4.881 μs |  0.99 | 0.0458 |      - |   5.13 KB |        1.01 |
| RuntimeVisitorCheck_SmallQuery        |  5.806 μs | 0.0223 μs | 0.0197 μs |  5.805 μs |  1.17 | 0.0458 |      - |   5.25 KB |        1.04 |
