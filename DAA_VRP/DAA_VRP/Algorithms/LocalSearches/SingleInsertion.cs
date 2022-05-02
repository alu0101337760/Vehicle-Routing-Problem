namespace DAA_VRP
{
    internal class SingleInsertion : ILocalSearch
    {
        List<List<int>> distanceMatrix = new List<List<int>>();
        int numberOfNodes = -1;
        /// <summary>
        /// Helper function for the single path insertion local search
        /// </summary>
        /// <param name="solution">the current solution</param>
        /// <param name="index">the index of the path to insert in</param>
        private Solution InsertSinglePath(Solution solution, int index)
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

                    int candidateDistance;

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

            GraspSolution newSolution = new GraspSolution(solution.problemId, solution.numberOfClients, solution.GetRclSize());
            newSolution.SetPaths(solution.paths);
            newSolution.totalDistance = minDistance;
            newSolution.paths[index].RemoveAt(indexToRemove);

            newSolution.paths[index].Insert(positionToInsert, nodeToInsert);
            return newSolution;
        }

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
                    Solution currentSolution = InsertSinglePath(bestSolution, pathsToCheck[i]);

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

        public Solution Shake(Problem problem, Solution solution)
        {
            throw new NotImplementedException();
        }
    }
}
