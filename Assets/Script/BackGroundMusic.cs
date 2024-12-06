using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMusic : MonoBehaviour
{
    // Start is called before the first frame update
    public TimeLineStory _TimeLine;
    public AudioSource _AudioSource;
    public Player player;
    float _SoundStarttime;
    public float resetTime;
    public AudioSource _ZombieAudioSource;
    bool _IsPlayingBGMusic = false;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        bool checkPlayerIsDead = player.PlayerIsDead();
        if (checkPlayerIsDead)
        {
            _AudioSource.Stop();
            _ZombieAudioSource.Stop();
            Time.timeScale = 0;
        }
        bool _CheckTimeLine = _TimeLine.CheckTimeLineStart();
        if (_CheckTimeLine)
        {
            _AudioSource.Stop();
            _ZombieAudioSource.Stop();
        }
        else if(!_CheckTimeLine)
        {
            if (!_IsPlayingBGMusic)
            {
                _AudioSource.Play();
                _IsPlayingBGMusic=true;
            }
            if (Time.time > _SoundStarttime)
            {
                _ZombieAudioSource.Play();
                _SoundStarttime = Time.time + resetTime;
            }
        }
    }
}
