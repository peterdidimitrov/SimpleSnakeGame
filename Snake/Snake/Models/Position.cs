using System;
using System.Collections.Generic;

namespace Snake.Models;

public class Position
{
    public Position(int row, int colum)
    {
        Row = row;
        Colum = colum;
    }

    public int Row { get; }
    public int Colum { get; }
    public Position Translate(Direction dir)
    {
        return new Position(Row + dir.RowOffset, Colum + dir.ColumnOffset);
    }

    //The next parth of code compares the Postions
    public override bool Equals(object? obj)
    {
        return obj is Position position &&
               Row == position.Row &&
               Colum == position.Colum;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Row, Colum);
    }


    public static bool operator ==(Position? left, Position? right)
    {
        return EqualityComparer<Position>.Default.Equals(left, right);
    }

    public static bool operator !=(Position? left, Position? right)
    {
        return !(left == right);
    }
}
