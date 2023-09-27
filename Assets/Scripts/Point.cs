using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Point : MonoBehaviour
{
    public GameObject FloatingTextPrefab;
    private float point = 0;
    public Sprite[] sprites;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D hit)
    {
        var player = hit.gameObject.GetComponent<PlayerController>();
        if(player != null)
        {
            GameObject.FindWithTag("Trophy").GetComponent<ScoreSystem>().scoreUp();
            GridManager grid = GameObject.FindWithTag("Grid").GetComponent<GridManager>();
            point = Vector3.Distance(transform.position,grid.startWorld);
            if(point < 10)
                point = 5;
            else
                point = 8;
            if(player.timeValue + point > player.maxTime)
                player.timeValue = player.maxTime;
            else
                player.timeValue = player.timeValue + point;
            showFloatingText();
            FindObjectOfType<AudioManager>().Play("Coin");
            Destroy(this.gameObject);
        }
        var player2 = hit.gameObject.GetComponent<PlayerMode2>();
        if(player2 != null)
        {
            GridManager grid = GameObject.FindWithTag("Grid").GetComponent<GridManager>();
            point = Vector3.Distance(transform.position,grid.startWorld);
            if(point < 10)
                point = 3;
            else
                point = 5;
            if(player2.timeValue + point > player2.maxTime)
                player2.timeValue = player2.maxTime;
            else
                player2.timeValue = player2.timeValue + point;
            showFloatingText();
            FindObjectOfType<AudioManager>().Play("Coin");
            Destroy(this.gameObject);
        }
    }

    private void showFloatingText(){
        //var pointText = Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity,this.transform.parent);
        //pointText.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = "+" + point + "s";
    }
}
