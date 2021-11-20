using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PawnMouseInteractions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public delegate void PickAction(Move m);
    public static event PickAction pawnPicked;

    private Texture2D _pointerCursorTexture;
    private Vector2 _mouseTarget;

    private Material _material;
    private Color _pawnColor;

    private Material _destinationCellMaterial = null;
    private Color _destinationCellColor;

    private Material _altDestinationCellMaterial = null;
    private Color _altDestinationCellColor;

    private Move _possibleMove = null;
    private Move _altMove = null;

    private bool _belongsToCurrentPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
        _pawnColor = _material.color;

        _pointerCursorTexture = (Texture2D)Resources.Load("Cursors/pointer");

        //35,0 son le coordinate in pixel del punto di indicatamento nell'immagine (al momento 128x128)
        _mouseTarget = new Vector2(35, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }
    //aggiungere circa ovunque if(PhotonNetwork.localPlayer.NickName == currentPlayer.GetPhotonNickName()){}
    public void assignPossibleMove(Move m, bool belongsToCurrentPlayer)
    {
        _destinationCellMaterial = m.GetFinishCell().gameObject.GetComponent<MeshRenderer>().material;
        _destinationCellColor = _destinationCellMaterial.color;
        _possibleMove = m;
        _belongsToCurrentPlayer = belongsToCurrentPlayer;
    }

    public void assignAltMove(Move m, bool belongsToCurrentPlayer)
    {
        _altDestinationCellMaterial = m.GetFinishCell().gameObject.GetComponent<MeshRenderer>().material;
        _altDestinationCellColor = _altDestinationCellMaterial.color;
        _altMove = m;
        _belongsToCurrentPlayer = belongsToCurrentPlayer;
    }

    public void Hover()
    {
        if (_belongsToCurrentPlayer)
        {
            if (Keyboard.current.ctrlKey.isPressed)
            {
                if (_altMove != null)
                {
                    Cursor.SetCursor(_pointerCursorTexture, _mouseTarget, CursorMode.Auto);

                    _material.EnableKeyword("_EMISSION");
                    _material.SetColor("_EmissionColor", _pawnColor * 7);

                    _altDestinationCellMaterial.EnableKeyword("_EMISSION");
                    _altDestinationCellMaterial.SetColor("_EmissionColor", _altDestinationCellColor * 7);

                    DynamicGI.UpdateEnvironment();
                }
            }
            else if (_possibleMove != null)
            {
                Cursor.SetCursor(_pointerCursorTexture, _mouseTarget, CursorMode.Auto);

                _material.EnableKeyword("_EMISSION");
                _material.SetColor("_EmissionColor", _pawnColor * 7);

                _destinationCellMaterial.EnableKeyword("_EMISSION");
                _destinationCellMaterial.SetColor("_EmissionColor", _destinationCellColor * 7);

                DynamicGI.UpdateEnvironment();
            }
        }
    }

    public void UnHover()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        _material.DisableKeyword("_EMISSION");
        if (_destinationCellMaterial != null)
        {
            _destinationCellMaterial.DisableKeyword("_EMISSION");
        }
        if (_altDestinationCellMaterial != null)
        {
            _altDestinationCellMaterial.DisableKeyword("_EMISSION");
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

    public void ExecuteClick(bool forced = false)
    {
        if (_belongsToCurrentPlayer || forced)
        {
            if (Keyboard.current.ctrlKey.isPressed)
            {
                if (_altMove != null)
                {
                    UnHover();
                    pawnPicked(_altMove);
                }
            }
            else if (_possibleMove != null)
            {
                UnHover();
                pawnPicked(_possibleMove);
            }
        }
    }
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        ExecuteClick();
    }

    //USED ONLY FOR BOTS?
    public Move GetPossibleMove()
    {
        if (_possibleMove != null && !Keyboard.current.ctrlKey.isPressed)
        {
            return _possibleMove;
        }
        else if (_altMove != null && Keyboard.current.ctrlKey.isPressed && _belongsToCurrentPlayer)
        {
            return _altMove;
        }
        return null;
    }
    public void clearCell()
    {
        _possibleMove = null;
        _altMove = null;
    }

}
