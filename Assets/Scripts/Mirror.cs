using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour, IInteractable
{
    /// <summary>
    /// Degrees to rotate the window by upon interaction.
    /// </summary>
    private int degrees = 25;

    /// <summary>
    /// True when mirror has been completely turned to the right. False when completely turned to the left.
    /// </summary>
    private bool turned = false;
    
    /// <summary>
    /// Calls for the UI to be updated.
    /// </summary>
    /// <param name="interactor">The interactor component for the mirror.</param>
    /// <returns>True if the interaction was successful, false otherwise.</returns>
    public bool Interact(Interactor interactor)
    {
        if (transform.rotation.y > Quaternion.Euler(new Vector3(0, 40, 0)).y)
        {
            turned = true;
        }

        if (transform.rotation.y < Quaternion.Euler(new Vector3 (0, 50, 0)).y & !turned)
        {
            transform.rotation *= Quaternion.Euler(Vector3.up * degrees);
        }
        else
        {
            if (transform.rotation.y > Quaternion.Euler(new Vector3(0, -40, 0)).y)
            {
                transform.rotation *= Quaternion.Euler(Vector3.up * -degrees);
            }
            else turned = false;
        }

        return true;
    }
}
