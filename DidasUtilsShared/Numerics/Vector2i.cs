using System;

namespace DidasUtils.Numerics
{
    public readonly struct Vector2i
    {
        public readonly int x;
        public readonly int y;
        public float Magnitude { get => (int)Math.Sqrt(x * x + y * y); }



        public static readonly Vector2i Zero = new Vector2i(0, 0);



        public Vector2i(int x, int y) { this.x = x; this.y = y; }



        public static float DotProduct(Vector2i a, Vector2i b) => a.x * b.x + a.y * b.y;
        public static float Angle(Vector2i a, Vector2i b) => (float)Math.Acos(DotProduct(a, b) / (a.Magnitude * b.Magnitude));



        public static Vector2i operator +(Vector2i a) => a;
        public static Vector2i operator -(Vector2i a) => new Vector2i(-a.x, -a.y);
        public static Vector2i operator +(Vector2i a, Vector2i b) => new Vector2i(a.x + b.x, a.y + b.y);
        public static Vector2i operator -(Vector2i a, Vector2i b) => new Vector2i(a.x - b.x, a.y - b.y);
        public static Vector2i operator *(Vector2i a, Vector2i b) => new Vector2i(a.x * b.x, a.y * b.y);
        public static Vector2i operator /(Vector2i a, Vector2i b) => new Vector2i(a.x / b.x, a.y / b.y);
    }
}
