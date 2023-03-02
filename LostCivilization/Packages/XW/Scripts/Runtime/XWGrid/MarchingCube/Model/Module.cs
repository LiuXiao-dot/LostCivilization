using System;
using UnityEngine;
namespace XWGrid.MarchingCube
{
    /// <summary>
    /// 一个模块
    /// </summary>
    [Serializable]
    public class Module
    {
        public Mesh mesh;

        public Module(Mesh mesh)
        {
            this.mesh = mesh;
        }
    }
}