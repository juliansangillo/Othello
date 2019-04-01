using System.Collections;
using UnityEngine;

public class ArtificialIntelligence : MonoBehaviour {
    
    Tree tree;
    State current;

    void Start() {
        
        bool isBlack = true;
        ArrayList blackList = new ArrayList();
        ArrayList whiteList = new ArrayList();

        blackList.Add(new Space(3, 3));
        blackList.Add(new Space(4, 4));
        whiteList.Add(new Space(3, 4));
        whiteList.Add(new Space(4, 3));

        current = new State(isBlack, blackList, whiteList);

    }

    void playerMoves(Space selected) {

        current = current.calculateNextState(selected.x, selected.y, new ArrayList());

    }

    void aiMoves() {

        Node root = new Node(current);
        tree = new Tree(root, 3);
        
        Space choice = tree.miniMaxSearch();

        BroadcastMessage("addToBoard", choice);

    }

}

class Node {

    private Space move;
    private State state;
    private ArrayList child;

    private int heuristic = 0;

    public Node(State state) {

        this.state = state;
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

    public Tree(Node current, int depth) {

        root = current;
        buildTree(root, depth - 1);

    }

    void buildTree(Node node, int depth) {

            if(depth != 0)
                foreach(Space move in node.getState().getMoves()) {
                    State s = node.getState().calculateNextState(move.x, move.y, new ArrayList());
                    Node next = new Node(s);
                    next.setMove(move);
                    node.LinkTo(next);
                    buildTree(next, depth - 1);
                }

    }

    public Space miniMaxSearch() {

        Node choice = minMax(root.getChildren(), 1);

        return choice.getPriorMove();
    }

    Node minMax(ArrayList children, int player) {

        int bestH;
        Node choice = null;

        if(player == 1)
            bestH = int.MinValue;
        else
            bestH = int.MaxValue;

        foreach(Node child in children) {
            if(child.getChildren().Count == 0) {
                int black = child.getState().getBlackList().Count;
                int white = child.getState().getWhiteList().Count;

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
            else {
                Node childChoice = minMax(child.getChildren(), -player);
                child.setHeuristic(childChoice.getHeuristic());
            }

            if((player == 1 && child.getHeuristic() > bestH) || (player == -1 && child.getHeuristic() < bestH)) {
                bestH = child.getHeuristic();
                choice = child;
            }
        }

        return choice;
    }

}