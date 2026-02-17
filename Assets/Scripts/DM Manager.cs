using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.EventSystems;

public class DMManager : MonoBehaviour
{
    [SerializeField] Camera dmCamera;
    [SerializeField] Camera playerCamera;
    [SerializeField] PixelPerfectCamera dmCam;
    [SerializeField] PixelPerfectCamera pCam;
    private InputAction scroll;
    private InputAction rightClick;
    private Vector2 mousePosition;
    public static bool onGrid = true;
    private bool isDragging = false;

    void Awake()
    {
        for(int i = 1; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }
        dmCamera.targetDisplay = 0;
        playerCamera.targetDisplay = 1;

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Assign the scroll wheel action to scroll
        scroll = InputSystem.actions.FindAction("ScrollWheel");
        rightClick = InputSystem.actions.FindAction("RightClick");
        onGrid = true;
        isDragging = false;
        
    }

    // Update is called once per frame
    void Update()
    {

        //Check if on the grid
        if(!EventSystem.current.IsPointerOverGameObject())
        {
            //Zooms the camera if scroll value is found
            int scrollValue = (int) (scroll.ReadValue<Vector2>().y * 500 * Time.deltaTime);
            if (scrollValue != 0)
            {
                pCam.assetsPPU -= scrollValue;
                //Debug.Log(scrollValue);
                if (pCam.assetsPPU < 20)
                    pCam.assetsPPU = dmCam.assetsPPU = 20;
                else if (pCam.assetsPPU > 200)
                    pCam.assetsPPU = dmCam.assetsPPU = 200;
                else
                    dmCam.assetsPPU = pCam.assetsPPU;
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
                    playerCamera.transform.position = dmCamera.transform.position += (Vector3)((mousePosition - newPosition) * .25f);
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
