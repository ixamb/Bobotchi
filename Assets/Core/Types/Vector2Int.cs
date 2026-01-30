using System;

namespace Core.Types
{
    public readonly struct Vector2Int : IEquatable<Vector2Int>
    {
        public int X { get; }
        public int Y { get; }
        
        public Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public static Vector2Int Zero => new Vector2Int(0, 0);
        public static Vector2Int One => new Vector2Int(1, 1);
        public static Vector2Int Up => new Vector2Int(0, 1);
        public static Vector2Int Down => new Vector2Int(0, -1);
        public static Vector2Int Left => new Vector2Int(-1, 0);
        public static Vector2Int Right => new Vector2Int(1, 0);
        
        public static Vector2Int operator +(Vector2Int a)
            => new Vector2Int(a.X, a.Y);
        
        public static Vector2Int operator -(Vector2Int a)
            => new Vector2Int(-a.X, -a.Y);
        
        public static Vector2Int operator +(Vector2Int a, Vector2Int b)
            => new Vector2Int(a.X + b.X, a.Y + b.Y);

        public static Vector2Int operator -(Vector2Int a, Vector2Int b)
            => new Vector2Int(a.X - b.X, a.Y - b.Y);

        public static Vector2Int operator *(Vector2Int a, int scalar)
            => new Vector2Int(a.X * scalar, a.Y * scalar);

        public static Vector2Int operator /(Vector2Int a, int scalar)
            => new Vector2Int(a.X / scalar, a.Y / scalar);

        public static bool operator ==(Vector2Int a, Vector2Int b)
            => a.X == b.X && a.Y == b.Y;
        
        public static bool operator !=(Vector2Int a, Vector2Int b)
            => a.X != b.X || a.Y != b.Y;
        
        public override string ToString() => $"({X},{Y})";

        public bool Equals(Vector2Int other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is Vector2Int other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}