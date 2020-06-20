using System.Collections.Generic;
using System.Threading.Tasks;

namespace Matrix
{
    public static class GeneralAlgorithmConcurrent
    {
        private const int TasksUpperBound = 3000;
        public static async Task<double[,]> Multiply(double[,] srcMatrix1, double[,] srcMatrix2)
        {
            var tasks = new List<Task>();

            var resultMatrix = new double[srcMatrix1.GetUpperBound(0) + 1, srcMatrix2.GetUpperBound(1) + 1];
            for (var i = 0; i <= srcMatrix1.GetUpperBound(0); i++)
            {
                // For correct closure, each Task has its own correct i-value
                var indexI = i;
                tasks.Add(
                    new Task(() =>
                    {
                        for (int indexJ = 0; indexJ <= srcMatrix2.GetUpperBound(1); indexJ++)
                        {
                            resultMatrix[indexI, indexJ] = 0;
                            for (int k = 0; k <= srcMatrix1.GetUpperBound(1); k++)
                            {
                                resultMatrix[indexI, indexJ] += srcMatrix1[indexI, k] * srcMatrix2[k, indexJ];
                            }
                        }
                    })
                );

                if (tasks.Count > TasksUpperBound)
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
