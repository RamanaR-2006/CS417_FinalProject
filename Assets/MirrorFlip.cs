using UnityEngine;
using UnityEngine.Rendering;

public class MirrorFlip : MonoBehaviour
{
    Camera mirrorCam;

    void Start()
    {
        mirrorCam = GetComponent<Camera>();
        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
    }

    void OnDestroy()
    {
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
    }

    void OnBeginCameraRendering(ScriptableRenderContext context, Camera cam)
    {
        if (cam == mirrorCam)
        {
            GL.invertCulling = true;
            mirrorCam.projectionMatrix = mirrorCam.projectionMatrix * Matrix4x4.Scale(new Vector3(-1, 1, 1));
        }
    }

    void OnEndCameraRendering(ScriptableRenderContext context, Camera cam)
    {
        if (cam == mirrorCam)
        {
            GL.invertCulling = false;
            mirrorCam.ResetProjectionMatrix();
        }
    }
}