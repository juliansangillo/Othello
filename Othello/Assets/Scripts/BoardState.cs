using System.Collections;
using UnityEngine;

public class BoardState : MonoBehaviour {

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
    private bool _isBlack;
    public bool isBlack {

        get {
            return _isBlack;
        }

    }
    private ArrayList _move;
    public ArrayList moves {

        get {
            return _move;
        }

    }
    

    void Start() {

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
            _isBlack = false;
            break;
        case false:
            _whiteList.Add(selected);
            _isBlack = true;
            break;
        }
        
        recalculateMoves();

    }

    void recalculateMoves() {

        _move = new ArrayList();

        switch(isBlack) {
        case true:
            foreach(Vector2 black in _blackList)
                for(int i = -1; i <= 1; i++)
                    for(int j = -1; i <= 1; i++) {
                        Vector2 pos = black;
                        do {
                            pos.x += j;
                            pos.y += i;
                        } while(_whiteList.Contains(pos));
                        
                        if(pos.x >= 0 && pos.y >= 0 && !_blackList.Contains(pos) && !_move.Contains(pos))
                            _move.Add(pos);
                    }
            break;
        case false:
            foreach(Vector2 white in _whiteList)
                for(int i = -1; i <= 1; i++)
                    for(int j = -1; i <= 1; i++) {
                        Vector2 pos = white;
                        do {
                            pos.x += j;
                            pos.y += i;
                        } while(_blackList.Contains(pos));
                        
                        if(pos.x >= 0 && pos.y >= 0 && !_whiteList.Contains(pos) && !_move.Contains(pos))
                            _move.Add(pos);
                    }
            break;
        }

    }

}
