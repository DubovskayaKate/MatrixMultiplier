using System;
using System.IO;

namespace MatrixIO
{
    public static class MatrixImport
    {
        public static double[,] FromFile(string filePath)
        {
            double[,] matrix;
            using (var reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
            {
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                var size = reader.ReadInt32();
                matrix = new double[size, size];
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        matrix[i, j] = reader.ReadDouble();
                    }
                }
            }

            return matrix;
        } 
    }
}
