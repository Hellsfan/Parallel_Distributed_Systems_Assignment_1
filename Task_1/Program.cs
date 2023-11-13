using System;
using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        int[] threads = new int[] {1, 2, 3, 4, 6 };
        Stopwatch stopwatch = new Stopwatch();

        Random random = new Random();
        List<int> list = new List<int>();
        for (int i = 0; i < 100000; i++)
        {
            list.Add(random.Next(1000));
        }

        Console.WriteLine("This will take some time. Approximately 1-2 minutes. Thank you for your patience!");
        foreach (var numOfThreads in threads)
        {
            stopwatch.Reset();
            stopwatch.Start();
            ParallelSorting(list, numOfThreads);
            stopwatch.Stop();
            Console.WriteLine($"The task is done. {numOfThreads} threads have executed the task for {stopwatch.Elapsed}.");
        }

        Console.ReadLine();
    }

    public static List<int> ParallelSorting(List<int> _inputList, int numOfThreads)
    {
        List<int> inputList = new List<int>();
        foreach (var item in _inputList)
        {
            inputList.Add(item);
        }

        List<List<int>> workToDelegate = new List<List<int>>();

        int highestNumberForBatch = inputList.Max() % numOfThreads == 0? inputList.Max() / numOfThreads : inputList.Max() / numOfThreads+1;
        int indexMultiplierForHighestBatchNumber = 1;

        var outputList = new List<int>();

        while (inputList.Count > 0)
        {
            List<int> delegatedWork = new List<int>();
            for (int i = 0; i < inputList.Count; i++)
            {
                if (inputList[i] < highestNumberForBatch * indexMultiplierForHighestBatchNumber)
                {
                    delegatedWork.Add(inputList[i]);
                }

            }
            workToDelegate.Add(delegatedWork);
            inputList.RemoveAll(i => delegatedWork.Contains(i));
            indexMultiplierForHighestBatchNumber++;
        }

        List<Thread> threads = new List<Thread>();
        for (int i = 0; i < numOfThreads; i++)
        {
            var currentWork = workToDelegate[i];
            Thread t = new Thread(() =>
            {
                BubbleSort(currentWork);
            });

            t.Start();
            threads.Add(t);
        }

        foreach (var t in threads)
        {
            t.Join();
        }

        var result = new List<int>();
        foreach (var list in workToDelegate)
        {
            foreach (var item in list)
            {
                result.Add(item);
            }
        }

        return result;
    }

    public static List<int> BubbleSort(List<int> input)
    {
        var n = input.Count;

        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (input[j] > input[j + 1])
                {
                    var tempVar = input[j];
                    input[j] = input[j + 1];
                    input[j + 1] = tempVar;
                }
            }
        }

        return input;
    }
}