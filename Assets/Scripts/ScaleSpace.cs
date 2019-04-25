using UnityEngine;

public class ScaleSpace : MonoBehaviour {
    private float scaleX;
    private float scaleY;
    private float oldScaleX;
    private float oldScaleY;
    
    void Start() {
        oldScaleX = scaleX = transform.localScale.x;
        oldScaleY = scaleY = transform.localScale.y;
        
    }

    void Update() {
        scaleX = 
            Formulas.K(new Vector2(PlayerProperties.v.x, 0f))
            * oldScaleX;
        scaleY = 
            Formulas.K(new Vector2(0f, PlayerProperties.v.y))
            * oldScaleY;
        transform.localScale = 
            new Vector3(
                scaleX,
                scaleY,
                transform.localScale.z
            );
    }
}
