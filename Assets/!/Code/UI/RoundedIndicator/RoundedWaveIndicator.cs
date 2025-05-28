using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(Collider2D), typeof(Rigidbody2D))]
public class RoundedWaveIndicator : MonoBehaviour
{
    private LineRenderer lineRenderer;

    [SerializeField]
    private int pointsCount = 100;

    [SerializeField]
    private float radius = 2f;

    private float arcAngle = 360f;

    private Vector3[] positions;
    private readonly List<Transform>[] objectGroups = new List<Transform>[3];
    private Vector2 tempDirection;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        positions = new Vector3[pointsCount];

        for (int i = 0; i < objectGroups.Length; i++)
        {
            objectGroups[i] = new List<Transform>();
        }
    }

    private void Start()
    {
        lineRenderer.positionCount = pointsCount;
        lineRenderer.loop = true;
        lineRenderer.useWorldSpace = false;
        DrawWaveArc();
    }

    private void Update()
    {
        DrawWaveArc();
    }

    private void DrawWaveArc()
    {
        float angleStep = arcAngle / (pointsCount - 1);
        float currentTime = Time.time;

        for (int i = 0; i < pointsCount; i++)
        {
            float currentAngle = -arcAngle / 2 + i * angleStep;
            float angleRad = Mathf.Deg2Rad * currentAngle;
            float localFluctuation = 0f;
            float spikeHeight = 0f;

            for (int typeIndex = 0; typeIndex < objectGroups.Length; typeIndex++)
            {
                var objects = objectGroups[typeIndex];

                for (int j = objects.Count - 1; j >= 0; j--)
                {
                    var obj = objects[j];
                    if (obj == null)
                    {
                        objects.RemoveAt(j);
                        continue;
                    }

                    tempDirection =
                        (Vector2)obj.position
                        - (Vector2)transform.position
                        + new Vector2(4.4f, 7.70f);
                    float objectAngle =
                        Mathf.Atan2(tempDirection.y, tempDirection.x) * Mathf.Rad2Deg;
                    float angleDiff = Mathf.Abs(Mathf.DeltaAngle(currentAngle, objectAngle));

                    float distanceFactor = Mathf.Clamp01(1f - angleDiff / 7f);

                    if (distanceFactor > 0f)
                    {
                        switch (typeIndex)
                        {
                            case 0:
                                localFluctuation =
                                    Mathf.PerlinNoise(i * 0.1f, currentTime * 0.5f) * 0.05f
                                    + distanceFactor * Random.Range(-0.05f, 0.15f);
                                break;
                            case 1:
                                spikeHeight += Mathf.Lerp(0f, 0.5f, distanceFactor / 2);
                                break;
                            case 2:
                                spikeHeight += Mathf.Lerp(0f, 0.3f, distanceFactor);
                                break;
                        }
                    }
                }
            }

            float finalRadius = radius + spikeHeight + localFluctuation;
            float x = finalRadius * Mathf.Cos(angleRad);
            float y = finalRadius * Mathf.Sin(angleRad);
            positions[i] =
                transform.parent.position + new Vector3(x, y, 0) + new Vector3(4.4f, 7.70f, 0);
        }
        lineRenderer.useWorldSpace = false;

        lineRenderer.SetPositions(positions);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out RoundedIndicatorObjectType riot))
        {
            int typeIndex = (int)riot.type;
            if (!objectGroups[typeIndex].Contains(riot.transform))
            {
                objectGroups[typeIndex].Add(riot.transform);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.TryGetComponent(out RoundedIndicatorObjectType riot))
        {
            int typeIndex = (int)riot.type;
            objectGroups[typeIndex].Remove(riot.transform);
        }
    }
}
