using UnityEngine;

public class FixedXorshiftSample : MonoBehaviour
{
    public float[] values;
    public Vector2 insideUnitSphere;
    public Vector2 range;

    private void Start()
    {
        values           = FixedXorshift.Values(600);
        insideUnitSphere = FixedXorshift.InsideUnitSphere;
        range = FixedXorshift.Range(Vector2.one * 0.5f, Vector2.one * 3f);
    }
}