namespace DAA_VRP
{
    public class GreedySolution : Solution
    {
        public GreedySolution(string problemId, int numberOfClients, int totalDistance, long elapsedMilliseconds) { 
            this.problemId = problemId;
            this.numberOfclients = numberOfClients;
            this.totalDistance = totalDistance;
            this.elapsedMilliseconds = elapsedMilliseconds;
        }
    }
}
