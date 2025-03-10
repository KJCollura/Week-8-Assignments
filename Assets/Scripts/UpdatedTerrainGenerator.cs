using UnityEngine;

public class UpdatedTerrainGenerator : MonoBehaviour
{
    public int width = 256;   // Terrain width
    public int height = 256;  // Terrain height (Z-axis size)
    public int depth = 50;    // Terrain height scale (Y-axis)

    public float scale = 20f;     // Terrain smoothness
    public float offsetX = 100f;  // Perlin noise offset
    public float offsetY = 100f;

    private Terrain terrain;

    private void Start()
    {
        // Get the Terrain component
        terrain = GetComponent<Terrain>();
        if (terrain == null)
        {
            Debug.LogError("No Terrain component found! Attach this script to a GameObject with a Terrain.");
            return;
        }

        // Assign random offsets for variety
        offsetX = Random.Range(0f, 9999f);
        offsetY = Random.Range(0f, 9999f);

        GenerateTerrain();
    }

    void GenerateTerrain()
    {
        TerrainData terrainData = terrain.terrainData;

        // Ensure heightmap resolution is correctly set
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, height);
        
        // Set the generated heights
        terrainData.SetHeights(0, 0, GenerateHeights());
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width + 1, height + 1];

        for (int x = 0; x <= width; x++)
        {
            for (int y = 0; y <= height; y++)
            {
                heights[x, y] = CalculateHeight(x, y);
            }
        }
        return heights;
    }

    float CalculateHeight(int x, int y)
    {
        float xCoord = (float)x / width * scale + offsetX;
        float yCoord = (float)y / height * scale + offsetY;
        return Mathf.PerlinNoise(xCoord, yCoord) * depth / terrain.terrainData.size.y;
    }
}
