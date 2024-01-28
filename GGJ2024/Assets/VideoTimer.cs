using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoTimer : MonoBehaviour
{
    VideoPlayer player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<VideoPlayer>();
        player.loopPointReached += Player_loopPointReached;
        //WaitToStartPlay();
    }

    async Task WaitToStartPlay()
    {
        await Task.Delay(1100);
        player.Play();
    }

    private void Player_loopPointReached(VideoPlayer source)
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        ScreenFader.instance.FadeWithLevelLoad((sceneIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
