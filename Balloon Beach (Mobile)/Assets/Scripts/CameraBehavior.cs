using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public GameObject Player;
    public float distance = 10f;

    private void Start()
    {
        distance = Vector3.Distance(Player.gameObject.transform.position, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, Player.transform.position.z - distance), Time.deltaTime*1000);
    }
}
