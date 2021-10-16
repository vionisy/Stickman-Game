using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class TerainCreator2d : MonoBehaviour
{
    public int TreeSpawning = 5;
    public GameObject Tree;
    public int Scale = 1000;
    private SpriteShapeController shape;
    public int numberOfPoints = 150;
    public int Edgesmoothness = 3;
    public int TerrainDiversity = 50;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hi");
        float distance_Between_points = Scale / numberOfPoints;
        shape = GetComponent<SpriteShapeController>();

        shape.spline.SetPosition(2, shape.spline.GetPosition(2) + Vector3.right * Scale);
        shape.spline.SetPosition(3, shape.spline.GetPosition(3) + Vector3.right * Scale);

        for(int i = 0; i < numberOfPoints; i++)
        {
            float xPosition = shape.spline.GetPosition(i + 1).x + distance_Between_points;
            shape.spline.InsertPointAt(i + 2, new Vector3(xPosition , TerrainDiversity * Mathf.PerlinNoise(i * Random.Range(5.0f, 5 + 10), 0)));
            float random = Random.Range(1, TreeSpawning);
            if (Tree && random == 1)
            {
                GameObject theTree = Instantiate(Tree, new Vector2(transform.position.x + shape.spline.GetPosition(i).x, shape.spline.GetPosition(i).y + 8), Quaternion.identity);
                theTree.transform.localScale = new Vector3(Random.Range(1.8f, 2.2f), Random.Range(1.9f, 2.3f), Random.Range(1.8f, 2.2f));
            }
        }

        for (int i = 2; i < numberOfPoints + 2; i++)
        {
            shape.spline.SetTangentMode(i, ShapeTangentMode.Continuous);
            shape.spline.SetLeftTangent(i, new Vector3(-Edgesmoothness, 0, 0));
            shape.spline.SetRightTangent(i, new Vector3(Edgesmoothness, 0, 0));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
