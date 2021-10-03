using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PawnMouseInteractions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Material _material;
    Color pawnColor;

    // Start is called before the first frame update
    void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
        pawnColor = _material.color;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Hover()
    {
        _material.EnableKeyword("_EMISSION");
        _material.SetColor("_EmissionColor", pawnColor * 7);
        DynamicGI.UpdateEnvironment();
    }

    public void UnHover()
    {
        _material.DisableKeyword("_EMISSION");
        DynamicGI.UpdateEnvironment();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Hover();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UnHover();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked");
    }
}
