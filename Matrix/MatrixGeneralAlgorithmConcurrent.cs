using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Matrix
{
    public static class MatrixGeneralAlgorithmConcurrent
    { 
        public static async Task<double[,]> Multiply(double[,] srcMatrix1, double[,] srcMatrix2)
        {
            var tasks = new List<Task>();

            var resultMatrix = new double[srcMatrix1.GetUpperBound(0) + 1, srcMatrix2.GetUpperBound(1) + 1];
            for (int i = 0; i <= srcMatrix1.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= srcMatrix2.GetUpperBound(1); j++)
                {
                    var indexI = i;
                    var indexJ = j;
                    tasks.Add(
                        new Task(() =>
                        {
                            resultMatrix[indexI, indexJ] = 0;
                            for (int k = 0; k <= srcMatrix1.GetUpperBound(1); k++)
                            {
                                resultMatrix[indexI, indexJ] += srcMatrix1[indexI, k] * srcMatrix2[k, indexJ];
                            }
                        }));
                }

                if (tasks.Count > 3000)
                {
                    foreach (var task in tasks)
                    {
                        task.Start();
                    }
                    await Task.WhenAll(tasks);
                    tasks.Clear();
                }
            }
            foreach (var task in tasks)
            {
                task.Start();
            }

            await Task.WhenAll(tasks);

            return resultMatrix;
        }
    }
}
