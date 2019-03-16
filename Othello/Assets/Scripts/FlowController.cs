using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowController : MonoBehaviour {

    private GameObject[ , ] space;
    private Renderer rend;
    
    void Start() {

        space = new GameObject[8 , 8];

        string tile;
        for(int i = 0; i < 8; i++)
            for(int j = 0; j < 8; j++) {
                tile = "/Board/TileRow" + i + "/Tile" + i + j;
                space[i , j] = GameObject.Find(tile);
            }
        
        rend = space[5 , 3].GetComponent<Renderer>();
        rend.material = Reference.clickableMaterial;
        rend = space[4 , 2].GetComponent<Renderer>();
        rend.material = Reference.clickableMaterial;
        rend = space[3 , 5].GetComponent<Renderer>();
        rend.material = Reference.clickableMaterial;
        rend = space[2 , 4].GetComponent<Renderer>();
        rend.material = Reference.clickableMaterial;

    }

}
