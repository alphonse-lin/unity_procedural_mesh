using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PM.GeometryClass;
using System;

namespace PM.Application{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class PM_MeshExtrude : MonoBehaviour
    {
        private bool invertFaces = true;
        private MeshExtrusion.Edge[] precomputedEdges;

        private Matrix4x4[] matrix = {
            new Matrix4x4(
            new Vector4(1,0,0,0),
            new Vector4(0,1,0,0),
            new Vector4(0,0,1,0),
            new Vector4(0,0,0,1)),

            new Matrix4x4(
            new Vector4(1,0,0,0),
            new Vector4(0,1,0,0),
            new Vector4(0,0,1,0),
            new Vector4(0,0,-5,1))
            };

        // Start is called before the first frame update
        void Start()
        {
            var srcMesh = GetComponent<MeshFilter>().mesh;
            precomputedEdges = MeshExtrusion.BuildManifoldEdges(srcMesh);
            MeshExtrusion.ExtrudeMesh(srcMesh, GetComponent<MeshFilter>().mesh, matrix, precomputedEdges, invertFaces);
        }
    }    
}
