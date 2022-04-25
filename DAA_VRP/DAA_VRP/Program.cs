namespace DAA_VRP
{
    class Program
    {

        /// <summary>
        /// Helper function to print the greedy solutions
        /// </summary>
        public static void PrintGreedySolutions(List<GreedySolution> greedySolutions)
        {
            Console.WriteLine("Greedy Solutions:");
            Console.WriteLine(String.Format("\tfilename\tnodes\tcost\tmilliseconds"));
            foreach (GreedySolution solution in greedySolutions)
            {
                Console.WriteLine(solution.problemId + "\t" + solution.numberOfClients + "\t" + solution.totalDistance + "\t" + solution.elapsedMilliseconds);
            }
            Console.WriteLine("\n");
        }
        
        /// <summary>
        /// Helper function to print the grasp solutions
        /// </summary>
        public static void PrintGraspSolutions(List<GraspSolution> greedySolutions)
        {
            Console.WriteLine("Greedy Solutions:");
            Console.WriteLine(String.Format("\tfilename\tnodes\tRCL\tcost\tmilliseconds"));
            foreach (GraspSolution solution in greedySolutions)
            {
                Console.WriteLine(solution.problemId + "\t" + solution.numberOfClients + "\t" + 
                                  solution.rclSize + "\t" + solution.totalDistance + "\t" + solution.elapsedMilliseconds);
            }
            Console.WriteLine("\n");
        }

        /// <summary>
        /// Main entry of the program
        /// </summary>
        public static void Main(string[] args)
        {
            int RCL_SIZE = 2;
            GraspTypes DEFAULT_TYPE = GraspTypes.GRASP_MULTI_ROUTE_INSERTION;
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
                Console.WriteLine("expected cost: " + solution.totalDistance);
                Console.WriteLine("real cost: " + vrp.CalculateDistance(solution.paths));

                graspSolutions.Add(solution);

            }

            PrintGreedySolutions(greedySolutions);
            PrintGraspSolutions(graspSolutions);
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

