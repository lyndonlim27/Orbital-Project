using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class EntityBehaviour : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; protected set; }

    [SerializeField] protected RoomManager currentRoom;
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected bool inAudio;
    [SerializeField] protected AudioClip footStep;

    protected PoolManager poolManager;

    protected int health;

    public bool isDead;

    public bool inAnimation;

    public bool Debuffed;

    protected virtual void Awake()
    {
        poolManager = FindObjectOfType<PoolManager>(true);
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 1;
        audioSource = GetComponent<AudioSource>();
        inAudio = false;
        Debuffed = false;
        footStep = Resources.Load("Sounds/Player/FootStep") as AudioClip;
   
    }


    protected virtual void OnEnable()
    {
        if (GetData() != null)
        {
            spriteRenderer.sprite = GetData().sprite;
        }
        
    }

    


    public abstract void SetEntityStats(EntityData stats);

    public abstract void Defeated();

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0 && !isDead)
        {
            isDead = true;
            Defeated();
        }
    }

    protected virtual void ResettingColor()
    {
        Color c = spriteRenderer.material.color;
        c.a = 1;
        spriteRenderer.material.color = c;

    }

    public abstract EntityData GetData();

    protected virtual IEnumerator FootStepAudio()
    {
        if (!GetData().floating && GetData().moveable && !inAudio)
        {
            inAudio = true;
            audioSource.clip = footStep;
            audioSource.Play();
            yield return new WaitForSeconds(footStep.length);
            inAudio = false;
        }
    }


    public virtual IEnumerator FadeOut()
    {
        for (float f = 1f; f >= -0.05f; f -= 0.05f)
        {
            Color c = spriteRenderer.material.color;
            c.a = f;
            spriteRenderer.material.color = c;
            yield return new WaitForSeconds(0.05f);
        }
        //this.gameObject.SetActive(false);
        poolManager.ReleaseObject(this);
    }

    public virtual void SetCurrentRoom(RoomManager roomManager)
    {
        currentRoom = roomManager;
    }

    protected IEnumerator LoadSingleAudio(AudioClip audioClip)
    {
        audioSource.Stop();
        inAudio = true;
        float ogpitch = audioSource.pitch;
        audioSource.pitch = 1f;
        audioSource.clip = audioClip;
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        inAudio = false;
        audioSource.pitch = ogpitch;

    }

}
