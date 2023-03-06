using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LockedDoor : MonoBehaviour, IInteractable
{

    [SerializeField] GameObject playerObject;

    PlayerControl playerControl;

    [SerializeField] GameObject tilemapObject;

    Tilemap tilemap;

    [SerializeField] private Vector3Int doorCoords;

    [SerializeField] private int doorWidth;

    [SerializeField] private int doorHeight;

    // Start is called before the first frame update
    void Start()
    {
        playerControl = playerObject.GetComponent<PlayerControl>();
        tilemap = tilemapObject.GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool Interact(Interactor interactor)
    {
        if (playerControl.hasKey)
        {
            // something something tilemap destroy
            for (int i = 0; i < doorWidth; i++)
            {
                for (int j = 0; j < doorHeight; j++)
                {
                    tilemap.SetTile(new Vector3Int(doorCoords.x + i, doorCoords.y + j, doorCoords.z), null);
                }
            }
            gameObject.SetActive(false);
        }
        else
        {
            
        }
        return true;
    }
}
