using System.Collections.Generic;
using System.Threading.Tasks;

namespace Matrix
{
    public class MatrixWinogradAlgorithmConcurrent
    {
        private double[] RowFactor;
        private double[] ColumnFactor;
        private double[,] ResultMatrix;
        public async Task<double[,]> Multiply(double[,] srcMatrix1, double[,] srcMatrix2)
        {
            var tasks = new List<Task>
            {
                new Task(async () => await this.CalculateRowFactors(srcMatrix1)),
                new Task(async() =>  await this.CalculateColumnFactors(srcMatrix2))
            };
            foreach (var task in tasks)
            {
                task.Start();
            }

            await Task.WhenAll(tasks);

            await CalculateMultiply(srcMatrix1, srcMatrix2, RowFactor, ColumnFactor);
            return ResultMatrix;
        }

        private async Task CalculateRowFactors(double[,] srcMatrix1)
        {
            RowFactor = new double[srcMatrix1.GetUpperBound(0) + 1];
            var tasks = new List<Task>();

            for (var i = 0; i <= srcMatrix1.GetUpperBound(0); ++i)
            {
                var index = i;
                tasks.Add(
                    new Task(() =>
                    {
                        RowFactor[index] = srcMatrix1[index, 0] * srcMatrix1[index, 1];
                        for (var j = 1; j < (srcMatrix1.GetUpperBound(1) + 1) / 2; ++j)
                        {
                            RowFactor[index] += srcMatrix1[index, 2 * j] * srcMatrix1[index, 2 * j + 1];
                        }
                    }));
            }
            foreach (var task in tasks)
            {
                task.Start();
            }

            await Task.WhenAll(tasks);
        }

        private async Task CalculateColumnFactors(double[,] srcMatrix2)
        {
            ColumnFactor = new double[srcMatrix2.GetUpperBound(0) + 1];
            var tasks = new List<Task>();

            for (int i = 0; i <= srcMatrix2.GetUpperBound(0); i++)
            {
                var index = i;
                tasks.Add(
                    new Task(() =>
                    {
                        ColumnFactor[index] = srcMatrix2[0, index] * srcMatrix2[1, index];
                        for (int j = 1; j < (srcMatrix2.GetUpperBound(1) + 1) / 2; j++)
                        {
                            ColumnFactor[index] += srcMatrix2[2 * j, index] * srcMatrix2[2 * j + 1, index];
                        }
                    }));
            }
            foreach (var task in tasks)
            {
                task.Start();
            }

            await Task.WhenAll(tasks);
        }

        private  async Task CalculateMultiply(double[,] srcMatrix1, double[,] srcMatrix2, double[] rowFactors, double[] columnFactors)
        {
            ResultMatrix = new double[srcMatrix1.GetUpperBound(0) + 1, srcMatrix2.GetUpperBound(0) + 1];
            var tasks = new List<Task>();

            for (int i = 0; i <= srcMatrix1.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= srcMatrix2.GetUpperBound(0); j++)
                {
                    var indexI = i;
                    var indexJ = j;
                    tasks.Add(
                        new Task(() =>
                        {
                            ResultMatrix[indexI, indexJ] = -rowFactors[indexI] - columnFactors[indexJ];
                            for (int k = 0; k < (srcMatrix2.GetUpperBound(1) + 1) / 2; k++)
                            {
                                ResultMatrix[indexI, indexJ] +=
                                    (srcMatrix1[indexI, 2 * k] + srcMatrix2[2 * k + 1, indexJ]) *
                                    (srcMatrix1[indexI, 2 * k + 1] + srcMatrix2[2 * k, indexJ]);
                            }
                        }));
                }
            }

            foreach (var task in tasks)
            {
                task.Start();
            }

            await Task.WhenAll(tasks);
            tasks.Clear();

            if ((srcMatrix1.GetUpperBound(1) & 1) == 0)
            {
                for (int i = 0; i <= srcMatrix1.GetUpperBound(0); i++)
                {
                    var indexI = i;
                    tasks.Add(
                        new Task(() =>
                        {
                            for (int j = 0; j <= srcMatrix2.GetUpperBound(0); j++)
                            {
                                ResultMatrix[indexI, j] = ResultMatrix[indexI, j] +
                                                     srcMatrix1[indexI, srcMatrix1.GetUpperBound(1)] *
                                                     srcMatrix2[srcMatrix2.GetUpperBound(0), j];
                            }
                        }));
                }
            }
            foreach (var task in tasks)
            {
                task.Start();
            }
            await Task.WhenAll(tasks);

        }
    }
}