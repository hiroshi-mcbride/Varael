using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GroundCheck : MonoBehaviour
{
    [HideInInspector] public bool isGrounded = true;
    private bool wasGrounded;
    private List<Collider> colliders = new List<Collider>();
    private Collider col;

    public float nearestDistance = .0f;
    
    private void Start()
    {
        col = GetComponent<Collider>();
        isGrounded = colliders.Count > 0;
    }
    
    private void OnTriggerStay(Collider _other)
    {
        if (colliders.Contains(_other))
        {
            return;
        }
        colliders.Add(_other);
        
        isGrounded = true;
        if (!wasGrounded)
        {
            wasGrounded = true;
        }
    }

    private void OnTriggerExit(Collider _other)
    {
        
        if (colliders.Contains(_other))
        {
            colliders.Remove(_other);
        }

        if (colliders.Count == 0)
        {
            isGrounded = false;
            if (wasGrounded)
            {
                wasGrounded = false;
            }
        }
    }

    public float GetNearestDistance()
    {
        if (colliders.Count == 0)
        {
            return .0f;
        }
        float distance = Mathf.Infinity;

        foreach (Collider t in colliders)
        {
            float colliderTop = t.bounds.max.y;
            float diff = transform.position.y - colliderTop;
            if (diff < distance)
            {
                distance = diff;
            }
        }

        if (distance < col.bounds.size.y && distance > .0f)
        {
            return distance;
        }

        return .0f;
    }

}
