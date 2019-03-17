using UnityEngine;

public class Reference : MonoBehaviour {
    
    public static Material hoverMaterial;

    public Material HoverMaterial;

    void Awake() {

        hoverMaterial = HoverMaterial;

    }

}
