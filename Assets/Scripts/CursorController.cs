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

    private Vector2 selectPointer = Vector2.zero;
    private Vector2 paintPointer = new Vector2(0, 64);
    private Vector2 boxPointer = new Vector2(0, 64);
    private Vector2 fillPointer = new Vector2(32, 55);
    private Vector2 erasePointer = new Vector2(0, 64);
    private Vector2 dragPointer = new Vector2(32, 32);
    private Vector2 pastePointer = new Vector2(0, 64);

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
                Cursor.SetCursor(select, selectPointer, CursorMode.Auto);
                break;
            case ToolState.Paint:
                Cursor.SetCursor(paint, paintPointer, CursorMode.Auto);
                break;
            case ToolState.Box:
                Cursor.SetCursor(box, boxPointer, CursorMode.Auto);
                break;
            case ToolState.Fill:
                Cursor.SetCursor(fill, fillPointer, CursorMode.Auto);
                break;
            case ToolState.Erase:
                Cursor.SetCursor(erase, erasePointer, CursorMode.Auto);
                break;
            case ToolState.Drag:
                Cursor.SetCursor(drag, dragPointer, CursorMode.Auto);
                break;
            case ToolState.Paste:
                Cursor.SetCursor(paste, pastePointer, CursorMode.Auto);
                break;

        }
    }
}
