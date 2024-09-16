using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PortalScript : MonoBehaviour
{
    public PortalScript linkedPortal;
    public MeshRenderer screen;
    Camera playerCam;
    Camera portalCam;
    RenderTexture viewTexture;

    void Awake()
    {
        playerCam = Camera.main;
        portalCam = GetComponentInChildren<Camera>();
    }

    void CreateViewTexture()
    {
        if(viewTexture == null || viewTexture.width != Screen.width || viewTexture.height != Screen.height)
        {
            if(viewTexture != null)
            {
                viewTexture.Release();
            }
            viewTexture = new RenderTexture(Screen.width, Screen.height, 0);

            portalCam.targetTexture = viewTexture;

            linkedPortal.screen.material.SetTexture("_Texture", viewTexture);
        }
    }

    private void OnDestroy()
    {
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
    }

    public void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        if (camera != playerCam)
            return;

        screen.enabled = false;
        CreateViewTexture();

        Matrix4x4 m = linkedPortal.transform.worldToLocalMatrix * transform.localToWorldMatrix * playerCam.transform.localToWorldMatrix;
        portalCam.transform.SetPositionAndRotation(m.GetColumn(3), m.rotation);

        portalCam.Render();

        screen.enabled = true;
    }

    void LateUpdate()
    {
        
    }
}
