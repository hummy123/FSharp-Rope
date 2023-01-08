
BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22000.1335/21H2)
AMD Ryzen 3 5300U with Radeon Graphics, 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 7.0.1 (7.0.122.56804), X64 RyuJIT AVX2 DEBUG
  Job-VLVUCF : .NET 7.0.1 (7.0.122.56804), X64 RyuJIT AVX2

InvocationCount=1000  UnrollFactor=1  

                 Method | insertTimes |     Mean |     Error |    StdDev |   Median |   Gen0 |   Gen1 | Allocated |
----------------------- |------------ |---------:|----------:|----------:|---------:|-------:|-------:|----------:|
  **DeleteFromStartOfrope** |         **100** | **2.019 μs** | **0.0237 μs** | **0.0198 μs** | **2.024 μs** |      **-** |      **-** |   **1.97 KB** |
 DeleteFromMiddleOfrope |         100 | 2.205 μs | 0.2558 μs | 0.7542 μs | 1.740 μs | 1.0000 |      - |   2.86 KB |
    DeleteFromEndOfrope |         100 | 3.347 μs | 0.0661 μs | 0.1087 μs | 3.327 μs | 1.0000 |      - |    3.7 KB |
  **DeleteFromStartOfrope** |        **1000** | **1.330 μs** | **0.0503 μs** | **0.1369 μs** | **1.294 μs** |      **-** |      **-** |   **2.44 KB** |
 DeleteFromMiddleOfrope |        1000 | 2.001 μs | 0.0399 μs | 0.0850 μs | 1.961 μs |      - |      - |   4.08 KB |
    DeleteFromEndOfrope |        1000 | 2.195 μs | 0.0437 μs | 0.1039 μs | 2.146 μs |      - |      - |   4.73 KB |
  **DeleteFromStartOfrope** |       **10000** | **1.528 μs** | **0.0072 μs** | **0.0060 μs** | **1.529 μs** |      **-** |      **-** |   **3.04 KB** |
 DeleteFromMiddleOfrope |       10000 | 3.730 μs | 0.0740 μs | 0.2000 μs | 3.753 μs | 1.0000 | 1.0000 |   5.86 KB |
    DeleteFromEndOfrope |       10000 | 3.609 μs | 0.0875 μs | 0.2581 μs | 3.604 μs | 1.0000 | 1.0000 |   5.97 KB |
