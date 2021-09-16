using TMPro;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainSim : TerrainBase
{
    public GameObject flagPrefab;

    private void Start()
    {
        Init();
        LoadTerrain();
        GenerateFlags();
    }

    public void GenerateFlags()
    {
        for (int x = 0; x < groundSize; x += 5)
        {
            GameObject flag = Instantiate(flagPrefab, new Vector3(x, GetHeightAtPoint(x), 5), Quaternion.identity, transform);
            GameObject dist = flag.transform.GetChild(2).gameObject;
            TextMeshPro text = dist.GetComponent<TextMeshPro>();
            text.SetText(x.ToString() + "m");
        }
    }
}