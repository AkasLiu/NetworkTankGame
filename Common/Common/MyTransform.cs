using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    /// <summary>
    /// 自定义Transform
    /// 保存的是欧拉角而不是四元数
    /// </summary>
    public class MyTransform
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public float RX { get; set; }
        public float RY { get; set; }
        public float RZ { get; set; }

        public MyTransform(float x, float y, float z, float rx, float ry, float rz)
        {
            X = x;
            Y = y;
            Z = z;
            RX = rx;
            RY = ry;
            RZ = rz;
        }

        public MyTransform()
        {
        }
    }
}
