using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] private Vector3 movementVector;
    [SerializeField] private float speed;
    float movementFactor;
    Vector3 startPosition;
    Vector3 endPosition;
    void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition + movementVector;
    }

    // Update is called once per frame
    void Update()
    {
        movementFactor = Mathf.PingPong(Time.time * speed, 1f);
        transform.position = Vector3.Lerp(startPosition, endPosition, movementFactor);
    }
}
