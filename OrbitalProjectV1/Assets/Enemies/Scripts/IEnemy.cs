using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    void attack(GameObject target);
    void start();
    void update();
    void exit();
}
