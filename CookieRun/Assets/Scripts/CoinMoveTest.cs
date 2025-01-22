using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMoveTest : MonoBehaviour
{
    public float currentSpeed;
    public int Score = 100;
    public int Coins = 100;
    public float hp = 0f;
    public float duration = 0f;

    private void FixedUpdate()
    {
        transform.Translate(Vector3.left * currentSpeed * Time.deltaTime, Space.World);

        if (transform.position.x < -15f)
        {
            transform.position = new Vector3(9.6f, -2.5f, 0f);
        }
    }
}
