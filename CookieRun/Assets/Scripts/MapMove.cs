using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Renderer objectRenderer;
    private MapManager mapManager;
    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        mapManager = FindObjectOfType<MapManager>();
    }

    private void Update()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

        if (!objectRenderer.isVisible && gameObject.activeSelf)
        {
            DeactivateSelf();
        }
    }

    public void DeactivateSelf()
    {
        gameObject.SetActive(false);

        if (mapManager != null)
        {
            mapManager.DeactivatePrefab(gameObject);
        }


    }
}
 