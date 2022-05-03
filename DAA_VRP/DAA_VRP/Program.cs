namespace DAA_VRP
{
    class Program
    {

        /// <summary>
        /// Helper function to print the greedy solutions
        /// </summary>
        public static void PrintGreedySolutions(List<GreedySolution> greedySolutions)
        {
            Console.WriteLine(String.Format("\tfilename\tnodes\tcost\tmilliseconds"));
            foreach (GreedySolution solution in greedySolutions)
            {
                Console.WriteLine(solution.problemId + "\t" + solution.numberOfClients + "\t" + solution.totalDistance + "\t" + solution.elapsedMilliseconds);
            }
            Console.WriteLine("\n");
        }

        public static void PrintGvnsSolutions(List<GvnsSolution> GvnsSolutions)
        {
            Console.WriteLine(String.Format("\tfilename\tnodes\tcost\tmilliseconds"));
            foreach (GvnsSolution solution in GvnsSolutions)
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

            ///////////////// MODIFICIACION //////////////////
            List<GvnsSolution> sequentialGvnsSolutions = new List<GvnsSolution>();
            List<GvnsSolution> sequentialGvnsSolutionsAlternativeOrder = new List<GvnsSolution>();            
            //////////////////////////////////////////////////
            
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


                ///////////////// MODIFICIACION //////////////////
                sequentialGvnsSolutions.Add(vrp.SolveGvns(RCL_SIZE, GvnsTypes.SEQUENTIAL_VND_1));
                sequentialGvnsSolutionsAlternativeOrder.Add(vrp.SolveGvns(RCL_SIZE, GvnsTypes.SEQUENTIAL_VND_2));
                //////////////////////////////////////////////////
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

            Console.WriteLine("Gvns solutions:");
            PrintGvnsSolutions(gvnsSolutions);


            Console.WriteLine("Gvns solutions with sequential VND:");
            PrintGvnsSolutions(sequentialGvnsSolutions);


            Console.WriteLine("Gvns solutions with sequential and an alternative order:");
            PrintGvnsSolutions(sequentialGvnsSolutions);


        }

        //public static void Main(string[] args)
        //{

        //    int RCL_SIZE = 2;
        //    string path;
        //    if (args.Length > 1)
        //    {
        //        path = args[0];
        //    }
        //    else
        //    {
        //        path = "C:\\Users\\enriq\\source\\repos\\P07_DAA_VRP\\DAA_VRP\\DAA_VRP\\Input_files\\";
        //    }

        //    List<Problem> problems = new List<Problem>();

        //    List<GreedySolution> bestGreedySolutions = new List<GreedySolution>();
        //    List<GraspSolution> bestMultiInsertSolutions = new List<GraspSolution>();
        //    List<GraspSolution> bestSingleInsertionSolutions = new List<GraspSolution>();
        //    List<GraspSolution> bestSingleSwapSolutions = new List<GraspSolution>();
        //    List<GraspSolution> bestMultiSwapSolutions = new List<GraspSolution>();
        //    List<GvnsSolution> bestGvnsSolutions = new List<GvnsSolution>();

        //    foreach (string filename in Directory.EnumerateFiles(path, "*.txt"))
        //    {
        //        problems.Add(new Problem(filename));
        //        VRP vrp = new VRP(problems.Last());
        //        bestGreedySolutions.Add(vrp.SolveGreedy());

        //        bestSingleInsertionSolutions.Add(vrp.SolveGrasp(RCL_SIZE, GraspTypes.GRASP_SINGLE_ROUTE_INSERTION));
        //        bestMultiInsertSolutions.Add(vrp.SolveGrasp(RCL_SIZE, GraspTypes.GRASP_MULTI_ROUTE_INSERTION));
        //        bestSingleSwapSolutions.Add(vrp.SolveGrasp(RCL_SIZE, GraspTypes.GRASP_SINGLE_ROUTE_SWAP));
        //        bestMultiSwapSolutions.Add(vrp.SolveGrasp(RCL_SIZE, GraspTypes.GRASP_MULTI_ROUTE_SWAP));
        //        bestGvnsSolutions.Add(vrp.SolveGvns(RCL_SIZE));
        //    }

        //    for (int it = 0; it < 100; it++)
        //    {
        //        for (int i = 0; i < problems.Count; i++)
        //        {
        //            Problem problem = problems[i];
        //            VRP vrp = new VRP(problem);
        //            GreedySolution currentGreedy = vrp.SolveGreedy();
        //            if (currentGreedy.totalDistance < bestGreedySolutions[i].totalDistance)
        //            {
        //                bestGreedySolutions[i] = currentGreedy;
        //            }
        //            GraspSolution currentGraspSolution = vrp.SolveGrasp(RCL_SIZE, GraspTypes.GRASP_SINGLE_ROUTE_INSERTION);
        //            if (currentGraspSolution.totalDistance < bestSingleInsertionSolutions[i].totalDistance)
        //            {
        //                bestSingleInsertionSolutions[i] = currentGraspSolution;
        //            }

        //            GraspSolution currentMultiIterationSolution = vrp.SolveGrasp(RCL_SIZE, GraspTypes.GRASP_MULTI_ROUTE_INSERTION);
        //            if (currentMultiIterationSolution.totalDistance < bestMultiInsertSolutions[i].totalDistance)
        //            {
        //                bestMultiInsertSolutions[i] = currentMultiIterationSolution;
        //            }

        //            GraspSolution currentSingleSwapSolution = vrp.SolveGrasp(RCL_SIZE, GraspTypes.GRASP_SINGLE_ROUTE_SWAP);
        //            if (currentSingleSwapSolution.totalDistance < bestSingleSwapSolutions[i].totalDistance)
        //            {
        //                bestSingleSwapSolutions[i] = currentSingleSwapSolution;
        //            }

        //            GraspSolution currentMultiSwapSolution = vrp.SolveGrasp(RCL_SIZE, GraspTypes.GRASP_MULTI_ROUTE_SWAP);
        //            if (currentMultiSwapSolution.totalDistance < bestMultiSwapSolutions[i].totalDistance)
        //            {
        //                bestMultiSwapSolutions[i] = currentMultiSwapSolution;
        //            }

        //            GvnsSolution curretGvnsSolution = vrp.SolveGvns(RCL_SIZE);
        //            if (curretGvnsSolution.totalDistance < bestGvnsSolutions[i].totalDistance)
        //            {
        //                bestGvnsSolutions[i] = curretGvnsSolution;
        //            }

        //        }
        //    }

        //    Console.WriteLine("Greedy solutions:");
        //    PrintGreedySolutions(bestGreedySolutions);
        //    Console.WriteLine("Is validated: " + ValidateGreedySolutions(problems, bestGreedySolutions));

        //    Console.WriteLine("Multi route Insertion solutions:");
        //    PrintGraspSolutions(bestMultiInsertSolutions);
        //    Console.WriteLine("Is validated: " + ValidateGraspSolutions(problems, bestMultiInsertSolutions));

        //    Console.WriteLine("Single route Insertion solutions:");
        //    PrintGraspSolutions(bestSingleInsertionSolutions);
        //    Console.WriteLine("Is validated: " + ValidateGraspSolutions(problems, bestSingleInsertionSolutions));

        //    Console.WriteLine("Single swap Insertion solutions:");
        //    PrintGraspSolutions(bestSingleSwapSolutions);
        //    Console.WriteLine("Is validated: " + ValidateGraspSolutions(problems, bestSingleSwapSolutions));

        //    Console.WriteLine("Multi swap Insertion solutions:");
        //    PrintGraspSolutions(bestMultiSwapSolutions);
        //    Console.WriteLine("Is validated: " + ValidateGraspSolutions(problems, bestMultiSwapSolutions));

        //    Console.WriteLine("Gvns solutions:");
        //    PrintGvnsSolutions(bestGvnsSolutions);
        //    Console.WriteLine("Is validated: " + ValidateGvnsSolutions(problems, bestGvnsSolutions));
        //}

        /// <summary>
        /// Helper function to calculate the total distance of a given path.
        /// </summary>
        public static bool ValidateGreedySolutions(List<Problem> problems, List<GreedySolution> solutions)
        {
            foreach (Problem p in problems)
            {
                List<List<int>> distanceMatrix = p.distanceMatrix;
                foreach (Solution solution in solutions)
                {
                    List<List<int>> paths = solution.paths;
                    int distance = 0;
                    for (int i = 0; i < paths.Count; i++)
                    {
                        for (int j = 0; j < paths[i].Count - 1; j++)
                        {
                            distance += p.distanceMatrix[paths[i][j]][paths[i][j + 1]];
                        }
                    }
                    if (distance != solution.totalDistance)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Helper function to calculate the total distance of a given path.
        /// </summary>
        public static bool ValidateGraspSolutions(List<Problem> problems, List<GraspSolution> solutions)
        {
            foreach (Problem p in problems)
            {
                List<List<int>> distanceMatrix = p.distanceMatrix;
                foreach (Solution solution in solutions)
                {
                    List<List<int>> paths = solution.paths;
                    int distance = 0;
                    for (int i = 0; i < paths.Count; i++)
                    {
                        for (int j = 0; j < paths[i].Count - 1; j++)
                        {
                            distance += p.distanceMatrix[paths[i][j]][paths[i][j + 1]];
                        }
                    }
                    if (distance != solution.totalDistance)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Helper function to calculate the total distance of a given path.
        /// </summary>
        public static bool ValidateGvnsSolutions(List<Problem> problems, List<GvnsSolution> solutions)
        {
            foreach (Problem p in problems)
            {
                List<List<int>> distanceMatrix = p.distanceMatrix;
                foreach (Solution solution in solutions)
                {
                    List<List<int>> paths = solution.paths;
                    int distance = 0;
                    for (int i = 0; i < paths.Count; i++)
                    {
                        for (int j = 0; j < paths[i].Count - 1; j++)
                        {
                            distance += p.distanceMatrix[paths[i][j]][paths[i][j + 1]];
                        }
                    }
                    if (distance != solution.totalDistance)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }



}

