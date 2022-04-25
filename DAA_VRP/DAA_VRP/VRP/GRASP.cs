using System.Diagnostics;

namespace DAA_VRP
{
    /// <summary>
    /// GraspTypes represent different local searches of the grasp algorithm.
    /// </summary>
    public enum GraspTypes
    {
        GRASP_MULTI_ROUTE_INSERTION,
        GRASP_SINGLE_ROUTE_INSERTION,
        GRASP_SINGLE_ROUTE_SWAP,
        GRASP_MULTI_ROUTE_SWAP,
        GRASP_2_OPT
    };

    /// <summary>
    /// GRASP class implements the GRASP algorithm.
    /// </summary>
    public class GRASP
    {
        Problem problem;
        int numberOfNodes = -1;
        int totalDistance = -1;
        List<List<int>> distanceMatrix = new List<List<int>>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="problem">problem to solve with the current instance</param>
        public GRASP(Problem problem)
        {
            this.numberOfNodes = problem.numberOfClients;
            this.distanceMatrix = problem.distanceMatrix;
            this.problem = problem;
        }

        /// <summary>
        /// Helper function for the single path insertion local search
        /// </summary>
        /// <param name="solution">the current solution</param>
        /// <param name="index">the index of the path to insert in</param>
        private GraspSolution InsertSinglePath(GraspSolution solution, int index)
        {
            List<int> path = solution.paths[index];
            int indexToRemove = 0;
            int positionToInsert = 0;
            int minDistance = solution.totalDistance;

            for (int originIndex = 1; originIndex < path.Count - 1; originIndex++)
            {
                int distanceAfterRemoving = solution.totalDistance -
                    distanceMatrix[path[originIndex]][path[originIndex + 1]] -
                    distanceMatrix[path[originIndex - 1]][path[originIndex]] +
                    distanceMatrix[path[originIndex - 1]][path[originIndex + 1]];

                for (int destinationIndex = 1; destinationIndex < path.Count - 1; destinationIndex++)
                {
                    if (destinationIndex == originIndex || destinationIndex + 1 == originIndex) { continue; }

                    int candidateDistance = int.MaxValue;

                    if (destinationIndex > originIndex)
                    {
                        candidateDistance = distanceAfterRemoving +
                            distanceMatrix[path[destinationIndex]][path[originIndex]] +
                            distanceMatrix[path[originIndex]][path[destinationIndex + 1]] -
                            distanceMatrix[path[destinationIndex]][path[destinationIndex + 1]];
                    }
                    else
                    {
                        candidateDistance = distanceAfterRemoving +
                            distanceMatrix[path[originIndex]][path[destinationIndex]] +
                            distanceMatrix[path[destinationIndex - 1]][path[originIndex]] -
                            distanceMatrix[path[destinationIndex - 1]][path[destinationIndex]];
                    }

                    if (candidateDistance < minDistance)
                    {
                        minDistance = candidateDistance;
                        indexToRemove = originIndex;
                        positionToInsert = destinationIndex;
                    }
                }
            }

            int nodeToInsert = path[indexToRemove];

            GraspSolution newSolution = new GraspSolution(solution.problemId, numberOfNodes, solution.rclSize);
            newSolution.SetPaths(solution.paths);
            newSolution.totalDistance = minDistance;
            newSolution.paths[index].RemoveAt(indexToRemove);

            newSolution.paths[index].Insert(positionToInsert, nodeToInsert);
            return newSolution;
        }

        /// <summary>
        /// Implements the single route insertion local search
        /// </summary>
        /// <param name="solution">The current solution to explore</param>
        private GraspSolution GraspSingleRouteInsertion(GraspSolution solution)
        {
            int MAX_ITERATIONS_WITHOUT_IMPROVEMENT = 100;
            List<int> pathsToCheck = Enumerable.Range(0, solution.paths.Count).ToList();
            int it = 0;

            GraspSolution bestSolution = solution;

            while (pathsToCheck.Count > 0 && it < MAX_ITERATIONS_WITHOUT_IMPROVEMENT)
            {
                for (int i = 0; i < pathsToCheck.Count; i++)
                {
                    GraspSolution currentSolution = InsertSinglePath(bestSolution, pathsToCheck[i]);

                    if (currentSolution.totalDistance < bestSolution.totalDistance)
                    {
                        bestSolution = currentSolution;
                        it = 0;
                        // Búsqueda ansiosa:  pathsToCheck = Enumerable.Range(0, solution.paths.Count).ToList();
                    }
                    else
                    {
                        pathsToCheck.RemoveAt(i);
                    }
                }
                it++;
            }

            return bestSolution;
        }


