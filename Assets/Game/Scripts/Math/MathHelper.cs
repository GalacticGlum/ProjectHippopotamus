using UnityEngine;

public static class MathHelper
{
    public const float Tau = Mathf.PI * 2.0f;

    public static Vector2i ToVector2i(this Vector2 vector)
    {
        return new Vector2i(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));
    }

    public static Vector2i ToVector2i(this Vector3 vector)
    {
        return new Vector2i(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));
    }

    public static Vector2i ToVector2i(this Vector4 vector)
    {
        return new Vector2i(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));
    }

    public static bool Approximately(Vector3 a, Vector3 b, double precision = 0.0001)
    {
        return Vector3.SqrMagnitude(a - b) < precision;
    }
}

