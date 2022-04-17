using System.IO;
namespace DAA_VRP
{
    class Program
    {
        public static void Main(string[] args)
        {
            string path;
            if (args.Length > 1)
            {
                path = args[0];
            }
            else
            {
                path = "C:\\Users\\enriq\\source\\repos\\P07_DAA_VRP\\DAA_VRP\\DAA_VRP\\Input_files\\";
            }

            foreach (string filename in Directory.EnumerateFiles(path, "*.txt"))
            {
                VRP vrp = new VRP(filename);
                GreedySolution greedySolution = vrp.SolveGreedy();
                Console.WriteLine(greedySolution.GetInfo());

            }

        }
    }
}

