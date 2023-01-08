
BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22000.1335/21H2)
AMD Ryzen 3 5300U with Radeon Graphics, 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 7.0.1 (7.0.122.56804), X64 RyuJIT AVX2 DEBUG
  Job-VLVUCF : .NET 7.0.1 (7.0.122.56804), X64 RyuJIT AVX2

InvocationCount=1000  UnrollFactor=1  

                 Method | insertTimes |     Mean |    Error |    StdDev |   Median | Allocated |
----------------------- |------------ |---------:|---------:|----------:|---------:|----------:|
  **InsertIntoRopeAtStart** |         **100** | **777.6 ns** | **85.05 ns** | **246.74 ns** | **830.0 ns** |     **904 B** |
 InsertIntoRopeAtMiddle |         100 | 937.4 ns | 14.79 ns |  25.11 ns | 929.9 ns |     976 B |
    InsertIntoRopeAtEnd |         100 | 854.2 ns | 53.31 ns | 145.94 ns | 861.0 ns |     832 B |
  **InsertIntoRopeAtStart** |        **1000** | **578.0 ns** | **11.16 ns** |  **16.01 ns** | **572.4 ns** |    **1120 B** |
 InsertIntoRopeAtMiddle |        1000 | 624.1 ns | 12.49 ns |  32.02 ns | 615.1 ns |    1192 B |
    InsertIntoRopeAtEnd |        1000 | 583.6 ns |  9.62 ns |  22.69 ns | 575.8 ns |    1048 B |
  **InsertIntoRopeAtStart** |       **10000** | **823.8 ns** |  **8.36 ns** |   **7.41 ns** | **822.0 ns** |    **1408 B** |
 InsertIntoRopeAtMiddle |       10000 | 775.8 ns | 11.60 ns |  10.85 ns | 775.5 ns |    1408 B |
    InsertIntoRopeAtEnd |       10000 | 733.1 ns |  6.27 ns |   5.56 ns | 733.1 ns |    1264 B |
