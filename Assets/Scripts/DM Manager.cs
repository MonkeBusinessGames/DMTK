using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Assets.Scripts;

public class DMManager : MonoBehaviour
{
    [SerializeField] Camera uiCamera;
    [SerializeField] Camera sceneCamera;
    [SerializeField] Camera playerCamera;
    [SerializeField] PixelPerfectCamera dmCam;
    [SerializeField] PixelPerfectCamera pCam;
    [SerializeField] SceneInput sceneData;

    [SerializeField] private float scrollSpeed = 1;
    [SerializeField] private float dragSpeed = 0.25f;

    public static bool onGrid;

    private InputAction scroll;
    private InputAction rightClick;
    private Vector2 mousePosition;
    private bool isDragging;

    void Awake()
    {
        for(int i = 1; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
            Debug.Log("activated display: " + i);
        }   

        uiCamera.targetDisplay = sceneCamera.targetDisplay = 0;
        playerCamera.targetDisplay = 1;

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Assign the scroll wheel action to scroll
        scroll = InputSystem.actions.FindAction("ScrollWheel");
        rightClick = InputSystem.actions.FindAction("RightClick");
        onGrid = false;
        isDragging = false; 
    }

    // Update is called once per frame
    void Update()
    {

        onGrid = sceneData.isHoveringViewport;
        //Check if on the grid
        if (onGrid)
        {
            //Debug.Log(sceneCamera.aspect);
            //Debug.Log(playerCamera.aspect);
            //Debug.Log(sceneCamera.targetTexture.width);
            //Debug.Log(sceneCamera.targetTexture.height);

            //Zooms the camera if scroll value is found
            int scrollValue = (int) (scroll.ReadValue<Vector2>().y * scrollSpeed * 100 * Time.deltaTime);
            if (scrollValue != 0)
            {
                //pCam.assetsPPU -= scrollValue;
                ////Debug.Log(scrollValue);
                //if (pCam.assetsPPU < 20)
                //    pCam.assetsPPU = dmCam.assetsPPU = 20;
                //else if (pCam.assetsPPU > 200)
                //    pCam.assetsPPU = dmCam.assetsPPU = 200;
                //else
                //    dmCam.assetsPPU = pCam.assetsPPU;


                sceneCamera.orthographicSize -= scrollValue;

                if (sceneCamera.orthographicSize < 2)
                    sceneCamera.orthographicSize = playerCamera.orthographicSize = 2;
                else if (sceneCamera.orthographicSize > 30)
                    sceneCamera.orthographicSize = playerCamera.orthographicSize = 30;
                else
                    playerCamera.orthographicSize = sceneCamera.orthographicSize;
            }
            //Click and drag the camera
            if (!isDragging)
            {
                //If a click is detecting, start dragging
                if (rightClick.IsPressed())
                {
                    isDragging = true;
                    CursorController.Instance.SetCursor(ToolState.Drag);
                    mousePosition = Mouse.current.position.value;
                }
            }
            else
            {
                //If a click is still detected, drag by the mouse movement delta times a dampener value
                if (rightClick.IsPressed())
                {
                    Vector2 newPosition = Mouse.current.position.value;
                    playerCamera.transform.position = sceneCamera.transform.position += (Vector3)((mousePosition - newPosition) * dragSpeed);
                    mousePosition = newPosition;
                }
                //If a click is not detected, stop dragging
                else
                {
                    isDragging = false;
                    CursorController.Instance.SetCursor(ToolState.Select);
                }
            }
        }
    }
}
