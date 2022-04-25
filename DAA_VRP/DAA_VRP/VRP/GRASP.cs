
using System.Diagnostics;
namespace DAA_VRP
{
    public enum GraspTypes
    {
        GRASP_REINSERTION_INTRA,
        GRASP_REINSERTION_INTER,
        GRASP_SINGLE_ROUTE_SWAP,
        GRASP_MULTI_ROUTE_SWAP,
        GRASP_2_OPT
    };

    public class GRASP
    {
        Problem problem;
        int numberOfNodes = -1;
        int totalDistance = -1;
        int numberOfPaths = -1;
        List<List<int>> distanceMatrix = new List<List<int>>();

        public GRASP(Problem problem)
        {
            this.numberOfNodes = problem.numberOfClients;
            this.distanceMatrix = problem.distanceMatrix;
            this.problem = problem;
            this.numberOfPaths = problem.numberOfVehicles;
        }

        private GraspSolution ReinsertIntraPath(GraspSolution solution, int index)
        {
            List<int> path = solution.paths[index];
            int indexToRemove = 0;
            int positionToInsert = 0;
            int minDistance = solution.totalDistance;

            for (int currentIndex = 1; currentIndex < path.Count - 1; currentIndex++)
            {
                int distanceAfterRemoving = solution.totalDistance -
                    distanceMatrix[path[currentIndex]][path[currentIndex + 1]] -
                    distanceMatrix[path[currentIndex - 1]][path[currentIndex]] +
                    distanceMatrix[path[currentIndex - 1]][path[currentIndex + 1]];

                for (int candidatePosition = currentIndex + 1; candidatePosition < path.Count - 1; candidatePosition++)
                {
                    if (candidatePosition == currentIndex || candidatePosition + 1 == currentIndex) { continue; }


                    int nextCandidate = candidatePosition + 1;
                    int candidateDistance = distanceAfterRemoving +
                        distanceMatrix[path[candidatePosition]][path[currentIndex]] +
                        distanceMatrix[path[currentIndex]][path[nextCandidate]] -
                        distanceMatrix[path[candidatePosition]][path[nextCandidate]];


                    if (candidateDistance < minDistance)
                    {
                        minDistance = candidateDistance;
                        indexToRemove = currentIndex;
                        positionToInsert = candidatePosition;
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

        private GraspSolution GraspReinsertionIntra(GraspSolution solution)
        {
            int MAX_ITERATIONS = 2000;
            List<int> pathsToCheck = Enumerable.Range(0, solution.paths.Count).ToList();
            int it = 0;

            GraspSolution bestSolution = solution;

            while (pathsToCheck.Count > 0 && it < MAX_ITERATIONS)
            {
                for (int i = 0; i < pathsToCheck.Count; i++)
                {
                    GraspSolution currentSolution = ReinsertIntraPath(bestSolution, pathsToCheck[i]);
                    if (currentSolution.totalDistance == bestSolution.totalDistance)
                    {
                        pathsToCheck.RemoveAt(i);
                    }
                    if (currentSolution.totalDistance < bestSolution.totalDistance)
                    {
                        bestSolution = currentSolution;
                        // Búsqueda ansiosa:  pathsToCheck = Enumerable.Range(0, solution.paths.Count).ToList();
                    }
                }
                it++;
            }

            return bestSolution;
        }

        private GraspSolution GraspReinsertionInter(GraspSolution solution)
        {
            GraspSolution bestSolution = solution;

            for (int currentRoute = 0; currentRoute < solution.paths.Count; currentRoute++)
            {
                List<int> originPath = solution.paths[currentRoute];
                for (int currentIndex = 1; currentIndex < solution.paths[currentRoute].Count - 1; currentIndex++)
                {
                    int distanceAfterRemoving = solution.totalDistance -
                    distanceMatrix[originPath[currentIndex]][originPath[currentIndex + 1]] -
                    distanceMatrix[originPath[currentIndex - 1]][originPath[currentIndex]] +
                    distanceMatrix[originPath[currentIndex - 1]][originPath[currentIndex + 1]];

                    for (int destinationRoute = 0; destinationRoute < solution.paths.Count; destinationRoute++)
                    {
                        List<int> destinationPath = solution.paths[destinationRoute];

                        for (int candidatePosition = 1; candidatePosition < destinationPath.Count - 1; candidatePosition++)
                        {
                            int nextCandidate = candidatePosition + 1;
                            int candidateDistance = distanceAfterRemoving +
                                distanceMatrix[destinationPath[candidatePosition]][originPath[currentIndex]] +
                                distanceMatrix[originPath[currentIndex]][destinationPath[nextCandidate]] -
                                distanceMatrix[destinationPath[candidatePosition]][destinationPath[nextCandidate]];

                            if (candidateDistance < bestSolution.totalDistance)
                            {
                                GraspSolution newSolution = new GraspSolution(solution.problemId, numberOfNodes, solution.rclSize);
                                newSolution.SetPaths(solution.paths);
                                newSolution.totalDistance = candidateDistance;
                                newSolution.paths[currentRoute].RemoveAt(currentIndex);
                                newSolution.paths[destinationRoute].Insert(candidatePosition, originPath[currentIndex]);
                                bestSolution = newSolution;
                            }
                        }
                    }

                }
            }

            return bestSolution;

        }

        private GraspSolution SingleRouteSwap(GraspSolution solution, int index)
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

        private GraspSolution GraspSingleRouteSwap(GraspSolution solution)
        {
            int MAX_ITERATIONS = 2000;
            List<int> pathsToCheck = Enumerable.Range(0, solution.paths.Count).ToList();
            int it = 0;

            GraspSolution bestSolution = solution;

            while (pathsToCheck.Count > 0 && it < MAX_ITERATIONS)
            {
                for (int i = 0; i < pathsToCheck.Count; i++)
                {
                    GraspSolution currentSolution = SingleRouteSwap(bestSolution, pathsToCheck[i]);
                    if (currentSolution.totalDistance == bestSolution.totalDistance)
                    {
                        pathsToCheck.RemoveAt(i);
                    }
                    if (currentSolution.totalDistance < bestSolution.totalDistance)
                    {
                        bestSolution = currentSolution;
                        // Búsqueda ansiosa:  pathsToCheck = Enumerable.Range(0, solution.paths.Count).ToList();
                    }
                }
                it++;
            }
            return bestSolution;
        }

        private GraspSolution GraspMultiRouteSwap(GraspSolution solution)
        {
            GraspSolution bestSolution = solution;
            for (int currentRoute = 0; currentRoute < solution.paths.Count; currentRoute++)
            {
                List<int> path = solution.paths[currentRoute];
                for (int currentIndex = 1; currentIndex < path.Count - 1; currentIndex++)
                {
                    int distanceAfterRemoving = solution.totalDistance -
                        distanceMatrix[path[currentIndex]][path[currentIndex + 1]] -
                        distanceMatrix[path[currentIndex - 1]][path[currentIndex]];

                    for (int destinationRoute = 0; destinationRoute < solution.paths.Count; destinationRoute++)
                    {
                        List<int> destinationPath = solution.paths[destinationRoute];
                        for (int candidatePosition = 1; candidatePosition < destinationPath.Count - 1; candidatePosition++)
                        {
                            int nextCandidate = candidatePosition + 1;
                            int candidateDistance = distanceAfterRemoving +
                                distanceMatrix[destinationPath[candidatePosition]][path[currentIndex]] +
                                distanceMatrix[path[currentIndex]][destinationPath[nextCandidate]] -
                                distanceMatrix[destinationPath[candidatePosition]][destinationPath[nextCandidate]];

                            if (candidateDistance < bestSolution.totalDistance)
                            {
                                GraspSolution newSolution = new GraspSolution(solution.problemId, numberOfNodes, solution.rclSize);
                                newSolution.SetPaths(solution.paths);
                                newSolution.totalDistance = candidateDistance;
                                newSolution.paths[currentRoute][currentIndex] = destinationPath[candidatePosition];
                                newSolution.paths[destinationRoute][candidatePosition] = path[currentIndex];
                                bestSolution = newSolution;
                            }
                        }
                    }
                }
            }

            return bestSolution;
        }

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

                case GraspTypes.GRASP_REINSERTION_INTER:
                    return GraspReinsertionInter(solution);

                case GraspTypes.GRASP_REINSERTION_INTRA:
                    return GraspReinsertionIntra(solution);


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
