using Unity.Properties;
using UnityEngine;

[CreateAssetMenu(fileName = "ShipData", menuName = "Scriptable Objects/ShipData")]
public class ShipData : ScriptableObject
{
    public float SailStatus { get; set; }
    public float SailRotation { get; set; }
    public float Velocity { get; set; }

    [CreateProperty]
    public string StatusText => $"Sail: {Mathf.RoundToInt(SailStatus)}%";
    [CreateProperty]
    public string RotationText => $"Rotation: {Mathf.RoundToInt(SailRotation)}°";
    [CreateProperty]
    public string VelocityText => $"Velocity: {Mathf.RoundToInt(Velocity)}";
}
