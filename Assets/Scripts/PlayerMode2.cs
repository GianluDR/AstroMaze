using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Touchscreen = UnityEngine.InputSystem.Touchscreen;
using TMPro;

public class PlayerMode2 : MonoBehaviour
{
    [Header("Player time")]
    public float maxTime;
    public float timeValue;
    public float timeOT = 10f;
    public GameObject timeBar;

    [Header("Player attribute")]
    public Vector3 direction;
    public Vector2 movementInput;
    private Vector2 movInQ;
    private ContactFilter2D movementFilter;
    private Rigidbody2D rb;
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    public bool isMoving;
    public ScoreSystem scoreSys;
    public ScoreSystem movesSys;
    public Vector2 lastDir;
    public float lastPosX;
    public float lastPosY;
    public GameObject gameOver;
    public GameObject pause;
    public GameObject UI;

    public float moveSpeed = 2f;
    public float collisionOffset = 0.005f;
    public static PlayerMode2 Instance;
    public int score = 0;
    public int nToSolve;

    public float maxSpeed = 30f;
    public float timeZeroToMax = 1f;
    float accelRatePerSec;
    //float velocity;

    void Awake(){
        accelRatePerSec = maxSpeed / timeZeroToMax;
        //velocity = 0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        FindObjectOfType<AudioManager>().Play("Music2");
        FindObjectOfType<AudioManager>().normalVolume("Music2");
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
        rb = GetComponent<Rigidbody2D>();
        Instance = this;
        //GameObject.DontDestroyOnLoad(this.gameObject);
        isMoving = false;
        score = 0;
        GameObject.FindWithTag("Combo").GetComponent<ScoreSystem>().resetScore();
        gameOver.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        pause.transform.GetChild(0).gameObject.SetActive(false);

        //PlayerPrefs.DeleteAll();
        int highscore = PlayerPrefs.GetInt("HighScore");
        gameOver.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = highscore + "";
        lastDir = Vector2.zero;
        direction = Vector3.zero;
        rb.isKinematic = false;
        enabled = true;
    }
    
    private bool flag2 = false;
    void Update(){
        Slider timeSlider = timeBar.GetComponent<Slider>();
        Animator anim = timeBar.transform.GetChild(0).gameObject.GetComponent<Animator>();
        if(timeValue < 13){
            anim.SetBool("lowTime",true);
            FindObjectOfType<AudioManager>().AudioSpeed("Music2");
        }else{
            anim.SetBool("lowTime",false);
            FindObjectOfType<AudioManager>().AudioNormal("Music2");
        }
        timeValue = timeValue - timeOT * Time.deltaTime;
        timeSlider.value = timeValue / maxTime;
        if((nToSolve <= 0 || timeValue <=0) && !flag2){
            Time.timeScale = 0;
            anim.SetBool("lowTime",false);
            FindObjectOfType<AudioManager>().AudioNormal("Music2");
            FindObjectOfType<AudioManager>().StopPlaying("Music2");
            gameOver.transform.GetChild(0).gameObject.SetActive(true);
            gameOver.transform.GetChild(0).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = score + " Rooms cleared";
            int highscore = PlayerPrefs.GetInt("HighScore");
            if(highscore < score){
                PlayerPrefs.SetInt("HighScore",score);
                gameOver.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                gameOver.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = score + "";
            }
            flag2 = true;
        }
    }

    bool flag = false;
    void FixedUpdate()
    {
        direction = movementInput;
        direction = direction.normalized;
        direction = getDirection(direction);
        if(movInQ != Vector2.zero){
            lastDir = movInQ;
            lastDir = lastDir.normalized;
            lastDir = getDirection(lastDir);
        }

        bool moved = false;
        moved = TryMove(direction);
        direction = lastDir;

        //velocity += accelRatePerSec * Time.fixedDeltaTime;
        //velocity = Mathf.Min(velocity,maxSpeed);
        //moveSpeed = velocity;

        if(!isMoving && flag){
            nToSolve = nToSolve - 1;
            movesSys.scoreDown();
            flag = false;
        }else if(isMoving){
            flag = true;
        }
    }

    private Vector2 getDirection(Vector2 direction){
        Vector2 dir = Vector2.zero;
        if(direction.y > 0.7){
            dir = Vector2.up;
        }else if(direction.y < -0.7){
            dir = Vector2.down;
        }else if(direction.x < -0.7){
            dir = Vector2.left;
        }else if(direction.x > 0.7){
            dir = Vector2.right;
        }else{
            dir = Vector2.zero;
        }
        return dir;
    }

