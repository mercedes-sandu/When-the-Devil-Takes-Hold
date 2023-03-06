using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LockedDoor : MonoBehaviour, IInteractable
{

    [SerializeField] GameObject playerObject;

    PlayerControl playerControl;


    // Start is called before the first frame update
    void Start()
    {
        playerControl = playerObject.GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool Interact(Interactor interactor)
    {
        if (playerControl.hasKey)
        {
            gameObject.SetActive(false);
        }
        else
        {
            
        }
        return true;
    }
}
