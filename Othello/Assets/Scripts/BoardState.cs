using System.Collections;
using UnityEngine;

public class BoardState : MonoBehaviour {

    private GameObject[ , ] _pieces;
    public GameObject[ , ] pieces {

        set {
            _pieces = value;
        }

        get {
            return _pieces;
        }

    }

    private ArrayList _blackList;
    public ArrayList blackList {

        get {
            return _blackList;
        }

    }
    private ArrayList _whiteList;
    public ArrayList whiteList {

        get {
            return _whiteList;
        }

    }
    private int _blackScore;
    public  int blackScore {

        get {
            return _blackScore;
        }

    }
    private int _whiteScore;
    public int whiteScore {

        get {
            return _whiteScore;
        }

    }
    private ArrayList _move;
    public ArrayList move {

        get {
            return _move;
        }

    }
    private bool _isBlack;
    public bool isBlack {

        get {
            return _isBlack;
        }

    }
    private Transform indicator;
    private int coroutinesRunning = 0;

    void Start() {

        _pieces = new GameObject[8 , 8];

        _pieces[3 , 3] = GameObject.Find("Black");
        _pieces[4 , 3] = GameObject.Find("White");
        _pieces[3 , 4] = GameObject.Find("White (1)");
        _pieces[4 , 4] = GameObject.Find("Black (1)");

        _blackList = new ArrayList();
        _blackList.Add(new Vector2(4, 4));
        _blackList.Add(new Vector2(3, 3));

        _whiteList = new ArrayList();
        _whiteList.Add(new Vector2(4, 3));
        _whiteList.Add(new Vector2(3, 4));

        _move = new ArrayList();
        _move.Add(new Vector2(5, 3));
        _move.Add(new Vector2(4, 2));
        _move.Add(new Vector2(3, 5));
        _move.Add(new Vector2(2, 4));

        _isBlack = true;
        indicator = GameObject.Find("TurnIndicator").transform.GetChild(0);

    }

    public void update(Vector2 selected) {

        switch(isBlack) {
        case true:
            _blackList.Add(selected);
            break;
        case false:
            _whiteList.Add(selected);
            break;
        }

        recalculatePieces(selected);
        StartCoroutine(waitForCors());

    }

    void recalculatePieces(Vector2 selected) {

        switch(isBlack) {
        case true:
            for(int i = -1; i <= 1; i++)
                for(int j = -1; j <= 1; j++) {
                    Vector2 pos = selected;
                    ArrayList white = new ArrayList();
                    do {
                        pos.x += j;
                        pos.y += i;
                        if(_whiteList.Contains(pos))
                            white.Add(pos);
                    } while(_whiteList.Contains(pos));

                    if(_blackList.Contains(pos))
                        StartCoroutine(flip(white, "Black"));
                }
            break;
        case false:
            for(int i = -1; i <= 1; i++)
                for(int j = -1; j <= 1; j++) {
                    Vector2 pos = selected;
                    ArrayList black = new ArrayList();
                    do {
                        pos.x += j;
                        pos.y += i;
                        if(_blackList.Contains(pos))
                            black.Add(pos);
                    } while(_blackList.Contains(pos));

                    if(_whiteList.Contains(pos))
                        StartCoroutine(flip(black, "White"));
                }
            break;
        }

    }

    IEnumerator flip(ArrayList p, string state) {

        coroutinesRunning++;

        foreach(Vector2 piece in p) {
            Animator anim = pieces[(int)piece.y , (int)piece.x].transform.GetChild(0).GetComponent<Animator>();
            anim.Play(state);

            if(state == "Black") {
                _blackList.Add(piece);
                _whiteList.Remove(piece);
            }
            else {
                _whiteList.Add(piece);
                _blackList.Remove(piece);
            }
            _blackScore = blackList.Count;
            _whiteScore = whiteList.Count;
            yield return new WaitForSeconds(0.2f);
        }

        coroutinesRunning--;

    }

    void recalculateMoves() {

        SendMessageUpwards("disableAlerts");

        _move = new ArrayList();

        bool blackHasMoves = true, whiteHasMoves = true;

        do {
            switch(isBlack) {
            case true:
                foreach(Vector2 black in _blackList)
                    for(int i = -1; i <= 1; i++)
                        for(int j = -1; j <= 1; j++) {
                            Vector2 pos = black;
                            int whiteCount = 0;
                            do {
                                pos.x += j;
                                pos.y += i;
                                if(_whiteList.Contains(pos))
                                    whiteCount++;
                            } while(_whiteList.Contains(pos));

                            if(pos.x >= 0 && pos.x < 8 && pos.y >= 0 && pos.y < 8 && whiteCount > 0 && 
                            !_blackList.Contains(pos) && !_move.Contains(pos))
                                _move.Add(pos);
                        }
                if(move.Count == 0) {
                    blackHasMoves = false;
                    _isBlack = false;
                    indicator.GetComponent<Animator>().Play("White");
                    continue;
                }
                break;
            case false:
                foreach(Vector2 white in _whiteList)
                    for(int i = -1; i <= 1; i++)
                        for(int j = -1; j <= 1; j++) {
                            Vector2 pos = white;
                            int blackCount = 0;
                            do {
                                pos.x += j;
                                pos.y += i;
                                if(_blackList.Contains(pos))
                                    blackCount++;
                            } while(_blackList.Contains(pos));

                            if(pos.x >= 0 && pos.x < 8 && pos.y >= 0 && pos.y < 8 && blackCount > 0 && 
                            !_whiteList.Contains(pos) && !_move.Contains(pos))
                                _move.Add(pos);
                        }
                if(move.Count == 0) {
                    whiteHasMoves = false;
                    _isBlack = true;
                    indicator.GetComponent<Animator>().Play("Black");
                    continue;
                }
                break;
            }
            break;
        } while(blackHasMoves || whiteHasMoves);

        if(!blackHasMoves && !whiteHasMoves)
            SendMessageUpwards("endGame");
        else if(!blackHasMoves)
            SendMessageUpwards("outOfMoves", 1);
        else if(!whiteHasMoves)
            SendMessageUpwards("outOfMoves", 2);

    }

    IEnumerator waitForCors() {

        while(coroutinesRunning > 0) {
            yield return null;
        }

        _isBlack = !_isBlack;
        if(_isBlack)
            indicator.GetComponent<Animator>().Play("Black");
        else
            indicator.GetComponent<Animator>().Play("White");

        recalculateMoves();
        SendMessageUpwards("enableOptions");

    }

}
