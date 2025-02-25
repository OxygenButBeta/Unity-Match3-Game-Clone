using System;
using System.Collections.Generic;
using UnityEngine;

public class g : MonoBehaviour{
    private List<string> list;
    private IReadOnlyList<string> _enumerable;

    private void Awake(){
        list = new List<string>();
        list.Add("1");
        list.Add("2");
        list.Add("3");
        _enumerable = list;
    }

    void Update(){
        foreach (var se in _enumerable){
            
        }
    }
}