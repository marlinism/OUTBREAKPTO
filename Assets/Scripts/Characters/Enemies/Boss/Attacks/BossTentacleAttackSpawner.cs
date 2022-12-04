using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BossTentacleAttackSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject tentaclePrefab;

    [SerializeField]
    private GameObject spawnBoundTopRight;

    [SerializeField]
    private GameObject spawnBoundBottomLeft;

    private Vector2[] spawnDisplacement;
    private Vector2 topRightBound;
    private Vector2 bottomLeftBound;

    private ContactFilter2D overlapFilter;

    void Awake()
    {
        Assert.IsNotNull(tentaclePrefab);
        Assert.IsNotNull(spawnBoundTopRight);
        Assert.IsNotNull(spawnBoundBottomLeft);

        float distFromPlayer = 1.75f;
        float dirMagnitude = distFromPlayer / Mathf.Sqrt(2);
        spawnDisplacement = new Vector2[4];
        spawnDisplacement[0] = new(dirMagnitude, dirMagnitude);
        spawnDisplacement[1] = new(dirMagnitude, -dirMagnitude);
        spawnDisplacement[2] = new(-dirMagnitude, -dirMagnitude);
        spawnDisplacement[3] = new(-dirMagnitude, dirMagnitude);

        topRightBound = spawnBoundTopRight.transform.position;
        bottomLeftBound = spawnBoundBottomLeft.transform.position;

        overlapFilter = new();
        overlapFilter.useTriggers = true;
        overlapFilter.useLayerMask = true;
        overlapFilter.layerMask = LayerMask.GetMask("Obstacle") | LayerMask.GetMask("Signal");
    }

    public void Spawn(int tentacleCount)
    {
        if (tentacleCount <= 0)
        {
            return;
        }

        GameObject player = PlayerSystem.Inst.GetPlayer();
        if (player == null)
        {
            return;
        }

        if (tentacleCount >= 4)
        {
            TrySpawn((Vector2)player.transform.position - spawnDisplacement[0]);
            TrySpawn((Vector2)player.transform.position - spawnDisplacement[1]);
            TrySpawn((Vector2)player.transform.position - spawnDisplacement[2]);
            TrySpawn((Vector2)player.transform.position - spawnDisplacement[3]);
        }
        else
        {
            StartCoroutine(SpawnCoroutine(tentacleCount, player));
        }
    }

    private IEnumerator SpawnCoroutine(int tentacleCount, GameObject player)
    {
        for (int i = 0; i < tentacleCount; ++i)
        {
            int displacementIdx = Random.Range(0, 3);

            for (int j = 0; j < 4; j++)
            {
                if (TrySpawn((Vector2)player.transform.position - spawnDisplacement[displacementIdx]))
                {
                    break;
                }
                displacementIdx = (displacementIdx + 1) % 4;
            }

            yield return null;
        }

        yield break;
    }

    // Spawn a wave of tentacles across the spawn bounds
    // If tentacleCount < 0, tentacleCount will be calculated based on the area of the bounds
    public void SpawnWave(int tentacleCount = -1)
    {
        if (tentacleCount < 0)
        {
            float boundsArea = (Mathf.Abs(bottomLeftBound.x) + Mathf.Abs(topRightBound.x)) 
                    * (Mathf.Abs(bottomLeftBound.y) + Mathf.Abs(topRightBound.y));
            tentacleCount = (int)(boundsArea / 16);
        }

        StartCoroutine(SpawnWaveCoroutine(tentacleCount));
    }

    private IEnumerator SpawnWaveCoroutine(int tentacleCount)
    {
        for (int i = 0; i < tentacleCount; ++i)
        {
            Vector2 spawnPos = new();
            spawnPos.x = Random.Range(bottomLeftBound.x, topRightBound.x);
            spawnPos.y = Random.Range(bottomLeftBound.y, topRightBound.y);
            TrySpawn(spawnPos, Random.Range(0f, 0.5f));
            yield return null;
        }

        yield break;
    }

    // Try to spawn the a tentacle at a given spawn location
    // Returns false if there was an obstruction in the way or if the location was out of bounds
    // Otherwise returns true
    public bool TrySpawn(Vector2 spawnLocation, float tentacleDelaySeconds = 0f)
    {
        // Check bounds spawn
        if (spawnLocation.x < bottomLeftBound.x || spawnLocation.x > topRightBound.x)
        {
            return false;
        }
        if (spawnLocation.y < bottomLeftBound.y || spawnLocation.y > topRightBound.y)
        {
            return false;
        }

        // Check obstacle/tentacle spawn overlap
        List<Collider2D> results = new();
        Physics2D.OverlapCircle(spawnLocation, 0.3f, overlapFilter, results);
        foreach (Collider2D collider in results)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                return false;
            }
            if (collider.gameObject.HasTag("BossTentacle"))
            {
                return false;
            }
        }

        GameObject tentacle = Instantiate(tentaclePrefab);
        tentacle.transform.position = spawnLocation;

        BossTentacleAttackController tentacleController = tentacle.GetComponent<BossTentacleAttackController>();
        tentacleController.StartDelaySeconds = tentacleDelaySeconds;
        return true;
    }
}
