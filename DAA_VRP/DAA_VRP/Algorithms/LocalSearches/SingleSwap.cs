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
            GraspSolution newSolution = new GraspSolution(solution.problemId, solution.numberOfClients, solution.GetRclSize());
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
                    Solution currentSolution = SwapSinglePath(bestSolution, pathsToCheck[i]);

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
            
            Random rnd = new Random();
            List<int> candidatePaths = new List<int>();

            for (int i = 0; i < solution.paths.Count; i++)
            {
                if (solution.paths[i].Count > 2)
                {
                    candidatePaths.Add(i);
                }
            }

            int pathToSwap = candidatePaths[rnd.Next(candidatePaths.Count)];


            int indexA = rnd.Next(1, solution.paths[pathToSwap].Count - 2);
            
            List<int> candidates = new List<int>();            
            for (int i = 1; i < solution.paths[pathToSwap].Count - 2; i++)
            {
                if (i != indexA - 1 && i != indexA + 1 && i != indexA)                
                {
                    candidates.Add(i);
                }
            }
            if(candidates.Count == 0)
            {
                return solution;
            }
            int indexB = candidates[rnd.Next(0, candidates.Count - 1)];
            this.distanceMatrix = problem.distanceMatrix;

            distanceMatrix = problem.distanceMatrix;
            List<int> path = solution.paths[pathToSwap];

            int distanceAfterRemoving = solution.totalDistance -
                  distanceMatrix[path[indexA]][path[indexA + 1]] -
                  distanceMatrix[path[indexA - 1]][path[indexA]];

            int minDistance = distanceAfterRemoving -
                distanceMatrix[path[indexB - 1]][path[indexB]] -
                distanceMatrix[path[indexB]][path[indexB + 1]] +

                distanceMatrix[path[indexA - 1]][path[indexB]] +
                distanceMatrix[path[indexB]][path[indexA + 1]] +

                distanceMatrix[path[indexB - 1]][path[indexA]] +
                distanceMatrix[path[indexA]][path[indexB + 1]];

            int originNode = path[indexA];
            int destinationNode = path[indexB];
            GvnsSolution newSolution = new GvnsSolution(solution.problemId, solution.numberOfClients, solution.GetRclSize());
            newSolution.SetPaths(solution.paths);
            newSolution.totalDistance = minDistance;
            newSolution.paths[pathToSwap][indexA] = destinationNode;
            newSolution.paths[pathToSwap][indexB] = originNode;

            if (newSolution.totalDistance != CalculateDistance(newSolution.paths))
            {
                throw new Exception("Distance is not equal");
            }

            return newSolution;
        }

        public int CalculateDistance(List<List<int>> paths)
        {
            int distance = 0;
            for (int i = 0; i < paths.Count; i++)
            {
                for (int j = 0; j < paths[i].Count - 1; j++)
                {
                    distance += distanceMatrix[paths[i][j]][paths[i][j + 1]];
                }
            }
            return distance;
        }

    }

}
