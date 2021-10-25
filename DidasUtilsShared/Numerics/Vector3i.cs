using System;

namespace DidasUtils.Numerics
{
    public readonly struct Vector3i
    {
        public readonly int x;
        public readonly int y;
        public readonly int z;
        public float Magnitude { get => (float)Math.Sqrt(x * x + y * y + z * z); }



        public static readonly Vector3i Zero = new Vector3i(0, 0, 0);



        public Vector3i(int x, int y, int z) { this.x = x; this.y = y; this.z = z; }



        public static float DotProduct(Vector3i a, Vector3i b) => a.x * b.x + a.y * b.y + a.z * b.z;
        public static float Angle(Vector3i a, Vector3i b) => (float)Math.Acos(DotProduct(a, b) / (a.Magnitude * b.Magnitude));



        public static Vector3i operator +(Vector3i a) => a;
        public static Vector3i operator -(Vector3i a) => new Vector3i(-a.x, -a.y, -a.z);
        public static Vector3i operator +(Vector3i a, Vector3i b) => new Vector3i(a.x + b.x, a.y + b.y, a.z + b.z);
        public static Vector3i operator -(Vector3i a, Vector3i b) => new Vector3i(a.x - b.x, a.y - b.y, a.z - b.z);
        public static Vector3i operator *(Vector3i a, Vector3i b) => new Vector3i(a.x * b.x, a.y * b.y, a.z * b.z);
        public static Vector3i operator /(Vector3i a, Vector3i b) => new Vector3i(a.x / b.x, a.y / b.y, a.z / b.z);
    }
}
