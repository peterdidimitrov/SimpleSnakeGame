
using Snake.Enums;
using System;
using System.Collections.Generic;

namespace Snake.Models
{
    public class GameState
    {
        private readonly LinkedList<Position> snakePosition = new LinkedList<Position>();
        private readonly Random random = new Random();

        public GameState(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;

            Grid = new GridValue[rows, cols];
            Dir = Direction.Right;

            AddSnake();
            AddFood();
        }
        public int Rows { get; }
        public int Cols { get; }
        public GridValue[,] Grid { get; }
        public Direction Dir { get; private set; }
        public int Score { get; private set; }
        public bool GameOver { get; private set; }
        private void AddSnake()
        {
            int r = Rows / 2;
            for (int c = 1; c <= 3; c++)
            {
                Grid[r, c] = GridValue.Snake;
                snakePosition.AddFirst(new Position(r, c));
            }
        }
        private IEnumerable<Position> EmptyPosition()
        {
            for (int r = 1; r <= Rows; r++)
            {
                for (int c = 1; c <= Cols; c++)
                {
                    if (Grid[r, c] == GridValue.EmptySpace)
                    {
                        yield return new Position(r, c);
                    }
                }
            }
        }

        private void AddFood()
        {
            List<Position> empty = new List<Position>(EmptyPosition());

            if (empty.Count == 0)
            {
                return;
            }
            Position pos = empty[random.Next(empty.Count)];
            Grid[pos.Row, pos.Colum] = GridValue.Food;
        }
        public Position HeadPosition()
        {
            return snakePosition.First.Value;
        }
        public Position TailPosition()
        {
            return snakePosition.Last.Value;
        }
        public IEnumerable<Position> SnakePositions()
        {
            return snakePosition;
        }
        private void AddHead(Position pos)
        {
            snakePosition.AddFirst(pos);
            Grid[pos.Row, pos.Colum] = GridValue.Snake;
        }
        private void RemoveTail()
        {
            Position tail = snakePosition.Last.Value;
            Grid[tail.Row, tail.Colum] = GridValue.EmptySpace;
            snakePosition.RemoveLast();
        }
        public void ChangeDirection(Direction dir)
        {
            Dir = dir;
        }
        public bool OutsideGrid(Position pos)
        {
            return pos.Row < 0 || pos.Row >= Rows ||
                pos.Colum < 0 || pos.Colum >= Cols;
        }
        private GridValue WillHit(Position newHeadPosotion)
        {
            if (OutsideGrid(newHeadPosotion))
            {
                return GridValue.Outside;
            }
            if (newHeadPosotion == TailPosition())
            {
                return GridValue.EmptySpace;
            }
            return Grid[newHeadPosotion.Row, newHeadPosotion.Colum];
        }

        public void Move()
        {
            Position newHeadPosition = HeadPosition().Translate(Dir);
            GridValue hit = WillHit(newHeadPosition);

            if (hit == GridValue.Outside || hit == GridValue.Snake)
            {
                GameOver = true;
            }
            else if (hit == GridValue.EmptySpace)
            {
                RemoveTail();
                AddHead(newHeadPosition);
            }
            else if (hit == GridValue.Food)
            {
                AddHead(newHeadPosition);
                Score++;
                AddFood();
            }
        }
    }
}
