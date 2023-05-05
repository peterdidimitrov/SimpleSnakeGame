
using Snake.Enums;
using System;
using System.Collections.Generic;

namespace Snake.Models
{
    public class GameState
    {
        private readonly LinkedList<Position> snakePosition = new LinkedList<Position>();
        private readonly Random random = new Random();
        private readonly LinkedList<Direction> dirChanges = new LinkedList<Direction>();
        public GameState(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;

            Grid = new GridValue[rows, cols];
            Dir = Direction.Right;

            SpeedOfSnake = 1;
            IsThereBonus = false;

            AddSnake();
            AddFood();
        }


        public int Rows { get; }
        public int Cols { get; }
        public GridValue[,] Grid { get; }
        public Direction Dir { get; private set; }
        public Position PositionOfBonus { get; private set; }
        public int MoveCount { get; private set; }
        public int Score { get; private set; }
        public bool GameOver { get; private set; }
        public bool IsThereBonus { get; private set; }
        public int SpeedOfSnake { get; set; }

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
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
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
        private void AddBonus()
        {
            List<Position> empty = new List<Position>(EmptyPosition());

            if (empty.Count == 0)
            {
                return;
            }
            Position pos = empty[random.Next(empty.Count)];
            PositionOfBonus = pos;
            Grid[pos.Row, pos.Colum] = GridValue.Bonus;
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
        private Direction GetLastDirection()
        {
            if (dirChanges.Count == 0)
            {
                return Dir;
            }
            return dirChanges.Last.Value;
        }
        private bool CanChangeDirection(Direction newDir)
        {
            if (dirChanges.Count == 2)
            {
                return false;
            }
            Direction lastDir = GetLastDirection();
            return newDir != lastDir && newDir != lastDir.Opposite();
        }
        public void ChangeDirection(Direction dir)
        {
            if (CanChangeDirection(dir))
            {
                dirChanges.AddLast(dir);
            }
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
            MoveCount++;

            if (dirChanges.Count > 0)
            {
                Dir = dirChanges.First.Value;
                dirChanges.RemoveFirst();
            }
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
            else if (hit == GridValue.Bonus)
            {
                AddHead(newHeadPosition);
                AddHead(newHeadPosition);
                Score += 3;
                IsThereBonus = false;
            }
            if (IsThereBonus == false && MoveCount % 40 == 0)
            {
                AddBonus();
                IsThereBonus = true;
            }
            if (IsThereBonus == true && MoveCount % 70 == 0)
            {
                RemoveBonus();
                IsThereBonus = false;
                PositionOfBonus = default;
                MoveCount = 0;
            }
        }
        private void RemoveBonus()
        {
            Grid[PositionOfBonus.Row, PositionOfBonus.Colum] = GridValue.EmptySpace;
        }
    }
}
