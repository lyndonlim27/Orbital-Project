using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
using GameManagement;
using EntityDataMgt;

namespace EntityCores
{
    [RequireComponent(typeof(Animator), typeof(Collider2D), typeof(DoorAudioComp))]
    public class DoorBehaviour : EntityBehaviour
    {
        public bool unlocked;
        protected Animator animator;
        private HashSet<RoomManager> roomManagers;

        private DoorAudioComp doorAudioComp;
        private Tilemap terrainWallTilemap;
        private Tilemap innerWallTilemap;
        private Tilemap outerWallTilemap;

        private GridsHolder gridsHolder;

        private bool removed;

        private List<string> conditions;
        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
            roomManagers = new HashSet<RoomManager>();


        }

        protected override void Start()
        {
            base.Start();
            ResetStatus();
            GetTerrains();
            CheckForCollisions();

        }

        private void GetTerrains()
        {
            gridsHolder = GridsHolder.instance;
            if (gridsHolder != null)
            {
                innerWallTilemap = gridsHolder.GetTilemap("Wall");
                outerWallTilemap = gridsHolder.GetTilemap("Outerwall");
            }
        }

        private void ResetStatus()
        {
            isDead = false;
            unlocked = false;
            removed = false;
        }

        private void CheckForCollisions()
        {
            Collider2D col = Physics2D.OverlapCircle(transform.position, 0.01f, LayerMask.GetMask("Obstacles"));
            if (col != null && col.CompareTag("Tiles"))
            {
                Destroy(this.gameObject);
            }
        }

        //in case perlin fails, this is fallback check.
        public void RemoveTerrainWalls()
        {
            bool terraingenerated = _GameManager.allTerrainsGenerated;

            if (terraingenerated && !removed)
            {

                removed = true;
                TerrainGenerator _tergen = TerrainGenerator.instance;

                if (_tergen == null)
                {
                    return;
                }
                else
                {
                    terrainWallTilemap = _tergen.GetTerrainWall();
                    Vector3Int currentpos = Vector3Int.FloorToInt(transform.position);

                    Vector3Int startingPos = Vector3Int.back;
                    for (int i = -1; i <= 1; i++)
                    {
                        if (i == 0)
                        {
                            continue;
                        }
                        var pos1 = currentpos + new Vector3Int(i, 0);
                        var pos2 = currentpos + new Vector3Int(0, i);


                        if (terrainWallTilemap.HasTile(pos1))
                        {
                            startingPos = pos1;

                        }

                        if (terrainWallTilemap.HasTile(pos2))
                        {
                            startingPos = pos2;
                        }

                    }

                    if (startingPos != Vector3Int.back)
                    {
                        //means there is wall blocking the entrace.
                        FindCheapestPath(startingPos, currentpos);
                    }

                }
            }

        }

        private void ClearPavement(Vector3Int curr, Vector3Int dest)
        {
            List<Vector3Int> path = new List<Vector3Int>();
            path.Add(curr);
            while (curr != dest)
            {

                if (curr.x < dest.x)
                {
                    var newpos = curr + Vector3Int.right;
                    path.Add(newpos);
                    curr = newpos;
                }
                else if (curr.x > dest.x)
                {
                    var newpos = curr + Vector3Int.left;
                    path.Add(newpos);
                    curr = newpos;
                }
                else
                {
                    if (curr.y < dest.y)
                    {
                        var newpos = curr + Vector3Int.up;
                        path.Add(newpos);
                        curr = newpos;
                    }
                    else
                    {
                        var newpos = curr + Vector3Int.down;
                        path.Add(newpos);
                        curr = newpos;
                    }

                }
            }

            foreach (Vector3Int vec in path)
            {
                terrainWallTilemap.SetTile(vec, null);
            }

        }

        protected virtual void Update()
        {
            if (unlocked)
            {

                CheckDoorUnlockedSound();
                UnlockDoor();

            }
            else
            {
                CheckDoorLockedSound();
                LockDoor();

            }

        }

        protected virtual void CheckDoorUnlockedSound()
        {
            if (!animator.GetBool(gameObject.name.Substring(0, 4)))
            {
                doorAudioComp.UnlockAudio();
            }
        }

        protected virtual void CheckDoorLockedSound()
        {
            if (animator.GetBool(gameObject.name.Substring(0, 4)))
            {
                doorAudioComp.LockAudio();
            }
        }
        public override void Defeated()
        {

        }

        public override EntityData GetData()
        {
            return null;
        }

        public override void SetEntityStats(EntityData stats)
        {
            return;
        }

        public virtual void UnlockDoor()
        {
            animator.SetBool(gameObject.name.Substring(0, 4), true);
            GetComponent<Collider2D>().enabled = false;


        }

        public virtual void LockDoor()
        {
            animator.SetBool(gameObject.name.Substring(0, 4), false);
            GetComponent<Collider2D>().enabled = true;
        }



        public override IEnumerator FadeOut()
        {
            for (float f = 1f; f > 0; f -= 0.05f)
            {
                Color c = spriteRenderer.material.color;
                c.a = f;
                spriteRenderer.material.color = c;
                yield return new WaitForSeconds(0.05f);
            }
            inAnimation = false;
        }

        protected IEnumerator FadeIn()
        {
            for (float f = 0f; f < 1f; f += 0.05f)
            {
                Color c = spriteRenderer.material.color;
                c.a = f;
                spriteRenderer.material.color = c;
                yield return new WaitForSeconds(0.05f);
            }

            inAnimation = false;
        }

        public void SetRoomControllers(RoomManager room)
        {
            roomManagers.Add(room);
        }

        public HashSet<RoomManager> GetRoomManagers()
        {
            return roomManagers;
        }

        private void FindCheapestPath(Vector3Int currentpos, Vector3Int startingPos)
        {
            List<Vector3Int> pointsTocheck = new List<Vector3Int>();
            System.Comparison<Vector3Int> comparer = (a, b) => Vector3Int.Distance(a, startingPos).CompareTo(Vector3Int.Distance(b, startingPos));
            pointsTocheck.Add(currentpos);
            Vector3Int destination = Vector3Int.back;
            List<Vector3Int> visited = new List<Vector3Int>();
            List<Vector3Int> path = new List<Vector3Int>();
            while (pointsTocheck.Count > 0)
            {
                List<Vector3Int> newlist = new List<Vector3Int>();
                bool found = false;
                foreach (Vector3Int vec in pointsTocheck)
                {

                    if (vec == startingPos || visited.Contains(vec) || CheckInsideWall(vec))
                    {
                        continue;
                    }
                    if (!terrainWallTilemap.HasTile(vec))
                    {
                        destination = vec;
                        found = true;

                    }
                    else
                    {
                        visited.Add(vec);
                        for (int i = -1; i <= 1; i++)
                        {
                            if (i == 0)
                            {
                                continue;
                            }
                            else
                            {
                                newlist.Add(vec + new Vector3Int(i, 0));
                                newlist.Add(vec + new Vector3Int(0, i));
                            }

                        }
                    }
                }
                if (found)
                {

                    break;
                }
                else
                {
                    pointsTocheck = newlist;
                    pointsTocheck.Sort(comparer);
                }

            }

            if (destination == Vector3Int.back)
            {
                Debug.Log("No path found, this is weird");
            }
            else
            {
                terrainWallTilemap.SetTile(startingPos, null);
                ClearPavement(currentpos, destination);

            }
        }

        private bool CheckInsideWall(Vector3Int vec)
        {
            return innerWallTilemap.HasTile(vec) || outerWallTilemap.HasTile(vec);
        }


    }
}
