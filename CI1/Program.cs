using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main( string[] args )
    {
        #region String Parse Test

        int[] toPrint = ParseTxtToString( "C:\\Users\\Matthijs\\stack\\UU\\CI\\Prac1\\CI1\\CI1\\TestSudokuVeryEz.txt" );

        for ( int i = 0; i < Math.Sqrt( toPrint.Length ); i++ )
        {
            // for ( int j = 0; j < Math.Sqrt( toPrint.Length ); j++ )
            //    Console.Write( toPrint[ i * (int)Math.Sqrt( toPrint.Length ) + j ] );
            //Console.Write( "\n" );
        }

        Sudoku sdk = new Sudoku( toPrint );
        sdk.Print();
        #endregion

        #region Sudoku Class Test
        Sudoku s = new Sudoku( null, 9 );
        Console.WriteLine( "Find which Col/Row/Block?" );
        int index = int.Parse( Console.ReadLine() );

        // Rows
        Console.WriteLine( "Row " + index + " is:" );
        int[] r = s.Row( index );
        for ( int i = 0; i < r.Length; i++ )
            Console.WriteLine( r[ i ] );
        // Cols
        Console.WriteLine( "Column " + index + " is:" );
        r = s.Col( index );
        for ( int i = 0; i < r.Length; i++ )
            Console.WriteLine( r[ i ] );
        // Block
        Console.WriteLine( "Block " + index + " is:" );
        r = s.Block( index );
        for ( int i = 0; i < r.Length; i++ )
            Console.WriteLine( r[ i ] );
        Console.ReadLine();
        #endregion

        // Input sudoku to solve / Parse it

        // Start timer

        // Start solving

        // If timer elapsed, stop solving

        // If solution found, return it + statistics


    }

    static int[] ParseTxtToString( string Path )
    {
        string CompiledString = "";

        StreamReader sr = new StreamReader( Path );
        String read = sr.ReadLine();
        while ( read != null && read != "" )
        {
            CompiledString += read;
            read = sr.ReadLine();
        }

        int[] result = new int[ CompiledString.Length ];

        // Traverse the entire string, adding the chars to the array in int form.
        for ( int i = 0; i < CompiledString.Length; i++ )
        {
            // An empty space
            if ( CompiledString[ i ] == '.' )
                result[ i ] = 0;
            // A non-empty space
            else
                result[ i ] = int.Parse( "" + CompiledString[ i ] );
        }

        return result;
    }
}

//TODO
//Parse txt to string
// Print Sudoku
// The loop
// Next-succ methode met alleen passende getallen

class Sudoku
{
    int N, sqrtN;
    int[] things;

    public Sudoku( int[] Content = null, int Dim = 9 )
    {
        N = Dim;
        sqrtN = (int)Math.Sqrt( Dim );
        // Fill the sudoku with numbers
        if ( Content == null || Content.Length < N * N )
        {
            things = new int[ N * N ];
            for ( int i = 0; i < things.Length; i++ )
                things[ i ] = i * i % N;
        }
        // Fill the sudoku with the string.
        else
        {
            things = Content;
        }
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

        for ( int i = 0; i < N; i++ )
        {
            for ( int j = 0; j < N; j++ )
            {
                if ( things[ i * N + j ] == 0 )
                    Console.Write( "." );
                else
                    Console.Write( things[ i * N + j ] );
            }
            Console.Write( "\n" );
        }
    }

    #region Return Content
    public int[] Row( int Which )
    {
        int[] result = new int[ N ];

        for ( int i = 0; i < N; i++ )
            result[ i ] = things[ Which * N + i ];

        return result;
    }

    public int[] Col( int Which )
    {
        int[] result = new int[ N ];

        for ( int i = 0; i < N; i++ )
            result[ i ] = things[ i * N + Which ];

        return result;
    }

    public int[] Block( int Which )
    {
        int[] result = new int[ N ];

        // Take all the rows that form the block, and with the correct offset, write the relevant spaces to the result.
        int[] currentRow;
        int offset = (Which % sqrtN) * sqrtN;
        for ( int row = 0; row < sqrtN; row++ )
        {
            currentRow = Row( (Which / sqrtN * sqrtN) + row );

            for ( int i = 0; i < sqrtN; i++ )
                result[ i + (row * sqrtN) ] = currentRow[ i + offset ];
        }

        return result;
    }
    #endregion
}