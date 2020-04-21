using UnityEngine;
using System.Collections.Generic;
using System;

// List<T>
[Serializable]
public class JsonList<T> {
    [SerializeField]
    List<T> target;
    public List<T> ToList() {
        return target;
    }
    public JsonList(List<T> target) {
        this.target = target;
    }
}

// Dictionary<TKey, TValue>
[Serializable]
public class JsonDictionary<TKey, TValue> : ISerializationCallbackReceiver {
    [SerializeField]
    List<TKey> keys;
    [SerializeField]
    List<TValue> values;
    Dictionary<TKey, TValue> target;
    public Dictionary<TKey, TValue> ToDictionary() {
        return target;
    }
    public JsonDictionary(Dictionary<TKey, TValue> target) {
        this.target = target;
    }
    public void OnBeforeSerialize() {
        keys = new List<TKey>(target.Keys);
        values = new List<TValue>(target.Values);
    }
    public void OnAfterDeserialize() {
        var count = Math.Min(keys.Count, values.Count);
        target = new Dictionary<TKey, TValue>(count);
        for (var i = 0; i < count; ++i) {
            target.Add(keys[i], values[i]);
        }
    }
}
class TestJsonListAndDic {
    void Test() {

        List<TestEnemy> enemies01 = new List<TestEnemy>();

        // List<T> -> Json ( 例 : List<Enemy> )
        string str01 = JsonUtility.ToJson(new JsonList<TestEnemy>(enemies01));
        // 输出 : {"target":[{"name":"怪物1,"skills":["攻击"]},{"name":"怪物2","skills":["攻击","恢复"]}]}
        
        // Json-> List<T>
        List<TestEnemy> enemies02 = JsonUtility.FromJson<JsonList<TestEnemy>>(str01).ToList();

        Dictionary<int, TestEnemy> enemies11 = new Dictionary<int, TestEnemy>();

        // Dictionary<TKey,TValue> -> Json( 例 : Dictionary<int, Enemy> )
        string str11 = JsonUtility.ToJson(new JsonDictionary<int, TestEnemy>(enemies11)); 
        // 输出 : {"keys":[1000,2000],"values":[{"name":"怪物1","skills":["攻击"]},{"name":"怪物2","skills":["攻击","恢复"]}]}

        // Json -> Dictionary<TKey,TValue>
        Dictionary<int, TestEnemy> enemies = JsonUtility.FromJson<JsonDictionary<int, TestEnemy>>(str11).ToDictionary();
    }
}

class TestEnemy {
    public string name = "";
    public string skills = "";
}