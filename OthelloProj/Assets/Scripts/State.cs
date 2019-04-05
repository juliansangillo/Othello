using System.Collections;

public struct Space {

    public int x, y;

    public Space(int x, int y) {

        this.x = x;
        this.y = y;

    }

}

public class State {

    public enum Color { EMPTY, BLACK, WHITE }

    private Color[ , ] board;
    private bool isBlack;
    private ArrayList moves = new ArrayList();

    public State(Color[ , ] board, bool isBlack) {

        this.isBlack = isBlack;
        this.board = board;
        calculateMoves();

    }

    public State calculateNextState(Space selected, ArrayList piecesToFlip) {

        Color [ , ] board = new Color[8 , 8];
        bool isBlack = this.isBlack;
		
		for(int y = 0; y < 8; y++)
			for(int x = 0; x < 8; x++)
				board[x , y] = this.board[x , y];
        
        switch(isBlack) {
        case true:
            board[selected.x , selected.y] = Color.BLACK;
            for(int y = -1; y <= 1; y++)
                for(int x = -1; x <= 1; x++) {
                    Space pos = new Space(selected.x, selected.y);
                    bool outOfBounds = false;
                    
                    ArrayList white = new ArrayList();
                    do {
                        pos.x += x;
                        pos.y += y;
                        if(pos.x < 0 || pos.x >= 8 || pos.y < 0 || pos.y >= 8) {
                            outOfBounds = true;
                            break;
                        }
                        else if(board[pos.x , pos.y] == Color.WHITE)
                            white.Add(new Space(pos.x, pos.y));
                    } while(board[pos.x , pos.y] == Color.WHITE);

                    if(!outOfBounds && board[pos.x , pos.y] == Color.BLACK)
                        foreach(Space sp in white) {
                            board[sp.x, sp.y] = Color.BLACK;
                            piecesToFlip.Add(sp);
                        }
                }
            break;
        case false:
            board[selected.x , selected.y] = Color.WHITE;
            for(int y = -1; y <= 1; y++)
                for(int x = -1; x <= 1; x++) {
                    Space pos = new Space(selected.x, selected.y);
                    bool outOfBounds = false;

                    ArrayList black = new ArrayList();
                    do {
                        pos.x += x;
                        pos.y += y;
                        if(pos.x < 0 || pos.x >= 8 || pos.y < 0 || pos.y >= 8) {
                            outOfBounds = true;
                            break;
                        }
                        else if(board[pos.x , pos.y] == Color.BLACK)
                            black.Add(new Space(pos.x, pos.y));
                    } while(board[pos.x , pos.y] == Color.BLACK);

                    if(!outOfBounds && board[pos.x , pos.y] == Color.WHITE)
                        foreach(Space sp in black) {
                            board[sp.x , sp.y] = Color.WHITE;
                            piecesToFlip.Add(sp);
                        }
                }
            break;
        }

        State newState = new State(board, !isBlack);

        return newState;

    }

    public void calculateMoves() {

        switch(isBlack) {
        case true:
            ArrayList black = getColor(Color.BLACK);
            foreach(Space b in black) {
                for(int y = -1; y <= 1; y++)
                    for(int x = -1; x <= 1; x++) {
                        Space pos = new Space(b.x, b.y);
						int count = -1;
                        bool outOfBounds = false;
                        do {
                            pos.x += x;
                            pos.y += y;
							count++;
                            if(pos.x < 0 || pos.x >= 8 || pos.y < 0 || pos.y >= 8) {
                                outOfBounds = true;
                                break;
                            }
                        } while(board[pos.x , pos.y] == Color.WHITE);

                        if(!outOfBounds && count > 0 && board[pos.x , pos.y] == Color.EMPTY)
                            moves.Add(pos);
                    }
            }
            break;
        case false:
            ArrayList white = getColor(Color.WHITE);
            foreach(Space w in white) {
                for(int y = -1; y <= 1; y++)
                    for(int x = -1; x <= 1; x++) {
                        Space pos = new Space(w.x, w.y);
						int count = -1;
                        bool outOfBounds = false;
                        do {
                            pos.x += x;
                            pos.y += y;
							count++;
                            if(pos.x < 0 || pos.x >= 8 || pos.y < 0 || pos.y >= 8) {
                                outOfBounds = true;
                                break;
                            }
                        } while(board[pos.x , pos.y] == Color.BLACK);

                        if(!outOfBounds && count > 0 && board[pos.x , pos.y] == Color.EMPTY)
                            moves.Add(pos);
                    }
            }
            break;
        }
        
    }

    public void changeTurn() {

        isBlack = !isBlack;

    }

    public int getCount(Color col) {

        int count = 0;

        for(int y = 0; y < 8; y++)
            for(int x = 0; x < 8; x++)
                if(board[x , y] == col)
                    count++;

        return count;
    }

    public Color[ , ] getBoard() {


        return board;
    }

    public bool isBlackTurn() {

        return isBlack;
    }

    public ArrayList getMoves() {

        return moves;
    }

    private ArrayList getColor(Color col) {

        ArrayList pieces = new ArrayList();

        for(int y = 0; y < 8; y++)
            for(int x = 0; x < 8; x++) {
                if(board[x , y] == col)
                    pieces.Add(new Space(x, y));
            }

        return pieces;
    }

}