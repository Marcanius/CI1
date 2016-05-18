using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Poging2
{
    class Program
    {
        static int[] Sudoku;
        static Stack<int> positionsChanged;
        static int N, Nsq, sqrtN;
        static bool done = false;

        static void Main(string[] args)
        {
            N = 9;
            Nsq = N * N;
            sqrtN = (int)Math.Sqrt(N);
            Sudoku = new int[Nsq];
            positionsChanged = new Stack<int>();
            Sudoku = ParseTxtToArray("E:\\Documents\\Visual Studio 2015\\Projects\\CI 1\\CI1\\CI1\\TestSudokuVeryEz.txt");
            
            BackTrack();
            Print();
            Console.ReadLine();
        }

        static void BackTrack()
        {  
            // Faillure
            if (FaillureTest())
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
                Print();
                BackTrack();
                if (done)
                    return;
            }
        }

        static void FirstSuccessor()
        {
            int n = FindFirstEmpty();
            if (n >= 0) // Sudoku is full
                positionsChanged.Push(n);
        }

        static bool NextSuccessor()
        {
            int pos = positionsChanged.First();
            if (Sudoku[pos] == N)
            {
                positionsChanged.Pop();
                Sudoku[pos] = 0;
                return false;
            }
            else
            {
                Sudoku[pos]++;
                return true;
            }
        }

        static bool FaillureTest()
        {
            if (positionsChanged.Count == 0) // begin
            {
                return false;
            }

            int pos = positionsChanged.First();

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

        public static bool HasDuplicate(int[] array, int n)
        {
            if (Array.FindAll(array, x => x == n).Length > 1)
                return true;
            return false;
        }

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
    }
}
