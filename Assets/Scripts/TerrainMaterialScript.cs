using UnityEngine;

public class TerrainMaterialScript : MonoBehaviour
{
    public Terrain terrain;
    public Material terrainMaterial;

    void Start()
    {
        if (terrain != null && terrainMaterial != null)
        {
            terrain.materialTemplate = terrainMaterial;
        }
        else
        {
            Debug.LogError("Assign a Terrain and Material in the Inspector!");
        }
    }
}
