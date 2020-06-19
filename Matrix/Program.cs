using System;
using System.IO;
using MatrixIO;

namespace Matrix
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter file path for the first matrix");
            var srcPath1 = Console.ReadLine();

            Console.WriteLine("Enter file path for the second matrix");
            var srcPath2 = Console.ReadLine();

            Console.WriteLine("Enter file path for the result");
            var dstPath = Console.ReadLine();

            var matrix1 = MatrixImport.FromFile(srcPath1);
            var matrix2 = MatrixImport.FromFile(srcPath2);

            var dist = MatrixWinogradAlgorithm.Multiply(matrix1, matrix2);

            var dist2 = MatrixGeneralAlgorithm.Multiply(matrix1, matrix2);
            var result = MatrixExport.ToFile(dist, dstPath);
            if (result)
            {
                Console.WriteLine("Success");
            }
        }

    }
}
