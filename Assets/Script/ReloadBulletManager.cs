using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReloadBulletManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float _TimeReload;
    [SerializeField] private float _ResetTime;
    void Start()
    {
        _TimeReload = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _TimeReload -= Time.deltaTime;
    }
    public bool CheckDKReload()
    {
        if(_TimeReload <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public float ResetTimeReload()
    {
        return _TimeReload = _ResetTime;
    }
}
