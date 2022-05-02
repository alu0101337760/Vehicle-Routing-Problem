namespace DAA_VRP
{
    public class MultiInsertion : ILocalSearch
    {
        public Solution Search(Problem problem, Solution solution, bool shaking = false)
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
    }
}
