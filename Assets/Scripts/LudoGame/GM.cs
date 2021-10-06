using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private Pawn _pickedPawn = null;

    [SerializeField]
    private List<Material> playerMaterials;
    private Dice dice;
    private Player currentPlayer;
    private List<Player> players;
    private static bool gameFinished = false;
    private bool isTurnGoing = false;
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

    private void rollingFinished()
    {
        _isDiceRolling = false;
    }
    private void requestRoll()
    {
        if (!_mayStartRolling && !gameFinished)
        {
            _diceCam.enabled = true;
            _mayStartRolling = true;
            _rollDiceButton.interactable = false;
        }
    }

    private void pawnPicked(Pawn p)
    {
        if (currentPlayer.GetPawns().Contains(p))
        {
            _pickedPawn = p;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        dice = GameObject.FindGameObjectWithTag("Dice").GetComponent<Dice>();
        _diceCam.enabled = false;

        players = new List<Player>();
        List<Photon.Realtime.Player> punplayers = new List<Photon.Realtime.Player>(PhotonNetwork.PlayerList);
        int photonIndex = 0;
        foreach (Material m in playerMaterials)
        {
            if (photonIndex < punplayers.Count)
            {
                players.Add(new Player(m, punplayers[photonIndex]));
                photonIndex++;
            }
            else
            {
                players.Add(new Player(m));
            }
        }
        currentPlayer = players[0];

    }
    private void StartTurn()
    {
        isTurnGoing = true;
        StartCoroutine("Turn");
    }
    public IEnumerator Turn()
    {
        _currentPlayerText.color = _pawnMaterials[currentPlayer.GetPlayerNumber() - 1].color;
        _currentPlayerText.text = "Player " + currentPlayer.GetPlayerNumber();

        if (_automaticThrows)
        {
            requestRoll();
        }

        yield return new WaitUntil(() => _mayStartRolling);
        _mayStartRolling = false;

        dice.RollDice();
        _isDiceRolling = true;

        yield return new WaitUntil(() => !_isDiceRolling);

        int result = dice.GetResult();
        //chose move for player.
        //Debug.Log("player " + currentPlayer.GetPlayerNumber() + "rolled a " + result);
        bool hasMoves = currentPlayer.AssignMoves(result);
        if (hasMoves)
        {
            yield return new WaitUntil(() => _pickedPawn != null);
        }
        _pickedPawn = null;
        currentPlayer.ChooseMove(result);
        currentPlayer.clearPawnSuggestions();
        currentPlayer = players[currentPlayer.GetPlayerNumber() % 4];
        //yield return new WaitForSeconds(10f);
        _diceCam.enabled = false;
        _rollDiceButton.interactable = true;
        isTurnGoing = false;
        Debug.Log("_________________________________________");
    }


    // Update is called once per frame
    void Update()
    {

        if (!gameFinished && !isTurnGoing && Time.frameCount > 10)
        {
            StartTurn();
        }
    }

    public List<Player> GetPlayers()
    {
        return players;
    }

}
