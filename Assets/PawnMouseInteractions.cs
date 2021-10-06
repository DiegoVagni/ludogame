using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PawnMouseInteractions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public delegate void PickAction(Pawn p);
    public static event PickAction pawnPicked;

    private Material _material;
    private Color _pawnColor;

    private Material _destinationCellMaterial = null;
    private Color _destinationCellColor;
    private bool _isMovable = false;

    // Start is called before the first frame update
    void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
        _pawnColor = _material.color;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void assignDestinationCell(Cell cell)
    {
        _destinationCellMaterial = cell.gameObject.GetComponent<MeshRenderer>().material;
        _destinationCellColor = _destinationCellMaterial.color;
        this._isMovable = true;
    }

    public void Hover()
    {
        Debug.Log("A");
        if (_isMovable)
        {
            Debug.Log("B");
            _material.EnableKeyword("_EMISSION");
            _material.SetColor("_EmissionColor", _pawnColor * 7);

            _destinationCellMaterial.EnableKeyword("_EMISSION");
            _destinationCellMaterial.SetColor("_EmissionColor", _destinationCellColor * 7);

            DynamicGI.UpdateEnvironment();
        }
    }

    public void UnHover()
    {
        _material.DisableKeyword("_EMISSION");
        if (_destinationCellMaterial != null)
        {
            _destinationCellMaterial.DisableKeyword("_EMISSION");
        }
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
        if (_isMovable)
        {
            pawnPicked(this.gameObject.GetComponent<Pawn>());
        }
    }

    public void clearCell()
    {
        _isMovable = false;
    }
}
