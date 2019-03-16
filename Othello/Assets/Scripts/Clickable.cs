using System.Collections;
using UnityEngine;

public class Clickable : MonoBehaviour {

    private Material mainMat;
    private Material clickMat;
    private Renderer rend;
    private bool isClickable;

    void Start()
    {
        
        mainMat = gameObject.GetComponent<Material>();
        clickMat = Reference.clickableMaterial;
        rend = gameObject.GetComponent<Renderer>();
        isClickable = false;

    }

    void enableClick() {

        isClickable = true;
        rend.material = clickMat;

    }

    void disableClick() {

        isClickable = false;
        rend.material = mainMat;

    }

    void select() {

        

    }

}