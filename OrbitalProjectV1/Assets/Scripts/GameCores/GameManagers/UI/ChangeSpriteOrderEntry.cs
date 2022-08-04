using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ChangeSpriteOrderEntry : MonoBehaviour
{
    [SerializeField]
    GameObject spriteLayer;
    SpriteRenderer[] spriteRenderers;
    Color transparent;
    Color originalcolor;

    private void Awake()
    {

        spriteRenderers = spriteLayer.GetComponentsInChildren<SpriteRenderer>();
        transparent = new Color(0, 0, 0, 0);
        originalcolor = new Color(1, 1, 1, 1);
        // 2 ways to do it. Either changing alpha or changing sorting order, dk how performance is affected here, but ok.
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Stealth")
        {
            foreach (SpriteRenderer renderer in spriteRenderers)
            {
                renderer.color = transparent;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Stealth")
        {
            foreach (SpriteRenderer renderer in spriteRenderers)
            {
                renderer.color = originalcolor;
            }
        }
    }

}
