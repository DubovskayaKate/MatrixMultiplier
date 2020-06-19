using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MatrixIO
{
    public static class MatrixExport
    {
        public static bool ToFile(double[,] matrix, string filePath)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
            {
                writer.Write(matrix.GetUpperBound(0) + 1);
                for (int i = 0; i <= matrix.GetUpperBound(0); i++)
                {
                    for (int j = 0; j <= matrix.GetUpperBound(1); j++)
                    {
                        writer.Write(matrix[i, j]);
                    }
                }
            }

            return true;
        }
    }
}
