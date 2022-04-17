namespace DAA_VRP
{
    public class VRP
    {

        string sourceFilename = "";
        int numberOfClients = -1;
        int numberOfVehicles = -1;
        int totalDistance = 0;
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


        private int FindMinDistanceIndex(int currentNode, HashSet<int> availableNodes)
        {
            int minCost = int.MaxValue;
            int minNode = -1;
            foreach (int node in availableNodes)
            {
                if (distanceMatrix[currentNode][node] < minCost)
                {
                    minCost = distanceMatrix[currentNode][node];
                    minNode = node;
                }
            }
            return minNode;
        }

        public GreedySolution SolveGreedy()
        {
            HashSet<int> nodes = new HashSet<int>(Enumerable.Range(1, numberOfClients - 1).ToList());
            List<List<int>> paths = new List<List<int>>();

            for (int i = 0; i < numberOfVehicles; i++)
            {
                paths.Add(new List<int>());
                paths[i].Add(0);
            }

            int numberOfPaths = numberOfVehicles;
            while (nodes.Count > 0)
            {
                if (nodes.Count < numberOfPaths)
                {
                    numberOfPaths = nodes.Count;
                }
                for (int i = 0; i < numberOfPaths; i++)
                {
                    int lastNode = paths[i][paths[i].Count - 1];
                    int closestNode = FindMinDistanceIndex(lastNode, nodes);

                    paths[i].Add(closestNode);
                    nodes.Remove(closestNode);

                    totalDistance += distanceMatrix[lastNode][closestNode];
                }
            }

            for (int i = 0; i < numberOfVehicles; i++)
            {

                int lastNode = paths[i][paths[i].Count - 1];
                paths[i].Add(0);
                totalDistance += distanceMatrix[lastNode][0];
            }


            return new GreedySolution(sourceFilename, numberOfClients, totalDistance, -1, paths);
        }


        private List<int> MakeRCL(HashSet<int> availableNodes, int currentNode, int rclSize = 1)
        {
            List<int> rcl = Enumerable.Repeat(int.MaxValue, rclSize).ToList();
            List<int> distance = distanceMatrix[currentNode];

            foreach (int candidate in availableNodes)
            {
                int currentMinDistance = distance[candidate];
                for(int i =0; i < rcl.Count; i++)
                {
                    if()
                }
            }

            return rcl;
        }

        private GraspSolution GraspConstructivePhase(int rclSize)
        {
            GraspSolution solution = new GraspSolution(sourceFilename, numberOfClients, rclSize);
            HashSet<int> nodes = new HashSet<int>(Enumerable.Range(1, numberOfClients - 1).ToList());
            List<List<int>> paths = new List<List<int>>();

            for (int i = 0; i < numberOfVehicles; i++)
            {
                paths.Add(new List<int>());
                paths[i].Add(0);
            }

            int numberOfPaths = numberOfVehicles;
            while (nodes.Count > 0)
            {
                if (nodes.Count < numberOfPaths)
                {
                    numberOfPaths = nodes.Count;
                }
                for (int i = 0; i < numberOfPaths; i++)
                {
                    int lastNode = paths[i][paths[i].Count - 1];
                    int closestNode = FindMinDistanceIndex(lastNode, nodes);

                    paths[i].Add(closestNode);
                    nodes.Remove(closestNode);

                    totalDistance += distanceMatrix[lastNode][closestNode];
                }
            }

            for (int i = 0; i < numberOfVehicles; i++)
            {
                int lastNode = paths[i][paths[i].Count - 1];
                paths[i].Add(0);
                totalDistance += distanceMatrix[lastNode][0];
            }


            return solution;

        }

        public GraspSolution SolveGrasp()
        {
            throw new NotImplementedException();
        }

    }
}
