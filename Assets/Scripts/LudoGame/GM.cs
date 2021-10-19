using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GM : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _currentPlayerText;
    [SerializeField] private Button _rollDiceButton;
    [SerializeField] private RawImage _diceCam;
    [SerializeField] private List<Material> _pawnMaterials;
    [SerializeField] private bool _automaticThrows;

    private bool _mayStartRolling = false;
    private bool _isDiceRolling = false;
    private Move _pickedMove = null;
    private bool _hasMove = false;

    [SerializeField]
    private List<Material> playerMaterials;
    private Dice dice;
    private Player currentPlayer;
    private List<Player> players;
    private static bool gameFinished = false;
    private bool isTurnGoing = false;
    private int _diceResult = -1;
    private List<bool> _diceThreads;
    private List<Photon.Realtime.Player> _punplayers;

    //scusa fra è giusto al volo per quando finisce il gioco così testo
    public static void EndGame()
    {
        gameFinished = true;
    }

    private void OnEnable()
    {
        Dice.diceRolled += rollingFinished;
        _rollDiceButton.onClick.AddListener(requestRoll);
        PawnMouseInteractions.pawnPicked += pawnPicked;
    }

    private void OnDisable()
    {
        _rollDiceButton.onClick.RemoveListener(requestRoll);
        Dice.diceRolled -= rollingFinished;
        PawnMouseInteractions.pawnPicked -= pawnPicked;
    }

    private void rollingFinished(int result)
    {
        PhotonView pw = PhotonView.Get(this);
        int index = _punplayers.FindIndex((p) => p.NickName == PhotonNetwork.LocalPlayer.NickName);
        pw.RPC("SetDiceThreadFinished", RpcTarget.All, index);

        if (PhotonNetwork.IsMasterClient)
        {
            pw.RPC("AssignResult", RpcTarget.All, result);
        }
        _isDiceRolling = false;
    }


    [PunRPC]
    public void SetDiceThreadFinished(int index)
    {
        _diceThreads[index] = true;
    }

    [PunRPC]
    public void AssignResult(int result)
    {
        _diceResult = result;
    }

    public Player GetPlayerByNumber(int pn)
    {

        return players[pn - 1];
    }
    private void requestRoll()
    {
        PhotonView pw = PhotonView.Get(this);
        pw.RPC("RequestRollRPC", RpcTarget.All);
    }
    [PunRPC]
    private void RequestRollRPC()
    {
        if (!_mayStartRolling && !gameFinished)
        {
            _diceCam.enabled = true;
            _mayStartRolling = true;
            _rollDiceButton.interactable = false;
        }
    }

    private void pawnPicked(Move m)
    {
        _pickedMove = m;
    }

    // Start is called before the first frame update
    void Start()
    {
        dice = GameObject.FindGameObjectWithTag("Dice").GetComponent<Dice>();
        _diceCam.enabled = false;

        players = new List<Player>();
        _punplayers = new List<Photon.Realtime.Player>(PhotonNetwork.PlayerList);
        _diceThreads = new List<bool>(new bool[_punplayers.Count]);
        int photonIndex = 0;
        foreach (Material m in playerMaterials)
        {
            if (photonIndex < _punplayers.Count)
            {
                players.Add(new Player(m, _punplayers[photonIndex]));
                photonIndex++;
            }
            else
            {
                players.Add(new Player(m));
            }
        }
        currentPlayer = players[0];

    }
    //[PunRPC]
    private void StartTurn()
    {
        isTurnGoing = true;
        StartCoroutine("Turn");
    }
    public IEnumerator Turn()
    {
        //Debug.LogError("Giovanni rana");
        _currentPlayerText.color = _pawnMaterials[currentPlayer.GetPlayerNumber() - 1].color;
        _currentPlayerText.text = string.Format("Player {0} ({1})", currentPlayer.GetPlayerNumber(), currentPlayer.GetPhotonNickName());
        _hasMove = false;
        _diceThreads = new List<bool>(new bool[_punplayers.Count]);

        if (_automaticThrows || (PhotonNetwork.IsMasterClient && currentPlayer.GetPlayerNumber().ToString() == currentPlayer.GetPhotonNickName()))
        {
            requestRoll();
        }

        _rollDiceButton.interactable = PhotonNetwork.LocalPlayer.NickName == currentPlayer.GetPhotonNickName();
        yield return new WaitUntil(() => _mayStartRolling);
        _mayStartRolling = false;

        dice.RollDice();
        _isDiceRolling = true;

        yield return new WaitUntil(() => !_isDiceRolling && _diceResult > 0 && !_diceThreads.Contains(false));

        //int result = dice.GetResult();
        Debug.LogError(_diceResult + " vs " + dice.GetResult());
        bool hasMoves = currentPlayer.AssignMoves(_diceResult, PhotonNetwork.LocalPlayer.NickName == currentPlayer.GetPhotonNickName());
        if (hasMoves)
        {
            if (PhotonNetwork.IsMasterClient && currentPlayer.GetPlayerNumber().ToString() == currentPlayer.GetPhotonNickName())
            {
                Debug.Log("controllo l'ia");
                foreach (Pawn p in currentPlayer.GetPawns())
                {
                    if (p.GetComponent<PawnMouseInteractions>().GetPossibleMove() != null)
                    {
                        p.GetComponent<PawnMouseInteractions>().ExecuteClick(true);
                        //servono gli eventi
                        //ExecuteEvents.Execute<IPointerClickHandler>(p.GetPawn(), new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
                        break;
                    }
                }
                yield return new WaitUntil(() => _pickedMove != null);
                PhotonView ps = PhotonView.Get(this);
                ps.RPC("ChoosedMoveApplication", RpcTarget.All, new object[] { _pickedMove.GetPawn()[0].GetPawnName() });
            }
            else if (PhotonNetwork.LocalPlayer.NickName == currentPlayer.GetPhotonNickName())
            {
                yield return new WaitUntil(() => _pickedMove != null);
                PhotonView ps = PhotonView.Get(this);
                ps.RPC("ChoosedMoveApplication", RpcTarget.All, new object[] { _pickedMove.GetPawn()[0].GetPawnName() });
            }
            yield return new WaitUntil(() => { return _hasMove; });
        }
        else
        {
            _hasMove = true;
            //yield return new WaitForSeconds(0.5f);
        }
        _pickedMove = null;
        currentPlayer.clearPawnSuggestions();
        //sarebbe come + 1 per il fatto che il conteggio parte da 1
        currentPlayer = players[currentPlayer.GetPlayerNumber() % 4];
        _diceCam.enabled = false;
        isTurnGoing = false;
        _diceResult = -1;
    }
    [PunRPC]
    public void ChoosedMoveApplication(string pawn)
    {
        //pawname del pawn che si muove
        Pawn p = currentPlayer.GetPawn(pawn);
        if (p == null)
        {
            Debug.LogError("pawn " + pawn + " not found in " + currentPlayer.GetPlayerNumber());
            return;
        }
        currentPlayer.ChooseMove(p.GetComponent<PawnMouseInteractions>().GetPossibleMove());
        _hasMove = true;
    }
    [PunRPC]
    private void EndTurn()
    {

        isTurnGoing = false;
        Debug.Log("_________________________________________");
    }
    // Update is called once per frame
    void Update()
    {

        if (!gameFinished && !isTurnGoing && Time.frameCount > 10)
        {
            //PhotonView pw = PhotonView.Get(this);
            //pw.RPC("StartTurn", RpcTarget.All);
            StartTurn();
        }
    }

    public List<Player> GetPlayers()
    {
        return players;
    }

}
