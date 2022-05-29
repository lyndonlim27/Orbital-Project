using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlicker : MonoBehaviour
{
    private Material _flickerMaterial; 
    private float _duration = 0.125f;
    private SpriteRenderer _spriteRenderer;
    private Material _originalMaterial;
    private bool _playing;
    private Coroutine _flickerRoutine;
    // Start is called before the first frame update


    void Start()
    {
        _flickerMaterial = Resources.Load<Material>("Material/FlickerMaterial");
       // _playing = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Flicker(EntityBehaviour entityBehaviour)
    {
        if (_spriteRenderer == null && _originalMaterial == null)
        {
            _spriteRenderer = entityBehaviour.GetComponent<SpriteRenderer>();
            _originalMaterial = _spriteRenderer.material;
        }

        // If the flashRoutine is not null, then it is currently running.
        if (_flickerRoutine != null)
        {
            // In this case, we should stop it first.
            // Multiple FlashRoutines the same time would cause bugs.
            StopCoroutine(_flickerRoutine);
        }

        // Start the Coroutine, and store the reference for it.
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
