using System;

using UnityEngine;

public class PlayerControler : MonoBehaviour {
    private Rigidbody2D body;
    private bool isStoping;
    
    void Start() {
        transform.position = Vector3.zero;
        body = GetComponent<Rigidbody2D>();
        isStoping = false;
    }
    
    void Update() {
        if (Input.GetKeyDown(KeyCode.H)) {
            PlayerProperties.a = PlayerProperties.v = Vector2.zero;
        }
        SetA();
        PlayerProperties.v = Formulas.V(Time.deltaTime);
        PlayerProperties.r = Formulas.R(Time.deltaTime);
        PlayerProperties.time = Formulas.RealityTime(Time.deltaTime);

        body.MovePosition(PlayerProperties.r);
        
        float cos = Vector2.up.Cos(PlayerProperties.v);
        if (Vector2.right.Cos(PlayerProperties.v) >= 0) {
            transform.rotation = 
                new Quaternion(
                    0f,
                    0f,
                    (float)Math.Sin(-Math.Acos(cos) / 2),
                    (float)Math.Cos(-Math.Acos(cos) / 2)
                );
        } else {
            transform.rotation = 
                new Quaternion(
                    0f,
                    0f,
                    (float)Math.Sin(Math.Acos(cos) / 2),
                    (float)Math.Cos(Math.Acos(cos) / 2)
                );
        }
    }

    void SetA() {
        float axisVertical = Input.GetAxis("Vertical"),
            axisHorizontal = Input.GetAxis("Horizontal");
        if (isStoping
            && axisVertical == 0
            && axisHorizontal == 0
            ) {
            Vector2 tempV = Formulas.V(Time.deltaTime);
            if (tempV.Cos(PlayerProperties.a) > 0) {
                PlayerProperties.a = Vector2.zero;
                PlayerProperties.v = Vector2.zero;
            }
        } else if (isStoping
            && (axisHorizontal != 0
            || axisVertical != 0)
            ) {
            PlayerProperties.a =
                Vector2.up * axisVertical
                + Vector2.right * axisHorizontal;
            isStoping = false;
        } else if (!isStoping
            && axisVertical == 0
            && axisHorizontal == 0
            ) {
            PlayerProperties.a = -PlayerProperties.v;
            isStoping = true;
        } else if (!isStoping
            && (axisVertical != 0
            || axisHorizontal == 0)
            ) {
            PlayerProperties.a =
                Vector2.up * axisVertical
                + Vector2.right * axisHorizontal;
        }
    }
}
