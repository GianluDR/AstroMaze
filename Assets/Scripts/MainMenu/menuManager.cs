using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject playMenu;
    public GameObject settingMenu;
    public GameObject exitMenu;
    private int[] gamemode;
    private int actualGM;

    private const int maxGM = 2;

    void Start(){
        //FindObjectOfType<AudioManager>().Play("Music");
        //FindObjectOfType<AudioManager>().lowVolume("Music");
        gamemode = new int[2];
        actualGM = 0;
    }

//MAINMENU
    public void normalPlay(){
        FindObjectOfType<AudioManager>().Play("ButtonPressed");
        SceneManager.LoadScene("Game");
    }
    public void hardcorePlay(){
        FindObjectOfType<AudioManager>().Play("ButtonPressed");
        SceneManager.LoadScene("Game2");
    }
    public void rightPlay(){
        mainMenu.transform.GetChild(0).GetChild(actualGM).gameObject.SetActive(false);
        if(actualGM+1 >= maxGM)
            actualGM = 0;
        else
            actualGM++;
        FindObjectOfType<AudioManager>().Play("ButtonPressed");
        mainMenu.transform.GetChild(0).GetChild(actualGM).gameObject.SetActive(true);
    }
    public void leftPlay(){
        mainMenu.transform.GetChild(0).GetChild(actualGM).gameObject.SetActive(false);
        if(actualGM-1 < 0)
            actualGM = maxGM-1;
        else
            actualGM--;
        FindObjectOfType<AudioManager>().Play("ButtonPressed");
        mainMenu.transform.GetChild(0).GetChild(actualGM).gameObject.SetActive(true);
    }

//PLAY
    public void inPlay(){
        mainMenu.transform.GetChild(0).gameObject.SetActive(false);
        playMenu.transform.GetChild(0).gameObject.SetActive(true);
        FindObjectOfType<AudioManager>().Play("ButtonPressed");
    }
    public void backPlay(){
        mainMenu.transform.GetChild(0).gameObject.SetActive(true);
        playMenu.transform.GetChild(0).gameObject.SetActive(false);
        FindObjectOfType<AudioManager>().Play("ButtonPressed");
    }
    

//SETTINGS
    public void inSetting(){
        mainMenu.transform.GetChild(0).gameObject.SetActive(false);
        settingMenu.transform.GetChild(0).gameObject.SetActive(true);
        FindObjectOfType<AudioManager>().Play("ButtonPressed");
    }
    public void backSetting(){
        mainMenu.transform.GetChild(0).gameObject.SetActive(true);
        settingMenu.transform.GetChild(0).gameObject.SetActive(false);
        FindObjectOfType<AudioManager>().Play("ButtonPressed");
    }

//EXIT
    public void inExit(){
        mainMenu.transform.GetChild(0).gameObject.SetActive(false);
        exitMenu.transform.GetChild(0).gameObject.SetActive(true);
        FindObjectOfType<AudioManager>().Play("ButtonPressed");
    }
    public void backExit(){
        mainMenu.transform.GetChild(0).gameObject.SetActive(true);
        exitMenu.transform.GetChild(0).gameObject.SetActive(false);
        FindObjectOfType<AudioManager>().Play("ButtonPressed");
    }
    public void yesExit(){
        Application.Quit();
        FindObjectOfType<AudioManager>().Play("ButtonPressed");
    }
}
