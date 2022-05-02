namespace DAA_VRP
{
    public class MultiInsertion : ILocalSearch
    {
        public Solution Search(Problem problem, Solution solution)
        {
            List<List<int>> distanceMatrix = problem.distanceMatrix;
            int numberOfNodes = problem.numberOfClients;
            Solution bestSolution = solution;

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

        public Solution Shake(Problem problem, Solution solution)
        {
            Random rnd = new Random();
            int pathToRemove = rnd.Next(0, solution.paths.Count - 1);
            int random = rnd.Next(0, solution.paths.Count - 1);
            int pathToInsert = random == pathToRemove ? pathToRemove + 1 : random;
            pathToInsert = pathToInsert == solution.paths.Count ? 0 : pathToInsert;
            int indexToRemove = rnd.Next(1, solution.paths[pathToRemove].Count - 2);
            int positionToInsert = rnd.Next(1, solution.paths[pathToInsert].Count - 2);
            int nodeToInsert = solution.paths[pathToRemove][indexToRemove];

            List<List<int>> distanceMatrix = problem.distanceMatrix;
            List<int> originPath = solution.paths[pathToRemove];
            List<int> destinationPath = solution.paths[pathToInsert];


            int distanceAfterRemoving = solution.totalDistance -
                       distanceMatrix[originPath[indexToRemove]][originPath[indexToRemove + 1]] -
                       distanceMatrix[originPath[indexToRemove - 1]][originPath[indexToRemove]] +
                       distanceMatrix[originPath[indexToRemove - 1]][originPath[indexToRemove + 1]];

            int minDistance = distanceAfterRemoving +
                                distanceMatrix[originPath[indexToRemove]][destinationPath[positionToInsert]] +
                                distanceMatrix[destinationPath[positionToInsert - 1]][originPath[indexToRemove]] -
                                distanceMatrix[destinationPath[positionToInsert - 1]][destinationPath[positionToInsert]];

            GvnsSolution newSolution = new GvnsSolution(solution.problemId, solution.numberOfClients, solution.GetRclSize());
            newSolution.SetPaths(solution.paths);
            newSolution.totalDistance = solution.totalDistance;
            
            newSolution.paths[pathToRemove].RemoveAt(indexToRemove);
            newSolution.paths[pathToInsert].Insert(positionToInsert, nodeToInsert);
            newSolution.totalDistance = minDistance;
            return newSolution;
        }
    }
}
