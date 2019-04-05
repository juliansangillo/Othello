using System.Collections;
using UnityEngine;

public class ArtificialIntelligence : MonoBehaviour {

    private Tree tree;
    private int depth = 3;
    private State current;

    void initiateAiTurn() {

        Space choice = new Space();

        current = gameObject.GetComponent<FlowController>().gameState;

        Node root = new Node(current);
        tree = new Tree(root, depth);
        
        tree.root.setHeuristic(tree.miniMax(tree.root.getChildren(), 1));

        foreach(Node child in tree.root.getChildren())
            if(child.getHeuristic() == tree.root.getHeuristic())
                choice = child.getPriorMove();

        BroadcastMessage("addToBoard", choice);

    }

}

class Node {

    private Space move;
    private State state;
    private ArrayList child;

    private int heuristic = 0;

    public Node(State state) {

        State.Color[ , ] board = state.getBoard();
        bool isBlack = state.isBlackTurn();
        this.state = new State(board, isBlack);

        child = new ArrayList();

    }

    public void setMove(Space move) {

        this.move = move;

    }

    public void setHeuristic(int h) {

        this.heuristic = h;

    }

    public Space getPriorMove() {

        return move;
    }

    public State getState() {

        return state;
    }

    public ArrayList getChildren() {

        return child;
    }

    public int getHeuristic() {

        return heuristic;
    }

    public void LinkTo(Node node) {

        child.Add(node);

    }

}

class Tree {
    
    public Node root;

    public Tree(Node node, int depth) {

        root = node;
        buildTree(root, depth);

    }

    void buildTree(Node node, int depth) {

            if(depth != 0)
                foreach(Space move in node.getState().getMoves()) {
                    State s = node.getState().calculateNextState(move, new ArrayList());
                    Node next = new Node(s);
                    next.setMove(move);
                    node.LinkTo(next);
                    buildTree(next, depth - 1);
                }

    }

    public int miniMax(ArrayList children, int player) {

        int bestH;

        if(player == 1)
            bestH = int.MinValue;
        else
            bestH = int.MaxValue;

        foreach(Node child in children) {
            if(child.getChildren().Count == 0) {
                int black = child.getState().getCount(State.Color.BLACK);
                int white = child.getState().getCount(State.Color.WHITE);

                switch(!Settings.playerIsBlack) {
                case true:
                    if(player == 1)
                        child.setHeuristic(black - white);
                    else
                        child.setHeuristic(white - black);
                    break;
                case false:
                    if(player == 1)
                        child.setHeuristic(white - black);
                    else
                        child.setHeuristic(black - white);
                    break;
                }
            }
            else
                child.setHeuristic(miniMax(child.getChildren(), -player));

            if((player == 1 && child.getHeuristic() > bestH) || (player == -1 && child.getHeuristic() < bestH))
                bestH = child.getHeuristic();
        }

        return bestH;
    }

}