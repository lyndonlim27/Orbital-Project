//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class LaserBeam : EntityBehaviour
//{

//    private LineRenderer laser;
//    private List<Vector2> laserhits;

//    public override void Defeated()
//    {
//        throw new System.NotImplementedException();
//    }

//    public override EntityData GetData()
//    {
//        throw new System.NotImplementedException();
//    }

//    public override void SetEntityStats(EntityData stats)
//    {
//        throw new System.NotImplementedException();
//    }

//    protected override void Awake()
//    {
//        base.Awake();
//        laser = gameObject.AddComponent<LineRenderer>();
//    }

//    // Start is called before the first frame update
//    void Start()
//    {
        
//    }

//    private void SetUpLaser(Vector2 startpos, Vector2 dir)
//    {
//        laser.startColor = Color.red;
//        laser.endColor = Color.red;

//    }

//    private CastBeam()
//    {

//    }

//    private void OnEnable()
//    {
//        laserhits = new List<Vector2>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
        
//    }
//}
