using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        #region String Parse Test

        int[] toPrint = ParseTxtToString("E:\\Documents\\Visual Studio 2015\\Projects\\CI 1\\CI1\\CI1\\TestSudokuVeryEz.txt");

        for (int i = 0; i < Math.Sqrt(toPrint.Length); i++)
        {
            // for ( int j = 0; j < Math.Sqrt( toPrint.Length ); j++ )
            //    Console.Write( toPrint[ i * (int)Math.Sqrt( toPrint.Length ) + j ] );
            //Console.Write( "\n" );
        }

        Sudoku sdk = new Sudoku(toPrint);
        
        Stack<Sudoku> stack = new Stack<Sudoku>();
        stack.Push(sdk);

        Sudoku nextSdk = BackTrack(stack);

        nextSdk.Print();
        #endregion

        #region Sudoku Class Test
        Sudoku s = new Sudoku(null, 9);
        Console.WriteLine("Find which Col/Row/Block?");
        int index = int.Parse(Console.ReadLine());

        // Rows
        Console.WriteLine("Row " + index + " is:");
        int[] r = s.Row(index);
        for (int i = 0; i < r.Length; i++)
            Console.WriteLine(r[i]);
        // Cols
        Console.WriteLine("Column " + index + " is:");
        r = s.Col(index);
        for (int i = 0; i < r.Length; i++)
            Console.WriteLine(r[i]);
        // Block
        Console.WriteLine("Block " + index + " is:");
        r = s.Block(index);
        for (int i = 0; i < r.Length; i++)
            Console.WriteLine(r[i]);
        Console.ReadLine();
        #endregion

        // Input sudoku to solve / Parse it

        // Start timer

        // Start solving

        // If timer elapsed, stop solving

        // If solution found, return it + statistics


    }

    static int[] ParseTxtToString(string Path)
    {
        string CompiledString = "";

        StreamReader sr = new StreamReader(Path);
        String read = sr.ReadLine();
        while (read != null && read != "")
        {
            CompiledString += read;
            read = sr.ReadLine();
        }

        int[] result = new int[CompiledString.Length];

        // Traverse the entire string, adding the chars to the array in int form.
        for (int i = 0; i < CompiledString.Length; i++)
        {
            // An empty space
            if (CompiledString[i] == '.')
                result[i] = 0;
            // A non-empty space
            else
                result[i] = int.Parse("" + CompiledString[i]);
        }

        return result;

    }

    public static Sudoku BackTrack(Stack<Sudoku> stack)
    {
        Sudoku sdk = stack.First();
        if (Failure(sdk))
        {
            stack.Pop();
            return null;
        }

        if (Goal(sdk))
            return sdk;

        Sudoku child = FirstSuccessor(sdk);
        stack.Push(child);
        child.Print();
        Sudoku result = BackTrack(stack);

        while (result == null)
        {
            child = NextSuccessor(sdk);

            if (child == null)
            {
                stack.Pop();
                return null;
            }
            else
            {
                stack.Push(child);
                child.Print();
                result = BackTrack(stack);
            }
        }
        return result;
    }

    public static bool Failure(Sudoku sdk)
    {
        if (sdk == null)
            return true;

        return !ValidMove(sdk, sdk.lastChanged);
    }

    public static Sudoku FirstSuccessor(Sudoku sdk)
    {
        int i = GetNextEmptyPos(sdk);

        if (i == -1)
            return null;

        Sudoku result = new Sudoku(sdk.things);
        
        result.things[i]++;
        result.lastChanged = i;

        return result;
    }

    public static Sudoku NextSuccessor(Sudoku sdk)
    {
        
        if (sdk.things[sdk.lastChanged] == 9)
            return null;
        else
            sdk.things[sdk.lastChanged]++;
        return sdk;
    }

    public class SuccessorPair
    {
        public Sudoku Parent;
        public Sudoku Child;
    }

    public static int GetNextEmptyPos(Sudoku t)
    {
        int pos = 0;

        while (pos < t.N * t.N && t.things[pos] != 0)
            pos++;

        if (pos == t.N * t.N)
            return -1;

        return pos;
    }

    public static Queue<int> GetMoves(Sudoku t, int pos)
    {
        Queue<int> result = new Queue<int>();

        for (int i = t.things[pos] + 1; i < 10; i++)
        {
            if (ValidMove(t, pos))
            {
                result.Enqueue(i);
            }
        }
        return result;
    }

    public static bool ValidMove(Sudoku sdk, int pos)
    {
        if(sdk.things[pos] == 0)
        {
            return true;
        }
        int n = sdk.things[pos];
        int colPos = pos % sdk.N;
        int rowPos = pos / sdk.N;
        int[] row = sdk.Row(rowPos);
        int[] col = sdk.Col(colPos);
        int[] block = sdk.Block((rowPos / sdk.sqrtN * sdk.sqrtN) + colPos / sdk.sqrtN);

        if (HasDuplicate(row, n) || HasDuplicate(col, n) || HasDuplicate(block, n))
            return false;

        return true;
    }

    public static bool HasDuplicate(int[] array, int n)
    {
        if (Array.FindAll(array, x => x == n).Length > 1)
            return true;
        return false;
    }


    public static bool Goal(Sudoku t)
    {
        if (t.things.Contains(0))
        {
            return false;
        }
        return true;
    }
}

