using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarEnemy : MonoBehaviour
{
    public GameObject hpBarController;
    private Animator[] acHpBars;
    private Animator currBar;
    public EliteMonsterA boss;
    public int currlength;

    private void Awake()
    {
        acHpBars = hpBarController.GetComponentsInChildren<Animator>();
        currlength = acHpBars.Length;
    }

    private void Update()
    {


    }


    public IEnumerator MinusHealth()
    {
        currBar = acHpBars[--currlength];
        currBar.SetTrigger("Hurt");
        yield return new WaitForSeconds(.5f);
        currBar.gameObject.SetActive(false);

    }

    public void TakeDamage()
    {

        if(currlength < 0)
        {
            return;

        } else
        {
            StartCoroutine(MinusHealth());
        }
        
    }

    public void RegainHealth()
    {
        if (currlength >= acHpBars.Length)
        {
            return;
        }
        else
        {
            StartCoroutine(RegenHP());
        }
    }

    public IEnumerator RegenHP()
    {
         
        yield return new WaitForSeconds(.5f);
        currBar = acHpBars[currlength++];
        currBar.gameObject.SetActive(true);
        currBar.SetTrigger("Regen");
        
        
    }

    public bool HPBarFull()
    {
        return currlength == acHpBars.Length;
    }

    public bool HalfHP()
    {
        return currlength <= (int)(acHpBars.Length / 2);
    }

    public bool QuarterHP()
    {
        return currlength <= (int) (acHpBars.Length / 4);
    }
}
