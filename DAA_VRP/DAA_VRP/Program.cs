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
            int RCL_SIZE = 5;
            GraspTypes DEFAULT_TYPE = GraspTypes.GRASP_REINSERTION_INTRA;
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
            List<GraspSolution> graspSolutions = new List<GraspSolution>();

            foreach (string filename in Directory.EnumerateFiles(path, "*.txt"))
            {
                Problem problem = new Problem(filename);
                VRP vrp = new VRP(problem);
                greedySolutions.Add(vrp.SolveGreedy());
                GraspSolution solution = vrp.SolveGrasp(RCL_SIZE, DEFAULT_TYPE);                
                graspSolutions.Add(solution);
            }

   

            PrintGreedySolutions(greedySolutions);

            foreach (GreedySolution solution in greedySolutions)
            {

                Console.WriteLine(solution.totalDistance);
                Console.WriteLine(solution.GetPathsString());
            }

            foreach (GraspSolution solution in graspSolutions)
            {
                Console.WriteLine(solution.totalDistance);
                Console.WriteLine(solution.GetPathsString());                
            }

        }
    }
}

