﻿using System;
using UnityEngine;

public class Clickable : MonoBehaviour {

    private Renderer rend;
    private Material mat;
    private Color albedo;
    private bool isPlus = false;

    void Awake() {
        
        rend = gameObject.GetComponent<Renderer>();
        mat = (Material)rend.materials.GetValue(1);
        albedo = mat.GetColor("_Color");

    }

    void Update() {

        if(albedo.a <= 0 || albedo.a >= 1f)
            isPlus = !isPlus;

        if(isPlus)
            albedo.a += 0.015f;
        else
            albedo.a -= 0.015f;

        mat.SetColor("_Color", albedo);
        rend.materials.SetValue(mat, 1);

    }

    void disable() {

        gameObject.GetComponent<Clickable>().enabled = false;

    }

    void OnMouseEnter() {
        
        if(gameObject.GetComponent<Clickable>().enabled) {
            albedo.r = 1;
            albedo.g = 0;
            albedo.b = 0;
        }

    }

    void OnMouseExit() {

        if(gameObject.GetComponent<Clickable>().enabled) {
            albedo.r = 0;
            albedo.g = 0;
            albedo.b = 1;
        }

    }

    void OnMouseDown() {
        
        if(gameObject.GetComponent<Clickable>().enabled) {
            Space selected;
            
            char x = name[name.Length - 1];
            char y = name[name.Length - 2];

            selected.x = (int)Char.GetNumericValue(x);
            selected.y = (int)Char.GetNumericValue(y);

            SendMessageUpwards("addToBoard", selected);
        }

    }

    void OnDisable() {

        isPlus = false;
        albedo.a = 0;
        mat.SetColor("_Color", albedo);
        rend.materials.SetValue(mat, 1);

    }

}