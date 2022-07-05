using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Size of the dungeon")]
    public int width;
    public int height;
    [Header("Look it up on the sprite or the transform")]
    public float tileSize;
    [Header("Percentage that will be filled with floor between 0-100")]
    public float percentage;
    private float realPercentage;
    public GameObject[] walls;
    public GameObject[] floors;

    private int[,] matrix;
    private Vector2 matrixPosition;
    private int blockNum;

    private Vector2 position;
    private Vector2 origin;

    // Start is called before the first frame update
    void Start()
    {
        Generate(); //Change when it is called if you want
    }

    // Update is called once per frame
    void Update()
    {

    }

    //This function generates the dungeon randomly
    private void Generate()
    {
        //Set the percentage
        realPercentage = percentage * 0.01f;
        
        if (realPercentage > 0.9f)
        {
            realPercentage = 0.9f;
        }
        else if (realPercentage < 0.1f)
        {
            realPercentage = 0.1f;
        }
        
        //Calculate max num of blocks
        blockNum = (int)(width * height * realPercentage);
        
        //Set position to current position
        position = transform.position;
        origin = transform.position;
        
        //Starts the matrix
        matrix = new int[width + 2,height + 2];

        for (int i = 1; i < width - 1; i++)
        {
            for (int j = 1; j < height - 1; j++)
            {
                matrix[i, j] = 0;
            }
        }
        
        //Start from the center
        matrixPosition = new Vector2(width/2, height/2);
        
        //Set floor in center position
        matrix[(int) matrixPosition.x, (int) matrixPosition.y] = 1;
        Instantiate(floors[Random.Range(0,floors.Length)], position, Quaternion.identity);

        //Start algorithm
        int k = 0;
        bool place = true;

        while (k < blockNum)
        {
            //Selects random direction
            int dir = Random.Range(0,4);
            
            /*Check the path that has followed with this
            Debug.Log("Direction is: " + dir + " in " + k + " iteration");
            Debug.Log("Position is: " + matrixPosition + " in " + k + " iteration");
            */
            
            //"Moves" towards direction checking if does not exceed width or height
            switch (dir)
            {
                case 0:
                    //Left
                    if (matrixPosition.x > 1)
                    {
                        matrixPosition.x--;
                        position.x-=tileSize;
                    }
                    else
                    {
                        place = false;
                    }
                    
                    break;
                case 1:
                    //Up
                    if (matrixPosition.y > 1)
                    {
                        matrixPosition.y--;
                        position.y+=tileSize;
                    }
                    else
                    {
                        place = false;
                    }
                    break;
                case 2:
                    //Right
                    if (matrixPosition.x < width - 2)
                    {
                        matrixPosition.x++;
                        position.x+=tileSize;
                    }
                    else
                    {
                        place = false;
                    }
                    break;
                case 3:
                    //Down
                    if (matrixPosition.y < height - 2)
                    {
                        matrixPosition.y++;
                        position.y-=tileSize;
                    }
                    else
                    {
                        place = false;
                    }
                    break;
            }

            //Check if there is already floor there
            if (matrix[(int) matrixPosition.x, (int) matrixPosition.y] != 1 && place)
            {
                matrix[(int) matrixPosition.x, (int) matrixPosition.y] = 1;
                Instantiate(floors[Random.Range(0,floors.Length)], position, Quaternion.identity);
                k++;
            }

            //Resets placement variable
            place = true;

            //Set walls in matrix if possible
            //Check if left is 0
            if (matrixPosition.x > 0 && matrix[(int) matrixPosition.x - 1, (int) matrixPosition.y] == 0)
            {
                matrix[(int) matrixPosition.x - 1, (int) matrixPosition.y] = 2;
            }
            //Check if Up is 0
            if (matrixPosition.y > 0 && matrix[(int) matrixPosition.x, (int) matrixPosition.y - 1] == 0)
            {
                matrix[(int) matrixPosition.x, (int) matrixPosition.y - 1] = 2;
            }
            //Check if right is 0
            if (matrixPosition.x < width - 1 && matrix[(int) matrixPosition.x + 1, (int) matrixPosition.y] == 0)
            {
                matrix[(int) matrixPosition.x + 1, (int) matrixPosition.y] = 2;
            }
            //Check if down is 0
            if (matrixPosition.y < height - 1 && matrix[(int) matrixPosition.x, (int) matrixPosition.y + 1] == 0)
            {
                matrix[(int) matrixPosition.x, (int) matrixPosition.y + 1] = 2;
            }
            
            //Check LeftUp
            if (matrixPosition.x > 0 && matrix[(int) matrixPosition.x - 1, (int) matrixPosition.y - 1] == 0 && matrixPosition.y > 0)
            {
                matrix[(int) matrixPosition.x - 1, (int) matrixPosition.y - 1] = 2;
            }
            //Check UpRight
            if (matrixPosition.y > 0 && matrix[(int) matrixPosition.x + 1, (int) matrixPosition.y - 1] == 0 && matrixPosition.x < width - 1)
            {
                matrix[(int) matrixPosition.x + 1, (int) matrixPosition.y - 1] = 2;
            }
            //Check RightDown
            if (matrixPosition.x < width - 1 && matrix[(int) matrixPosition.x + 1, (int) matrixPosition.y + 1] == 0 && matrixPosition.y < height - 1)
            {
                matrix[(int) matrixPosition.x + 1, (int) matrixPosition.y + 1] = 2;
            }
            //Check DownLeft
            if (matrixPosition.y < height - 1 && matrix[(int) matrixPosition.x - 1, (int) matrixPosition.y + 1] == 0 && matrixPosition.x > 0)
            {
                matrix[(int) matrixPosition.x - 1, (int) matrixPosition.y + 1] = 2;
            }
        }
        
        Debug.Log(origin);
        
        //Create walls
        //We use origin --> middle of the matrix --> width/2 and height/2
        Vector2 startPos = origin - new Vector2(width/2 * tileSize, - (height/2 * tileSize));
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (matrix[i, j] == 2)
                {
                    Instantiate(walls[Random.Range(0,walls.Length)], new Vector3(startPos.x + i*tileSize, startPos.y - j*tileSize, 0), Quaternion.identity);
                }
                
            }
        }
        Debug.Log(startPos);
    }
}
