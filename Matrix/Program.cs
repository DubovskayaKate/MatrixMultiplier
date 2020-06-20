using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
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
            
            //#region IO
            //Console.WriteLine("Enter file path for the first matrix");
            //var srcPath1 = Console.ReadLine();

            //Console.WriteLine("Enter file path for the second matrix");
            //var srcPath2 = Console.ReadLine();

            //Console.WriteLine("Enter file path for the result");
            //var dstPath = Console.ReadLine();


            //#endregion
            
            //var matrix1 = MatrixImport.FromFile(srcPath1);
            //var matrix2 = MatrixImport.FromFile(srcPath2);

            //var algorithm = new WinogradAlgorithmConcurrent();
            //var dist = await algorithm.Multiply(matrix1, matrix2);

            ////var dist2 = await GeneralAlgorithmConcurrent.Multiply(matrix1, matrix2);
            //var result = MatrixExport.ToFile(dist, dstPath);
            //if (result)
            //{
            //    Console.WriteLine("Success");
            //    /*for (int i = 0; i <= dist.GetUpperBound(0); i++)
            //    {
            //        for (int j = 0; j < dist.GetUpperBound(1); j++)
            //        {
            //            Console.Write($"{dist[i, j]}");
            //        }
            //        Console.WriteLine();
            //    }*/
            //}

            var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }

    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [RPlotExporter]
    public class AlgorithmsWithIO
    {
        public struct DataIO
        {
            public string srcPath1;
            public string srcPath2;
            public string dstPath;
        }

        [ParamsSource(nameof(ListOfDataIO))]
        public DataIO DataIo { get; set; }

        private WinogradAlgorithmConcurrent WinogradMultiplyObj = 
            new WinogradAlgorithmConcurrent();

        public IEnumerable<DataIO> ListOfDataIO => new List<DataIO>
        {
            new DataIO
            {
                srcPath1 = "C:/_MyFiles/TestFiles/5.txt",
                srcPath2 = "C:/_MyFiles/TestFiles/5.txt",
                dstPath = "C:/_MyFiles/TestFiles/res.txt"
            },
            new DataIO
            {
                srcPath1 = "C:/_MyFiles/TestFiles/10.txt",
                srcPath2 = "C:/_MyFiles/TestFiles/10.txt",
                dstPath = "C:/_MyFiles/TestFiles/res.txt"
            },
            new DataIO
            {
                srcPath1 = "C:/_MyFiles/TestFiles/50.txt",
                srcPath2 = "C:/_MyFiles/TestFiles/50.txt",
                dstPath = "C:/_MyFiles/TestFiles/res.txt"
            },
            new DataIO
            {
                srcPath1 = "C:/_MyFiles/TestFiles/100.txt",
                srcPath2 = "C:/_MyFiles/TestFiles/100.txt",
                dstPath = "C:/_MyFiles/TestFiles/res.txt"
            },
            /*new DataIO
            {
                srcPath1 = "C:/_MyFiles/TestFiles/500.txt",
                srcPath2 = "C:/_MyFiles/TestFiles/500.txt",
                dstPath = "C:/_MyFiles/TestFiles/res.txt"
            },
            new DataIO
            {
                srcPath1 = "C:/_MyFiles/TestFiles/1000.txt",
                srcPath2 = "C:/_MyFiles/TestFiles/1000.txt",
                dstPath = "C:/_MyFiles/TestFiles/res.txt"
            }*/
        };

        private double[,] srcMatrix1;
        private double[,] srcMatrix2;

        [GlobalSetup]
        public void Setup()
        {
            srcMatrix1 = MatrixImport.FromFile(DataIo.srcPath1);
            srcMatrix2 = MatrixImport.FromFile(DataIo.srcPath2);
        }
        
        [Benchmark]
        public void WinogradMultiply()
        {
            WinogradAlgorithm.Multiply(srcMatrix1, srcMatrix2);
        }

        [Benchmark]
        public async Task WinogradMultiplyConcurrent()
        {
            await WinogradMultiplyObj.Multiply(srcMatrix1, srcMatrix2);
        }

        [Benchmark]
        public void GeneralMultiply()
        {
            GeneralAlgorithm.Multiply(srcMatrix1, srcMatrix2);
        }

        [Benchmark]
        public async Task GeneralMultiplyConcurrent()
        {
            await GeneralAlgorithmConcurrent.Multiply(srcMatrix1, srcMatrix2);
        }

        [Benchmark]
        public void WinogradMultiplyWithIO()
        {
            var matrix1 = MatrixImport.FromFile(DataIo.srcPath1);
            var matrix2 = MatrixImport.FromFile(DataIo.srcPath2);
            var dstMatrix = WinogradAlgorithm.Multiply(matrix1, matrix2);
            MatrixExport.ToFile(dstMatrix, DataIo.dstPath);
        }

        [Benchmark]
        public async Task WinogradMultiplyConcurrentWithIO()
        {
            var matrix1 = MatrixImport.FromFile(DataIo.srcPath1);
            var matrix2 = MatrixImport.FromFile(DataIo.srcPath2);
            var dstMatrix = await WinogradMultiplyObj.Multiply(matrix1, matrix2);
            MatrixExport.ToFile(dstMatrix, DataIo.dstPath);
        }

        [Benchmark]
        public void GeneralMultiplyWithIO()
        {
            var matrix1 = MatrixImport.FromFile(DataIo.srcPath1);
            var matrix2 = MatrixImport.FromFile(DataIo.srcPath2);
            var dstMatrix = GeneralAlgorithm.Multiply(matrix1, matrix2);
            MatrixExport.ToFile(dstMatrix, DataIo.dstPath);
        }

        [Benchmark]
        public async Task GeneralMultiplyConcurrentWithIO()
        {
            var matrix1 = MatrixImport.FromFile(DataIo.srcPath1);
            var matrix2 = MatrixImport.FromFile(DataIo.srcPath2);
            var dstMatrix = await GeneralAlgorithmConcurrent.Multiply(matrix1, matrix2);
            MatrixExport.ToFile(dstMatrix, DataIo.dstPath);
        }
    }
}

   
