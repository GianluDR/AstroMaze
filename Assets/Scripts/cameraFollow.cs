using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public GameObject player;
    public Vector2 offset;
    public float offsetSmoothing;
    private Vector3 playerPosition;

    void Start(){
    }

    void LateUpdate()
    {
        CamFollow();
        //transform.position = Vector3.Lerp (transform.position, target.position, smoothSpeed);
        //transform.LookAt(target);
    }

    private void CamFollow(){
        playerPosition = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);

        if(player.transform.localScale.x > 0f)
        {
            playerPosition = new Vector3(playerPosition.x + offset.x, playerPosition.y + offset.y, playerPosition.z);
        }
        else
        {
            playerPosition = new Vector3(playerPosition.x - offset.x, playerPosition.y + offset.y, playerPosition.z);
        }

        transform.position = Vector3.Lerp(transform.position, playerPosition, offsetSmoothing * Time.deltaTime);
    }
    

}
