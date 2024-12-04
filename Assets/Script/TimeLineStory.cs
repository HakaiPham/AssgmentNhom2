using UnityEngine;
using UnityEngine.Playables;


public class TimeLineStory : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private PlayableDirector _TimeLineStartStory;
    [SerializeField] private PlayableDirector _TimeLineBadEnding;
    [SerializeField] private PlayableDirector _TimeLineHappyEnding;
    public Player _Player;
    public QuestNV2 _Quest;
    bool _StopBadEnding = false;
    bool _StopHappyEnding = false;
    bool isStartTimeLine = false;
    void Start()
    {
        if (_TimeLineStartStory != null)
            _TimeLineStartStory.stopped += EndStartStory;
        if (_TimeLineBadEnding != null)
            _TimeLineBadEnding.stopped += EndBadEnding;
        if (_TimeLineHappyEnding != null)
            _TimeLineHappyEnding.stopped += EndHappyEnding;
        if (_TimeLineStartStory != null)
        {
            //Chạy theo thời gian thực 
            _TimeLineStartStory.timeUpdateMode = DirectorUpdateMode.UnscaledGameTime;
            Time.timeScale = 0f;
            isStartTimeLine = true;
            _TimeLineStartStory.Play();
        }    
    }   

    // Update is called once per frame
    void Update()
    {
        if(!_StopBadEnding)
        {
            StartBadEnding();
        }
        if(!_StopHappyEnding)
        {
            StartHappyEnding();
        }
        
    }
    private void EndStartStory(PlayableDirector director)
    {
        Time.timeScale = 1f;
        isStartTimeLine = false;

    }
    private void StartBadEnding()
    {
        bool checkPlayerDead = _Player.PlayerIsDead();
        if(checkPlayerDead&&_TimeLineBadEnding.state!=PlayState.Playing)
        {
            _TimeLineBadEnding.timeUpdateMode = DirectorUpdateMode.UnscaledGameTime;
            Time.timeScale = 0f;
            _TimeLineBadEnding.Play();
            _StopBadEnding = true;
            isStartTimeLine = true;
        }
    }
    private void EndBadEnding(PlayableDirector director)
    {
        Time.timeScale = 1f;
        isStartTimeLine = false;

    }
    private void StartHappyEnding()
    {
        bool checkMissionFinished = _Quest.CheckProgressMission();
        if(checkMissionFinished&&_TimeLineHappyEnding.state!=PlayState.Playing)
        {//ĐIều kiện này TimeLine chỉ chạy 1 lần xác định TimeLine có đang chạy hay không?
            //Tránh việc bị chạy nhiều lần?
            _TimeLineHappyEnding.timeUpdateMode = DirectorUpdateMode.UnscaledGameTime;
            Time.timeScale = 0;
            _StopHappyEnding = true;
            _TimeLineHappyEnding.Play();
            isStartTimeLine = true;

        }
    }
    private void EndHappyEnding(PlayableDirector director)
    {
        Time.timeScale = 1f;
        isStartTimeLine = false;
    }
    public bool CheckTimeLineStart()
    {
        return isStartTimeLine;
    }
}
