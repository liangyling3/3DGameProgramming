using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UFOFactory : MonoBehaviour {
    public List<GameObject> used = new List<GameObject>();
    public List<GameObject> buf = new List<GameObject>();

    // 产生UFO
    public void produceUFO() {
        GameObject ufo;
        if (buf.Count == 0) {
            ufo = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/UFO"), Vector3.zero, Quaternion.identity);
        }
        else {
            ufo = buf[0];
            buf.RemoveAt(0);
        }
        float x = Random.Range(-10.0f, 10.0f);
        // 随机产生位置
        ufo.transform.position = new Vector3(x, 0, 0);
        // 随机角度
        ufo.transform.Rotate(new Vector3(x < 0? -x*9 : x*9, 0, 0));
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);
        Color color = new Color(r, g, b);
        // 随机产生颜色
        ufo.transform.GetComponent<Renderer>().material.color = color;
        used.Add(ufo);
    }

    // 回收UFO
    public void recycleUFO(GameObject obj) {
        obj.transform.position = Vector3.zero;
        buf.Add(obj);
    }
}