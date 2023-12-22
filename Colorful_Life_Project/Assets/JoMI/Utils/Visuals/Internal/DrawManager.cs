using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace jomi.vis.Internal {
    public static class DrawManager {

        public static CommandBuffer cmd;
        static int lastFrameWithDrawCommands;
        const string buffName = "Vis Draw Commands";
        public static MaterialPropertyBlock materialProperties;


        public static void EnsureFrameInitialized()
        {
            // Only need to init if this is the first draw request of current frame
            if (lastFrameWithDrawCommands != Time.frameCount)
            {

                lastFrameWithDrawCommands = Time.frameCount;
                if (cmd == null)
                {

                    cmd = new CommandBuffer();
                    cmd.name = buffName;
                }
                cmd.Clear();

                DrawMaterials.Init();

                materialProperties ??= new MaterialPropertyBlock();
            }
        }


        static void OnPreRender(Camera cam)
        {
            CameraEvent drawCameraEvent = CameraEvent.BeforeImageEffects; //BeforeImageEffects
            var allBuffers = cam.GetCommandBuffers(drawCameraEvent);

            // Remove buffer by name.
            // This is done because there are situations in which the buffer can be
            // null (but still attached to camera), and I don't want to think about it.
            foreach (var b in allBuffers)
                if (string.Equals(b.name, buffName, System.StringComparison.Ordinal))
                    cam.RemoveCommandBuffer(drawCameraEvent, b);


            if (lastFrameWithDrawCommands == Time.frameCount && cmd != null)
                cam.AddCommandBuffer(drawCameraEvent, cmd);
        }


        // Called on enter playmode (before awake), and on script recompile in editor
        static void Init()
        {
            lastFrameWithDrawCommands = -1;
            Camera.onPreRender -= OnPreRender;
            Camera.onPreRender += OnPreRender;
        }

    #if UNITY_EDITOR
        [UnityEditor.Callbacks.DidReloadScripts]
    #endif

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void InitializeOnLoad()
        {
            Init();
        }
    }
}