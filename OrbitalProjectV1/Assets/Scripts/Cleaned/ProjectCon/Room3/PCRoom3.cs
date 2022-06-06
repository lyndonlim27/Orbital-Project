using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PCRoom3 : RoomManager
{
    float cooldown;
    //ConsumableItemData consumables;

    public PolygonCollider2D consumableSpawn;
    private Vector2 consumableminArea;
    private Vector2 consumablemaxArea;
    private Material _ogmat;
    private Material _newmat;

    [SerializeField] private Light lightSource;
    [SerializeField] private TilemapRenderer[] tilemapRenderers;
    [SerializeField] private ConsumableItemBehaviour consumablePrefab;
    [SerializeField] List<ConsumableItemData> consumables;
    [SerializeField] private HealthBarEnemy bosshp;
    [SerializeField] private Image WarningPanel;

    private bool Coroutinerunning;
    private float intensity;
    private bool up;
    private float multiplier = 1;

    // Start is called before the first frame update
    void Start()
    {
        cooldown = 15;
        consumableminArea = consumableSpawn.bounds.min;
        consumablemaxArea = consumableSpawn.bounds.max;
        _ogmat = tilemapRenderers[0].material;
        _newmat = Resources.Load<Material>("Material/Alert");
        Coroutinerunning = false;
        intensity = 0.1f;
        up = true;

    }

    protected override void Update()
    {
        //activated && conditions == 0;
        if (WarningPanel == null && activated && CheckEnemiesDead())
        {
            StopAllCoroutines();
            foreach (TilemapRenderer tmr in tilemapRenderers)
            {
                tmr.material = _ogmat;
            }
        }
        else if (activated && !CheckEnemiesDead())
        {

            if (!Coroutinerunning)
            {
                ChangeMaterial();
                StartCoroutine(StartScene());
            }

            LightsOut();

            SpawnConsumables();
        }
        base.Update();
        RoomChecker();
        

    }

    private void LightsOut()
    {
        if (up)
        {
            lightSource.intensity = intensity;
            intensity += Time.deltaTime * multiplier ;
            if (intensity >= 0.6f)
            {
                up = !up;
            }
        }
        else
        {
            lightSource.intensity = intensity;
            intensity -= Time.deltaTime * multiplier ;
            if (intensity <= 0.1f)
            {
                up = !up;
            }
        }
    }

    private void SpawnConsumables()
    {
        if (cooldown <= 0)
        {
            InstantiateConsumables();
            ResetCooldown();
        }
        else
        {
            cooldown-= Time.deltaTime;
        }
    }

    private IEnumerator StartScene()
    {
        Coroutinerunning = true;
        WarningPanel.gameObject.SetActive(true);
        AudioSource au = GetComponent<AudioSource>();
        
        for (int i = 0; i < 3; i++)
        {
            //Fade in
            au.Play();
            float g;
            for (g = 0.5f; g <= 1f; g += 0.05f)
            {
                Color c = WarningPanel.color;
                c.a = g;
                WarningPanel.color = c;
                yield return new WaitForSeconds(0.03f);

            }

            //Fade Out
            for (; g >= 0.5f; g -= 0.05f)
            {
                Color c = WarningPanel.color;
                c.a = g;
                WarningPanel.color = c;
                yield return new WaitForSeconds(0.03f);
            }
         
            

        }
        Destroy(WarningPanel.gameObject);
        multiplier = 0.5f;
        bosshp.gameObject.SetActive(true);

    }

    private void ChangeMaterial()
    {
        foreach(TilemapRenderer tmr in tilemapRenderers)
        {
            tmr.material = _newmat;
        }
    }

    private void InstantiateConsumables()
    {
        foreach (ConsumableItemData data in consumables)
        {
            Vector2 randomPoint = GetRandomPoint(consumableminArea, consumablemaxArea);
            consumablePrefab.SetEntityStats(data);
            consumablePrefab.GetComponent<SpriteRenderer>().sprite = data.sprite;
            Instantiate(consumablePrefab, randomPoint, Quaternion.identity).SetCurrentRoom(this);
        }
        
        
    }

    private void ResetCooldown()
    {
        cooldown = 15;
    }


}
