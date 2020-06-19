using System;
using System.IO;
using System.Threading.Tasks;
using MatrixIO;

namespace Matrix
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter file path for the first matrix");
            var srcPath1 = Console.ReadLine();

            Console.WriteLine("Enter file path for the second matrix");
            var srcPath2 = Console.ReadLine();

            Console.WriteLine("Enter file path for the result");
            var dstPath = Console.ReadLine();

            var matrix1 = MatrixImport.FromFile(srcPath1);
            var matrix2 = MatrixImport.FromFile(srcPath2);

            var algorithm = new MatrixWinogradAlgorithmConcurrent();
            var dist = await algorithm.Multiply(matrix1, matrix2);

            var dist2 = MatrixGeneralAlgorithm.Multiply(matrix1, matrix2);
            var result = MatrixExport.ToFile(dist, dstPath);
            if (result)
            {
                Console.WriteLine("Success");
                for (int i = 0; i <= dist.GetUpperBound(0); i++)
                {
                    for (int j = 0; j < dist.GetUpperBound(1); j++)
                    {
                        Console.Write($"{dist[i, j]}");
                    }
                    Console.WriteLine();
                }
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
        }

    }
}
