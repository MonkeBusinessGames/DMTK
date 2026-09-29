using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

namespace Assets.Scripts
{
	public class SceneInput: MonoBehaviour
	{
		public bool isHoveringViewport;
		public Vector2 localPosition;
		public Vector2 viewportPosition;
		public Vector3 worldPosition;
		public Vector2Int gridPosition;
        private int lastWidth;
        private int lastHeight;
        private Display display;
        [SerializeField] private AspectRatioFitter aspectRatioFitter;
        [SerializeField] private RawImage sceneImage;
        [SerializeField] private Camera sceneCamera;
        public int xOffset;
        public int yOffset;


        private void Start()
        {
            if (Display.displays.Length > 1)
            {
                display = Display.displays[1];
            }
            else
                display = Display.main;

            lastWidth = display.renderingWidth;
            lastHeight = display.renderingHeight;

            UpdateViewport();
        }


        private void Update()
        {
            if (display.renderingWidth != lastWidth || display.renderingHeight != lastHeight)
            {

                lastWidth = display.renderingWidth;
                lastHeight = display.renderingHeight;

                UpdateViewport();
            }

            //Check Mouse is in the image
            RectTransform imageRect = sceneImage.rectTransform;

			//Get the local position and whether we are hovering over the scene image
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(imageRect, Mouse.current.position.ReadValue(), null, out localPosition))
            {
                isHoveringViewport = false;
            }
			
			isHoveringViewport = imageRect.rect.Contains(localPosition);    

            //If the mouse is not over the rect, don't bother continuing the rest of the updates
            if (!isHoveringViewport)
				return;

            Rect rect = imageRect.rect;

			//Calculate Viewport position using localPosition and rect
            viewportPosition = new Vector2((localPosition.x - rect.x)/rect.width, (localPosition.y - rect.y)/rect.height);

			//Calculate the world position using the camera and viewport position
			worldPosition = sceneCamera.ViewportToWorldPoint(viewportPosition);

			//Calcauate grid position using world position
			gridPosition = new Vector2Int(Mathf.FloorToInt(worldPosition.x) + xOffset, Mathf.FloorToInt(worldPosition.y) + yOffset);
        }

        private void UpdateViewport()
        {
            Debug.Log("Player Screen size changed to: " + lastWidth + ", " + lastHeight);

            RenderTexture renderTexture = new RenderTexture(lastWidth, lastHeight, 24);

            sceneCamera.targetTexture = renderTexture;
            sceneImage.texture = renderTexture;

            aspectRatioFitter.aspectRatio = (float) lastWidth / (float) lastHeight;

        }

    }
}