using System.Collections.Generic;
using System.Threading.Tasks;

namespace Matrix
{
    public class WinogradAlgorithmConcurrent
    {
        private const int TasksUpperBound = 3000;

        private double[] _rowFactor;
        private double[] _columnFactor;
        private double[,] _resultMatrix;

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

            await CalculateMultiply(srcMatrix1, srcMatrix2, _rowFactor, _columnFactor);
            return _resultMatrix;
        }

        private async Task CalculateRowFactors(double[,] srcMatrix1)
        {
            _rowFactor = new double[srcMatrix1.GetUpperBound(0) + 1];
            var tasks = new List<Task>();

            for (var i = 0; i <= srcMatrix1.GetUpperBound(0); ++i)
            {
                // For correct closure, each Task has its own correct i-value
                var index = i;
                tasks.Add(
                    new Task(() =>
                    {
                        _rowFactor[index] = srcMatrix1[index, 0] * srcMatrix1[index, 1];
                        for (var j = 1; j < (srcMatrix1.GetUpperBound(1) + 1) / 2; ++j)
                        {
                            _rowFactor[index] += srcMatrix1[index, 2 * j] * srcMatrix1[index, 2 * j + 1];
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
        }

        private async Task CalculateColumnFactors(double[,] srcMatrix2)
        {
            _columnFactor = new double[srcMatrix2.GetUpperBound(0) + 1];
            var tasks = new List<Task>();

            for (var i = 0; i <= srcMatrix2.GetUpperBound(0); i++)
            {
                // For correct closure, each Task has own correct i-value
                var index = i;
                tasks.Add(
                    new Task(() =>
                    {
                        _columnFactor[index] = srcMatrix2[0, index] * srcMatrix2[1, index];
                        for (var j = 1; j < (srcMatrix2.GetUpperBound(1) + 1) / 2; j++)
                        {
                            _columnFactor[index] += srcMatrix2[2 * j, index] * srcMatrix2[2 * j + 1, index];
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
        }

        private  async Task CalculateMultiply(double[,] srcMatrix1, double[,] srcMatrix2, double[] rowFactors, double[] columnFactors)
        {
            _resultMatrix = new double[srcMatrix1.GetUpperBound(0) + 1, srcMatrix2.GetUpperBound(0) + 1];
            var tasks = new List<Task>();

            for (var i = 0; i <= srcMatrix1.GetUpperBound(0); i++)
            {
                for (var j = 0; j <= srcMatrix2.GetUpperBound(0); j++)
                {
                    // For correct closure, each Task has its own correct i and j value
                    var indexI = i;
                    var indexJ = j;
                    tasks.Add(
                        new Task(() =>
                        {
                            _resultMatrix[indexI, indexJ] = -rowFactors[indexI] - columnFactors[indexJ];
                            for (var k = 0; k < (srcMatrix2.GetUpperBound(1) + 1) / 2; k++)
                            {
                                _resultMatrix[indexI, indexJ] +=
                                    (srcMatrix1[indexI, 2 * k] + srcMatrix2[2 * k + 1, indexJ]) *
                                    (srcMatrix1[indexI, 2 * k + 1] + srcMatrix2[2 * k, indexJ]);
                            }
                        })
                    );
                }

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
            tasks.Clear();

            if ((srcMatrix1.GetUpperBound(1) & 1) == 0)
            {
                for (var i = 0; i <= srcMatrix1.GetUpperBound(0); i++)
                {
                    // For correct closure, each Task has its own correct i-value
                    var indexI = i;
                    tasks.Add(
                        new Task(() =>
                        {
                            for (var j = 0; j <= srcMatrix2.GetUpperBound(0); j++)
                            {
                                _resultMatrix[indexI, j] += srcMatrix1[indexI, srcMatrix1.GetUpperBound(1)] *
                                                            srcMatrix2[srcMatrix2.GetUpperBound(0), j];
                            }
                        })
                    );
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