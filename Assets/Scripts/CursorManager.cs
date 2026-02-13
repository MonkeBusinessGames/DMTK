using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public enum ToolState
{
    Select,
    Paint,
    Box,
    Fill,
    Erase,
    Drag,
    Paste
}

public class CursorController : MonoBehaviour
{
    public ToolState tool;
    [SerializeField] Texture2D select;
    [SerializeField] Texture2D paint;
    [SerializeField] Texture2D box;
    [SerializeField] Texture2D fill;
    [SerializeField] Texture2D erase;
    [SerializeField] Texture2D drag;
    [SerializeField] Texture2D paste;

    public static CursorController Instance;

    private void Awake()
    {
        //Prevent duplicates of this object from existing
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        //Make this object accessible to other objects and don't destory it.
        Instance = this;
    }

    public void SetCursor(ToolState newState)
    {
        tool = newState;

        switch (newState)
        {
            case ToolState.Select:
                Cursor.SetCursor(select, Vector2.zero, CursorMode.Auto);
                break;
            case ToolState.Paint:
                Cursor.SetCursor(paint, Vector2.zero, CursorMode.Auto);
                break;
            case ToolState.Box:
                Cursor.SetCursor(box, Vector2.zero, CursorMode.Auto);
                break;
            case ToolState.Fill:
                Cursor.SetCursor(fill, Vector2.zero, CursorMode.Auto);
                break;
            case ToolState.Erase:
                Cursor.SetCursor(erase, Vector2.zero, CursorMode.Auto);
                break;
            case ToolState.Drag:
                Cursor.SetCursor(drag, Vector2.zero, CursorMode.Auto);
                break;
            case ToolState.Paste:
                Cursor.SetCursor(paste, Vector2.zero, CursorMode.Auto);
                break;

        }
    }
}
