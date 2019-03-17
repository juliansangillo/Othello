using System.Collections;
using TMPro;
using UnityEngine;

public class FlowController : MonoBehaviour {

    public GameObject board;
    public GameObject blackFab;
    public GameObject whiteFab;
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
    
    void Start() {

        space = new GameObject[8 , 8];

        Transform b = board.transform;
        for(int i = 0; i < 8; i++) {
            Transform row = b.GetChild(i);
            for(int j = 0; j < 8; j++)
                space[i , j] = row.GetChild(j).gameObject;
        }
        
    }

    void enableOptions() {

        ArrayList moves = board.GetComponent<BoardState>().move;
        foreach(Vector2 move in moves) {
            space[(int)move.y, (int)move.x].GetComponent<Clickable>().enabled = true;
        }

    }

    void addToBoard(Vector2 selected) {

        BroadcastMessage("disable");

        GameObject piece = null;
        Vector3 target;
        bool isBlack;

        target = space[(int)selected.y, (int)selected.x].GetComponent<Transform>().position;
        target.y = 0.5f;

        isBlack = board.GetComponent<BoardState>().isBlack;

        switch(isBlack) {
        case true:
            piece = Instantiate(blackFab, blackSpawn, new Quaternion());
            break;
        case false:
            piece = Instantiate(whiteFab, whiteSpawn, new Quaternion());
            break;
        }

        board.GetComponent<BoardState>().pieces[(int)selected.y, (int)selected.x] = piece;

        Transform piecePos = piece.GetComponent<Transform>();
        StartCoroutine(moveToTarget(piecePos, selected, target));

    }

    IEnumerator moveToTarget(Transform piece, Vector2 selected, Vector3 target) {

        float delay = 0.01f;
        float speed = 0.5f;

        while(piece.position != target) {
            yield return new WaitForSeconds(delay);
            piece.position = Vector3.MoveTowards(piece.position, target, speed);
        }

        board.GetComponent<BoardState>().update(selected);
        
        blackScoreText.SetText(board.GetComponent<BoardState>().blackScore.ToString());
        whiteScoreText.SetText(board.GetComponent<BoardState>().whiteScore.ToString());

        enableOptions();

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

        if(board.GetComponent<BoardState>().blackScore > board.GetComponent<BoardState>().whiteScore)
            blackWin.SetActive(true);
        else if(board.GetComponent<BoardState>().whiteScore > board.GetComponent<BoardState>().blackScore)
            whiteWin.SetActive(true);
        else
            draw.SetActive(true);

    }

}