namespace DAA_VRP
{
    internal class SingleSwap : ILocalSearch
    {
        List<List<int>> distanceMatrix = new List<List<int>>();
        int numberOfNodes = -1;

        /// <summary>
        /// Helper function for the single path swap local search
        /// </summary>
        /// <param name="solution">the current solution</param>
        /// <param name="index">the index of the path to insert in</param>
        private GraspSolution SwapSinglePath(Solution solution, int index)
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
            GraspSolution newSolution = new GraspSolution(solution.problemId, numberOfNodes, solution.GetRclSize());
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
        public Solution Search(Problem problem, Solution solution)
        {
            this.distanceMatrix = problem.distanceMatrix;
            this.numberOfNodes = problem.numberOfClients;
            int MAX_ITERATIONS_WITHOUT_IMPROVEMENT = 100;
            List<int> pathsToCheck = Enumerable.Range(0, solution.paths.Count).ToList();
            int it = 0;

            Solution bestSolution = solution;

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

    }
}
