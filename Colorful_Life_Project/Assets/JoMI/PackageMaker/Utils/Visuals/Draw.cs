using UnityEngine;
using jomi.vis.Internal;
using static jomi.vis.Internal.DrawManager;
using jomi.vis.Internal.MeshGenerator;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.UIElements;

namespace jomi.vis {
    public static class Draw {

        public static void Mesh(Mesh mesh, Matrix4x4 matrix, Material material)
        {
            EnsureFrameInitialized();
            cmd.DrawMesh(mesh, matrix, material, 0, 0);
        }

        public static void Mesh(Mesh mesh, Material material)
        {
            Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
            Mesh(mesh, matrix, material);
        }

        public static void Mesh(Mesh mesh, Vector3 position, Material material)
        {
            Matrix4x4 matrix = Matrix4x4.TRS(position, Quaternion.identity, Vector3.one);
            Mesh(mesh, matrix, material);
        }

        public static void Mesh(Mesh mesh, Vector3 position,float scale, Material material)
        {
            Matrix4x4 matrix = Matrix4x4.TRS(position, Quaternion.identity, new Vector3(scale,scale,scale));
            Mesh(mesh, matrix, material);
        }


        public static void Point(Vector3 centre, float radius, Color col)
        {
            if (radius == 0 || col.a == 0) { return; }

            EnsureFrameInitialized();

            Vector3 scale = new(radius * 2, radius * 2, 1);
            Matrix4x4 matrix = Matrix4x4.TRS(centre, Quaternion.identity, scale);
            Mesh mesh = QuadMeshGenerator.GetQuadMesh();
            materialProperties.SetColor(DrawMaterials.colorID, col);
            cmd.DrawMesh(mesh, matrix, DrawMaterials.pointMat, 0, 0, materialProperties);
        }

        public static void Quad(Vector2 a, Vector2 b, Vector2 c, Vector2 d, Color col)
        {
            EnsureFrameInitialized();
            Mesh mesh = QuadMeshGenerator.GetQuadMesh();
            materialProperties.SetColor(DrawMaterials.colorID, col);
            materialProperties.SetVector(DrawMaterials.quadPointA, a);
            materialProperties.SetVector(DrawMaterials.quadPointB, b);
            materialProperties.SetVector(DrawMaterials.quadPointC, c);
            materialProperties.SetVector(DrawMaterials.quadPointD, d);
            cmd.DrawMesh(mesh, Matrix4x4.identity, DrawMaterials.quadMat, 0, 0, materialProperties);
        }
    }
}
