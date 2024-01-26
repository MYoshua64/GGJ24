using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    bool setToExplode;
    List<GameObject> objectsToPush = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Child") || other.gameObject.name.Contains("Monster"))
        {
            objectsToPush.Add(other.gameObject);
            if (setToExplode) return;
            Debug.Log("About to explode!");
            WaitForExplosion();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (objectsToPush.Contains(other.gameObject))
        {
            objectsToPush.Remove(other.gameObject);
        }
    }

    async Task WaitForExplosion()
    {
        setToExplode = true;
        await Task.Delay(1500);
        foreach (GameObject obj in objectsToPush)
        {
            Debug.Log(obj);
            obj.GetComponentInParent<ControlledEntity>().Push((obj.transform.position - transform.position).normalized);
        }
        setToExplode = false;
        //Destroy(gameObject);
    }
}
