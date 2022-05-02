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
            Console.WriteLine("Grasp Solutions:");
            Console.WriteLine(String.Format("\tfilename\tnodes\tRCL\tcost\tmilliseconds"));
            foreach (GraspSolution solution in greedySolutions)
            {
                Console.WriteLine(solution.problemId + "\t" + solution.numberOfClients + "\t" +
                                  solution.rclSize + "\t" + solution.totalDistance + "\t" + solution.elapsedMilliseconds);
            }
            Console.WriteLine("\n");
        }

        public static void PrintPaths(List<Solution> solutions)
        {
            foreach (Solution solution in solutions)
            {
                Console.WriteLine("Total cost: " + solution.totalDistance);
                Console.WriteLine(solution.GetPathsString());
            }
        }

        /// <summary>
        /// Main entry of the program
        /// </summary>
        public static void Main(string[] args)
        {
            int RCL_SIZE = 2;
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
            List<GraspSolution> multiInsertSolutions = new List<GraspSolution>();
            List<GraspSolution> singleInsertionSolutions = new List<GraspSolution>();
            List<GraspSolution> singleSwapSolutions = new List<GraspSolution>();
            List<GraspSolution> multiSwapSolutions = new List<GraspSolution>();
            List<GvnsSolution> gvnsSolutions = new List<GvnsSolution>();


            foreach (string filename in Directory.EnumerateFiles(path, "*.txt"))
            {
                Problem problem = new Problem(filename);
                VRP vrp = new VRP(problem);
                greedySolutions.Add(vrp.SolveGreedy());

                singleInsertionSolutions.Add(vrp.SolveGrasp(RCL_SIZE, GraspTypes.GRASP_SINGLE_ROUTE_INSERTION));
                multiInsertSolutions.Add(vrp.SolveGrasp(RCL_SIZE, GraspTypes.GRASP_MULTI_ROUTE_INSERTION));
                singleSwapSolutions.Add(vrp.SolveGrasp(RCL_SIZE, GraspTypes.GRASP_SINGLE_ROUTE_SWAP));
                multiSwapSolutions.Add(vrp.SolveGrasp(RCL_SIZE, GraspTypes.GRASP_MULTI_ROUTE_SWAP));
                gvnsSolutions.Add(vrp.SolveGvns(RCL_SIZE));
                Console.WriteLine("Real gvns cost" + vrp.CalculateDistance(gvnsSolutions.Last().paths));
            }

            PrintGreedySolutions(greedySolutions);
            
            Console.WriteLine("Multi route Insertion solutions:");
            PrintGraspSolutions(multiInsertSolutions);

            Console.WriteLine("Single route Insertion solutions:");
            PrintGraspSolutions(singleInsertionSolutions);

            Console.WriteLine("Single swap Insertion solutions:");
            PrintGraspSolutions(singleSwapSolutions);

            Console.WriteLine("Multi swap Insertion solutions:");
            PrintGraspSolutions(multiSwapSolutions);

        }
    }
}