        /// <summary>
        /// Implements the multi route insertion local search
        /// </summary>
        /// <param name="solution">The current solution to explore</param>
        private GraspSolution GraspMultiRouteInsertion(GraspSolution solution)
        {
            GraspSolution bestSolution = solution;

            int pathToRemove = 0;
            int indexToRemove = 0;
            int pathToInsert = 0;
            int positionToInsert = 0;
            int nodeToInsert = 0;
            int minDistance = solution.totalDistance;

            bool foundSolution = true;
            while (foundSolution)
            {
                foundSolution = false;
                for (int currentRoute = 0; currentRoute < bestSolution.paths.Count; currentRoute++)
                {
                    List<int> originPath = bestSolution.paths[currentRoute];
                    for (int originIndex = 1; originIndex < originPath.Count - 1; originIndex++)
                    {
                        int distanceAfterRemoving = bestSolution.totalDistance -
                        distanceMatrix[originPath[originIndex]][originPath[originIndex + 1]] -
                        distanceMatrix[originPath[originIndex - 1]][originPath[originIndex]] +
                        distanceMatrix[originPath[originIndex - 1]][originPath[originIndex + 1]];

                        for (int destinationRoute = 0; destinationRoute < bestSolution.paths.Count; destinationRoute++)
                        {

                            if (destinationRoute == currentRoute || bestSolution.paths[destinationRoute].Count + 1 > (numberOfNodes / bestSolution.paths.Count) + (numberOfNodes * 0.1))
                            { continue; }
                            List<int> destinationPath = bestSolution.paths[destinationRoute];
                            for (int candidateIndex = 1; candidateIndex < destinationPath.Count - 1; candidateIndex++)
                            {
                                int candidateDistance = distanceAfterRemoving +
                                    distanceMatrix[originPath[originIndex]][destinationPath[candidateIndex]] +
                                    distanceMatrix[destinationPath[candidateIndex - 1]][originPath[originIndex]] -
                                    distanceMatrix[destinationPath[candidateIndex - 1]][destinationPath[candidateIndex]];

                                if (candidateDistance < minDistance)
                                {
                                    indexToRemove = originIndex;
                                    positionToInsert = candidateIndex;
                                    nodeToInsert = originPath[originIndex];
                                    minDistance = candidateDistance;
                                    pathToRemove = currentRoute;
                                    pathToInsert = destinationRoute;
                                    foundSolution = true;
                                }
                            }
                        }
                    }
                }
                if (foundSolution)
                {
                    bestSolution.paths[pathToRemove].RemoveAt(indexToRemove);
                    bestSolution.paths[pathToInsert].Insert(positionToInsert, nodeToInsert);
                    bestSolution.totalDistance = minDistance;
                }
            }
            return bestSolution;

        }

