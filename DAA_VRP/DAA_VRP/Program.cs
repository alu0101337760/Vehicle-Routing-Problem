
namespace DAA_VRP
{
    class Program
    {
        public static void Main()
        {
            // Create a new instance of the problem.
            VRP vrp = new VRP();

            string testFile = "C:\\Users\\enriq\\source\\repos\\P07_DAA_VRP\\DAA_VRP\\DAA_VRP\\I40j_2m_S1_1.txt";

            vrp.BuildFromFile(testFile);

        }
    }
}

