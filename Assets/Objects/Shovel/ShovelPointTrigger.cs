using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ShovelPointTrigger : MonoBehaviour
{
    public GameObject dirtPrefab;
    public GameObject chestPrefab;
    public GameObject keyPrefab; 
    public float spawnCooldown = 1.0f;
    public AudioClip[] dirtSfx;
    public AudioClip chestSfx;
    public AudioClip successSfx;
    public Slider cooldownSlider;
    private AudioSource audioSource;
    private bool canSpawn = true;
    private static List<string> availableTreasures = new List<string>{ "chest", "key" };

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (cooldownSlider != null)
        {
            cooldownSlider.gameObject.SetActive(false);
            cooldownSlider.minValue = 0;
            cooldownSlider.maxValue = 1;
        }

        if (availableTreasures.Count == 0)
        {
            availableTreasures = new List<string>{ "chest", "key" };
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canSpawn && other.CompareTag("DirtArea") && dirtPrefab != null)
        {
            float halfHeight = dirtPrefab.GetComponent<Renderer>().bounds.extents.y;
            Instantiate(dirtPrefab, transform.position + Vector3.down * halfHeight, Quaternion.identity);

            if (dirtSfx != null && dirtSfx.Length > 0 && audioSource != null)
                audioSource.PlayOneShot(dirtSfx[Random.Range(0, dirtSfx.Length)]);
            StartCoroutine(CooldownRoutine());
        }

        if (other.CompareTag("InterestArea") && availableTreasures.Count > 0)
        {
            int randomIndex = Random.Range(0, availableTreasures.Count);
            string selectedTreasure = availableTreasures[randomIndex];
            SpawnTreasure(selectedTreasure == "chest");
            
            availableTreasures.RemoveAt(randomIndex);

            if (chestSfx != null && audioSource != null)
                audioSource.PlayOneShot(successSfx);

            Destroy(other.gameObject);
        }
    }

    private void SpawnTreasure(bool spawnChest)
    {
        GameObject prefabToUse = spawnChest ? chestPrefab : keyPrefab;
        if (prefabToUse != null)
        {
            float halfHeight = prefabToUse.GetComponent<Collider>().bounds.extents.y;
            float extraOffset = spawnChest ? 0.5f : 1.5f;
            Vector3 spawnPosition = transform.position + Vector3.up * (halfHeight / 2 + extraOffset);
            //log "spawned at
            Debug.Log("Spawned at: " + spawnPosition);
            
            Instantiate(prefabToUse, spawnPosition, prefabToUse.transform.rotation);
        }
    }

    private IEnumerator CooldownRoutine()
    {
        canSpawn = false;
        if (cooldownSlider != null)
        {
            cooldownSlider.gameObject.SetActive(true);
            cooldownSlider.value = 1;
        }
        
        float elapsedTime = 0f;
        while (elapsedTime < spawnCooldown)
        {
            if (cooldownSlider != null)
                cooldownSlider.value = 1 - (elapsedTime / spawnCooldown);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        if (cooldownSlider != null)
            cooldownSlider.gameObject.SetActive(false);
        
        canSpawn = true;
    }

}


