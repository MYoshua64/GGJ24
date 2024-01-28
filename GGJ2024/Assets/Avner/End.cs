using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    [SerializeField] AudioClip successClip;

    // Start is called before the first frame update
    Dictionary<string, bool> enteredEntities = new Dictionary<string, bool>
    {
        { "Monster", false },
        { "Child", false }
    };
    private void OnTriggerEnter(Collider other)
    {
        // if (enteredEntities.ContainsKey(other.name))
        // {
        //     enteredEntities[other.name] = true;
        // }
        //
        // foreach (bool entered in enteredEntities.Values)
        // {
        //     
        //     if (!entered)
        //     {
        //         return;
        //     }
        // }
        // Debug.Log("End of level!");
        //trigger level end

        if (other.TryGetComponent<Child>(out Child child))
        {
            if (child.IsReunited)
            {
                // TODO: Play win sound
                AudioSource.PlayClipAtPoint(successClip, Camera.main.transform.position, 0.5f);
                int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
                ScreenFader.instance.FadeWithLevelLoad(nextSceneIndex % SceneManager.sceneCountInBuildSettings);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (enteredEntities.ContainsKey(other.name))
        {
            enteredEntities[other.name] = false;
        }
    }
}
