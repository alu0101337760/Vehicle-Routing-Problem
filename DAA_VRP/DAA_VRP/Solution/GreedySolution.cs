namespace DAA_VRP
{
    public class GreedySolution : Solution
    {
        public GreedySolution(string problemId, int numberOfClients, int totalDistance, long elapsedMilliseconds, List<List<int>> paths)
        {
            this.problemId = problemId;
            this.numberOfClients = numberOfClients;
            this.totalDistance = totalDistance;
            this.elapsedMilliseconds = elapsedMilliseconds;
            this.paths = paths;
        }

        public override string GetInfo()
        {
            return problemId + "\t" + numberOfClients + "\t" + totalDistance + "\t" + elapsedMilliseconds;
        }

        public override string GetPaths()
        {
            string paths = "";
            foreach (List<int> path in this.paths)
            {
                paths += "{" + path[0];
                for (int i = 1; i < path.Count; i++)
                {
                    paths += "," + path[i];
                }
                paths += "}\n";
            }
            return paths;
        }
    }
}
