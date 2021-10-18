using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PawnMouseInteractions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public delegate void PickAction(Move m);
    public static event PickAction pawnPicked;

    private Material _material;
    private Color _pawnColor;

    private Material _destinationCellMaterial = null;
    private Color _destinationCellColor;
    private Move _possibleMove = null;

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
    //aggiungere circa ovunque if(PhotonNetwork.localPlayer.NickName == currentPlayer.GetPhotonNickName()){}
    public void assignPossibleMove(Move m)
    {
        _destinationCellMaterial = m.GetFinishCell().gameObject.GetComponent<MeshRenderer>().material;
        _destinationCellColor = _destinationCellMaterial.color;
        _possibleMove = m;
    }

    public void Hover()
    {
        if (_possibleMove!=null)
        {
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
        if(_possibleMove!=null)
        {
            UnHover();
            pawnPicked(_possibleMove);
        }
    }
    public Move GetPossibleMove() {
        return _possibleMove;
    }
    public void clearCell()
    {
        _possibleMove = null;
    }
}
