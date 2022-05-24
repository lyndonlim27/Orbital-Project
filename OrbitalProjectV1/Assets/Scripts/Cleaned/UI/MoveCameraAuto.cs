using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraAuto : MonoBehaviour
{
    /**
     * Move Camera with time.
     */
    void Update()
    {
        transform.Translate(1 * Time.deltaTime, 0, 0);
    }
}