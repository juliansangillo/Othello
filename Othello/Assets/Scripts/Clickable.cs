using System;
using UnityEngine;

public class Clickable : MonoBehaviour {

    private Renderer rend;
    private Material blueMat;
    private Material redMat;
    private Color albedo;
    private bool isPlus = false;

    void Awake() {
        
        rend = gameObject.GetComponent<Renderer>();
        blueMat = (Material)rend.materials.GetValue(1);
        redMat = Reference.hoverMaterial;
        albedo = blueMat.GetColor("_Color");

    }

    void Update() {

        if(albedo.a <= 0 || albedo.a >= 1)
            isPlus = !isPlus;

        if(isPlus)
            albedo.a += 0.02f;
        else
            albedo.a -= 0.02f;

        blueMat.SetColor("_Color", albedo);
        rend.materials.SetValue(blueMat, 1);

    }

    void disable() {

        gameObject.GetComponent<Clickable>().enabled = false;

    }

    void OnMouseEnter() {
        
        if(gameObject.GetComponent<Clickable>().enabled)
            rend.materials.SetValue(redMat, 1);

    }

    void OnMouseExit() {

        if(gameObject.GetComponent<Clickable>().enabled)
            rend.materials.SetValue(blueMat, 1);

    }

    void OnMouseDown() {
        
        if(gameObject.GetComponent<Clickable>().enabled) {
            Vector2 selected;
            
            char i = name[name.Length - 2];
            char j = name[name.Length - 1];

            selected.x = (int)Char.GetNumericValue(j);
            selected.y = (int)Char.GetNumericValue(i);

            SendMessageUpwards("addToBoard", selected);
        }

    }

    void OnDisable() {

        isPlus = false;
        albedo.a = 0;
        blueMat.SetColor("_Color", albedo);
        rend.materials.SetValue(blueMat, 1);

    }

}