using UnityEngine;

/// <summary>
/// Spins UI
/// </summary>
public class SpinningUI : MonoBehaviour
{
    [SerializeField] private float spinningSpeed = 2f;

    private void Update()
    {
        Spin();
    }

    private void Spin()
    {
        transform.Rotate(Vector3.forward * spinningSpeed * Time.deltaTime);
    }
}
