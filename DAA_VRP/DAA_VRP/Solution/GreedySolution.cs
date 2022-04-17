namespace DAA_VRP
{
    public class GreedySolution : Solution
    {
        public GreedySolution(string problemId, int numberOfClients, int totalDistance, long elapsedMilliseconds, List<List<int>> paths)
        {
            string[] splittedProblemId = problemId.Split('\\');
            this.problemId = splittedProblemId[splittedProblemId.Length - 1];
            this.numberOfClients = numberOfClients;
            this.totalDistance = totalDistance;
            this.elapsedMilliseconds = elapsedMilliseconds;
            this.paths = paths;
        }

        public override string GetInfo()
        {
            return String.Format("{0,4} {1,4} {2,4} {3,4}", problemId, numberOfClients, totalDistance, elapsedMilliseconds);
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
