using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static void Main( string[] args )
    {
        #region Sudoku Class Test
        Sudoku s = new Sudoku( 81 );
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
}

class Sudoku
{
    int dims, sqrtDims;
    int[] things;

    public Sudoku( int Dim = 9 )
    {
        dims = Dim;
        sqrtDims = (int)Math.Sqrt( Dim );
        things = new int[ dims * dims ];
        for ( int i = 0; i < things.Length; i++ )
            things[ i ] = i;
    }

    public int[] Row( int Which )
    {
        int[] result = new int[ dims ];

        for ( int i = 0; i < dims; i++ )
            result[ i ] = things[ Which * dims + i ];

        return result;
    }

    public int[] Col( int Which )
    {
        int[] result = new int[ dims ];

        for ( int i = 0; i < dims; i++ )
            result[ i ] = things[ i * dims + Which ];

        return result;
    }

    public int[] Block( int Which )
    {
        int[] result = new int[ dims ];

        // Take all the rows that form the block, and with the correct offset, write the relevant spaces to the result.
        int[] currentRow;
        int offset = (Which % sqrtDims) * sqrtDims;
        for ( int row = 0; row < sqrtDims; row++ )
        {
            currentRow = Row( (Which / sqrtDims * sqrtDims) + row );

            for ( int i = 0; i < sqrtDims; i++ )
                result[ i + (row * sqrtDims) ] = currentRow[ i + offset ];
        }

        return result;
    }
}