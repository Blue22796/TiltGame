using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Tilt_Game;

namespace Tilt_Game_pro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        char[][] State;
        static int n;
        int TargetX;
        int TargetY;
        string[] lines;
        int minMoves;
        int count;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_select(object sender, RoutedEventArgs e)
        {


            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set filter options and filter index
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;

            // Call the ShowDialog method to show the dialog
            bool? result = openFileDialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                try
                {
                    string filename = openFileDialog.FileName;
                    selectedFile.Text = System.IO.Path.GetFileName(filename);
                    string[] lines = File.ReadAllLines(filename);

                    n = int.Parse(lines[0]);


                    State = new char[n][];


                    for (int i = 0; i < n; i++)
                    {
                        State[i] = lines[i + 1].Split(", ").Select(s => s[0]).ToArray();
                    }


                    string[] targetValues = lines[lines.Length - 1].Split(", ");
                    TargetX = int.Parse(targetValues[0]);
                    TargetY = int.Parse(targetValues[1]);


                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Button_run(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Player Player = new Player();
            Player.Solve(State, TargetX, TargetY, 1);
            sw.Stop();
            TimeSpan elapsed = sw.Elapsed;
            time.Text = "Time Elapsed: " + elapsed.TotalSeconds + " Seconds";
            string fileName = string.Format("./output{0}.txt", 1);
            lines = File.ReadAllLines(fileName);
            bool solvable = lines[0].Trim() == "Solvable";
            count = 0;
            Prevbtn.Visibility = Visibility.Collapsed;
            nextbtn.Visibility = Visibility.Collapsed;
            if (solvable)
            {
                minMoves = int.Parse(lines[1].Split(": ")[1]);

                if (n <= 7)
                {

                    prepareBoard(BoardGrid);
                    moveState.Visibility = Visibility.Visible;

                    int i = 0;
                    int idx = i + 3 + (i * 6);
                    string currmove = "Current move: " + lines[idx];
                    moveState.Text = currmove;
                    char[,] currBoard = FileInto2DCharArray(lines, idx + 1);
                    BoardInitializer(currBoard, BoardGrid, TargetX, TargetY);
                    statebtns.Visibility = Visibility.Visible;
                    nextbtn.Visibility = Visibility.Visible;
                }
                else
                {
                    StateSeq.Visibility = Visibility.Visible;
                    minseq.Text = lines[1];
                    seq.Text = lines[2];
                    BoardGrid.Visibility = Visibility.Collapsed;
                    unsolve.Visibility = Visibility.Collapsed;
                    moveState.Visibility = Visibility.Collapsed;
                    statebtns.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                BoardGrid.Visibility = Visibility.Collapsed;
                unsolve.Visibility = Visibility.Visible;
                StateSeq.Visibility = Visibility.Collapsed;
                moveState.Visibility = Visibility.Collapsed;
                statebtns.Visibility = Visibility.Collapsed;
            }

        }
        private void Button_next(object sender, RoutedEventArgs e)
        {
            count++;
            int i = count;
            int i_prev = count - 1;
            int idx = i + 3 + (i * (n + 1));
            int idx_prev = i_prev + 3 + (i_prev * (n + 1));
            string currmove = "Current move: " + lines[idx];
            moveState.Text = currmove;
            char[,] currBoard = FileInto2DCharArray(lines, idx + 1);
            char[,] prevBoard = FileInto2DCharArray(lines, idx_prev + 1);
            BoardCleaner(prevBoard, BoardGrid);
            BoardInitializer(currBoard, BoardGrid, TargetX, TargetY);
            if (count == minMoves)
                nextbtn.Visibility = Visibility.Collapsed;
            Prevbtn.Visibility = Visibility.Visible;
        }
        private void Button_prev(object sender, RoutedEventArgs e)
        {
            count--;
            int i = count;
            int i_next = count + 1;
            int idx = i + 3 + (i * (n + 1));
            int idx_next = i_next + 3 + (i_next * (n + 1));
            string currmove = "Current move: " + lines[idx];
            moveState.Text = currmove;
            char[,] currBoard = FileInto2DCharArray(lines, idx + 1);
            char[,] nextBoard = FileInto2DCharArray(lines, idx_next + 1);
            BoardCleaner(nextBoard, BoardGrid);
            BoardInitializer(currBoard, BoardGrid, TargetX, TargetY);
            if (count == 0)
                Prevbtn.Visibility = Visibility.Collapsed;
            nextbtn.Visibility = Visibility.Visible;
        }
        private static char[,] FileInto2DCharArray(string[] lines, int index)
        {
            char[,] array2D = new char[n, n];

            for (int i = 0; i < n; i++)
            {
                string[] rowValues = lines[index + i].Split(", ");
                for (int j = 0; j < n; j++)
                {
                    array2D[i, j] = rowValues[j][0];
                }
            }

            return array2D;
        }
        private static void prepareBoard(Grid BoardGrid)
        {
            int visibleCount = 0;
            int rowCount = 0;
            int columnCount = 0;
            foreach (UIElement element in BoardGrid.Children)
            {
                if (element is Border border)
                {
                    if (visibleCount < n * n)
                    {
                        if (columnCount == n)
                            break;
                        if (rowCount == 7)
                        {
                            rowCount = 0;
                            columnCount++;
                        }


                        if (rowCount >= n)
                        {
                            rowCount++;
                            continue;
                        }

                        border.Visibility = Visibility.Visible;
                        rowCount++;
                        visibleCount++;

                    }
                    else
                        break;
                }

            }
        }
        private static void BoardInitializer(char[,] currBoard, Grid BoardGrid, int x, int y)
        {
            for (int j = 0; j < n; j++)
            {
                for (int k = 0; k < n; k++)
                {
                    Border border = BoardGrid.Children.Cast<Border>().FirstOrDefault(
                        b => Grid.GetRow(b) == j && Grid.GetColumn(b) == k);

                    if (border != null)
                    {
                        Grid innerGrid = border.Child as Grid;
                        if (innerGrid != null && innerGrid.Children.Count >= 2)
                        {
                            Rectangle block = innerGrid.Children[0] as Rectangle;
                            Rectangle ball = innerGrid.Children[1] as Rectangle;

                            if (currBoard[j, k] == '#')
                            {
                                block.Visibility = Visibility.Visible;
                                ball.Visibility = Visibility.Collapsed;
                            }
                            else if (currBoard[j, k] == 'o')
                            {
                                block.Visibility = Visibility.Collapsed;
                                ball.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                block.Visibility = Visibility.Collapsed;
                                ball.Visibility = Visibility.Collapsed;
                            }
                            if (j == y && k == x)
                            {
                                border.BorderBrush = Brushes.Gold;
                                border.BorderThickness = new Thickness(4);
                            }
                            else
                            {
                                border.BorderBrush = Brushes.Black;
                                border.BorderThickness = new Thickness(1);
                            }
                        }
                    }
                }
            }
        }
        private static void BoardCleaner(char[,] currBoard, Grid BoardGrid)
        {
            for (int j = 0; j < n; j++)
            {
                for (int k = 0; k < n; k++)
                {
                    Border border = BoardGrid.Children.Cast<Border>().FirstOrDefault(
                        b => Grid.GetRow(b) == j && Grid.GetColumn(b) == k);

                    if (border != null)
                    {
                        Grid innerGrid = border.Child as Grid;
                        if (innerGrid != null && innerGrid.Children.Count >= 2)
                        {
                            Rectangle ball = innerGrid.Children[1] as Rectangle;
                            if (currBoard[j, k] == 'o')
                            {
                                ball.Visibility = Visibility.Collapsed;
                            }
                        }
                    }
                }
            }
        }
    }
}