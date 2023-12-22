using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jomi.vis.Internal {
    public static class DrawMaterials {
        public static Material pointMat { get; private set; }
        public static Material quadMat { get; private set; }

        public static readonly int colorID = Shader.PropertyToID("_Color");
        public static readonly int sizeID = Shader.PropertyToID("_Size");
        public static readonly int Scale = Shader.PropertyToID("_Scale");

        public static readonly int quadPointA = Shader.PropertyToID("PosA");
        public static readonly int quadPointB = Shader.PropertyToID("PosB");
        public static readonly int quadPointC = Shader.PropertyToID("PosC");
        public static readonly int quadPointD = Shader.PropertyToID("PosD");


        public static void Init() {
            if (pointMat == null) pointMat = new Material(Shader.Find("Vis/UnlitPoint"));

            if (quadMat == null) quadMat = new Material(Shader.Find("Vis/Quad"));
        }
    }
}
