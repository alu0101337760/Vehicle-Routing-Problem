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

        public override string GetPathsString()
        {
            string output = "{ 0, ";

            for (int i = 1; i < this.paths.Count - 1; i++)
            {
                if (paths[i] == 0)
                {
                    output += "0 }\n{ 0, ";
                }
                else
                {
                    output += paths[i]+ ", ";
                }
            }
            output += "0 }\n";
            return output;
        }

    }
}
