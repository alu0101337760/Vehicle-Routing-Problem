namespace DAA_VRP
{
    class Program
    {
        public static void PrintGreedySolutions(List<GreedySolution> greedySolutions)
        {
            Console.WriteLine("Greedy Solutions:");
            Console.WriteLine(String.Format("\tfilename\tnodes\tcost\tmilliseconds"));
            foreach (GreedySolution solution in greedySolutions)
            {
                Console.WriteLine(solution.problemId + "\t" + solution.numberOfClients + "\t" + solution.totalDistance + "\t" + solution.elapsedMilliseconds);
            }
        }


        public static void Main(string[] args)
        {
            string path;
            if (args.Length > 1)
            {
                path = args[0];
            }
            else
            {
                path = "C:\\Users\\enriq\\source\\repos\\P07_DAA_VRP\\DAA_VRP\\DAA_VRP\\Input_files\\";
            }

            List<GreedySolution> greedySolutions = new List<GreedySolution>();

            foreach (string filename in Directory.EnumerateFiles(path, "*.txt"))
            {
                VRP vrp = new VRP(filename);
                greedySolutions.Add(vrp.SolveGreedy());
            }

            PrintGreedySolutions(greedySolutions);
        }
    }
}

