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

    public State gameState;

    private GameObject[ , ] space;
    private GameObject[ , ] pieces;

    private bool gameOver = false;
    
    void Awake() {

        space = new GameObject[8 , 8];
        pieces = new GameObject[8 , 8];

        for(int y = 0; y < 8; y++) {
            Transform row = board.transform.GetChild(y);
            for(int x = 0; x < 8; x++)
                space[x , y] = row.GetChild(x).gameObject;
        }

        State.Color[ , ] gameBoard = new State.Color[8 , 8];

        for(int y = 0; y < 8; y++)
            for(int x = 0; x < 8; x++)
                if((x == 3 && y == 3) || (x == 4 && y == 4))
                    gameBoard[x , y] = State.Color.BLACK;
                else if((x == 3 && y == 4) || (x == 4 && y == 3))
                    gameBoard[x , y] = State.Color.WHITE;
                else
                    gameBoard[x , y] = State.Color.EMPTY;

        pieces[3 , 3] = GameObject.Find("Black");
        pieces[4 , 4] = GameObject.Find("Black (1)");
        pieces[3 , 4] = GameObject.Find("White");
        pieces[4 , 3] = GameObject.Find("White (1)");

        gameState = new State(gameBoard, true);

        enterNextTurn();
        
    }

    void enterNextTurn() {

        if(Settings.playerIsBlack == gameState.isBlackTurn()) {
            ArrayList moves = gameState.getMoves();
            foreach(Space move in moves) {
                space[move.x, move.y].GetComponent<Clickable>().enabled = true;
            }
        }
        else {
            BroadcastMessage("initiateAiTurn");
        }

    }

    void addToBoard(Space selected) {

        BroadcastMessage("disable");

        GameObject piece = null;
        Vector3 target;
        bool isBlack;

        target = space[selected.x , selected.y].GetComponent<Transform>().position;
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

    }

    void setState(Space selected) {

        ArrayList piecesToFlip = new ArrayList();
        int blackCount = gameState.getCount(State.Color.BLACK);
        int whiteCount = gameState.getCount(State.Color.WHITE);
        gameState = gameState.calculateNextState(selected, piecesToFlip);
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

        //Continue after piece flipping-------------------------------------------------
        Animator indicate = turnIndicator.transform.GetChild(0).GetComponent<Animator>();
        switch(gameState.isBlackTurn()) {
        case true:
            indicate.Play("Black");
            break;
        case false:
            indicate.Play("White");
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

        if(count == 0)
            endGame();
        else if(count == 1)
            switch(!gameState.isBlackTurn()) {
            case true:
                outOfMoves('B');
                break;
            case false:
                outOfMoves('W');
                break;
            }
        else
            disableAlerts();

        if(!gameOver)
            enterNextTurn();

    }

    void outOfMoves(char piece) {

        switch(piece) {
        case 'B':
            blackAlert.SetActive(true);
            break;
        case 'W':
            whiteAlert.SetActive(true);
            break;
        }

    }

    void disableAlerts() {

        blackAlert.SetActive(false);
        whiteAlert.SetActive(false);

    }

    void endGame() {

        disableAlerts();

        int blackScore = gameState.getCount(State.Color.BLACK);
        int whiteScore = gameState.getCount(State.Color.WHITE);

        if(blackScore > whiteScore)
            blackWin.SetActive(true);
        else if(whiteScore > blackScore)
            whiteWin.SetActive(true);
        else
            draw.SetActive(true);

        gameOver = true;

    }

}