using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using jomi.utils;
using jomi.vis;
using UnityEngine.UIElements;

namespace jomi.CharController3D {
    public class PlayerLookMouse : MonoBehaviour {

        //private float _xRotation;
        public Camera cam;
        public bool DebugPosition;
        public Mesh mesh;
        public Material mat;

        private void Update() {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Vector3 pos = jaux.dontknow(ray.origin, ray.direction, transform.position.y);
            this.transform.forward = pos - transform.position;
            if(DebugPosition)Draw.Mesh(mesh,pos,0.2f,mat);
        }
    }
}
