using System.Collections;
using TMPro;
using UnityEngine;

public class FlowController : MonoBehaviour {

    public GameObject board;
    public GameObject blackFab;
    public GameObject whiteFab;
    public GameObject turnIndicator;
    public Vector3 blackSpawn;
    public Vector3 whiteSpawn;
    public TextMeshProUGUI blackScoreText;
    public TextMeshProUGUI whiteScoreText;
    public GameObject blackAlert;
    public GameObject whiteAlert;
    public GameObject blackWin;
    public GameObject whiteWin;
    public GameObject draw;

    private GameObject[ , ] space;
    private GameObject[ , ] pieces;
    private State gameState;

    private bool gameOver = false;
    
    void Start() {

        space = new GameObject[8 , 8];
        pieces = new GameObject[8 , 8];

        Transform b = board.transform;
        for(int i = 0; i < 8; i++) {
            Transform row = b.GetChild(i);
            for(int j = 0; j < 8; j++)
                space[i , j] = row.GetChild(j).gameObject;
        }

        ArrayList blackList = new ArrayList();
        ArrayList whiteList = new ArrayList();

        blackList.Add(new Space(3, 3));
        blackList.Add(new Space(4, 4));
        whiteList.Add(new Space(3, 4));
        whiteList.Add(new Space(4, 3));

        pieces = new GameObject[8 , 8];
        pieces[3 , 3] = GameObject.Find("Black");
        pieces[4 , 4] = GameObject.Find("Black (1)");
        pieces[3 , 4] = GameObject.Find("White");
        pieces[4 , 3] = GameObject.Find("White (1)");

        gameState = new State(true, blackList, whiteList);

        enableOptions();
        
    }

    void enableOptions() {

        if(Settings.playerIsBlack == gameState.isBlackTurn()) {
            ArrayList moves = gameState.getMoves();
            foreach(Space move in moves) {
                space[move.y, move.x].GetComponent<Clickable>().enabled = true;
            }
        }
        else
            BroadcastMessage("aiMoves");

    }

    void addToBoard(Space selected) {

        BroadcastMessage("disable");

        GameObject piece = null;
        Vector3 target;
        bool isBlack;

        target = space[selected.y , selected.x].GetComponent<Transform>().position;
        target.y = 0.5f;

        isBlack = gameState.isBlackTurn();

        switch(isBlack) {
        case true:
            piece = Instantiate(blackFab, blackSpawn, new Quaternion());
            break;
        case false:
            piece = Instantiate(whiteFab, whiteSpawn, new Quaternion());
            break;
        }
        pieces[selected.x , selected.y] = piece;

        Transform piecePos = piece.GetComponent<Transform>();
        StartCoroutine(moveToTarget(piecePos, selected, target));

    }

    IEnumerator moveToTarget(Transform piece, Space selected, Vector3 target) {

        float delay = 0.01f;
        float speed = 0.5f;

        while(piece.position != target) {
            yield return new WaitForSeconds(delay);
            piece.position = Vector3.MoveTowards(piece.position, target, speed);
        }

        setState(selected);
        if(!gameOver)
            enableOptions();

    }

    void setState(Space selected) {

        ArrayList piecesToFlip = new ArrayList();
        int blackCount = gameState.getBlackList().Count;
        int whiteCount = gameState.getWhiteList().Count;
        gameState = gameState.calculateNextState(selected.x, selected.y, piecesToFlip);
        switch(!gameState.isBlackTurn()) {
        case true:
            blackCount++;
            StartCoroutine(flip(piecesToFlip, blackCount, whiteCount, "Black"));
            break;
        case false:
            whiteCount++;
            StartCoroutine(flip(piecesToFlip, blackCount, whiteCount, "White"));
            break;
        }

        int count = 2;
        do {
            if(gameState.getMoves().Count == 0) {
                gameState.changeTurn();
                gameState.calculateMoves();
                count--;
            }
            else
                break;
        } while(count != 0);

        if(count == 1)
            switch(!gameState.isBlackTurn()) {
            case true:
                outOfMoves(1);
                break;
            case false:
                outOfMoves(2);
                break;
            }
        else if(count == 0)
            endGame();
        else
            disableAlerts();

    }

    IEnumerator flip(ArrayList piece, int blackScore, int whiteScore, string state) {

        foreach (Space p in piece) {
            Animator anim = pieces[p.x, p.y].transform.GetChild(0).GetComponent<Animator>();
            anim.Play(state);

            switch(state) {
            case "Black":
                blackScore++;
                whiteScore--;
                break;
            case "White":
                whiteScore++;
                blackScore--;
                break;
            }

            blackScoreText.SetText(blackScore.ToString());
            whiteScoreText.SetText(whiteScore.ToString());
            yield return new WaitForSeconds(0.2f);
        }

        Animator indicate = turnIndicator.transform.GetChild(0).GetComponent<Animator>();
        switch(gameState.isBlackTurn()) {
        case true:
            indicate.Play("Black");
            break;
        case false:
            indicate.Play("White");
            break;
        }

    }

    void outOfMoves(int piece) {

        switch(piece) {
        case 1:
            blackAlert.SetActive(true);
            break;
        case 2:
            whiteAlert.SetActive(true);
            break;
        }

    }

    void disableAlerts() {

        blackAlert.SetActive(false);
        whiteAlert.SetActive(false);

    }

    void endGame() {

        int blackScore = gameState.getBlackList().Count;
        int whiteScore = gameState.getWhiteList().Count;

        if(blackScore > whiteScore)
            blackWin.SetActive(true);
        else if(whiteScore > blackScore)
            whiteWin.SetActive(true);
        else
            draw.SetActive(true);

        gameOver = true;

    }

}