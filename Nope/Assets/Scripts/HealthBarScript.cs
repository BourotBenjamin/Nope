using UnityEngine;
using System.Collections;

public class HealthBarScript : MonoBehaviour {

     
    public float initialLength;
    public GameObject healthBar;
    private Transform emptyBar;
    public float curHealth;
    public float maxHealth;
    public Transform cam;
     
    public float lastHealth;
     
    Vector3 greenPos;
    private Transform pos;
     
    CharactersAttributes healthScript;
         
    public float timer;
    float time = 2;
         
    void Awake ()
    {
        healthBar = gameObject;
        pos = transform;
        cam = Camera.allCameras[0].transform;
        //emptyBar = transform.parent.GetChild(0);
        //loads enemy health value from healthScript
        healthScript = transform.parent.gameObject.GetComponentInParent<CharactersAttributes>();
        curHealth = healthScript.currentHP = healthScript.hp;
        maxHealth = healthScript.hp;
         
        //stores two health values, will come in later
        lastHealth = curHealth;
    }
     
     
    void Update () 
    {
        
        greenPos = healthBar.transform.localPosition;
        greenPos.x = (-(maxHealth - curHealth)/maxHealth)/2;
        healthBar.transform.localPosition = greenPos;
        lastHealth = curHealth;
        curHealth = healthScript.currentHP;
        maxHealth = healthScript.hp;
         

        Vector3 greenScale = healthBar.transform.localScale;
        greenScale.x = (curHealth/maxHealth);
        pos.localScale = greenScale;
        //Camera cam = Camera.current;         
        //keeps bar facing camera

        pos.localRotation = Quaternion.LookRotation(cam.position-pos.position);
        //emptyBar.localRotation = Quaternion.LookRotation(cam.position - pos.position);
    }
}
