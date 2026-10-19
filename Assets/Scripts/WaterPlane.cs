using UnityEngine;

public class WaterPlane : MonoBehaviour
{
    [SerializeField] private Transform _shipTransform;

    private float _waterHeight;

    private void Start()
    {
        _waterHeight = transform.position.y;
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(
            _shipTransform.position.x,
            _waterHeight,
            _shipTransform.position.z
        );
    }
}
