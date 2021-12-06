using System;

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
        public static readonly Vector3i Zero = new Vector3i(0, 0, 0);



        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vector3i(int x, int y, int z) { this.x = x; this.y = y; this.z = z; }



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
        /// <param name="a"></param>
        /// <returns></returns>
        public static Vector3i operator +(Vector3i a) => a;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Vector3i operator -(Vector3i a) => new Vector3i(-a.x, -a.y, -a.z);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3i operator +(Vector3i a, Vector3i b) => new Vector3i(a.x + b.x, a.y + b.y, a.z + b.z);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3i operator -(Vector3i a, Vector3i b) => new Vector3i(a.x - b.x, a.y - b.y, a.z - b.z);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3i operator *(Vector3i a, Vector3i b) => new Vector3i(a.x * b.x, a.y * b.y, a.z * b.z);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3i operator /(Vector3i a, Vector3i b) => new Vector3i(a.x / b.x, a.y / b.y, a.z / b.z);
    }
}
