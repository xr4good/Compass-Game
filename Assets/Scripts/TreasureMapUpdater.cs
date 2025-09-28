using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TreasureMapUpdater : MonoBehaviour
{
    public GameObject pinPrefab;
    public Transform chestAreasContainer;
    public Transform mapObject; 
    public Transform terrainPivotBottomLeft;
    public Transform terrainPivotTopRight;
    public Transform mapPivotBottomLeft;
    public Transform mapPivotTopRight;

    private List<GameObject> spawnedPins = new List<GameObject>();

    private Transform pinsContainer;
    

    void Start()
    {
        pinsContainer = new GameObject("Pins").transform;
        pinsContainer.SetParent(mapObject);
        pinsContainer.localPosition = Vector3.zero;

        List<Transform> chestAreas = new List<Transform>();
        foreach (Transform child in chestAreasContainer)
        {
            chestAreas.Add(child);
        }

        if (chestAreas.Count < 2)
            return;

        List<Transform> selectedAreas = chestAreas.OrderBy(x => Random.value).Take(2).ToList();

        foreach (Transform area in chestAreas)
        { 
            
            area.gameObject.SetActive(selectedAreas.Contains(area));
        }  
        
        float terrainSize = Vector3.Distance(terrainPivotBottomLeft.position, terrainPivotTopRight.position);

        float mapSize = Vector3.Distance(mapPivotBottomLeft.position, mapPivotTopRight.position);

        foreach (var area in selectedAreas)
        {
            Vector3 pinPosition = WorldToMapCoordinates(area.position, terrainSize, mapSize);

            GameObject pin = Instantiate(pinPrefab, pinPosition, pinPrefab.transform.rotation);
            pin.transform.SetParent(pinsContainer, true);
            spawnedPins.Add(pin);
        }
    }

    Vector3 WorldToMapCoordinates(Vector3 worldPos, float terrainSize, float mapSize)
    {
        float percentX = (worldPos.x - terrainPivotBottomLeft.position.x) / terrainSize;
        float percentZ = (worldPos.z - terrainPivotBottomLeft.position.z) / terrainSize;

        float mapX = mapPivotBottomLeft.position.x + (percentX * mapSize);
        float mapZ = mapPivotBottomLeft.position.z + (percentZ * mapSize);

        return new Vector3(mapX, mapPivotBottomLeft.position.y + 0.0125f, mapZ);
    }
}
