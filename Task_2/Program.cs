using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using Task_2;

internal class Program
{
    static readonly object locker = new object();

    private static void Main(string[] args)
    {
        int[] threads = new int[] { 1, 1, 2, 3, 4, 6 , 48 };
        Stopwatch stopwatch = new Stopwatch();

        Inventory inventory = new Inventory();
        Dictionary<int, int> criteria = new Dictionary<int, int>()
        {
            { 1, 30},
            { 7, 15 },
            { 10, 8 },
        };


        var a = ParallelSearchForTools(inventory.Tools, criteria, 1);

        foreach (var item in a)
        {
            Console.WriteLine(item.Barcode);
        }

        foreach (var numOfThreads in threads)
        {
            stopwatch.Reset();
            stopwatch.Start();
            ParallelSearchForTools(inventory.Tools, criteria, numOfThreads);
            stopwatch.Stop();
            Console.WriteLine($"The task is done. {numOfThreads} threads have executed the task for {stopwatch.Elapsed}.");
        }

    }

    public static List<Tool> ParallelSearchForTools(List<Tool> _toolsList, Dictionary<int, int> _criteria, int numOfThreads)
    {
        Dictionary<int, int> criteria = _criteria;
        List<Tool> results = new List<Tool>();
        List<Tool> toolsInv = _toolsList;
        List<List<Tool>> batches = new List<List<Tool>>();
        List<List<Tool>> batchesResults = new List<List<Tool>>();

        int maxToolsPerBatch = toolsInv.Count % numOfThreads == 0 ? toolsInv.Count / numOfThreads : toolsInv.Count / numOfThreads + 1;

        for (int i = 0; i < numOfThreads; i++)
        {
            List<Tool> batch = new List<Tool>();
            for (int j = 0; j < maxToolsPerBatch; j++)
            {
                if (toolsInv.Count == 0) break;
                batch.Add(toolsInv[0]);
                toolsInv.RemoveAt(0);
            }
            batches.Add(batch);
        }

        List<Thread> threads = new List<Thread>();
        for (int i = 0; i < numOfThreads; i++)
        {
            var currentBatch = batches[i];
            Thread t = new Thread(() =>
            {
                batchesResults.Add(SearchForTools(currentBatch, criteria));
            });

            t.Start();
            threads.Add(t);
        }

        foreach (var t in threads)
        {
            t.Join();
        }

        foreach (var batch in batchesResults)
        {
            foreach (var tool in batch)
            {
                results.Add(tool);
            }
        }

        return results;
    }

    public static List<Tool> SearchForTools(List<Tool> _toolsList, Dictionary<int, int> criteria)
    {
        List<Tool> results = new List<Tool>();

        List<Tool> toolsInv = _toolsList;


        foreach (var tool in toolsInv)
        {
            if (!CheckIfToolIsNeeded(tool, criteria)) continue;
            lock (locker)
            {
                if (CheckIfCriteriaIsMet(criteria)) return results;

                InsertToolToList(tool, results, criteria);
            }
            if (CheckIfCriteriaIsMet(criteria)) return results;
        }


        return results;
    }

    public static void InsertToolToList(Tool tool, List<Tool> tools, Dictionary<int, int> criteria)
    {
        tools.Add(tool);
        criteria[tool.Type]--;
    }

    public static bool CheckIfToolIsNeeded(Tool tool, Dictionary<int, int> criteria)
    {
        switch (tool.Type)
        {
            case 1:
                if (criteria[1] == 0) return false;
                break;
            case 7:
                if (criteria[7] == 0) return false;
                break;
            case 10:
                if (criteria[10] == 0) return false;
                break;

            default:
                return false;
        }

        return true;
    }

    public static bool CheckIfCriteriaIsMet(Dictionary<int, int> criteria)
    {
        if (criteria[1] == 0 && criteria[7] == 0 && criteria[10] == 0) return true;
        return false;
    }
}