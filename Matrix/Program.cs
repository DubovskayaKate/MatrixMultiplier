using System;
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
            
            #region IO
            Console.WriteLine("Enter file path for the first matrix");
            var srcPath1 = Console.ReadLine();

            Console.WriteLine("Enter file path for the second matrix");
            var srcPath2 = Console.ReadLine();

            Console.WriteLine("Enter file path for the result");
            var dstPath = Console.ReadLine();


            #endregion
            
            var matrix1 = MatrixImport.FromFile(srcPath1);
            var matrix2 = MatrixImport.FromFile(srcPath2);

            var algorithm = new MatrixWinogradAlgorithmConcurrent();
            //var dist = await algorithm.Multiply(matrix1, matrix2);

            var dist2 = await MatrixGeneralAlgorithmConcurrent.Multiply(matrix1, matrix2);
            var result = MatrixExport.ToFile(dist2, dstPath);
            if (result)
            {
                Console.WriteLine("Success");
                /*for (int i = 0; i <= dist.GetUpperBound(0); i++)
                {
                    for (int j = 0; j < dist.GetUpperBound(1); j++)
                    {
                        Console.Write($"{dist[i, j]}");
                    }
                    Console.WriteLine();
                }*/
                Console.WriteLine("________");
                for (int i = 0; i <= dist2.GetUpperBound(0); i++)
                {
                    for (int j = 0; j < dist2.GetUpperBound(1); j++)
                    {
                        Console.Write($"{dist2[i,j]}");
                    }
                    Console.WriteLine();
                }
            }

            //var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }

    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [RPlotExporter]
    public class AlgorithmsWithIO
    {
        private MatrixWinogradAlgorithmConcurrent WinogradMultiplyObj = 
            new MatrixWinogradAlgorithmConcurrent();
    
        [Params("C:/_MyFiles/TestFiles/100.txt",
            "C:/_MyFiles/TestFiles/500.txt",
            "C:/_MyFiles/TestFiles/1000.txt",
            "C:/_MyFiles/TestFiles/5000.txt",
            "C:/_MyFiles/TestFiles/10000.txt")]
        public string filePath;

        private double[,] srcMatrix1;
        private double[,] srcMatrix2;

        [GlobalSetup]
        public void Setup()
        {
            srcMatrix1 = MatrixImport.FromFile(filePath);
            srcMatrix2 = MatrixImport.FromFile(filePath);
        }
        
        [Benchmark]
        public void WinogradMultiply()
        {
            MatrixWinogradAlgorithm.Multiply(srcMatrix1, srcMatrix2);
        }

        [Benchmark]
        public async Task WinogradMultiplyConcurrent()
        {
            await WinogradMultiplyObj.Multiply(srcMatrix1, srcMatrix2);
        }

        [Benchmark]
        public void GeneralMultiply()
        {
            MatrixGeneralAlgorithm.Multiply(srcMatrix1, srcMatrix2);
        }

        [Benchmark]
        public async Task GeneralMultiplyConcurrent()
        {
            await MatrixGeneralAlgorithmConcurrent.Multiply(srcMatrix1, srcMatrix2);
        }

        [Benchmark]
        public void WinogradMultiplyWithIO()
        {
            var matrix1 = MatrixImport.FromFile(filePath);
            var matrix2 = MatrixImport.FromFile(filePath);
            var dstMatrix = MatrixWinogradAlgorithm.Multiply(matrix1, matrix2);
            MatrixExport.ToFile(dstMatrix, "res.txt");
        }

        [Benchmark]
        public async Task WinogradMultiplyConcurrentWithIO()
        {
            var matrix1 = MatrixImport.FromFile(filePath);
            var matrix2 = MatrixImport.FromFile(filePath);
            var dstMatrix = await WinogradMultiplyObj.Multiply(matrix1, matrix2);
            MatrixExport.ToFile(dstMatrix, "res.txt");
        }

        [Benchmark]
        public void GeneralMultiplyWithIO()
        {
            var matrix1 = MatrixImport.FromFile(filePath);
            var matrix2 = MatrixImport.FromFile(filePath);
            var dstMatrix = MatrixGeneralAlgorithm.Multiply(matrix1, matrix2);
            MatrixExport.ToFile(dstMatrix, "res.txt");
        }

        [Benchmark]
        public async Task GeneralMultiplyConcurrentWithIO()
        {
            var matrix1 = MatrixImport.FromFile(filePath);
            var matrix2 = MatrixImport.FromFile(filePath);
            var dstMatrix = await MatrixGeneralAlgorithmConcurrent.Multiply(matrix1, matrix2);
            MatrixExport.ToFile(dstMatrix, "res.txt");
        }
    }
}

   
