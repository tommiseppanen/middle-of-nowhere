using System.Collections;
using UnityEngine;

namespace MiddleOfNowhere.Scripts
{
    public class Tile : InteractiveItemBase
    {
        public Menu Menu { get; set; }
        public Project ProjectData { get; set; }

        public IEnumerator LoadImage()
        {
            var rendererComponent = GetComponent<Renderer>();
            var imageRequest = new WWW(ProjectData.ImageUrl);
            yield return imageRequest;
            SetMaterialParameters(rendererComponent.material, imageRequest.texture);
        }

        private static void SetMaterialParameters(Material target, Texture texture)
        {
            target.mainTexture = texture;
            var height = texture.height;
            var width = texture.width;
            if (height > width)
            {
                var yScale = (float)width / height;
                target.mainTextureScale = new Vector2(1.0f, yScale);
                target.mainTextureOffset = new Vector2(0.0f, 1.0f - yScale);
            }
            else
                target.mainTextureScale = new Vector2((float)height / width, 1.0f);
        }

        protected override void HandleClick()
        {
            Menu.ShowProjectDetails(ProjectData);
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
}
