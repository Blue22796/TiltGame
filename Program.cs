char[][] State = new char[][]{ 
    new char[] { '#','#','.','.','.'}, 
    new char[] { '.','o','#','.','.'}, 
    new char[] { '.','.','o','.','.'}, 
    new char[] { '.','.','.','.','.'}, 
    new char[] { '#','#','#','.','.'} 
};
int TargetX = 4;
int TargetY = 3;

//Read input from file into State

var Player = new Tilt_Game.Player();
Player.Solve(State, TargetX, TargetY);

//Display output
