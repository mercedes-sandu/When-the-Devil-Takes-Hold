using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Demon : MonoBehaviour
{

    public GameObject FireballPrefab;

    public float fireballDistance;
    public float fireballY;
    public float fireballVelocity;

    public float fireTime;
    private int timeTick;

    // Start is called before the first frame update
    void Start()
    {
        timeTick = 0;
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    private void FixedUpdate()
    {
        if (timeTick > fireTime)
        {
            FireProjectile();
            timeTick = 0;
        }
        else
        {
            timeTick++;
        }
    }

    void FireProjectile()
    {
        Vector3 ball_pos = new Vector3(transform.position.x + fireballDistance, transform.position.y + fireballY, transform.position.z);
        GameObject fireball = Instantiate(FireballPrefab, ball_pos, Quaternion.identity);
        //fireball.GetComponent<Rigidbody2D>().velocity = new Vector2(fireballVelocity, 0);
        fireball.GetComponent<AIDestinationSetter>().target = PlayerControl.Instance.gameObject.transform;
    }

    // PlayerControl.Instance
}
