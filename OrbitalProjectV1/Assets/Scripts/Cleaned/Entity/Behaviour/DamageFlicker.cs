using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlicker : MonoBehaviour
{
    private Material _flickerMaterial; 
    private float _duration = 0.125f;
    private SpriteRenderer _spriteRenderer;
    private Material _originalMaterial;
    private Coroutine _flickerRoutine;
    // Start is called before the first frame update


    void Start()
    {
        _flickerMaterial = Resources.Load<Material>("Material/FlickerMaterial");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Flicker(EntityBehaviour entityBehaviour)
    {
        _spriteRenderer = entityBehaviour.GetComponent<SpriteRenderer>();
        _originalMaterial = _spriteRenderer.material;
        if (_flickerRoutine != null)
        {
            StopCoroutine(_flickerRoutine);
        }

        _flickerRoutine = StartCoroutine(FlickerRoutine());
    }

    private IEnumerator FlickerRoutine()
    {
        _spriteRenderer.material = _flickerMaterial;
        yield return new WaitForSeconds(_duration);
        _spriteRenderer.material = _originalMaterial;
        _flickerRoutine = null;
    }
}