//TODO
//Parse txt to string
// Print Sudoku
// The loop
// Next-succ methode met alleen passende getallen

public class Sudoku
{
    public int N, sqrtN;
    public int[] things;
    public int lastChanged;
    public Queue<int>[] possibleMoves;

    public Sudoku(int[] Content = null, int Dim = 9, int lastChanged = -1)
    {        
        this.lastChanged = lastChanged;
        N = Dim;
        sqrtN = (int)Math.Sqrt(Dim);
        // Fill the sudoku with numbers
        if (Content == null || Content.Length < N * N)
        {
            things = new int[N * N];
            for (int i = 0; i < things.Length; i++)
                things[i] = i * i % N;
        }
        // Fill the sudoku with the string.
        else
        {
            things = new int[Content.Length];
            for (int i = 0; i < Content.Length; i++)
            {
                things[i] = Content[i];
            }
            
        }
        this.lastChanged = Program.GetNextEmptyPos(this);
    }


    public void Print()
    {
        //String currentRow = "";
        //// The first N-sqrtN rows
        //for ( int h = 0; h < sqrtN - 1; h++ )
        //{
        //    // The first sqrtN rows
        //    for ( int j = 0; j < sqrtN; j++ )
        //    {
        //        for ( int i = 0; i < N; i++ )
        //            currentRow += Row( j + h )[ i ];
        //
        //        for ( int i = sqrtN - 1; i > 0; i-- )
        //            currentRow = currentRow.Insert( sqrtN * i, "|" );
        //
        //        Console.WriteLine( currentRow );
        //        currentRow = "";
        //    }
        //    // A Line of -'s
        //    currentRow = "";
        //    for ( int i = 0; i < N + sqrtN - 1; i++ )
        //        currentRow += '-';
        //    Console.WriteLine( currentRow );
        //}

        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                if (things[i * N + j] == 0)
                    Console.Write(".");
                else
                    Console.Write(things[i * N + j]);
            }
            Console.Write("\n");
        }
        Console.WriteLine("");
    }

    #region Return Content
    public int[] Row(int Which)
    {
        int[] result = new int[N];

        for (int i = 0; i < N; i++)
            result[i] = things[Which * N + i];

        return result;
    }

    public int[] Col(int Which)
    {
        int[] result = new int[N];

        for (int i = 0; i < N; i++)
            result[i] = things[i * N + Which];

        return result;
    }

    public int[] Block(int Which)
    {
        int[] result = new int[N];

        // Take all the rows that form the block, and with the correct offset, write the relevant spaces to the result.
        int[] currentRow;
        int offset = (Which % sqrtN) * sqrtN;
        for (int row = 0; row < sqrtN; row++)
        {
            currentRow = Row((Which / sqrtN * sqrtN) + row);

            for (int i = 0; i < sqrtN; i++)
                result[i + (row * sqrtN)] = currentRow[i + offset];
        }

        return result;
    }
    #endregion
}