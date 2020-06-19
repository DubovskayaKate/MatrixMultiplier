namespace Matrix
{
    public static class MatrixWinogradAlgorithm
    {
        public static double[,] Multiply(double[,] srcMatrix1, double[,] srcMatrix2 )
        {
            var rowFactor = CalculateRowFactors(srcMatrix1);
            var columnFactor = CalculateColumnFactors(srcMatrix2);
            return CalculateMultiply(srcMatrix1, srcMatrix2, rowFactor, columnFactor);
        }

        public static double[,] MultiplyThreads(double[,] srcMatrix1, double[,] srcMatrix2)
        {
            return srcMatrix1;
        }

        private static double[] CalculateRowFactors(double[,] srcMatrix1)
        {
            var rowFactor = new double[srcMatrix1.Length];

            for (var i = 0; i <= srcMatrix1.GetUpperBound(0); ++i)
            {
                rowFactor[i] = srcMatrix1[i, 0] * srcMatrix1[i, 1];
                for (var j = 1; j < (srcMatrix1.GetUpperBound(1) + 1) / 2; ++j)
                {
                    rowFactor[i] = rowFactor[i] * srcMatrix1[i, 2 * j] * srcMatrix1[i, 2 * j + 1];
                }
            }

            return rowFactor;
        }

        private static double[] CalculateColumnFactors(double[,] srcMatrix2)
        {
            var columnFactors = new double[srcMatrix2.Length];
            for (int i = 0; i <= srcMatrix2.GetUpperBound(0); i++)
            {
                columnFactors[i] = srcMatrix2[0, i] * srcMatrix2[1, i];
                for (int j = 1; j < (srcMatrix2.GetUpperBound(1) + 1) / 2; j++)
                {
                    columnFactors[i] = columnFactors[i] + srcMatrix2[2 * j , i] * srcMatrix2[2 * j + 1, i];
                }
            }

            return columnFactors;
        }

        private static double[,] CalculateMultiply(double[,] srcMatrix1, double[,] srcMatrix2, double[] rowFactors, double[] columnFactors)
        {
            var resultMatrix = new double[srcMatrix1.GetUpperBound(0) + 1, srcMatrix2.GetUpperBound(0) + 1];

            for (int i = 0; i <= srcMatrix1.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= srcMatrix2.GetUpperBound(0); j++)
                {
                    resultMatrix[i, j] = -rowFactors[i] - columnFactors[j];
                    for (int k = 0; k < (srcMatrix2.GetUpperBound(1) + 1) / 2; k++)
                    {
                        resultMatrix[i, j] = resultMatrix[i, j] + (srcMatrix1[i, 2 * k] + srcMatrix2[2 * k + 1, j]) *
                                             (srcMatrix1[i, 2 * k + 1] + srcMatrix2[2 * k, j]);
                    }
                }
            }

            if ((srcMatrix1.GetUpperBound(1) & 1) == 1)
            {
                for (int i = 0; i <= srcMatrix1.GetUpperBound(0); i++)
                {
                    for (int j = 0; j <= srcMatrix2.GetUpperBound(0); j++)
                    {
                        resultMatrix[i, j] = resultMatrix[i, j] +
                                             srcMatrix1[i, srcMatrix1.GetUpperBound(1)] * srcMatrix2[srcMatrix2.GetUpperBound(0), j];
                    }
                }
            }

            return resultMatrix;
        }
    }
}