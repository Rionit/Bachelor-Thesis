using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Finish : MonoBehaviour
{
    public VideoPlayer video;
    
    void Start()
    {
        video.loopPointReached += EndReached;
    }

    void EndReached(VideoPlayer vp)
    {
        Destroy(gameObject);
    }
}
