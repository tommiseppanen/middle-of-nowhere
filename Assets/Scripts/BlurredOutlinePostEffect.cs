using UnityEngine;

namespace MiddleOfNowhere.Scripts
{
    public class BlurredOutlinePostEffect : MonoBehaviour
    {
        private Camera _mainCamera;
        public Shader BlurredOutlineShader;
        public Shader WhiteShader;
        private Camera _temporaryCamera;
        private Material _postEffectMaterial;

        private void Start()
        {
            _mainCamera = GetComponent<Camera>();
            _temporaryCamera = new GameObject().AddComponent<Camera>();
            _temporaryCamera.enabled = false;
            _postEffectMaterial = new Material(BlurredOutlineShader);
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
            _temporaryCamera.CopyFrom(_mainCamera);
            _temporaryCamera.clearFlags = CameraClearFlags.Color;
            _temporaryCamera.backgroundColor = Color.black;
            _temporaryCamera.cullingMask = 1 << LayerMask.NameToLayer("Outline");
        }

        private void RenderOutlineObjectsToTexture(RenderTexture temporaryTexture)
        {
            temporaryTexture.Create();
            _temporaryCamera.targetTexture = temporaryTexture;
            _temporaryCamera.RenderWithShader(WhiteShader, "");
        }

        private void RenderDestinationTexture(Texture source, RenderTexture destination, Texture temporaryTexture)
        {
            _postEffectMaterial.SetTexture("_SceneTex", source);
            Graphics.Blit(temporaryTexture, destination, _postEffectMaterial);
        }
    }
}

