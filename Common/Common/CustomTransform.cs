using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class CustomTransform
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public float RX { get; set; }
        public float RY { get; set; }
        public float RZ { get; set; }

        public CustomTransform(float x, float y, float z, float rx, float ry, float rz)
        {
            X = x;
            Y = y;
            Z = z;
            RX = rx;
            RY = ry;
            RZ = rz;
        }

        public CustomTransform()
        {
        }
    }
}
