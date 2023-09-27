using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    private TextMeshProUGUI textObject;
    public int score = 0;
    private string text;

    public void scoreUp(){
        score++;
        text = score + "";
        FindObjectOfType<AudioManager>().Play("Coin");
        setScore(text);
    }

    public void setScore(string text){
        textObject = GetComponent<TextMeshProUGUI>();
        if(text != null)
            textObject.text = text;
    }

    public void resetScore(){
        score = 0;
        text = score + "";
        setScore(text);
    }

    public void scoreDown(){
        score = score - 1;
        text = score + "";
        //this.gameObject.GetComponent<Animation>().Play();
        setScore(text);
    }

    public void setScoreInt(int newScore){
        score = newScore; 
        text = score + ""; 
        setScore(text);
    }

    // Start is called before the first frame update
    void Start()
    {
        textObject = GetComponent<TextMeshProUGUI>();
    }

    void FixedUpdate()
    {

    }
}
