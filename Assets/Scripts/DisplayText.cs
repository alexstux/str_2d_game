using UnityEngine;

public class DisplayText : MonoBehaviour {
    private TextMesh textLabel;
    private float oldY;
    void Start() {
        textLabel = GetComponent<TextMesh>();
        oldY = transform.position.y;
    }

    void Update() {
        textLabel.text =
            "t  = " + Time.time.ToString() + '\n'
            + "t' = " + PlayerProperties.time.ToString() + '\n'
            + "a = " + (PlayerProperties.a * 2E7f).ToString() + '\n'
            + "v = " + (PlayerProperties.v * 2E7f).ToString()
            + "(" + PlayerProperties.v.magnitude / Formulas.c + "c)\n";
    }
}
