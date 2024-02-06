using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollower : MonoBehaviour
{
    [SerializeField] float smoothDelta;
    [SerializeField] float yOffset;

    [SerializeField] Transform target;

    Transform camera;

    private void Awake()
    {
        camera = Camera.main.transform;

        if (camera == null)
        {
            print("No camera!");

            enabled = false;
        }

    }

    private void LateUpdate()
    {
        var targetPosition = target.position + Vector3.up * yOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothDelta * Time.deltaTime);

        var direction = camera.position - transform.position;

        var targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = targetRotation;
    }
}
