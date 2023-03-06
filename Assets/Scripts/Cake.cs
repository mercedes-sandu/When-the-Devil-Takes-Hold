using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class Cake : MonoBehaviour, IInteractable
{

    [SerializeField] private GameObject NPCObject;

    private NPC npc;

    [SerializeField] Vector3 stoppingPoint;

    [SerializeField] private GameObject playerObject;

    PlayerControl player;

    public bool pickedUp;

    [SerializeField] float window;

    [SerializeField] float weight;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        npc = NPCObject.GetComponent<NPC>();
        player = playerObject.GetComponent<PlayerControl>();
        pickedUp = false;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pickedUp)
        {
            gameObject.transform.position = player.GetPosition();
        }
    }

    void Place()
    {
        gameObject.transform.position = player.GetPosition();
        //spriteRenderer.enabled = false;
        pickedUp = false;
        if (true)//InLineOfSight())
        {
            stoppingPoint = gameObject.transform.position + npc.GetDirectionVector() * weight;
            npc.Walk(stoppingPoint);
        }
    }

    void PickUp()
    {
        //spriteRenderer.enabled = true;
        pickedUp = true;
    }

    bool InLineOfSight()
    {
        if (npc.GetDirection() == "Up" && npc.GetPosition().y < transform.position.y && npc.GetPosition().x <= transform.position.x + window && npc.GetPosition().x >= transform.position.x - window)
        {
            return true;
        }
        else if (npc.GetDirection() == "Down" && npc.GetPosition().y > transform.position.y && npc.GetPosition().x <= transform.position.x + window && npc.GetPosition().x >= transform.position.x - window)
        {
            return true;
        }
        else if (npc.GetDirection() == "Left" && npc.GetPosition().x > transform.position.x && npc.GetPosition().y <= transform.position.y + window && npc.GetPosition().y >= transform.position.y - window)
        {
            return true;
        }
        else if (npc.GetDirection() == "Right" && npc.GetPosition().x < transform.position.x && npc.GetPosition().y <= transform.position.y + window && npc.GetPosition().y >= transform.position.y - window)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Interact(Interactor interactor)
    {
        if (pickedUp)
        {
            Place();
        }
        else
        {
            PickUp();
        }
        return true;
    }
}