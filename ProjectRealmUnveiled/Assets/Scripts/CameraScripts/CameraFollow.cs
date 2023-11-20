using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Player;
    public Vector3 offset;
    [Range(1, 10)]
    public float smoothFactor;

    public Vector3 minValue, maxValue;
    public static CameraFollow Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    private void FixedUpdate()
    {
        Follow();
    }
    void Follow()
    {
        Vector3 playerPosition = Player.position + offset;
        Vector3 boundPosition = new Vector3(Mathf.Clamp(playerPosition.x, minValue.x, maxValue.x),
            Mathf.Clamp(playerPosition.y, minValue.y, maxValue.y),
            Mathf.Clamp(playerPosition.y, minValue.z, maxValue.z));

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, boundPosition, smoothFactor * Time.fixedDeltaTime);
        transform.position = smoothedPosition;

    }
}
