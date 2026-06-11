using UnityEngine;

public class FollowPhysics : MonoBehaviour
{
    public Transform target;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(target.transform.position);
    }
}
