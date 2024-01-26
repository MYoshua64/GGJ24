using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.name)
        {
            case "Monster":
                Destroy(gameObject);
                break;
            case "Child":
                Debug.Log("Player hit, game over");
                break;
            default:
                break;
        }
    }
}
