namespace DAA_VRP
{
    public class GreedySolution : Solution
    {
        public GreedySolution(string problemId, int numberOfClients, int totalDistance, long elapsedMilliseconds, List<List<int>> paths) { 
            this.problemId = problemId;
            this.numberOfclients = numberOfClients;
            this.totalDistance = totalDistance;
            this.elapsedMilliseconds = elapsedMilliseconds;
            this.paths = paths; 
        }
    }
}
