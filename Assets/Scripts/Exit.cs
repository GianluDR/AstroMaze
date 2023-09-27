using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public GridManager grid;

    private int exDimX;
    private int exDimY;
    private bool eventIn = false;
    private int eventSwitch = 0;
    private int roomNum = 0;
    private float multiplier = 1;
    private PlayerController playerInQ;

    public UIManager UI;


    void OnTriggerEnter2D(Collider2D hit)
    {
        var player = hit.gameObject.GetComponent<PlayerController>();
        if(player != null)
        {
            playerInQ = player;
            FindObjectOfType<AudioManager>().Play("LevelUp");
            grid.reset();
            EventChooser();
            grid.createMap2();
            player.score++;
            GameObject.FindWithTag("Combo").GetComponent<ScoreSystem>().scoreUp();
            if(player.timeValue + 15 > player.maxTime)
                player.timeValue = player.maxTime;
            else
                player.timeValue = player.timeValue + 15;
        }
        var player2 = hit.gameObject.GetComponent<PlayerMode2>();
        if(player2 != null)
        {
            FindObjectOfType<AudioManager>().Play("LevelUp");
            grid.reset();
            grid.createMap2();

            player2.score++;
            player2.timeValue = player2.maxTime;
            player2.UI.transform.GetChild(0).gameObject.SetActive(true);
            player2.UI.transform.GetChild(1).gameObject.SetActive(false);
            GameObject.FindWithTag("Combo").GetComponent<ScoreSystem>().scoreUp();
        }
    }

    private void FogEvent(){
        UI.LightToLocal();
        grid.dimX = 10;
        grid.dimY = 10;
    }

    IEnumerator Pause(float tempo){
       yield return new WaitForSeconds(tempo);
        if (Random.value < 0.5)
            UI.rotate90(true);
        else
            UI.rotate90(false);
    }

    private void RotateEvent(){
        //UI.rotate90(true);
        StartCoroutine(Pause(2.5f));
        
    }

    public void gridToNormal(){
        exDimX = grid.dimX;
        exDimY = grid.dimY;
    }


    private void EventChooser(){
        float x = (grid.dimX / 100f)*multiplier;
        if(!eventIn){
            if(Random.value < ((grid.dimX/100f)*multiplier)){
                eventIn = true;
                if(Random.value < 0.50)
                    eventSwitch = 1;
                else if(Random.value >= 0.50)
                   eventSwitch = 2;
                Handheld.Vibrate();
                exDimX = grid.dimX;
                exDimY = grid.dimY;
                UI.EventAppear(eventSwitch);
            }else{
                multiplier = multiplier + 0.6f;
            }
        }
        if(eventIn && roomNum <= 3){
            switch (eventSwitch){
                case 1:
                    RotateEvent();
                    break;
                case 2:
                    FogEvent();
                    break;
                default:
                    break;
            }
            roomNum++;
        }
        if(roomNum > 3) {
            eventEnd();
        }
    }

    public void eventEnd(){
        eventIn = false;
        eventSwitch = 0;
        roomNum = 0;
        multiplier = 1;
        CancelInvoke();
        UI.moveNum = 0;
        UI.resetMovement();
        UI.LightToGlobal();
        grid.dimX = exDimX;
        grid.dimY = exDimY;
        Camera.main.transform.eulerAngles = new Vector3(0, 0, 0);
        if (playerInQ.isZoomed)
            playerInQ.Zoom();
    }

}
