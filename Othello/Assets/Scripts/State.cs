using System.Collections;

public struct Space {

    public int x, y;

    public Space(int x, int y) {

        this.x = x;
        this.y = y;

    }

}

public class State {

    private bool isBlack;
    private ArrayList blackList;
    private ArrayList whiteList;
    private ArrayList moves = new ArrayList();

    public State(bool isBlack, ArrayList blackList, ArrayList whiteList) {

        this.isBlack = isBlack;
        this.blackList = blackList;
        this.whiteList = whiteList;
        calculateMoves();

    }

    public State calculateNextState(int x, int y, ArrayList piecesToFlip) {

        Space space = new Space(x, y);

        bool isBlack = this.isBlack;
        ArrayList blackList = new ArrayList(this.blackList);
        ArrayList whiteList = new ArrayList(this.whiteList);

        switch(isBlack) {
        case true:
            blackList.Add(space);
            for(int i = -1; i <= 1; i++)
                for(int j = -1; j <= 1; j++) {
                    Space pos;
                    pos.x = space.x;
                    pos.y = space.y;
                    
                    ArrayList white = new ArrayList();
                    do {
                        pos.x += j;
                        pos.y += i;
                        if(whiteList.Contains(pos))
                            white.Add(whiteList[whiteList.IndexOf(pos)]);
                    } while(whiteList.Contains(pos));

                    if(blackList.Contains(pos))
                        foreach(Space sp in white) {
                            whiteList.Remove(sp);
                            blackList.Add(sp);
                            piecesToFlip.Add(sp);
                        }
                }
            break;
        case false:
            whiteList.Add(space);
            for(int i = -1; i <= 1; i++)
                for(int j = -1; j <= 1; j++) {
                    Space pos;
                    pos.x = space.x;
                    pos.y = space.y;

                    ArrayList black = new ArrayList();
                    do {
                        pos.x += j;
                        pos.y += i;
                        if(blackList.Contains(pos))
                            black.Add(blackList[blackList.IndexOf(pos)]);
                    } while(blackList.Contains(pos));

                    if(whiteList.Contains(pos))
                        foreach(Space sp in black) {
                            blackList.Remove(sp);
                            whiteList.Add(sp);
                            piecesToFlip.Add(sp);
                        }
                }
            break;
        }

        State newState = new State(!isBlack, blackList, whiteList);

        return newState;

    }

    public void calculateMoves() {

        switch(isBlack) {
        case true:
            foreach(Space black in blackList)
                for(int i = -1; i <= 1; i++)
                    for(int j = -1; j <= 1; j++) {
                        Space pos = black;
                        int whiteCount = 0;
                        do {
                            pos.x += i;
                            pos.y += j;
                            if(whiteList.Contains(pos))
                                whiteCount++;
                        } while(whiteList.Contains(pos));

                        if(pos.x >= 0 && pos.x < 8 && pos.y >= 0 && pos.y < 8 && whiteCount > 0 && 
                        !blackList.Contains(pos) && !moves.Contains(pos))
                            moves.Add(pos);
                    }
            break;
        case false:
            foreach(Space white in whiteList)
                for(int i = -1; i <= 1; i++)
                    for(int j = -1; j <= 1; j++) {
                        Space pos = white;
                        int blackCount = 0;
                        do {
                            pos.x += i;
                            pos.y += j;
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

    public void changeTurn() {

        isBlack = !isBlack;

    }

    public bool isBlackTurn() {

        return isBlack;
    }

    public ArrayList getBlackList() {

        return blackList;
    }

    public ArrayList getWhiteList() {

        return whiteList;
    }

    public ArrayList getMoves() {

        return moves;
    }

}