using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float playerSpeed;
    public float directionalSpeed;
    public float clamp;
    public float boost;
    Rigidbody PlayerRB;
    float moveHorizontal;
    public AudioClip scoreUp;
    public AudioClip damage;
    public AudioClip coin;
    public GameObject SceneManager;

    bool isBoosted = false;
    int boostedScore;
    // Start is called before the first frame update
    void Start()
    {
        PlayerRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR || UNITY_WEBPLAYER || UNIT_STANDALONE 
        moveHorizontal = Input.GetAxis("Horizontal");
        transform.position = Vector3.Lerp(gameObject.transform.position, new Vector3(Mathf.Clamp(gameObject.transform.position.x + moveHorizontal, -clamp, clamp), gameObject.transform.position.y, gameObject.transform.position.z), directionalSpeed * Time.deltaTime);
#endif
        PlayerRB.velocity = (Vector3.forward * playerSpeed * Time.deltaTime);
        Vector2 touch = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10f));
        if(Input.touchCount > 0)
        {
            transform.position = Vector3.Lerp(gameObject.transform.position, new Vector3(touch.x, gameObject.transform.position.y, gameObject.transform.position.z), directionalSpeed * Time.deltaTime);
        }

        if(GetComponent<Score>().score != 0 && (GetComponent<Score>().score % 10) == 0 && !isBoosted)
        {
            playerSpeed *= boost;
            isBoosted = true;
            boostedScore = GetComponent<Score>().score;
        }

        if(isBoosted && GetComponent<Score>().score > boostedScore)
        {
            isBoosted = false;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "score gate")
        {
            GetComponent<AudioSource>().PlayOneShot(scoreUp, 1.0f);
        }
        else if (other.gameObject.tag == "triangle")
        {
            GetComponent<AudioSource>().PlayOneShot(damage, 1.0f);
            SceneManager.GetComponent<App_Initialize>().GameOver();
        }
        else if (other.gameObject.tag == "coin")
        {
            GetComponent<AudioSource>().PlayOneShot(coin, 1.0f);
            Destroy(other.gameObject);
        }
    }
}
