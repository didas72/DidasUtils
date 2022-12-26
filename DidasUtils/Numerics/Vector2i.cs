using System;
using System.Numerics;

namespace DidasUtils.Numerics
{
    /// <summary>
    /// Integer Vector2
    /// </summary>
    public readonly struct Vector2i
    {
        /// <summary>
        /// The x coordinate of the vector.
        /// </summary>
        public readonly int x;
        /// <summary>
        /// The y coordinate of the vector.
        /// </summary>
        public readonly int y;
        /// <summary>
        /// The length of the vector.
        /// </summary>
        public double Magnitude { get => Math.Sqrt(x * x + y * y); }



        /// <summary>
        /// Default 0,0 vector.
        /// </summary>
        public static readonly Vector2i Zero = new(0, 0);



        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vector2i(int x, int y) { this.x = x; this.y = y; }



        /// <summary>
        /// Calculates the dot product between two vectors.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double DotProduct(Vector2i a, Vector2i b) => a.x * b.x + a.y * b.y;
        /// <summary>
        /// Calculates the angle between two vectors.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double Angle(Vector2i a, Vector2i b) => Math.Acos(DotProduct(a, b) / (a.Magnitude * b.Magnitude));



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"<{x}:{y}>";
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Vector2i operator +(Vector2i a) => a;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Vector2i operator -(Vector2i a) => new(-a.x, -a.y);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector2i operator +(Vector2i a, Vector2i b) => new(a.x + b.x, a.y + b.y);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector2i operator -(Vector2i a, Vector2i b) => new(a.x - b.x, a.y - b.y);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector2i operator *(Vector2i a, Vector2i b) => new(a.x * b.x, a.y * b.y);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector2i operator /(Vector2i a, Vector2i b) => new(a.x / b.x, a.y / b.y);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Vector2i operator *(Vector2i a, int s) => new (a.x * s, a.y * s);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Vector2i operator *(Vector2i a, float s) => new ((int)(a.x * s), (int)(a.y * s));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Vector2i operator *(Vector2i a, double s) => new ((int)(a.x * s), (int)(a.y * s));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Vector2i operator /(Vector2i a, int s) => new(a.x / s, a.y / s);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Vector2i operator /(Vector2i a, float s) => new((int)(a.x / s), (int)(a.y / s));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Vector2i operator /(Vector2i a, double s) => new((int)(a.x / s), (int)(a.y / s));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>

        public static explicit operator Vector2(Vector2i a) => new (a.x, a.y);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        public static explicit operator Vector2i(Vector2 a) => new((int)a.X, (int)a.Y);
    }
}
