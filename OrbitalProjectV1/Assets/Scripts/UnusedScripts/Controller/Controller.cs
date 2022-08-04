using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public static Vector3 north = new Vector3(0, 1, 0);

    public void Move(Vector3 pos)
    {
        this.transform.position += pos;
    }
}
