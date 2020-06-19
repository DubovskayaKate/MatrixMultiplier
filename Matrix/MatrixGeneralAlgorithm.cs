namespace Matrix
{
    public static class MatrixGeneralAlgorithm
    {
        public static double[,] Multiply(double[,] srcMatrix1, double[,] srcMatrix2)
        {
            var resultMatrix = new double[srcMatrix1.GetUpperBound(0) + 1, srcMatrix2.GetUpperBound(1) + 1];
            for (int i = 0; i <= srcMatrix1.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= srcMatrix2.GetUpperBound(1); j++)
                {
                    resultMatrix[i, j] = 0;
                    for (int k = 0; k <= srcMatrix1.GetUpperBound(1); k++)
                    {
                        resultMatrix[i, j] += srcMatrix1[i, k] * srcMatrix2[k, j];
                    }
                }
            }

            return resultMatrix;
        }
    }
}