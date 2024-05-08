using Tilt_Game;

char[][] State ;
int n;
int TargetX ;
int TargetY ;

//Read input from file into State

void read(int x = 1)
{
    try
    {
        
        string filePath = string.Format("./Case{0}.txt",x);

       
        string[] lines = File.ReadAllLines(filePath);

        int n = int.Parse(lines[0]);

        
        char[][] State = new char[n][];

        
        for (int i = 0; i < n; i++)
        {
            State[i] = lines[i + 1].Split(", ").Select(s => s[0]).ToArray();
        }

        
        string[] targetValues = lines[lines.Length - 1].Split(", ");
        TargetX = int.Parse(targetValues[0]);
        TargetY = int.Parse(targetValues[1]);

        

       
        Console.WriteLine("Value of n: " + n);
        Console.WriteLine("State:");
        if(n<20)
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                Console.Write(State[i][j] + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine("Targetn: " + TargetX);
        Console.WriteLine("TargetY: " + TargetY);

        var Player = new Tilt_Game.Player();
        Player.Solve(State, TargetX, TargetY, x);
    }
    catch (Exception ex)
    {
        // Handle any exceptions
        Console.WriteLine("An error occurred: " + ex.Message);
    }
}
for(int i = 1; i<=6; i++)
    read(i);





//Display output
