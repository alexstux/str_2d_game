using UnityEngine;

public class CameraControler : MonoBehaviour {
    public GameObject player;
   
    void Update() {
        transform.position = 
            new Vector3(
                transform.position.x,
                player.transform.position.y,
                transform.position.z
            );
    }
}
