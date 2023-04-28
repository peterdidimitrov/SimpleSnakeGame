namespace Snake.Models;

using System;
using System.Collections.Generic;
public class Direction
{
    public readonly static Direction Right = new Direction(0, 1);
    public readonly static Direction Left = new Direction(0, -1);
    public readonly static Direction Up = new Direction(-1, 0);
    public readonly static Direction Down = new Direction(1, 0);
    private Direction(int rowOffset, int columnOffset)
    {
        RowOffset = rowOffset;
        ColumnOffset = columnOffset;
    }

    public int RowOffset { get; }
    public int ColumnOffset { get; }

    //The next parth of code is need to compare the Directions
    //Override Equals and GetHashCode to use class as the key in a Dictionary
    public override bool Equals(object? obj)
    {
        return obj is Direction direction &&
               RowOffset == direction.RowOffset &&
               ColumnOffset == direction.ColumnOffset;
    }
    public Direction Opposite()
    {
        return new Direction(-RowOffset, -ColumnOffset);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(RowOffset, ColumnOffset);
    }

    //Equality operator
    public static bool operator ==(Direction? left, Direction? right)
    {
        return EqualityComparer<Direction>.Default.Equals(left, right);
    }
    //Inequality operator
    public static bool operator !=(Direction? left, Direction? right)
    {
        return !(left == right);
    }
}
