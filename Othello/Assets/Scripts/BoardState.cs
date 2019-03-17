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
    private bool _isBlack;
    public bool isBlack {

        get {
            return _isBlack;
        }

    }
    private ArrayList _move;
    public ArrayList move {

        get {
            return _move;
        }

    }

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

        _isBlack = true;

        _move = new ArrayList();
        _move.Add(new Vector2(5, 3));
        _move.Add(new Vector2(4, 2));
        _move.Add(new Vector2(3, 5));
        _move.Add(new Vector2(2, 4));

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
        _isBlack = !_isBlack;
        
        recalculateMoves();

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
                        foreach(Vector2 piece in white) {
                            Animator anim = pieces[(int)piece.y , (int)piece.x].transform.GetChild(0).GetComponent<Animator>();
                            anim.Play("Black");
                            _blackList.Add(piece);
                            _whiteList.Remove(piece);
                            new WaitForSeconds(0.1f);
                        }
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
                        foreach(Vector2 piece in black) {
                            Animator anim = pieces[(int)piece.y , (int)piece.x].transform.GetChild(0).GetComponent<Animator>();
                            anim.Play("White");
                            _whiteList.Add(piece);
                            _blackList.Remove(piece);
                            new WaitForSeconds(0.1f);
                        }
                }
            break;
        }

        _blackScore = blackList.Count;
        _whiteScore = whiteList.Count;

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

}
