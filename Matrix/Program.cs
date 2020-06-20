using System.Collections.Generic;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using MatrixIO;

namespace Matrix
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }

    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [RPlotExporter]
    public class AlgorithmsWithIO
    {
        public struct DataIO
        {
            public string SrcPath1;
            public string SrcPath2;
            public string DstPath;
        }

        [ParamsSource(nameof(ListOfDataIO))]
        public DataIO DataIo { get; set; }

        private WinogradAlgorithmConcurrent WinogradMultiplyObj = 
            new WinogradAlgorithmConcurrent();

        public IEnumerable<DataIO> ListOfDataIO => new List<DataIO>
        {
            new DataIO
            {
                SrcPath1 = "C:/_MyFiles/TestFiles/5.txt",
                SrcPath2 = "C:/_MyFiles/TestFiles/5.txt",
                DstPath = "C:/_MyFiles/TestFiles/res.txt"
            },
            new DataIO
            {
                SrcPath1 = "C:/_MyFiles/TestFiles/10.txt",
                SrcPath2 = "C:/_MyFiles/TestFiles/10.txt",
                DstPath = "C:/_MyFiles/TestFiles/res.txt"
            },
            new DataIO
            {
                SrcPath1 = "C:/_MyFiles/TestFiles/50.txt",
                SrcPath2 = "C:/_MyFiles/TestFiles/50.txt",
                DstPath = "C:/_MyFiles/TestFiles/res.txt"
            },
            new DataIO
            {
                SrcPath1 = "C:/_MyFiles/TestFiles/100.txt",
                SrcPath2 = "C:/_MyFiles/TestFiles/100.txt",
                DstPath = "C:/_MyFiles/TestFiles/res.txt"
            },
            new DataIO
            {
                SrcPath1 = "C:/_MyFiles/TestFiles/500.txt",
                SrcPath2 = "C:/_MyFiles/TestFiles/500.txt",
                DstPath = "C:/_MyFiles/TestFiles/res.txt"
            },
            new DataIO
            {
                SrcPath1 = "C:/_MyFiles/TestFiles/1000.txt",
                SrcPath2 = "C:/_MyFiles/TestFiles/1000.txt",
                DstPath = "C:/_MyFiles/TestFiles/res.txt"
            }
        };

        private double[,] _srcMatrix1;
        private double[,] _srcMatrix2;

        [GlobalSetup]
        public void Setup()
        {
            _srcMatrix1 = MatrixImport.FromFile(DataIo.SrcPath1);
            _srcMatrix2 = MatrixImport.FromFile(DataIo.SrcPath2);
        }
        
        [Benchmark]
        public void WinogradMultiply()
        {
            WinogradAlgorithm.Multiply(_srcMatrix1, _srcMatrix2);
        }

        [Benchmark]
        public async Task WinogradMultiplyConcurrent()
        {
            await WinogradMultiplyObj.Multiply(_srcMatrix1, _srcMatrix2);
        }

        [Benchmark]
        public void GeneralMultiply()
        {
            GeneralAlgorithm.Multiply(_srcMatrix1, _srcMatrix2);
        }

        [Benchmark]
        public async Task GeneralMultiplyConcurrent()
        {
            await GeneralAlgorithmConcurrent.Multiply(_srcMatrix1, _srcMatrix2);
        }

        [Benchmark]
        public void WinogradMultiplyWithIO()
        {
            var matrix1 = MatrixImport.FromFile(DataIo.SrcPath1);
            var matrix2 = MatrixImport.FromFile(DataIo.SrcPath2);
            var dstMatrix = WinogradAlgorithm.Multiply(matrix1, matrix2);
            MatrixExport.ToFile(dstMatrix, DataIo.DstPath);
        }

        [Benchmark]
        public async Task WinogradMultiplyConcurrentWithIO()
        {
            var matrix1 = MatrixImport.FromFile(DataIo.SrcPath1);
            var matrix2 = MatrixImport.FromFile(DataIo.SrcPath2);
            var dstMatrix = await WinogradMultiplyObj.Multiply(matrix1, matrix2);
            MatrixExport.ToFile(dstMatrix, DataIo.DstPath);
        }

        [Benchmark]
        public void GeneralMultiplyWithIO()
        {
            var matrix1 = MatrixImport.FromFile(DataIo.SrcPath1);
            var matrix2 = MatrixImport.FromFile(DataIo.SrcPath2);
            var dstMatrix = GeneralAlgorithm.Multiply(matrix1, matrix2);
            MatrixExport.ToFile(dstMatrix, DataIo.DstPath);
        }

        [Benchmark]
        public async Task GeneralMultiplyConcurrentWithIO()
        {
            var matrix1 = MatrixImport.FromFile(DataIo.SrcPath1);
            var matrix2 = MatrixImport.FromFile(DataIo.SrcPath2);
            var dstMatrix = await GeneralAlgorithmConcurrent.Multiply(matrix1, matrix2);
            MatrixExport.ToFile(dstMatrix, DataIo.DstPath);
        }
    }
}

   
