using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;


namespace Poging2
{
    class Program
    {
        static int[] Sudoku, positionsFree;
        static Stack<int> positionsChanged;
        static int N, Nsq, sqrtN, posFreeIndex = -1;
        static bool done = false;
        static Stopwatch stopWatch;

        static void Main(string[] args)
        {
            N = 9;
            Nsq = N * N;
            sqrtN = (int)Math.Sqrt(N);
            Sudoku = new int[Nsq];
            positionsChanged = new Stack<int>();
            // Fill the empty Sudoku
            Sudoku = ParseTxtToArray("C:\\Users\\Merlijn\\Documents\\Visual Studio 2015\\Projects\\Backtrack\\Backtrack\\CI1\\Poging2\\TestSudokuVeryEz.txt");

            positionsFree = getFreePoss(false);
            stopWatch = new Stopwatch();
            stopWatch.Start();
            // Do The Twerk(tm)
            BackTrack();
            stopWatch.Stop();
            Console.WriteLine("ticks: " + stopWatch.ElapsedTicks);
            Console.WriteLine("ms: " + stopWatch.ElapsedMilliseconds);
            Print();
            Console.ReadLine();
        }

        static int[] getFreePoss(bool nonVariant = true)
        {
            List<Tuple<int, int>> MovesPerFreeSpace = new List<Tuple<int, int>>();
            int j = 0;
            int[] tmp = new int[Nsq];
            int[] result;
            if (nonVariant)
            {
                for (int i = 0; i < Nsq; i++)
                {
                    if (Sudoku[i] == 0)
                    {
                        tmp[j] = i;
                        j++;
                    }
                }
                result = new int[j];
                for (int i = 0; i < j; i++)
                {
                    result[i] = tmp[i];
                }
            }
            else
            {
                for (int i = 0; i < Nsq; i++)
                {
                    if (Sudoku[i] == 0)
                    {
                        int itemTwo = 0;
                        for (int n = 1; n <= N; n++)
                        {
                            int colPos = i % N;
                            int rowPos = i / N;

                            int[] row = Row(rowPos);
                            int[] col = Col(colPos);
                            int[] block = Block((rowPos / sqrtN * sqrtN) + colPos / sqrtN);

                            if (!HasDuplicate2(row, n) &&
                                 !HasDuplicate2(col, n) &&
                                 !HasDuplicate2(block, n))
                                itemTwo++;
                        }
                        Tuple<int, int> resultSpace = new Tuple<int, int>(i, itemTwo);
                        MovesPerFreeSpace.Add(resultSpace);
                    }
                }
                MovesPerFreeSpace.Sort((x, y) => y.Item2.CompareTo(x.Item2));
                result = new int[MovesPerFreeSpace.Count];
                for (int i = 0; i < MovesPerFreeSpace.Count; i++)
                {
                    result[i] = MovesPerFreeSpace[i].Item1;
                }
            }
            
            return result;
        }

        #region Backtrack

        static void BackTrack()
        {
            // Faillure
            if (FailureTest())
            {
                //NextSuccessor();
                return;
            }
            // Goal
            if (!Sudoku.Contains(0))
            {
                Console.WriteLine("Done and found!");
                done = true;
                return;
            }

            // FirstS
            FirstSuccessor();

            // NextS
            while (NextSuccessor())
            {
                if (done)
                    return;
                //Print();
                BackTrack();
                if (done)
                    return;
            }
        }

        static void FirstSuccessor()
        {
            posFreeIndex++;
        }

        static bool NextSuccessor()
        {
            int pos = positionsFree[posFreeIndex];
            if (Sudoku[pos] == N)
            {
                posFreeIndex--;
                Sudoku[pos] = 0;
                return false;
            }
            else
            {
                Sudoku[pos]++;
                return true;
            }
        }

        static bool FailureTest()
        {
            if (posFreeIndex == -1) // begin
            {
                return false;
            }

            int pos = positionsFree[posFreeIndex];

            int n = Sudoku[pos];
            if (n == 0)
                return false;

            int colPos = pos % N;
            int rowPos = pos / N;

            int[] row = Row(rowPos);
            int[] col = Col(colPos);
            int[] block = Block((rowPos / sqrtN * sqrtN) + colPos / sqrtN);

            if (HasDuplicate(row, n) || HasDuplicate(col, n) || HasDuplicate(block, n))
                return true;

            return false;
        }

        public static bool HasDuplicate2(int[] array, int n)
        {
            if (array.Contains(n))
                return true;
            return false;
        }

        public static bool HasDuplicate(int[] array, int n)
        {
            if (Array.FindAll(array, x => x == n).Length > 1)
                return true;
            return false;
        }

        #endregion

        #region Pretty Sudoku

        static int[] Row(int which)
        {
            int[] result = new int[N];

            for (int i = 0; i < N; i++)
                result[i] = Sudoku[which * N + i];

            return result;
        }

        static int[] Col(int which)
        {
            int[] result = new int[N];

            for (int i = 0; i < N; i++)
                result[i] = Sudoku[i * N + which];

            return result;
        }

        static int[] Block(int which)
        {
            int[] result = new int[N];

            // Take all the rows that form the block, and with the correct offset, write the relevant spaces to the result.
            int[] currentRow;
            int offset = (which % sqrtN) * sqrtN;
            for (int row = 0; row < sqrtN; row++)
            {
                currentRow = Row((which / sqrtN * sqrtN) + row);

                for (int i = 0; i < sqrtN; i++)
                    result[i + (row * sqrtN)] = currentRow[i + offset];
            }
            return result;
        }

        static int FindFirstEmpty()
        {
            for (int i = 0; i < Nsq; i++)
            {
                if (Sudoku[i] == 0)
                    return i;
            }

            return -1;
        }

        static void Print()
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (Sudoku[i * N + j] == 0)
                        Console.Write(".");
                    else
                        Console.Write(Sudoku[i * N + j]);
                }
                Console.Write("\n");
            }
            Console.WriteLine("");
        }

        static int[] ParseTxtToArray(string Path)
        {
            string[] totString = new string[Nsq];
            StreamReader sr = new StreamReader(Path);
            String read = sr.ReadLine();
            String[] sa;
            int offset = 0;
            while (read != null && read != "")
            {
                sa = read.Split(' ');
                for (int j = 0; j < N; j++)
                {
                    totString[offset + j] = sa[j];
                }

                read = sr.ReadLine();
                offset += N;
            }

            int[] result = new int[Nsq];

            // Traverse the entire string, adding the chars to the array in int form.
            for (int i = 0; i < Nsq; i++)
            {
                // An empty space
                if (totString[i] == ".")
                    result[i] = 0;
                // A non-empty space
                else
                    result[i] = int.Parse(totString[i]);
            }

            return result;

        }

        #endregion
    }
}
