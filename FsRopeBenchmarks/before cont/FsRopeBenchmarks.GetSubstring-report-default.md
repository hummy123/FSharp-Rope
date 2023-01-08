
BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22000.1335/21H2)
AMD Ryzen 3 5300U with Radeon Graphics, 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 7.0.1 (7.0.122.56804), X64 RyuJIT AVX2 DEBUG
  Job-VLVUCF : .NET 7.0.1 (7.0.122.56804), X64 RyuJIT AVX2

InvocationCount=1000  UnrollFactor=1  

                     Method | insertTimes |     Mean |   Error |  StdDev | Allocated |
--------------------------- |------------ |---------:|--------:|--------:|----------:|
  **GetSubstringAtStartOfrope** |         **100** | **486.2 ns** | **9.78 ns** | **8.67 ns** |     **256 B** |
 GetSubstringAtMiddleOfrope |         100 | 472.1 ns | 3.89 ns | 3.04 ns |     256 B |
    GetSubstringAtEndOfrope |         100 | 437.4 ns | 8.14 ns | 9.04 ns |     248 B |
  **GetSubstringAtStartOfrope** |        **1000** | **532.0 ns** | **7.88 ns** | **6.15 ns** |     **256 B** |
 GetSubstringAtMiddleOfrope |        1000 | 539.5 ns | 7.74 ns | 6.47 ns |     256 B |
    GetSubstringAtEndOfrope |        1000 | 488.4 ns | 9.61 ns | 9.87 ns |     248 B |
  **GetSubstringAtStartOfrope** |       **10000** | **351.4 ns** | **6.62 ns** | **6.19 ns** |     **256 B** |
 GetSubstringAtMiddleOfrope |       10000 | 332.5 ns | 4.29 ns | 4.01 ns |     256 B |
    GetSubstringAtEndOfrope |       10000 | 309.8 ns | 4.30 ns | 3.59 ns |     248 B |
