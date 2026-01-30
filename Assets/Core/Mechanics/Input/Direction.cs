using System;

namespace Core.Mechanics.Input
{
    public enum Direction { Up = 0, Right = 1, Down = 2, Left = 3 }

    public static class DirectionExtensions
    {
        public static Direction RotateClockwise(this Direction direction)
            => (Direction)(((int)direction + 1) % Enum.GetValues(typeof(Direction)).Length);
        
        public static Direction RotateCounterClockwise(this Direction direction)
        {
            var count = Enum.GetValues(typeof(Direction)).Length;
            return (Direction)(((int)direction - 1 + count) % count);
        }
    }
}