    public void stopMoving(){
        isMoving = false;
        direction = Vector3.zero;
        lastDir = Vector2.zero;
        movementInput = Vector2.zero;
        movInQ = Vector2.zero;
    }

    private bool TryMove(Vector2 direction)
    {
        Vector3 dir = direction;
        if (direction != Vector2.zero)
        {
            int count = rb.Cast(
                    direction,
                    movementFilter,
                    castCollisions,
                    (moveSpeed
                        * Time.fixedDeltaTime)
                        + collisionOffset);
            if (count == 0)
            {
                isMoving = true;
                rb.MovePosition(
                    rb.position
                    + direction
                    * moveSpeed
                    * Time.fixedDeltaTime);
                return true;
            } else {
                movementInput = lastDir;
                //velocity = 0;
                isMoving = false;
                return false;
            }
        }
        else {
            return false;
        }
    }

    public void OnJoystickMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }
    
    //private bool zoomed = true;
    public void OnButton()
    {
        /*if(zoomed){
            Camera.main.orthographicSize = 13;
            zoomed = false;
            Camera.main.gameObject.GetComponent<cameraFollow>().enabled = true;
        }else{
            Camera.main.orthographicSize = 25;
            zoomed = true;
            Camera.main.gameObject.GetComponent<cameraFollow>().enabled = false;
            Camera.main.transform.position = new Vector3(0,0,-10);
        }*/
        GridManager grid = GameObject.FindWithTag("Grid").GetComponent<GridManager>();
        stopMoving();
        GetComponent<TrailRenderer>().Clear();
        this.gameObject.SetActive(false);
        this.transform.position = new Vector3Int(((int)grid.startWorld.x),((int)grid.startWorld.y));
        this.gameObject.SetActive(true);
        FindObjectOfType<AudioManager>().Play("ButtonPressed");
        UI.transform.GetChild(0).gameObject.SetActive(false);
        UI.transform.GetChild(1).gameObject.SetActive(true);
    }
    

    public void OnMove(InputValue movementValue)
    {
        if(isMoving == false){
            movementInput = movementValue.Get<Vector2>();
        }
        movInQ = movementValue.Get<Vector2>();
    }

    public void OnResume()
    {
        pause.transform.GetChild(0).gameObject.SetActive(false);
        Time.timeScale = 1;
        FindObjectOfType<AudioManager>().normalVolume("Music2");
        FindObjectOfType<AudioManager>().Play("ButtonPressed");
    }

    public void OnStart()
    {
        pause.transform.GetChild(0).gameObject.SetActive(true);
        Time.timeScale = 0;
        FindObjectOfType<AudioManager>().lowVolume("Music2");
        FindObjectOfType<AudioManager>().Play("ButtonPressed");
    }

    public void OnSelect()
    {
        FindObjectOfType<AudioManager>().Play("ButtonPressed");
        pause.transform.GetChild(0).gameObject.SetActive(false);
        Time.timeScale = 1;
        Animator anim = timeBar.transform.GetChild(0).gameObject.GetComponent<Animator>();
        timeValue = maxTime;
        gameOver.transform.GetChild(0).gameObject.SetActive(false);
        GridManager grid = GameObject.FindWithTag("Grid").GetComponent<GridManager>();
        grid.reset();
        grid.dimX = 5;
        grid.dimY = 5;
        grid.createMap2();
        GameObject.FindWithTag("Combo").GetComponent<ScoreSystem>().resetScore();
        FindObjectOfType<AudioManager>().Play("Music2");
        FindObjectOfType<AudioManager>().normalVolume("Music2");
        score = 0;
        gameOver.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        UI.transform.GetChild(0).gameObject.SetActive(true);
        UI.transform.GetChild(1).gameObject.SetActive(false);
        flag2 = false;
    }

    public void OnMenu()
    {
        FindObjectOfType<AudioManager>().normalVolume("Music2");
        SceneManager.LoadScene("MainMenu");
    }

    public void OnReview(){
        gameOver.transform.GetChild(0).gameObject.SetActive(false);
        gameOver.transform.GetChild(1).gameObject.SetActive(true);
        GridManager grid = GameObject.FindWithTag("Grid").GetComponent<GridManager>();
        stopMoving();
        GetComponent<TrailRenderer>().Clear();
        this.gameObject.SetActive(false);
        this.transform.position = new Vector3Int(((int)grid.startWorld.x),((int)grid.startWorld.y));
        this.gameObject.SetActive(true);
    }

    public void OnBackReview(){
        gameOver.transform.GetChild(0).gameObject.SetActive(true);
        gameOver.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void setNToSolve(int n){
        nToSolve = n;
        movesSys.setScoreInt(nToSolve);
    }

}
