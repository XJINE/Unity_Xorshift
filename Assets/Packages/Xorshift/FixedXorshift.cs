using System.Runtime.CompilerServices;
using UnityEngine;

public static class FixedXorshift
{
    #region Field

    // CAUTION:
    // Seed value should be large enough value.
    // If do not, such random show always quite similar value.
    public static uint Seed = 88675123u;

    private const float RadianMin = 0;
    private const float RadianMax = 6.283185307179586f;
    private const float UintMax   = uint.MaxValue;
    private const uint  SeedT     = 123456789u;
    private const uint  T         = SeedT ^ (SeedT << 11);
    private const uint  Knuth     = 2654435761;

    #endregion Field

    #region Property

    public static uint  ValueNative => GetValueNative(Seed);
    public static float Value       => GetValue(Seed);
    public static float Radian      => GetRadian(Seed);
    public static int   Sign        => GetSign(Seed);

    public static Vector2 OnUnitCircle
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Mathf.Cos(Radian), Mathf.Sin(Radian));
    }

    public static Vector2 InsideUnitCircle
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
        get => new(Mathf.Cos(Radian) * Value, Mathf.Sin(Radian) * Value);
    }

    public static Vector3 OnUnitSphere
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
        get
        {
            var phi   = GetRadian(Seed);
            var theta = GetRadian(Seed + Knuth);
            return new Vector3(Mathf.Sin(phi) * Mathf.Cos(theta),
                               Mathf.Sin(phi) * Mathf.Sin(theta),
                               Mathf.Cos(phi));
        }
    }

    public static Vector3 InsideUnitSphere
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => OnUnitSphere * Value;
    }

    #endregion Property

    #region Method

    #region Get~

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetValue(uint seed)
    {
        return GetValueNative(seed) / UintMax;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint GetValueNative(uint seed)
    {
        return (seed ^ (seed >> 19) ^ T ^ (T >> 8));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetRadian(uint seed)
    {
        return GetRange(seed, RadianMin, RadianMax);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetSign(uint seed)
    {
        return GetRange(seed, 0, 2) == 0 ? -1 : 1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetRange(uint seed, int min, int max)
    {
        return (int)(min + (max - min) * GetValue(seed) - 0.5f);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetRange(uint seed, float min, float max)
    {
        return min + (max - min) * GetValue(seed);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetRange(uint seed, Vector2 range)
    {
        return GetRange(seed, range.x, range.y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 GetRange(uint seed, Vector2 min, Vector2 max)
    {
        return new Vector2(GetRange(seed, min.x, max.x), GetRange(seed + Knuth, min.y, max.y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int GetRange(uint seed, Vector2Int min, Vector2Int max)
    {
        return new Vector2Int(GetRange(seed, min.x, max.x), GetRange(seed + Knuth, min.y, max.y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 GetRange(uint seed, Vector3 min, Vector3 max)
    {
        return new Vector3(GetRange(seed,                 min.x, max.x),
                           GetRange(seed + Knuth,         min.y, max.y),
                           GetRange(seed + Knuth + Knuth, min.z, max.z));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3Int GetRange(uint seed, Vector3Int min, Vector3Int max)
    {
        return new Vector3Int(GetRange(seed,                 min.x, max.x),
                              GetRange(seed + Knuth,         min.y, max.y),
                              GetRange(seed + Knuth + Knuth, min.z, max.z));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 GetRange(uint seed, Vector4 min, Vector4 max)
    {
        return new Vector4(GetRange(seed,                         min.x, max.x),
                           GetRange(seed + Knuth,                 min.y, max.y),
                           GetRange(seed + Knuth + Knuth,         min.z, max.z),
                           GetRange(seed + Knuth + Knuth + Knuth, min.w, max.w));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 GetRange(uint seed, Rect rect)
    {
        return GetRange(seed, rect.min, rect.max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 GetRange(uint seed, Bounds bounds)
    {
        return GetRange(seed, bounds.min, bounds.max);
    }

    #endregion Get~

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Range(int min, int max)
    {
        return GetRange(Seed, min, max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Range(float min, float max)
    {
        return GetRange(Seed, min, max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Range(Vector2 range)
    {
        return GetRange(Seed, range.x, range.y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Range(Vector2 min, Vector2 max)
    {
        return GetRange(Seed, min, max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int Range(Vector2Int min, Vector2Int max)
    {
        return GetRange(Seed, min, max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Range(Vector3 min, Vector3 max)
    {
        return GetRange(Seed, min, max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3Int Range(Vector3Int min, Vector3Int max)
    {
        return GetRange(Seed, min, max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Range(Vector4 min, Vector4 max)
    {
        return GetRange(Seed, min, max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Range(Rect rect)
    {
        return GetRange(Seed, rect);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Range(Bounds bounds)
    {
        return GetRange(Seed, bounds);
    }

    public static float[] Values(int count)
    {
        var localSeed = Seed;
        var values    = new float[count];

        for (var i = 0u; i < count; i++)
        {
            localSeed += (i + 1) * Knuth;
            values[i] = GetValue(localSeed);
        }

        return values;
    }

    #endregion Method
}