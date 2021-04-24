using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAbillity : MonoBehaviour


{
    public float GravitationScale;
    public KeyCode Key;
    [SerializeField] private List<Rigidbody2D> Gravity01;

    void Update()
    {
        if (Input.GetKey(Key))
        {
            foreach (Rigidbody2D gravitation in Gravity01)
            {
                gravitation.gravityScale = GravitationScale;
            }
        }
        else
        {
            foreach (Rigidbody2D gravitation in Gravity01)
            {
                gravitation.gravityScale = 1f;
            }
        }
    }
}