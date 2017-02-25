using UnityEngine;

public class BlurredOutlinePostEffect : MonoBehaviour {
    private Camera mainCamera;
    public Shader BlurredOutlineShader;
    public Shader WhiteShader;
    private Camera temporaryCamera;
    private Material postEffectMaterial;

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
        temporaryCamera = new GameObject().AddComponent<Camera>();
        temporaryCamera.enabled = false;
        postEffectMaterial = new Material(BlurredOutlineShader);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        SetUpTemporaryCamera();
        var temporaryRenderTexture = new RenderTexture(source.width, source.height, 0, RenderTextureFormat.R8);
        RenderOutlineObjectsToTexture(temporaryRenderTexture);
        RenderDestinationTexture(source, destination, temporaryRenderTexture);
        temporaryRenderTexture.Release();
    }

    private void SetUpTemporaryCamera()
    {
        temporaryCamera.CopyFrom(mainCamera);
        temporaryCamera.clearFlags = CameraClearFlags.Color;
        temporaryCamera.backgroundColor = Color.black;
        temporaryCamera.cullingMask = 1 << LayerMask.NameToLayer("Outline");
    }

    private void RenderOutlineObjectsToTexture(RenderTexture temporaryTexture)
    {
        temporaryTexture.Create();
        temporaryCamera.targetTexture = temporaryTexture;
        temporaryCamera.RenderWithShader(WhiteShader, "");
    }

    private void RenderDestinationTexture(Texture source, RenderTexture destination, Texture temporaryTexture)
    {
        postEffectMaterial.SetTexture("_SceneTex", source);
        Graphics.Blit(temporaryTexture, destination, postEffectMaterial);
    }
}
