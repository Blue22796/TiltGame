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
        public Board parent;
        public String move;
        int n;

        public Board(char[][] state)
        {
            this.state = state;
            n = state.Length;
        }
       
        
        public Board(String state) { 
            //Convert String to 2d array
        }
        //Modify array when tilting the board in different directions
        #region Tilting Logic
        public Board TiltRight() {
            char[][] NewState = (char[][]) state.Clone();
            var toGo = (i: -1, j: -1);
            bool found = false;
            for (int i = 0; i < n; i++)
            {
                 found  = false;
                for (int j=n-1;j>= 0; j--)
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
            var Result = new Board(NewState);
            Result.parent = this;
            Result.move = "Right";
            return Result;
        }
        public Board TiltDown() {
            char[][] NewState = (char[][])state.Clone();
            for (int i = n - 1; i >= 0; i--)
            {
                for (int j = 0; j < n; j++)
                {
                    if (NewState[i][j] == 'o')
                    {
                        int toGo = i;
                        while (toGo < n - 1 && NewState[toGo + 1][j] == '.')
                        {
                            toGo++;
                        }
                        if (toGo != i)
                        {
                            NewState[toGo][j] = 'o';
                            NewState[i][j] = '.';
                        }
                    }
                }
            }
            var Result = new Board(NewState);
            Result.parent = this;
            Result.move = "Down";
            return Result;
        }
        public Board TiltUp() {
            char[][] NewState = (char[][])state.Clone();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (NewState[i][j] == 'o')
                    {
                        int toGo = i;
                        while (toGo > 0 && NewState[toGo - 1][j] == '.')
                        {
                            toGo--;
                        }
                        if (toGo != i)
                        {
                            NewState[toGo][j] = 'o';
                            NewState[i][j] = '.';
                        }
                    }
                }
            }
            var Result = new Board(NewState);
            Result.parent = this;
            Result.move = "Up";
            return Result;
        }
        public Board TiltLeft() {
            char[][] NewState = (char[][])state.Clone();
            var toGo = (i: -1, j: -1);
            bool found = false;
            for (int i = 0; i < n; i++)
            {
                found = false;
                for (int j = 0; j < n; j++)
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
            var Result = new Board(NewState);
            Result.parent = this;
            Result.move = "Left";
            return Result;
        }
        #endregion
        

        public String toString() 
        {
            char[] holder = new char[n * n];
            int i = 0;
            foreach (var a in state)
                foreach (char c in a)
                    holder[i++] = c;
            return holder.ToString();
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
            InitializeStates();
            Board current = new Board(initialBoard);
            Queue<Board> state_queue = new Queue<Board>();
            HashSet<Board> visited = new HashSet<Board>();
            while (!IsWinning(current)){
                visited.Add(current);
                Board[] successors = new Board[] { 
                    current.TiltUp(), current.TiltDown(), 
                    current.TiltLeft(), current.TiltRight()};

                foreach(Board successor in successors)
                    if (!visited.Contains(successor))
                        state_queue.Enqueue(successor);
                current = state_queue.Dequeue();
            }
            if (current == null) {
                Write(null, "Unsolvable", null);
                return null;
            }
            Stack<Board> sequence = new Stack<Board>();
            while (current.parent != null)
            {
                sequence.Push(current);
                current = current.parent;
            }
            List<Board> states = new List<Board>();
            states[0] = sequence.Pop();
            String[] moves = new string[sequence.Count];
            int c = 0;
            while (sequence.Count > 0)
            {
                states.Add(sequence.Pop());
                moves[c++] = states.Last().move; 
            }
            Write(states, "Solvable", moves);
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
            var board1 = new Board(state1);
            var state2_2 = board1.TiltDown();
            int n = state2.Length;
            for(int i = 0; i<n; i++) {
                for (int j = 0; j < n; j++)
                    if (state2[i][j] != state2_2.state[i][j]) 
                        throw new Exception("Tilt down failed.");
                    
            }
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

        public List<Board> InitializeStates(int x)
        {
            // Initialize a list to store Board objects representing the states
            List<Board> States = new List<Board>();

            // Initialize the first state
            Board state1 = new Board(new char[][]
            {
            new char[] { '#', '#', '.', '.', '.' },
            new char[] { '.', 'o', '#', '.', '.' },
            new char[] { '.', '.', 'o', '.', '.' },
            new char[] { '.', '.', '.', '.', '.' },
            new char[] { '#', '#', '#', '.', '.' }
            });

            // Create a Board object for the first state and add it to the list
            States.Add(state1);

            // Initialize the second state
            Board state2 = state1.TiltDown();

            // Create a Board object for the second state and add it to the list
            States.Add(state2);

            // Initialize the third state
            Board state3 = state2.TiltRight();

            // Create a Board object for the third state and add it to the list
            States.Add(state3);

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

                    if(solveState == "Unsolvable")
                    {
                        return;
                    }
                    writer.WriteLine("Min number of moves: " + (moves.Length - 1 ));


                    writer.WriteLine("Sequence of moves: " + string.Join(", ", moves.Skip(1)) + ",");


                    for (int stateIndex = 0; stateIndex < States.Count; stateIndex++)
                    {
                        writer.WriteLine(moves[stateIndex]);
                        char[][] currentState = States[stateIndex].state;
                        for (int i = 0; i < currentState.Length; i++)
                        {
                            for (int j = 0; j < currentState[i].Length; j++)
                            {
                                if(j != currentState[i].Length-1)
                                {
                                    writer.Write(currentState[i][j] + ", ");
                                }
                                else
                                {
                                    writer.Write(currentState[i][j] );
                                }
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
            return board.state[TargetX][TargetY] == 'o';
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
