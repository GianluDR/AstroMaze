using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.OnScreen;

public class UIManager : MonoBehaviour
{

    [Header("Movement")]
    public GameObject Up;
    public GameObject Down;
    public GameObject Left;
    public GameObject Right;
    public Image UpUI;
    public Image DownUI;
    public Image LeftUI;
    public Image RightUI;
    public Sprite UpImg;
    public Sprite DownImg;
    public Sprite LeftImg;
    public Sprite RightImg;
    public Sprite UpDone;
    public Sprite DownDone;
    public Sprite LeftDone;
    public Sprite RightDone;
    
    private string[] moveText = {"<Gamepad>/dpad/up",
                                "<Gamepad>/dpad/right",
                                "<Gamepad>/dpad/down",
                                "<Gamepad>/dpad/left"};
    private Color clicked;
    public int moveNum = 0;
    public GameObject camera;
    public Exit eventManager;

    [Header("EventTimebar")]
    public float maxTime;
    public float timeValue;
    public float timeOT;
    public GameObject EventUI;

    [Header("Lights")]
    public UnityEngine.Rendering.Universal.Light2D playerLight;
    public UnityEngine.Rendering.Universal.Light2D globalLight;

    void Start(){
        clicked = new Color(0.7f,1f,0.5f,1f);
    }

    void Update(){
        Slider timeSlider1 = EventUI.transform.GetChild(0).transform.GetChild(0).GetComponent<Slider>();
        Slider timeSlider2 = EventUI.transform.GetChild(0).transform.GetChild(1).GetComponent<Slider>();
        timeValue = timeValue - (timeOT) * Time.deltaTime;
        timeSlider1.value = timeValue / maxTime;
        timeSlider2.value = timeValue / maxTime;
        if(timeSlider1.value <= 0 || timeSlider2.value <= 0){
            EventUI.transform.GetChild(0).transform.GetChild(eventNum).gameObject.SetActive(false);
            EventUI.transform.GetChild(0).gameObject.SetActive(false);
        }
        Debug.Log(camera.transform.eulerAngles.z);
        if(moveNum == 0){
            if(camera.transform.eulerAngles.z > 1 && camera.transform.eulerAngles.z < 359){
                if(movePos)
                    camera.transform.Rotate (new Vector3 (0, 0, Time.deltaTime * 66f));
                else
                    camera.transform.Rotate (new Vector3 (0, 0, Time.deltaTime * -66f));
            }else{
                camera.transform.eulerAngles = new Vector3(0,0,0);
            }
        }
        if(moveNum == 1){
            if(camera.transform.eulerAngles.z < 89 || camera.transform.eulerAngles.z > 91){
                if(movePos)
                    camera.transform.Rotate (new Vector3 (0, 0, Time.deltaTime * 66f));
                else
                    camera.transform.Rotate (new Vector3 (0, 0, Time.deltaTime * -66f));
            }else
                camera.transform.eulerAngles = new Vector3(0,0,90);
        }
        if(moveNum == 2){
            if(camera.transform.eulerAngles.z < 179 || camera.transform.eulerAngles.z > 181){
                if(movePos)
                    camera.transform.Rotate (new Vector3 (0, 0, Time.deltaTime * 66f));
                else
                    camera.transform.Rotate (new Vector3 (0, 0, Time.deltaTime * -66f));
            }else
                camera.transform.eulerAngles = new Vector3(0,0,180);
        }
        if(moveNum == 3){
            if(camera.transform.eulerAngles.z < 269 || camera.transform.eulerAngles.z > 271){
                if(movePos)
                    camera.transform.Rotate (new Vector3 (0, 0, Time.deltaTime * 66f));
                else
                    camera.transform.Rotate (new Vector3 (0, 0, Time.deltaTime * -66f));
            }else
                camera.transform.eulerAngles = new Vector3(0,0,270);
        }
        
    }

    private bool movePos = true;
    public void rotate90(bool pos){
        if(pos){
            Debug.Log(pos);
            movePos = true;
            if(moveNum + 1 == 4)
                moveNum = 0;
            else
                moveNum = moveNum + 1;
        }else{
            Debug.Log(pos);
            movePos = false;
            if(moveNum - 1 == -1)
                moveNum = 3;
            else
                moveNum = moveNum - 1;
        }
        if(moveNum == 1){
            this.gameObject.transform.GetChild(3).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(5).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(6).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(4).gameObject.SetActive(true);
        }
        if(moveNum == 2){
            this.gameObject.transform.GetChild(4).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(6).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(3).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(5).gameObject.SetActive(true);
        }
        if(moveNum == 3){
            this.gameObject.transform.GetChild(3).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(4).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(5).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(6).gameObject.SetActive(true);
        }
        if(moveNum == 0){
            this.gameObject.transform.GetChild(4).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(5).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(6).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(3).gameObject.SetActive(true);
        }
    }

    public void eventReset(){
        eventManager.gridToNormal();
        eventManager.eventEnd();
    }

    public void resetMovement(){
        this.gameObject.transform.GetChild(4).gameObject.SetActive(false);
        this.gameObject.transform.GetChild(5).gameObject.SetActive(false);
        this.gameObject.transform.GetChild(6).gameObject.SetActive(false);
        this.gameObject.transform.GetChild(3).gameObject.SetActive(true);
        movePos = true;
    }

    public void SetMaxEventTime(){
        timeValue = maxTime;
    }

    private int eventNum;
    public void EventAppear(int eventN){
        eventNum = eventN+1;
        SetMaxEventTime();
        EventUI.transform.GetChild(0).gameObject.SetActive(true);
        EventUI.transform.GetChild(0).transform.GetChild(eventNum).gameObject.SetActive(true);
    }

    public void SetMoveAllActive(){
        UpUI.sprite = UpImg;
        DownUI.sprite = DownImg;
        LeftUI.sprite = LeftImg;
        RightUI.sprite = RightImg;
    }
    
    public void SetUpClick(){
        UpUI.sprite = UpDone;
    }
    public void SetDownClick(){
        DownUI.sprite = DownDone;
    }
    public void SetLeftClick(){
        LeftUI.sprite = LeftDone;
    }
    public void SetRightClick(){
        RightUI.sprite = RightDone;
    }
    public void SelectUp(){
        UpUI.color = clicked;
    }
    public void SelectDown(){
        DownUI.color = clicked;
    }
    public void SelectLeft(){
        LeftUI.color = clicked;
    }
    public void SelectRight(){
        RightUI.color = clicked;
    }
    public void DeselectAll(){
        UpUI.color = Color.white;
        DownUI.color = Color.white;
        LeftUI.color = Color.white;
        RightUI.color = Color.white;
    }

    public void LightToGlobal(){
        playerLight.intensity = 0;
        globalLight.intensity = 1;
    }
    public void LightToLocal(){
        playerLight.intensity = 1;
        globalLight.intensity = 0;
    }
}
