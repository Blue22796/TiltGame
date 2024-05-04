using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tilt_Game
{
    public class Board
    {

        public char[][] state;
        int n;
        int x;
        int y;

        public Board(char[][] state)
        {
            this.state = state;
        }
       
        
        public Board(String state) { 
            //Convert String to 2d array
        }
        //Modify array when tilting the board in different directions
        #region Tilting Logic
        public Board TiltRight() {
            char[][] NewState = (char[][]) state.Clone();
            int n= NewState.GetLength(0);
            int m= NewState.GetLength(1);
            var toGo = (i: -1, j: -1);
            bool found = false;
            for (int i = 0; i < n; i++)
            {
                 found  = false;
                for (int j=m-1;j>= 0; j--)
                {
                    if (NewState[i][j] == '.' && found == false)
                    {
                        toGo = (i: i, j: j);
                        found = true;
                    }
                    else if (NewState[i][j] == '#')
                        found = false;
                    else if (NewState[i][j] =='o')
                    {
                        if (found==true)
                        {
                            NewState[toGo.i][toGo.j] = 'o';
                            NewState[i][j] = '.';
                            toGo.j--;

                        }
                        
                    }
                }

            }
            return new Board(NewState);
        }
        public Board TilitDown() {
            char[][] NewState = (char[][])state.Clone();
            int n = NewState.GetLength(0);
            int m = NewState.GetLength(1);
            var toGo = (i: -1, j: -1);
            bool found = false;
            for (int i = n - 1; i >= 0; i--)
            {
                found = false;
                for (int j = 0; j < m; j++)
                {
                    if (NewState[i][j] == '.' && found == false)
                    {
                        toGo = (i: i, j: j);
                        found = true;
                    }
                    else if (NewState[i][j] == '#')
                    {
                        found = false;
                    }
                    else if (NewState[i][j] == 'o')
                    {
                        if (found == true)
                        {
                            NewState[toGo.i][toGo.j] = 'o';
                            NewState[i][j] = '.';
                            toGo.i++;
                        }
                    }
                }
            }
            return new Board(NewState);
        }
        public Board TiltUp() {
            char[][] NewState = (char[][])state.Clone();
            int n = NewState.GetLength(0);
            int m = NewState.GetLength(1);
            var toGo = (i: -1, j: -1);
            bool found = false;
            for (int i = 0; i < n; i++)
            {
                found = false;
                for (int j = 0; j < m; j++)
                {
                    if (NewState[i][j] == '.' && found == false)
                    {
                        toGo = (i: i, j: j);
                        found = true;
                    }
                    else if (NewState[i][j] == '#')
                    {
                        found = false;
                    }
                    else if (NewState[i][j] == 'o')
                    {
                        if (found == true)
                        {
                            NewState[toGo.i][toGo.j] = 'o';
                            NewState[i][j] = '.';
                            toGo.i--;
                        }
                    }
                }
            }
            return new Board(NewState);
        }
        public Board TiltLeft() {
            char[][] NewState = (char[][])state.Clone();
            int n = NewState.GetLength(0);
            int m = NewState.GetLength(1);
            var toGo = (i: -1, j: -1);
            bool found = false;
            for (int i = 0; i < n; i++)
            {
                found = false;
                for (int j = 0; j < m; j++)
                {
                    if (NewState[i][j] == '.' && found == false)
                    {
                        toGo = (i: i, j: j);
                        found = true;
                    }
                    else if (NewState[i][j] == '#')
                        found = false;
                    else if(NewState[i][j] == 'o')
                    {
                        if (found == true)
                        {
                            NewState[toGo.i][toGo.j] = 'o';
                            NewState[i][j] = '.';
                            toGo.j++;

                        }

                    }
                }

            }
            return new Board(NewState);
        }
        #endregion
        

        public String toString() 
        {
            //Returns String representation of board
            return null;
        }
       
        #region Ignore
        public override bool Equals(object? obj)
        {
            return this.toString().Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.toString().GetHashCode();
        }
        #endregion
    }



    public class Player 
    {
        List<Board> Sequence = new List<Board>();
        int TargetX;
        int TargetY;
        public List<Board> Solve(char[][] initialBoard, int targetX, int targetY)
        {
            //Generate Sequence starting with initialBoard
            string[] moves = new string[] { "right", "left", "up"};
            Write(InitializeStates(), "Solvable", moves);
            return Sequence;
        }
        //for testing write method 
        public List<Board> InitializeStates()
        {
            // Initialize a list to store Board objects representing the states
            List<Board> States = new List<Board>();

            // Initialize the first state
            char[][] state1 = new char[][]
            {
            new char[] { '#', '#', '.', '.', '.' },
            new char[] { '.', 'o', '#', '.', '.' },
            new char[] { '.', '.', 'o', '.', '.' },
            new char[] { '.', '.', '.', '.', '.' },
            new char[] { '#', '#', '#', '.', '.' }
            };

            // Create a Board object for the first state and add it to the list
            States.Add(new Board(state1));

            // Initialize the second state
            char[][] state2 = new char[][]
            {
            new char[] { '#', '#', '.', '.', '.' },
            new char[] { '.', '.', '#', '.', '.' },
            new char[] { '.', '.', '.', '.', '.' },
            new char[] { '.', 'o', 'o', '.', '.' },
            new char[] { '#', '#', '#', '.', '.' }
            };

            // Create a Board object for the second state and add it to the list
            States.Add(new Board(state2));

            // Initialize the third state
            char[][] state3 = new char[][]
            {
            new char[] { '#', '#', '.', '.', '.' },
            new char[] { '.', '.', '#', '.', '.' },
            new char[] { '.', '.', '.', '.', '.' },
            new char[] { '.', '.', '.', 'o', 'o' },
            new char[] { '#', '#', '#', '.', '.' }
            };

            // Create a Board object for the third state and add it to the list
            States.Add(new Board(state3));

            return States;
        }

        public void Write(List<Board> States, string solveState, string[] moves)
        {
            try
            {
                
                string filePath = "output.txt";


                using (StreamWriter writer = new StreamWriter(filePath))
                {

                    writer.WriteLine(solveState);


                    writer.WriteLine("Min number of moves: " + moves.Length);


                    writer.WriteLine("Sequence of moves: " + string.Join(", ", moves));


                    for (int stateIndex = 0; stateIndex < States.Count; stateIndex++)
                    {
                        writer.WriteLine("State " + (stateIndex + 1) + ":");
                        char[][] currentState = States[stateIndex].state;
                        for (int i = 0; i < currentState.Length; i++)
                        {
                            for (int j = 0; j < currentState[i].Length; j++)
                            {
                                writer.Write(currentState[i][j] + " ");
                            }
                            writer.WriteLine();
                        }
                        writer.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("An error occurred while writing to the file: " + ex.Message);
            }
        }

        public Boolean IsWinning(Board board) {
            //Check if the current configuration is winning
            return true;
        }
    }

    public class Cameraman 
    { 
        //public GUIComponent Representation(Board board)
        //{
        //Generate a grid GUI Component representing the current board configuration
        //}
    }
    
}
