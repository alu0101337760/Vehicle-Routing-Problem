namespace DAA_VRP
{
    public class GreedySolution : Solution
    {
        public GreedySolution(string problemId, int numberOfClients, int totalDistance, long elapsedMilliseconds)
        {
            string[] splittedProblemId = problemId.Split('\\');
            this.problemId = splittedProblemId[splittedProblemId.Length - 1];
            this.numberOfClients = numberOfClients;
            this.totalDistance = totalDistance;
            this.elapsedMilliseconds = elapsedMilliseconds;
        }

        public override string GetInfoString()
        {
            return String.Format("{0,4} {1,4} {2,4} {3,4}", problemId, numberOfClients, totalDistance, elapsedMilliseconds);
        }

     

    }
}
