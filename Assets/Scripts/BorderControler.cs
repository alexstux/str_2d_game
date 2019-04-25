using System;

using UnityEngine;

public class BorderControler : MonoBehaviour {
    public GameObject player;
    public GameObject borderRight;
    public GameObject borderLeft;
    private BoxCollider2D playerCollider;
    private float length;

    void Start() {
        playerCollider = player.GetComponent<BoxCollider2D>();
        length = 
            Math.Abs(
                borderRight.transform.position.x
                - borderLeft.transform.position.x
            );
    }

    void Update() {
        transform.position = 
            new Vector3(
                transform.position.x,
                player.transform.position.y,
                transform.position.z
            );        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            int signX = player.transform.position.x >= 0 ? 1 : -1;
            PlayerProperties.r = 
                new Vector2(
                    -player.transform.position.x
                    + 2 * signX * playerCollider.size.x,
                    player.transform.position.y
                );
            player.transform.position = PlayerProperties.r;
        }
    }
}
