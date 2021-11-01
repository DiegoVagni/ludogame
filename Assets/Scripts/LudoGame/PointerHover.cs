using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerHover : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    private Texture2D _pointerCursorTexture;
    private Vector2 _mouseTarget;

    public bool _enabled { private get; set; } = true;

    // Start is called before the first frame update
    void Start()
    {
        _pointerCursorTexture = (Texture2D)Resources.Load("Cursors/pointer");

        //35,0 son le coordinate in pixel del punto di indicatamento nell'immagine (al momento 128x128)
        _mouseTarget = new Vector2(35, 0);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_enabled)
        {
            Cursor.SetCursor(_pointerCursorTexture, _mouseTarget, CursorMode.Auto);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
