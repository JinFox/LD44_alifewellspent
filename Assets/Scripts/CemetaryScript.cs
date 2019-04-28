using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CemetaryScript : MonoBehaviour
{
    public CinemachineVirtualCamera cemetaryCam;

    public Transform flowerRoot;
    List<Transform> flowers;

    public GameObject[] tombstones;

    private void Start()
    {
        cemetaryCam.enabled = false;
        flowers = new List<Transform>();
        foreach (Transform T in flowerRoot) {
            flowers.Add(T);
            T.gameObject.SetActive(false);
        }
        
    }
    

    private void DisplayCorrectFlowers(float scoreEnjoy)
    {

        foreach (var flower in flowers) {
            if (UnityEngine.Random.Range(0f, 1f) <= scoreEnjoy) {
                flower.gameObject.SetActive(true);
            }
            else {
                flower.gameObject.SetActive(false);
            }
        }
    }

    private void DisplayCorrectTombstone(float profitScore)
    {
        if (tombstones == null || tombstones.Length < 1)
            return;
        foreach (GameObject tomb in tombstones) {
            tomb.SetActive(false);
        }
        int indexToEnable = Mathf.Min(
                Mathf.FloorToInt(Mathf.Clamp01(profitScore) * tombstones.Length),
                tombstones.Length - 1
            );

        tombstones[indexToEnable].SetActive(true);
    }

    public void EnableCemetary(GameReward scores)
    {
        gameObject.SetActive(true);
        flowerRoot.gameObject.SetActive(true);
        cemetaryCam.enabled = true;

        DisplayCorrectTombstone(scores.profit);
        DisplayCorrectFlowers(scores.enjoyment);
       
    }
    public void DisableCemetary()
    {
        flowerRoot.gameObject.SetActive(false);
    }
    
}