        /// <summary>
        /// Helper function for the single path swap local search
        /// </summary>
        /// <param name="solution">the current solution</param>
        /// <param name="index">the index of the path to insert in</param>
        private GraspSolution SwapSinglePath(GraspSolution solution, int index)
        {
            List<int> path = solution.paths[index];
            int originIndex = 0;
            int destinationIndex = 0;
            int minDistance = solution.totalDistance;

            for (int currentIndex = 1; currentIndex < path.Count - 1; currentIndex++)
            {
                int distanceAfterRemoving = solution.totalDistance -
                    distanceMatrix[path[currentIndex]][path[currentIndex + 1]] -
                    distanceMatrix[path[currentIndex - 1]][path[currentIndex]];

                for (int candidateIndex = currentIndex + 2; candidateIndex < path.Count - 1; candidateIndex++)
                {
                    int candidateDistance = distanceAfterRemoving -
                        distanceMatrix[path[candidateIndex - 1]][path[candidateIndex]] -
                        distanceMatrix[path[candidateIndex]][path[candidateIndex + 1]] +

                        distanceMatrix[path[currentIndex - 1]][path[candidateIndex]] +
                        distanceMatrix[path[candidateIndex]][path[currentIndex + 1]] +

                        distanceMatrix[path[candidateIndex - 1]][path[currentIndex]] +
                        distanceMatrix[path[currentIndex]][path[candidateIndex + 1]];

                    if (candidateDistance < minDistance)
                    {
                        minDistance = candidateDistance;
                        originIndex = currentIndex;
                        destinationIndex = candidateIndex;
                    }
                }
            }

            int originNode = path[originIndex];
            int destinationNode = path[destinationIndex];
            GraspSolution newSolution = new GraspSolution(solution.problemId, numberOfNodes, solution.rclSize);
            newSolution.SetPaths(solution.paths);
            newSolution.totalDistance = minDistance;
            newSolution.paths[index][originIndex] = destinationNode;
            newSolution.paths[index][destinationIndex] = originNode;
            return newSolution;
        }

        /// <summary>
        /// Implements the single route swap local search
        /// </summary>
        /// <param name="solution">The current solution to explore</param>
        private GraspSolution GraspSingleRouteSwap(GraspSolution solution)
        {
            int MAX_ITERATIONS_WITHOUT_IMPROVEMENT = 100;
            List<int> pathsToCheck = Enumerable.Range(0, solution.paths.Count).ToList();
            int it = 0;

            GraspSolution bestSolution = solution;

            while (pathsToCheck.Count > 0 && it < MAX_ITERATIONS_WITHOUT_IMPROVEMENT)
            {
                for (int i = 0; i < pathsToCheck.Count; i++)
                {
                    GraspSolution currentSolution = SwapSinglePath(bestSolution, pathsToCheck[i]);
           
                    if (currentSolution.totalDistance < bestSolution.totalDistance)
                    {
                        bestSolution = currentSolution;
                        it = 0;
                        // Búsqueda ansiosa:  pathsToCheck = Enumerable.Range(0, solution.paths.Count).ToList();
                    }
                    else
                    {
                        pathsToCheck.RemoveAt(i);
                    }
                }
                it++;
            }
            return bestSolution;
        }

        /// <summary>
        /// Implements the multi route swap local search
        /// </summary>
        /// <param name="solution">The current solution to explore</param>
        private GraspSolution GraspMultiRouteSwap(GraspSolution solution)
        {

            GraspSolution bestSolution = solution;

            int pathA = 0;
            int pathB = 0;
            int nodeA = 0;
            int nodeB = 0;
            int minDistance = solution.totalDistance;


            bool foundSolution = false;
            while (foundSolution)
            {
                foundSolution = false;
                for (int currentRoute = 0; currentRoute < bestSolution.paths.Count; currentRoute++)
                {
                    List<int> currentPath = bestSolution.paths[currentRoute];
                    for (int currentIndex = 1; currentIndex < currentPath.Count - 1; currentIndex++)
                    {
                        int distanceAfterRemoving = bestSolution.totalDistance -
                            distanceMatrix[currentPath[currentIndex]][currentPath[currentIndex + 1]] -
                            distanceMatrix[currentPath[currentIndex - 1]][currentPath[currentIndex]];

                        for (int destinationRoute = currentRoute; destinationRoute < bestSolution.paths.Count; destinationRoute++)
                        {
                            List<int> destinationPath = bestSolution.paths[destinationRoute];
                            int initializeValue = 1;

                            if (destinationRoute == currentRoute)
                            {
                                initializeValue = currentIndex + 2;
                            }

                            for (int candidatePosition = initializeValue; candidatePosition < destinationPath.Count - 1; candidatePosition++)
                            {
                                int candidateDistance = distanceAfterRemoving -
                                    distanceMatrix[destinationPath[candidatePosition - 1]][destinationPath[candidatePosition]] -
                                    distanceMatrix[destinationPath[candidatePosition]][destinationPath[candidatePosition + 1]] +

                                    distanceMatrix[currentPath[currentIndex - 1]][destinationPath[candidatePosition]] +
                                    distanceMatrix[destinationPath[candidatePosition]][currentPath[currentIndex + 1]] +

                                     distanceMatrix[destinationPath[candidatePosition - 1]][currentPath[currentIndex]] +
                                     distanceMatrix[currentPath[currentIndex]][destinationPath[candidatePosition + 1]];


                                if (candidateDistance < bestSolution.totalDistance)
                                {
                                    pathA = currentRoute;
                                    pathB = destinationRoute;
                                    nodeA = currentIndex;
                                    nodeB = candidatePosition;
                                    minDistance = candidateDistance;
                                    foundSolution = true;
                                }
                            }
                        }
                    }
                }
                if (foundSolution)
                {
                    bestSolution.totalDistance = minDistance;
                    int temp = bestSolution.paths[pathA][nodeA];
                    bestSolution.paths[pathA][nodeA] = bestSolution.paths[pathB][nodeB];
                    bestSolution.paths[pathB][nodeB] = temp;
                }
            }

            return bestSolution;
        }

