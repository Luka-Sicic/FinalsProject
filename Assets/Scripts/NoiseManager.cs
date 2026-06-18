using UnityEngine;

public static class NoiseManager
{
    public static void MakeNoise(Vector2 position, float radius)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius);
        foreach (var col in colliders)
        {
            if (col.TryGetComponent<INoiseListener>(out var listener))
            {
                listener.OnHearNoise(position);
            }
        }
    }
}
