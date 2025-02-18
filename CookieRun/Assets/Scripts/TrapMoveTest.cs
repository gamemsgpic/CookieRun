using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapMoveTest : MonoBehaviour
{
    public float currentSpeed;

    private void FixedUpdate()
    {
        transform.Translate(Vector2.left * currentSpeed * Time.deltaTime, Space.World);

        if (transform.position.x < -15f)
        {
            transform.position = new Vector2(9.6f, -2.5f);
        }
    }
}
