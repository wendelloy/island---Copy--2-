using UnityEngine;
using System.Collections.Generic;


public class WaterMovement : MonoBehaviour
{
    public float scrollSpeed = 0.1f; // Speed of the water movement
    private Material waterMaterial;

    void Start()
    {
        // Get the material from the water tile
        waterMaterial = GetComponent<Renderer>().material;
    }

    void Update()
    {
        // Move the texture over time to simulate flowing water
        float offset = Time.time * scrollSpeed;
        waterMaterial.SetTextureOffset("_MainTex", new Vector2(0, offset)); // Adjust "_MainTex" based on your texture
    }
}
