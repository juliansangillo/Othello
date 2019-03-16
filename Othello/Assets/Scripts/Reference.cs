using UnityEngine;

public class Reference : MonoBehaviour {
    
    public static Material clickableMaterial;

    public Material ClickableMaterial;

    void Awake() {

        clickableMaterial = ClickableMaterial;

    }

}