        /// <summary>
        /// Local search implements different local search algorithms for the problem.
        /// </summary>
        /// <param name="solution">Initial solution</param>
        /// <param name="type">The type of local search to use, its default to GRASP_SINGLE_ROUTE_SWAP</param>
        private GraspSolution LocalSearch(GraspSolution solution, GraspTypes type = GraspTypes.GRASP_SINGLE_ROUTE_SWAP)
        {
            switch (type)
            {
                case GraspTypes.GRASP_2_OPT:
                    break;

                case GraspTypes.GRASP_SINGLE_ROUTE_SWAP:
                    return GraspSingleRouteSwap(solution);

                case GraspTypes.GRASP_MULTI_ROUTE_SWAP:
                    return GraspMultiRouteSwap(solution);

                case GraspTypes.GRASP_SINGLE_ROUTE_INSERTION:
                    return GraspSingleRouteInsertion(solution);

                case GraspTypes.GRASP_MULTI_ROUTE_INSERTION:
                    return GraspMultiRouteInsertion(solution);
            }
            return solution;
        }

        /// <summary>
        /// Utility function, for the constructive part of the 
        /// GRASP algorithm
        /// </summary>
        /// <param name="rclSize">size of the RCL of the algorithm</param>
        private GraspSolution GraspConstructivePhase(int rclSize)
        {
            GreedyRCL greedy = new GreedyRCL(problem);

            GreedySolution greedySolution = greedy.Solve(rclSize);

            GraspSolution solution = new GraspSolution(problem.sourceFilename, numberOfNodes, rclSize);
            solution.SetPaths(greedySolution.paths);
            solution.totalDistance = totalDistance;
            solution.totalDistance = greedySolution.totalDistance;
            return solution;
        }

        /// <summary>
        /// Solves the problem with the GRASP algorithm
        /// </summary>
        /// <param name="rclSize">specifies the size of the rcl for the constructive part of the GRASP</param>
        /// <param name="type">A GraspType value for selecting which GRASP local search to use</param>
        public GraspSolution Solve(int rclSize, GraspTypes type)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            GraspSolution bestSolution = new GraspSolution(problem.sourceFilename, numberOfNodes, rclSize);
            bestSolution.totalDistance = int.MaxValue;
            for (int i = 0; i < 2000; i++)
            {
                GraspSolution candidate = GraspConstructivePhase(rclSize);
                GraspSolution processed = LocalSearch(candidate, type);
                if (processed.totalDistance < bestSolution.totalDistance)
                {
                    bestSolution = processed;
                    i = 0;
                }
            }
            sw.Stop();
            bestSolution.elapsedMilliseconds = sw.ElapsedMilliseconds;
            return bestSolution;
        }
    }
}
