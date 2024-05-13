using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tilt_Game
{

    public class GoodSet
    {
        public HashSet<Board>[] Sets;
        public GoodSet()
        {
            Sets = new HashSet<Board>[667333];
            for (int i = 0; i < 667333; i++)
            {
                Sets[i] = new HashSet<Board>();
            }
        }

        public bool Contains(Board b)
        {
            int h2 = b.getHash2();
            bool contains = Sets[h2].Contains(b);
            return contains;
        }

        public int Count = 0;

        public void Add(Board b)
        {
            if (Contains(b)) return;
            Count++;
            int h2 = b.getHash2();
            Sets[h2].Add(b);
        }


    }

    public class Board
    {

        public char[][] state;
        public static char[,] layout;
        public Board parent;
        public String move;
        public SortedSet<int> pos;
        public int stps = 0;
        int hash = 0;
        int hash2 = 0;
        int hash3 = 0;
        int hash4 = 0;
        static int coeff = (int)(new Random().NextInt64() % 911) + 1;
        static int coeff2 = (int)(new Random().NextInt64() % 911) + 1;
        static int coeff3 = (int)(new Random().NextInt64() % 911) + 1;
        static int coeff4 = (int)(new Random().NextInt64() % 911) + 911;
        static int n;

        public Board(char[][] state)
        {
            this.state = state;
            n = state.Length;
            layout = new char[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (state[i][j] == '#')
                        layout[i, j] = '#';
                    else layout[i, j] = '.';


            pos = new SortedSet<int>();
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (state[i][j] == 'o')
                        pos.Add(i * n + j);

            CalcHashes();
        }
        public Board(SortedSet<int> pos)
        {
            this.pos = pos;
            CalcHashes();
        }
        public void initState()
        {
            state = new char[n][];
            for (int i = 0; i < n; i++)
            {
                state[i] = new char[n];
                for (int j = 0; j < n; j++)
                    state[i][j] = layout[i, j];
            }
            foreach (int i in pos)
                state[i / n][i % n] = 'o';
        }
        //Modify array when tilting the board in different directions
        #region Tilting Logic
        public Board TiltRight()
        {
            HashSet<int> targetRows = new HashSet<int>();

            foreach (int i in pos)
            {
                layout[i / n, i % n] = 'o';
                targetRows.Add(i / n);
            }
            var newPos = new SortedSet<int>();
            foreach (int i in targetRows)
            {
                int cnt = 0;
                for (int j = 0; j <= n; j++)
                {
                    if (j == n || layout[i, j] == '#')
                        for (; cnt > 0; cnt--)
                            newPos.Add(i * n + j - cnt);
                    else if (layout[i, j] == 'o')
                    {
                        layout[i, j] = '.';
                        cnt++;
                    }
                }

            }
            if (newPos.Count != pos.Count)
                Console.WriteLine(-1);
            var Result = new Board(newPos);
            Result.parent = this;
            Result.move = "Right";
            Result.stps = this.stps + 1;
            return Result;
        }
        public Board TiltDown()
        {
            HashSet<int> targetCols = new HashSet<int>();
            foreach (int i in pos)
            {
                targetCols.Add(i % n);
                layout[i / n, i % n] = 'o';
            }
            SortedSet<int> newPos = new SortedSet<int>();
            foreach (int i in targetCols)
            {
                int cnt = 0;
                for (int j = 0; j <= n; j++)
                    if (j == n || layout[j, i] == '#')
                    {
                        for (int k = j - 1; cnt > 0; k--, cnt--)
                        {
                            newPos.Add(k * n + i);
                        }
                    }
                    else if (layout[j, i] == 'o')
                    {
                        cnt++;
                        layout[j, i] = '.';
                    }

            }
            if (newPos.Count != pos.Count)
                Console.WriteLine(-1);
            foreach (int i in newPos)
                layout[i / n, i % n] = '.';
            var Result = new Board(newPos);
            Result.parent = this;
            Result.move = "Down";
            Result.stps = this.stps + 1;
            return Result;
        }
        public Board TiltUp()
        {
            HashSet<int> targetCols = new HashSet<int>();
            SortedSet<int> newPos = new SortedSet<int>();
            foreach (int i in pos)
            {
                layout[i / n, i % n] = 'o';
                targetCols.Add(i % n);
            }
            foreach (int i in targetCols)
            {
                int cnt = 0;
                for (int j = n - 1; j >= -1; j--)
                {
                    if (j == -1 || layout[j, i] == '#')
                        for (; cnt > 0; cnt--)
                            newPos.Add((j + cnt) * n + i);
                    else if (layout[j, i] == 'o')
                    {
                        cnt++;
                        layout[j, i] = '.';
                    }
                }
                if (cnt > 0)
                    Console.WriteLine(-1);
            }
            if (newPos.Count != pos.Count)
                Console.WriteLine(-1);
            foreach (int i in newPos)
                layout[i / n, i % n] = '.';
            var Result = new Board(newPos);
            Result.parent = this;
            Result.move = "Up";
            Result.stps = this.stps + 1;
            return Result;
        }
        public Board TiltLeft()
        {
            SortedSet<int> newPos = new SortedSet<int>();
            HashSet<int> targetRows = new HashSet<int>();
            foreach (int i in pos)
            {
                targetRows.Add(i / n);
                layout[i / n, i % n] = 'o';
            }
            foreach (int i in targetRows)
            {
                int cnt = 0;
                for (int j = n - 1; j >= -1; j--)
                {
                    if (j == -1 || layout[i, j] == '#')
                    {
                        for (; cnt > 0; cnt--)
                            newPos.Add(i * n + (j + cnt));
                    }
                    else if (layout[i, j] == 'o')
                    {
                        cnt++;
                        layout[i, j] = '.';
                    }
                }
            }

            if (newPos.Count != pos.Count)
                Console.WriteLine(-1);
            foreach (int i in newPos)
                layout[i / n, i % n] = '.';
            var Result = new Board(newPos);
            Result.parent = this;
            Result.move = "Left";
            Result.stps = this.stps + 1;
            return Result;
        }
        #endregion




        #region Ignore
        public override bool Equals(object? obj)
        {
            var other = obj as Board;
            if (hash3 != other.hash3)
                return false;
            return hash4 == other.hash4;

        }


        public override int GetHashCode()
        {
            return hash;
        }

        public int getHash2()
        {
            return hash2;
        }


        void CalcHashes()
        {
            foreach (int i in pos)
                hash = (hash * coeff + i) % 667333;
            foreach (int i in pos)
                hash2 = (hash2 * coeff2 + i) % 667333;
            foreach (int i in pos)
                hash3 = (hash3 * coeff3 + i) % 667333;
            foreach (int i in pos)
                hash4 = (hash4 * coeff4 + i) % 667333;
        }

        #endregion
    }



    public class Player
    {
        System.Diagnostics.Stopwatch StopWatch = new System.Diagnostics.Stopwatch();
        int TargetX;
        int TargetY;
        int num;
        int dim;
        List<Board> Answer(Board current)
        {
            StopWatch.Stop();
            Console.WriteLine("Elapsed time: " + StopWatch.ElapsedMilliseconds / 1000);

            List<Board> sequence = new List<Board>();
            sequence.Add(current);
            while (current.parent != null)
            {
                current = current.parent;
                sequence.Add(current);
            }
            sequence.Reverse();

            Write(sequence, "Solvable");
            StopWatch.Stop();
            Console.WriteLine(StopWatch.ElapsedMilliseconds / 1000);

            return sequence;
        }

        public List<Board> Solve(char[][] initialBoard, int targetX, int targetY, int num)
        {
            StopWatch.Start();

            //Generate Sequence starting with initialBoard
            Board current = new Board(initialBoard);
            dim = initialBoard.Length;
            Queue<Board> state_queue = new Queue<Board>();
            GoodSet visited = new GoodSet();
            this.TargetX = targetX;
            this.TargetY = targetY;
            this.num = num;
            if (IsWinning(current))
                return Answer(current);
            visited.Add(current);
            int x = 0;
            while (true)
            {
                Board[] successors;
                if (current.move == null)
                    successors = new Board[4] {
                current.TiltDown(),
                current.TiltUp(),
                current.TiltRight(),
                current.TiltLeft()
                };
                else if (current.move == "Up" || current.move == "Down")
                    successors = new Board[2] {
                        current.TiltRight(),
                        current.TiltLeft()
                    };
                else
                    successors = new Board[2] {
                        current.TiltUp(),
                        current.TiltDown()
                    };

                foreach (Board successor in successors)
                    if (!visited.Contains(successor))
                    {
                        if (IsWinning(successor))
                            return Answer(successor);
                        visited.Add(successor);
                        state_queue.Enqueue(successor);
                    }
                if (state_queue.Count == 0)
                    break;
                current = state_queue.Dequeue();
            }


            Write(null, "Unsolvable");
            StopWatch.Stop();
            Console.WriteLine("Elapsed time: " + StopWatch.ElapsedMilliseconds / 1000);
            return null;
        }

        public void Write(List<Board> States, string solveState)
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
                    writer.WriteLine("Sequence of moves: " + string.Join(',', seq));

                    for (int i = 0; i < States.Count; i++)
                    {
                        States[i].initState();
                    }

                    foreach (Board state in States)
                    {
                        writer.WriteLine(state.move == null ? "Initial" : state.move);
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
            if (board.state != null)
                return board.state[TargetY][TargetX] == 'o';
            int Target = TargetY * dim + TargetX;
            return board.pos.Contains(Target);
        }
    }
}