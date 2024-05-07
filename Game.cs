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
        public String asString;
        int hash = 0;

        int n;

        public Board(char[][] state)
        {
            this.state = state;
            n = state.Length;
        }

        //Modify array when tilting the board in different directions
        #region Tilting Logic

        public char[][] cpy(char[][] orig)
        {
            char[][] new_state = new char[n][];
            for(int i = 0; i < n;i++)
                new_state[i] = (char[])orig[i].Clone();
            return new_state;
        }

        public Board TiltRight()
        {
            if (move == "Right")
                return this;
            char[][] NewState = cpy(state);
            var toGo = (i: -1, j: -1);
            bool found = false;
            
            for (int i = 0; i < n; i++)
            {
                found = false;
                for (int j = n - 1; j >= 0; j--)
                {
                    if (NewState[i][j] == '.' && found == false)
                    {
                        toGo = (i: i, j: j);
                        found = true;
                    }
                    else if (NewState[i][j] == '#')
                        found = false;
                    else if (NewState[i][j] == 'o')
                    {
                        if (found == true)
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
        public Board TiltDown()
        {
            if (move == "Down")
                return this;
            char[][] NewState = cpy(state);
            for (int i = 0; i < n - 1; i++)
            {
                int cnt = 0;
                for (int j = 0; j <= n; j++)
                    if (j==n || NewState[j][i] == '#')
                    {
                        for (int k = j - 1; cnt > 0; k--, cnt--)
                        {
                            NewState[k][i] = 'o';
                        }
                    }
                    else if (NewState[j][i] == 'o')
                    {
                        cnt++;
                        NewState[j][i] = '.';
                    }
          
            }
            
            var Result = new Board(NewState);
            Result.parent = this;
            Result.move = "Down";
            return Result;
        }
        public Board TiltUp()
        {
            if (move == "Up")
                return this;
            char[][] NewState = cpy(state);
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
        public Board TiltLeft()
        {
            if (move == "Left")
                return this;
            char[][] NewState = cpy(state);
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
                    else if (NewState[i][j] == 'o')
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
            if (asString == null)
            {
                char[] holder = new char[n * n];
                int i = 0;
                foreach (var a in state)
                    foreach (char c in a)
                        holder[i++] = c;
                asString = new string(holder);
            }
            return asString;
        }


        #region Ignore
        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;
            if(!obj.GetType().Equals(this.GetType()))
                return false;
            var brd = (Board)obj;
            return this.toString().Equals(brd.toString());
        }

        public override int GetHashCode()
        {
            if(hash == 0)
                hash = toString().GetHashCode();
            return hash;
        }

        public void write()
        {
            Console.WriteLine("-------------------------");
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine(" ");
                for (int j = 0; j < n; j++)
                {
                    Console.Write(state[i][j] + " ");
                }
            }
        }

        #endregion
    }



    public class Player
    {
        System.Diagnostics.Stopwatch StopWatch = new System.Diagnostics.Stopwatch();
        int TargetX;
        int TargetY;
        int num;
        List<Board> Answer(Board current) { 
            StopWatch.Stop();
            Console.WriteLine("Elapsed time: " + StopWatch.ElapsedMilliseconds / 1000);

            List<Board> sequence = new List<Board>();
            sequence.Add(current);
            while (current.parent!=null)
            {
                current = current.parent;
                sequence.Add(current);
            }
            sequence.Reverse();
            StopWatch.Stop();
            Console.WriteLine(StopWatch.ElapsedMilliseconds / 1000);

            Write(sequence, "Solvable", num);
            return sequence;
        }

        public List<Board> Solve(char[][] initialBoard, int targetX, int targetY, int num)
        {
            StopWatch.Start();

            //Generate Sequence starting with initialBoard
            Board current = new Board(initialBoard);
            Queue<Board> state_queue = new Queue<Board>();
            HashSet<Board> visited = new HashSet<Board>();
            this.TargetX = targetX;
            this.TargetY = targetY;
            this.num = num;
            if (IsWinning(current))
                return Answer(current);
            visited.Add(current);
            while (true)
            {
                Board[] successors = new Board[4] {
                current.TiltUp(),
                current.TiltDown(),
                current.TiltLeft(),
                current.TiltRight()
                };

                foreach (Board successor in successors)
                    if (!visited.Contains(successor))
                    {
                        if (IsWinning(successor))
                            return Answer(successor);
                        visited.Add (successor);
                        state_queue.Enqueue(successor);
                    }
                if (state_queue.Count == 0)
                    break;
                current = state_queue.Dequeue();
            }

            StopWatch.Stop();
            Console.WriteLine("Elapsed time: " + StopWatch.ElapsedMilliseconds / 1000);

            Write(null, "Unsolvable", 0);
            return null;
        }

        public void Write(List<Board> States, string solveState, int num)
        {
            

            try
            {

                string filePath = string.Format("output{0}.txt", num);


                using (StreamWriter writer = new StreamWriter(filePath))
                {

                    writer.WriteLine(solveState);
                    if (solveState == "Unsolvable")
                        return;

                    writer.WriteLine("Min number of moves: " + (States.Count - 1));

                    String[] seq = States.Skip(1).Select(b => b.move).ToArray();
                    writer.WriteLine("Sequence of moves: " + string.Join(',',seq));


                    foreach(Board state in States)
                    {
                        writer.WriteLine(state.move==null?"Initial":state.move);
                        char[][] currentState = state.state;
                        for (int i = 0; i < currentState.Length; i++)
                        {
                            for (int j = 0; j < currentState[i].Length; j++)
                            {
                                if (j != currentState[i].Length - 1)
                                {
                                    writer.Write(currentState[i][j] + ", ");
                                }
                                else
                                {
                                    writer.Write(currentState[i][j]);
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

        public Boolean IsWinning(Board board)
        {
            //Check if the current configuration is winning
            if (board.state[TargetY][TargetX] == 'o')
                return true;
            return false;
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
