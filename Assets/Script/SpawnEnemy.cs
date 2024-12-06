using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject[] _enemy;
    [SerializeField] private int _MaxEnemyOnScreen;
    List<GameObject> _CurrentQuantityEnemy;
     [SerializeField] private QuestNV2 _QuestNV2;
    Coroutine _Coroutine;
    [SerializeField] private float minDistance;
    bool _ChangeMaxEnemyOnScreen = false;//Biến bool xác định chỉ chạy 1 lần
    void Start()
    {
        _CurrentQuantityEnemy = new List<GameObject>();
        _Coroutine = StartCoroutine(SpawnEnemySystem());//gọi 1 lần TMDK Coroutine sẽ dừng lại
    }

    // Update is called once per frame
    void Update()
    {
        if(!_ChangeMaxEnemyOnScreen)
        {
            bool checkProgressBarMission2 = _QuestNV2.CheckProgressBarStart();
            if (checkProgressBarMission2)
            {
                _MaxEnemyOnScreen = 11;
                _ChangeMaxEnemyOnScreen = true;
            }
        }
    }
    public void CheckDkSpawn()//Hàm chạy Coroutine sẽ chạy lại khi Enemy bị tiêu diệt
    {
        if (_Coroutine == null&&_CurrentQuantityEnemy.Count <_MaxEnemyOnScreen)
        {
            _Coroutine = StartCoroutine(SpawnEnemySystem());
        }
    }
    public void Spawn()
    {
        //Random vị trí quái Spawn
        //Tổng số quái được xuất hiện trên map là 10
        //dưới 10 là được spawn tiếp
        //Xuất hiện quái tinh anh khi người chơi 
        //kích hoạt thanh tiến trình nv
        //Các quái không thể bị dính nhau có khoảng cách
    }
    IEnumerator SpawnEnemySystem()
    {
        int spawnAttempts = 0;
        while (spawnAttempts < 10 && _CurrentQuantityEnemy.Count < _MaxEnemyOnScreen)
        {
            spawnAttempts++;
            int randomEnemySpawn = RandomEnemy();
            Vector3 postionEnemy = RandomPositionSpawn();
            GameObject enemySpawn = Instantiate(_enemy[randomEnemySpawn], postionEnemy, Quaternion.identity);
            _CurrentQuantityEnemy.Add(enemySpawn);
            yield return new WaitForSeconds(Random.Range(0.5f,1f));
        }

        _Coroutine = null;
        yield break;

    }
    public Vector3 RandomPositionSpawn()
    {
        const int maxAttempts = 100;
        int attempts = 0;
        bool postionIsValid;
        Vector3 position = Vector3.zero;

        do
        {
            attempts++;
            int random = Random.Range(0,4);
            switch (random)
            {
                case 0: position = new Vector3(473f, 0.2000008f, 391f); break;
                case 1: position = new Vector3(404.2f, 0.2000008f, 328.62f); break;
                case 2: position = new Vector3(401.7f, 0.2000008f, 459.4f); break;
                case 3: position = new Vector3(342.5f, 0.2000008f, 395.6f); break;
                default: position = Vector3.zero; break;
            }

            postionIsValid = true;
            foreach (var enemy in _CurrentQuantityEnemy)
            {
                if (Vector3.Distance(position, enemy.transform.position) < minDistance)
                {
                    postionIsValid = false;
                    break;
                }
            }
        } while (!postionIsValid && attempts < maxAttempts);

        return position;

    }
    public int RandomEnemy() //tỉ lệ
    {
        int randomEnemySpawn = Random.Range(0, 100);
        bool isStartProgressMission = _QuestNV2.CheckProgressBarStart();
        if (randomEnemySpawn <= (isStartProgressMission?50:80)) //Nếu đúng là 50 ngược lại là 80
        {
            return 0;//Enemy thường
        }
        return 1;//Enemy tinh anh
       
    }
    public void reduceListEnemyWhenEnemyDead(GameObject gameObject)//Giảm 1 mỗi khi enemy bị tiêu diệt
    {
        _CurrentQuantityEnemy.Remove(gameObject);
        CheckDkSpawn();//Khởi động Coroutine
    }
}
