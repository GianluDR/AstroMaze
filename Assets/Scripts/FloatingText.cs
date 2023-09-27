using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float destroyTime = 3f;
    public Vector3 offset = new Vector3(0,1,0);
    public Vector3 RandomizeIntensitiy = new Vector3(0.5f,0.25f,0f);

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,destroyTime);
        transform.localPosition += offset;
        transform.localPosition += new Vector3(Random.Range(-RandomizeIntensitiy.x,RandomizeIntensitiy.x),
        Random.Range(-RandomizeIntensitiy.y,RandomizeIntensitiy.y),
        Random.Range(-RandomizeIntensitiy.z,RandomizeIntensitiy.z));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
