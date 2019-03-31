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

        Node thisNode = new Node(current.isBlackTurn(), current.getBlackList(), current.getWhiteList());
        tree = new Tree(thisNode, 3);
        
        Space choice = tree.miniMaxSearch();

        BroadcastMessage("addToBoard", choice);

    }

}

class Node {

    public Space move;
    public State state;
    public ArrayList link;

    public Node(bool isBlack, ArrayList blackList, ArrayList whiteList) {

        link = new ArrayList();
        state = new State(isBlack, blackList, whiteList);

    }

    public void addMove(Space move) {

        this.move = move;

    }

}

class Tree {
    
    public ArrayList nodes;

    public Tree(Node current, int depth) {

        nodes = new ArrayList();
        buildTree(current, depth - 1);

    }

    void buildTree(Node current, int depth) {

            nodes.Add(current);

            if(depth != 0)
                foreach(Space move in current.state.getMoves()) {
                    State s = current.state.calculateNextState(move.x, move.y, new ArrayList());
                    Node next = new Node(s.isBlackTurn(), s.getBlackList(), s.getWhiteList());
                    next.addMove(move);
                    current.link.Add(next);
                    buildTree(next, depth - 1);
                }

    }

    public Space miniMaxSearch() {

        Node cur = (Node)nodes[0];
        Node maximum = max(cur.link);

        return maximum.move;

    }

    Node max(ArrayList nodes) {

        int score = 0;
        int maxScore = -10000;
        Node maximum = null;

        foreach(Node n in nodes) {
            if(n.link.Count == 0) {
                switch(!Settings.playerIsBlack) {
                case true:
                    score = n.state.getBlackList().Count - n.state.getWhiteList().Count;
                    break;
                case false:
                    score = n.state.getWhiteList().Count - n.state.getBlackList().Count;
                    break;
                }
                if(score > maxScore) {
                    maxScore = score;
                    maximum = n;
                }
            }
            else {
                Node minimum = min(n.link);
                switch(!Settings.playerIsBlack) {
                case true:
                    score = minimum.state.getBlackList().Count - minimum.state.getWhiteList().Count;
                    break;
                case false:
                    score = minimum.state.getWhiteList().Count - minimum.state.getBlackList().Count;
                    break;
                }
                if(score > maxScore) {
                    maxScore = score;
                    maximum = minimum;
                }
            }
        }

        return maximum;

    }

    Node min(ArrayList nodes) {

        int score = 0;
        int minScore = -10000;
        Node minimum = null;

        foreach(Node n in nodes) {
            if(n.link.Count == 0) {
                switch(!Settings.playerIsBlack) {
                case true:
                    score = n.state.getBlackList().Count - n.state.getWhiteList().Count;
                    break;
                case false:
                    score = n.state.getWhiteList().Count - n.state.getBlackList().Count;
                    break;
                }
                if(score > minScore) {
                    minScore = score;
                    minimum = n;
                }
            }
            else {
                Node maximum = max(n.link);
                switch(!Settings.playerIsBlack) {
                case true:
                    score = maximum.state.getBlackList().Count - maximum.state.getWhiteList().Count;
                    break;
                case false:
                    score = maximum.state.getWhiteList().Count - maximum.state.getBlackList().Count;
                    break;
                }
                if(score < minScore) {
                    minScore = score;
                    minimum = maximum;
                }
            }
        }

        return minimum;

    }

}