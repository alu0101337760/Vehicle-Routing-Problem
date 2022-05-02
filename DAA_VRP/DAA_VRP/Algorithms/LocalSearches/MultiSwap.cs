namespace DAA_VRP
{
    internal class MultiSwap : ILocalSearch
    {


        /// <summary>
        /// Implements the multi route swap local search
        /// </summary>
        /// <param name="solution">The current solution to explore</param>
        public Solution Search(Problem problem, Solution solution)
        {
            List<List<int>> distanceMatrix = problem.distanceMatrix;
            int numberOfNodes = problem.numberOfClients;
            Solution bestSolution = solution;

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

        public Solution Shake(Problem problem, Solution solution)
        {
            Random rnd = new Random();
            int pathA = rnd.Next(0, solution.paths.Count - 1);
            int random = rnd.Next(0, solution.paths.Count - 1);
            int pathB = random == pathA ? pathA + 1 : random;
            pathB = pathB == solution.paths.Count ? 0 : pathB;
            int indexA = rnd.Next(1, solution.paths[pathA].Count - 2);
            int indexB = rnd.Next(1, solution.paths[pathA].Count - 2);

            List<List<int>> distanceMatrix = problem.distanceMatrix;
            List<int> originPath = solution.paths[pathA];
            List<int> destinationPath = solution.paths[pathB];

            int distanceAfterRemoving = solution.totalDistance -
                             distanceMatrix[originPath[indexA]][originPath[indexA + 1]] -
                             distanceMatrix[originPath[indexA - 1]][originPath[indexA]];

            int minDistance = distanceAfterRemoving -
                                        distanceMatrix[destinationPath[indexB - 1]][destinationPath[indexB]] -
                                        distanceMatrix[destinationPath[indexB]][destinationPath[indexB + 1]] +

                                        distanceMatrix[originPath[indexA - 1]][destinationPath[indexB]] +
                                        distanceMatrix[destinationPath[indexB]][originPath[indexA + 1]] +

                                         distanceMatrix[destinationPath[indexB - 1]][originPath[indexA]] +
                                         distanceMatrix[originPath[indexA]][destinationPath[indexB + 1]];

            GvnsSolution newSolution = new GvnsSolution(solution.problemId, solution.numberOfClients, solution.GetRclSize());
            newSolution.SetPaths(solution.paths);
            newSolution.totalDistance = solution.totalDistance;          

            newSolution.totalDistance = minDistance;
            int temp = newSolution.paths[pathA][indexA];
            newSolution.paths[pathA][indexA] = newSolution.paths[pathB][indexB];
            newSolution.paths[pathB][indexB] = temp;
            return newSolution;

        }
    }
    }
