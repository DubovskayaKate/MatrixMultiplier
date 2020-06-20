using System;
using System.IO;

namespace MatrixGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter file path");
            var filePath = Console.ReadLine();
            Console.WriteLine("Enter size of matrix");
            var size = Convert.ToInt32(Console.ReadLine());

            var matrix = GenerateMatrix(size);
            Console.WriteLine(MatrixIO.MatrixExport.ToFile(matrix, filePath)
                ? "File successfully created"
                : "Can't save matrix to file");
        }

        static double[,] GenerateMatrix(int size)
        {
            var matrix = new double[size, size];
            var rand = new Random();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    matrix[i, j] = rand.NextDouble();
                }
            }

            return matrix;
        }
    }
}
