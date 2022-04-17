namespace DAA_VRP
{
    public class VRP
    {

        string sourceFilename = "";
        int numberOfClients = -1;
        int numberOfVehicles = -1;
        int totalDistance = -1;
        List<List<int>> distanceMatrix = new List<List<int>>();

        public VRP(string filename)
        {
            BuildFromFile(filename);
        }

        private int RetrieveNumberOfClients(string line)
        {
            string[] split = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            return int.Parse(split[1]);

        }

        private int RetrieveNumberOfVehicles(string line)
        {
            string[] split = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            return int.Parse(split[1]);
        }

        private void buildDistanceMatrix(List<string> lines)
        {
            for (int i = 0; i < numberOfClients; i++)
            {
                List<int> row = new List<int>();
                for (int j = 0; j < numberOfClients; j++)
                {
                    row.Add(int.Parse(lines[i].Split(new char[0], StringSplitOptions.RemoveEmptyEntries)[j]));
                }
                distanceMatrix.Add(row);
            }
        }

        public void BuildFromFile(string filename)
        {
            sourceFilename = filename;
            List<string> lines = new List<string>(File.ReadAllLines(filename));
            this.numberOfClients = RetrieveNumberOfClients(lines[0]);
            this.numberOfVehicles = RetrieveNumberOfVehicles(lines[1]);
            buildDistanceMatrix(lines.GetRange(3, lines.Count - 4));

        }

        private List<int> MakeRCL(HashSet<int> availableNodes, int currentNode, int rclSize = 1)
        {

            List<int> rcl = Enumerable.Repeat(availableNodes.ToList()[0], rclSize).ToList();
            List<int> distance = distanceMatrix[currentNode];

            foreach (int node in availableNodes)
            {
                int candidate = node;
                int currentMinDistance = distance[candidate];

                if (currentMinDistance < distance[rcl[rcl.Count - 1]])
                {
                    for (int i = 0; i < rcl.Count; i++)
                    {
                        if (currentMinDistance < distance[rcl[i]])
                        {
                            currentMinDistance = distance[rcl[i]];

                            int temp = rcl[i];
                            rcl[i] = candidate;
                            candidate = temp;
                        }
                    }
                }
            }

            return rcl;
        }

        private List<List<int>> GreedyWithRCL(int rclSize)
        {
            HashSet<int> availableNodes = new HashSet<int>(Enumerable.Range(1, numberOfClients - 1).ToList());
            List<List<int>> paths = new List<List<int>>();

            totalDistance = 0;

            for (int i = 0; i < numberOfVehicles; i++)
            {
                paths.Add(new List<int>());
                paths[i].Add(0);
            }

            int numberOfPaths = numberOfVehicles;
            while (availableNodes.Count > 0)
            {
                if (availableNodes.Count < numberOfPaths)
                {
                    numberOfPaths = availableNodes.Count;
                }
                for (int i = 0; i < numberOfPaths; i++)
                {
                    int lastNode = paths[i][paths[i].Count - 1];

                    List<int> rcl = MakeRCL(availableNodes, lastNode, rclSize);
                    int randomNode = rcl[new Random().Next(rcl.Count)];

                    paths[i].Add(randomNode);
                    availableNodes.Remove(randomNode);

                    totalDistance += distanceMatrix[lastNode][randomNode];
                }
            }

            for (int i = 0; i < numberOfVehicles; i++)
            {
                int lastNode = paths[i][paths[i].Count - 1];
                paths[i].Add(0);
                totalDistance += distanceMatrix[lastNode][0];
            }

            return paths;
        }


        public GreedySolution SolveGreedy()
        {
            List<List<int>> paths = GreedyWithRCL(1);

            return new GreedySolution(sourceFilename, numberOfClients, totalDistance, -1, paths);
        }

        public GraspSolution GraspConstructivePhase(int rclSize)
        {
            List<List<int>> paths = GreedyWithRCL(rclSize);

            GraspSolution solution = new GraspSolution(sourceFilename, numberOfClients, rclSize);
            solution.paths = paths;
            solution.totalDistance = totalDistance;
            return solution;

        }

        public GraspSolution SolveGrasp()
        {
            throw new NotImplementedException();
        }

    }
}
