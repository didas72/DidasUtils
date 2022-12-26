using System;
using System.Numerics;

namespace DidasUtils.Numerics
{
    /// <summary>
    /// Integer Vector3
    /// </summary>
    public readonly struct Vector3i
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
        /// The z coordinate of the vector.
        /// </summary>
        public readonly int z;
        /// <summary>
        /// The length of the vector.
        /// </summary>
        public double Magnitude { get => Math.Sqrt(x * x + y * y + z * z); }



        /// <summary>
        /// Default 0,0,0 vector.
        /// </summary>
        public static readonly Vector3i Zero = new(0, 0, 0);



        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vector3i(int x, int y, int z) { this.x = x; this.y = y; this.z = z; }



        /// <summary>
        /// Convertes the Vector3i to a Vector3.
        /// </summary>
        /// <returns></returns>
        public Vector3 ToVector3() => new(x, y, z);



        /// <summary>
        /// Calculates the dot product between two vectors.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double DotProduct(Vector3i a, Vector3i b) => a.x * b.x + a.y * b.y + a.z * b.z;
        /// <summary>
        /// Calculates the angle between two vectors.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double Angle(Vector3i a, Vector3i b) => (float)Math.Acos(DotProduct(a, b) / (a.Magnitude * b.Magnitude));



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"<{x}:{y}:{z}>";
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Vector3i operator +(Vector3i a) => a;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Vector3i operator -(Vector3i a) => new(-a.x, -a.y, -a.z);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3i operator +(Vector3i a, Vector3i b) => new(a.x + b.x, a.y + b.y, a.z + b.z);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3i operator -(Vector3i a, Vector3i b) => new(a.x - b.x, a.y - b.y, a.z - b.z);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3i operator *(Vector3i a, Vector3i b) => new(a.x * b.x, a.y * b.y, a.z * b.z);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3i operator /(Vector3i a, Vector3i b) => new(a.x / b.x, a.y / b.y, a.z / b.z);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Vector3i operator *(Vector3i a, int s) => new(a.x * s, a.y * s, a.z * s);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Vector3i operator *(Vector3i a, float s) => new((int)(a.x * s), (int)(a.y * s), (int)(a.z * s));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Vector3i operator *(Vector3i a, double s) => new((int)(a.x * s), (int)(a.y * s), (int)(a.z * s));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Vector3i operator /(Vector3i a, int s) => new(a.x / s, a.y / s, a.z / s);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Vector3i operator /(Vector3i a, float s) => new((int)(a.x / s), (int)(a.y / s), (int)(a.z / s));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Vector3i operator /(Vector3i a, double s) => new((int)(a.x / s), (int)(a.y / s), (int)(a.z / s));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>

        public static explicit operator Vector3(Vector3i a) => new(a.x, a.y, a.z);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        public static explicit operator Vector3i(Vector3 a) => new((int)a.X, (int)a.Y, (int)a.Z);
    }
}
