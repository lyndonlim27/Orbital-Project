using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{

    LineRenderer lineRenderer;
    Transform origin;
    [SerializeField] private LayerMask layerMask;
    float duration;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>(); 
    }

    private void Start()
    {
        lineRenderer.enabled = false;
        lineRenderer.useWorldSpace = false;
        origin = transform;
    }

    public IEnumerator AnimateLine()
    {
        RaycastHit2D hit = Physics2D.Raycast(origin.localPosition, transform.right, Mathf.Infinity, layerMask);
        Debug.Log(hit.collider);
        if (hit)
        {
            lineRenderer.enabled = true;
            float startTime = Time.time;
            Debug.DrawLine(transform.localPosition, hit.point);
            Debug.Log(hit.collider);
            Vector2 pos = transform.localPosition;
            float startRotation = transform.eulerAngles.y;
            float endRotation = startRotation + 360.0f;
            float t = 0.0f;
            duration = 3f;
            while (pos != hit.point)
            {
                pos = Vector2.Lerp(origin.localPosition, hit.point, (Time.time - startTime) / 2f);
                lineRenderer.SetPosition(1, pos);
                CheckForPlayer(hit);
                yield return null;
            }

            
            while (t < duration)
            {
                t += Time.deltaTime;
                float zRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % 360.0f;
                lineRenderer.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zRotation);
                hit = Physics2D.Raycast(origin.position, -transform.right, Mathf.Infinity, layerMask);
                CheckForPlayer(hit);
                lineRenderer.SetPosition(1, hit.point);
                yield return null;
            }
            //this.enabled = false;
            ResetLineRenderer();
        }
    }

    private void CheckForPlayer(RaycastHit2D hit)
    {
        if (hit)
        {
            Player player = hit.collider.GetComponentInChildren<Player>();
            if (player != null)
            {
                player.TakeDamage(3);
            }
        }
        
    }

    private void ResetLineRenderer()
    {
        lineRenderer.enabled = false;
        //GetComponentInParent<RangedComponent>().parent.resetCooldown();
        lineRenderer.SetPosition(1, new Vector2(0, 0));
        transform.rotation = Quaternion.identity;
    }
}
