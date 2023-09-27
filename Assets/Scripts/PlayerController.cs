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

public class PlayerController : MonoBehaviour
{
    [Header("Player time")]
    public float maxTime;
    public float timeValue;
    public float timeOT = 1f;
    public float maxETime;
    public float timeEValue;
    public float timeEOT = 5f;
    public GameObject timeBar;

    [Header("Player attribute")]
    public Vector3 direction;
    private Vector2 movementInput;
    private Vector2 movInQ;
    private ContactFilter2D movementFilter;
    private Rigidbody2D rb;
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    public bool isMoving;
    public ScoreSystem scoreSys;
    public Vector2 lastDir;
    public float lastPosX;
    public float lastPosY;

    [Header("UI")]
    public UIManager UI;
    public GameObject gameOver;
    public GameObject pause;
    public Animator restartAnim;
    public Animator pauseAnim;

    public float moveSpeed = 2f;
    public float collisionOffset = 0.005f;
    public static PlayerController Instance;
    public int score = 0;

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
        FindObjectOfType<AudioManager>().Play("Music");
        FindObjectOfType<AudioManager>().normalVolume("Music");
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
        direction = Vector2.zero;
        rb.isKinematic = false;
        enabled = true;
    }
    
    private bool flag2 = false;
    void Update(){
        Slider timeErrorSlider = timeBar.transform.GetChild(0).GetComponent<Slider>();
        timeEValue = timeEValue - (timeEOT) * Time.deltaTime;
        timeErrorSlider.value = timeEValue / maxTime;
        Slider timeSlider = timeBar.GetComponent<Slider>();
        Animator anim = timeBar.transform.GetChild(1).gameObject.GetComponent<Animator>();
        if(timeValue < 13){
            anim.SetBool("lowTime",true);
            FindObjectOfType<AudioManager>().AudioSpeed("Music");
        }else{
            anim.SetBool("lowTime",false);
            FindObjectOfType<AudioManager>().AudioNormal("Music");
        }
        timeValue = timeValue - timeOT * Time.deltaTime;
        timeSlider.value = timeValue / maxTime;
        if(timeValue <= 0 && !flag2){
            Time.timeScale = 0;
            anim.SetBool("lowTime",false);
            FindObjectOfType<AudioManager>().AudioNormal("Music");
            FindObjectOfType<AudioManager>().StopPlaying("Music");
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

        //velocity += accelRatePerSec * Time.fixedDeltaTime;
        //velocity = Mathf.Min(velocity,maxSpeed);
        //moveSpeed = velocity;

        bool moved = false;
        Vector2 dir = new Vector2(direction.x,direction.y);
        if(!isMoving){
            UI.SetMoveAllActive();
        }else if(dir == Vector2.up){
            UI.SetUpClick();
        }else if(dir == Vector2.left){
            UI.SetLeftClick();
        }else if(dir == Vector2.right){
            UI.SetRightClick();
        }else if(dir == Vector2.down){
            UI.SetDownClick();
        }

        if(!isMoving){
            UI.DeselectAll();
        }else if(lastDir == Vector2.up){
            UI.DeselectAll();
            UI.SelectUp();
        }else if(lastDir == Vector2.left){
            UI.DeselectAll();
            UI.SelectLeft();
        }else if(lastDir == Vector2.right){
            UI.DeselectAll();
            UI.SelectRight();
        }else if(lastDir == Vector2.down){
            UI.DeselectAll();
            UI.SelectDown();
        }
        
        moved = TryMove(direction);
        direction = lastDir;
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

    public bool isZoomed = false;
    public void Zoom(){
        if(!isZoomed){
            Camera.main.orthographicSize = 8;
            isZoomed = true;
            Camera.main.gameObject.GetComponent<cameraFollow>().enabled = true;
        }else{
            Camera.main.orthographicSize = 25;
            isZoomed = false;
            Camera.main.gameObject.GetComponent<cameraFollow>().enabled = false;
            Camera.main.transform.position = new Vector3(0,0,-10);
        }
    }
    
    public void OnButton()
    {
        Slider timeErrorSlider = timeBar.transform.GetChild(0).GetComponent<Slider>();
        if(timeEValue < timeValue)
            timeEValue = timeValue;
        timeValue = timeValue - 15;
        restartAnim.SetTrigger("click");
        GridManager grid = GameObject.FindWithTag("Grid").GetComponent<GridManager>();
        stopMoving();
        GetComponent<TrailRenderer>().Clear();
        this.gameObject.SetActive(false);
        this.transform.position = new Vector3Int(((int)grid.startWorld.x),((int)grid.startWorld.y));
        this.gameObject.SetActive(true);
        FindObjectOfType<AudioManager>().Play("ButtonPressed");
    }
    

    public void OnMove(InputValue movementValue)
    {
        if(isMoving == false)
            movementInput = movementValue.Get<Vector2>();
        movInQ = movementValue.Get<Vector2>();
    }

    public void OnResume()
    {
        pause.transform.GetChild(0).gameObject.SetActive(false);
        Time.timeScale = 1;
        FindObjectOfType<AudioManager>().normalVolume("Music");
        FindObjectOfType<AudioManager>().Play("ButtonPressed");
    }

    public void OnStart()
    {
        pauseAnim.SetTrigger("click");
        pause.transform.GetChild(0).gameObject.SetActive(true);
        Time.timeScale = 0;
        FindObjectOfType<AudioManager>().lowVolume("Music");
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
        FindObjectOfType<AudioManager>().Play("Music");
        FindObjectOfType<AudioManager>().normalVolume("Music");
        score = 0;
        UI.eventReset();
        gameOver.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        flag2 = false;
    }

    public void OnMenu()
    {
        FindObjectOfType<AudioManager>().normalVolume("Music");
        SceneManager.LoadScene("MainMenu");
    }

    public void OnReview(){
        UI.eventReset();
        gameOver.transform.GetChild(0).gameObject.SetActive(false);
        gameOver.transform.GetChild(1).gameObject.SetActive(true);
        GridManager grid = GameObject.FindWithTag("Grid").GetComponent<GridManager>();
        stopMoving();
        UI.LightToGlobal();
        if(isZoomed)
            Zoom();
        GetComponent<TrailRenderer>().Clear();
        this.gameObject.SetActive(false);
        this.transform.position = new Vector3Int(((int)grid.startWorld.x),((int)grid.startWorld.y));
        this.gameObject.SetActive(true);
    }

    public void OnBackReview(){
        gameOver.transform.GetChild(0).gameObject.SetActive(true);
        gameOver.transform.GetChild(1).gameObject.SetActive(false);
    }

}
