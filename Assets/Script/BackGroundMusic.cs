using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMusic : MonoBehaviour
{
    // Start is called before the first frame update
    public TimeLineStory _TimeLine;
    AudioSource _AudioSource;
    bool _isPlaying = false;
    void Start()
    {
        _AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        bool _CheckTimeLine = _TimeLine.CheckTimeLineStart();
        if (_CheckTimeLine&&!_isPlaying)
        {
            _AudioSource.Stop();
            _isPlaying = true;
        }
        else if(!_CheckTimeLine&&_isPlaying)
        {
            _AudioSource.Play();
            _isPlaying = false;
        }
    }
}
