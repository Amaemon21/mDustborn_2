using UnityEngine;

public abstract class UIScreen : MonoBehaviour
{    
    public bool IsOpen { get; private set; } = false;

    public void Open()
    {
        gameObject.SetActive(true);
        IsOpen = true;
        OnOpen();
    }
    
    public void Close()
    {
        gameObject.SetActive(false);
        IsOpen = false;
        OnClose();
    }

    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    protected virtual void OnOpen() { }
    protected virtual void OnClose() { }
}