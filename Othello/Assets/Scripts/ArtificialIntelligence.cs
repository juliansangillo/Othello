using System.Collections;
using UnityEngine;

public class ArtificialIntelligence : MonoBehaviour {

    private Tree tree;

    void Start() {

        tree = new Tree();

    }

    public void playerMoves(Vector2 selected) {

        foreach(State state in tree.states)
            if(state.getPos() == selected) {
                tree.current = state;
                break;
            }

    }

    public void aiMoves() {

        State choice = tree.miniMaxSearch();

        BroadcastMessage("addToBoard", choice.getPos());

    }

}

class Tree {

    public State current;
    public ArrayList states = new ArrayList();

    public Tree() {

        ArrayList blackList = new ArrayList(), whiteList = new ArrayList();
        bool isBlack = true;

        blackList.Add(new Vector2(3, 3));
        blackList.Add(new Vector2(4, 4));
        whiteList.Add(new Vector2(3, 4));
        whiteList.Add(new Vector2(4, 3));

        State start = new State(blackList, whiteList, isBlack, new Vector2());
        states.Add(start);

        int depth = 4;

        depthCreate(start, states, depth);

    }
    
    private void depthCreate(State state, ArrayList states, int depth) {

        ArrayList blackList = new ArrayList(state.getBlackList()), whiteList = new ArrayList(state.getWhiteList());
        State newState;

        foreach(Vector2 move in state.getMoves()) {
            switch(state.isBlackTurn()) {
            case true:
                blackList.Add(move);
                for(int i = -1; i <= 1; i++)
                    for(int j = -1; j <= 1; j++) {
                        Vector2 pos = move;
                        ArrayList white = new ArrayList();
                        do {
                            pos.x += j;
                            pos.y += i;
                            if(whiteList.Contains(pos))
                                white.Add(pos);
                        } while(whiteList.Contains(pos));

                        if(blackList.Contains(pos))
                            foreach(Vector2 w in white) {
                                blackList.Add(w);
                                whiteList.Remove(w);
                            }
                    }
                newState = new State(blackList, whiteList, !state.isBlackTurn(), move);
                states.Add(newState);
                state.parentTo(newState);
                if(depth != 0)
                    depthCreate(newState, states, depth - 1);
                break;
            case false:
                whiteList.Add(move);
                for(int i = -1; i <= 1; i++)
                    for(int j = -1; j <= 1; j++) {
                        Vector2 pos = move;
                        ArrayList black = new ArrayList();
                        do {
                            pos.x += j;
                            pos.y += i;
                            if(blackList.Contains(pos))
                                black.Add(pos);
                        } while(blackList.Contains(pos));

                        if(whiteList.Contains(pos))
                            foreach(Vector2 b in black) {
                                whiteList.Add(b);
                                blackList.Remove(b);
                            }
                    }
                newState = new State(blackList, whiteList, !state.isBlackTurn(), move);
                states.Add(newState);
                state.parentTo(newState);
                if(depth != 0)
                    depthCreate(newState, states, depth - 1);
                break;
            }
        }

        if(state.getMoves().Count == 0 || depth == 0)
            states.Add(state);

        if(depth == 3) {
            Debug.Log("This is state: " + state.getPos());
            foreach(State child in state.getChildren())
                Debug.Log(child.getPos());
        }

    }

    public State miniMaxSearch() {

        State max = State.max(current.getChildren());

        current = max;

        return max;
    }

}

class State {

    private ArrayList blackList, whiteList;
    private bool isBlack;
    private ArrayList moves = new ArrayList();
    private int score = 2;
    private Vector2 pos;
    private ArrayList children = new ArrayList();

    public State(int num) {

        this.blackList = null;
        this.whiteList = null;
        this.isBlack = true;

        if(num > 0)
            this.score = 64;
        else
            this.score = -64;

    }

    public State(ArrayList blackList, ArrayList whiteList, bool isBlack, Vector2 pos) {
        
        this.blackList = new ArrayList(blackList);
        this.whiteList = new ArrayList(whiteList);
        this.isBlack = isBlack;
        this.pos = new Vector2(pos.x, pos.y);

        calcMoves();

    }

    public ArrayList getBlackList() {

        return blackList;
    }

    public ArrayList getWhiteList() {

        return whiteList;
    }

    public bool isBlackTurn() {

        return isBlack;
    }

    public ArrayList getMoves() {

        return moves;
    }

    public int getScore() {

        return score;
    }

    public Vector2 getPos() {

        return pos;
    }

    public ArrayList getChildren() {

        return children;
    }

    public void setScore(int score) {

        this.score = score;

    }

    public void parentTo(State child) {

        children.Add(child);

    }

    private void calcMoves() {

        switch(isBlack) {
        case true:
            foreach(Vector2 black in blackList)
                for(int i = -1; i <= 1; i++)
                    for(int j = -1; j <= 1; j++) {
                        Vector2 pos = black;
                        int whiteCount = 0;
                        do {
                            pos.x += j;
                            pos.y += i;
                            if(whiteList.Contains(pos))
                                whiteCount++;
                        } while(whiteList.Contains(pos));

                        if(pos.x >= 0 && pos.x < 8 && pos.y >= 0 && pos.y < 8 && whiteCount > 0 && 
                        !blackList.Contains(pos) && !moves.Contains(pos))
                            moves.Add(pos);
                    }
            break;
        case false:
            foreach(Vector2 white in whiteList)
                for(int i = -1; i <= 1; i++)
                    for(int j = -1; j <= 1; j++) {
                        Vector2 pos = white;
                        int blackCount = 0;
                        do {
                            pos.x += j;
                            pos.y += i;
                            if(blackList.Contains(pos))
                                blackCount++;
                        } while(blackList.Contains(pos));

                        if(pos.x >= 0 && pos.x < 8 && pos.y >= 0 && pos.y < 8 && blackCount > 0 && 
                        !whiteList.Contains(pos) && !moves.Contains(pos))
                            moves.Add(pos);
                    }
            break;
        }

    }

    private void evalScore() {

        switch(!Settings.playerIsBlack) {
        case true:
            score = blackList.Count - whiteList.Count;
            break;
        case false:
            score = whiteList.Count - blackList.Count;
            break;
        }

    }

    public static State max(ArrayList states) {

        State max = new State(-1);
        State min;

        foreach(State s in states) {
            if(s.getChildren().Count != 0) {
                min = State.min(s.getChildren());
                if(min.getScore() > max.getScore())
                    max = min;
            }
            else {
                s.evalScore();
                if(s.getScore() > max.getScore())
                    max = s;
            }
        }

        return max;

    }

    public static State min(ArrayList states) {

        State min = new State(1);
        State max;

        foreach(State s in states) {
            if(s.getChildren().Count != 0) {
                max = State.max(s.getChildren());
                if(max.getScore() < min.getScore())
                    min = max;
            }
            else {
                s.evalScore();
                if(s.getScore() < min.getScore())
                    min = s;
            }

        }

        return min;

    }

}