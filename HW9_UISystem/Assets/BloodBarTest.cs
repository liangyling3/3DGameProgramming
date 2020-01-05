using UnityEngine;
using System.Collections;

public class BloodBarTest : MonoBehaviour
{
    public GUISkin theSkin;
    public float bloodValue = 0.0f;
    private float tmpValue = 1.0f;
    private Rect rctBloodBar;
    private Rect rctUpButton;
    private Rect rctDownButton;
    private bool onoff;

    void Start()
    {
        //血条或者进度条-横向  
        GameObject obj = Instantiate(Resources.Load("prefabs/Blood")) as GameObject;
        GameObject par = GameObject.Find("Red");
        obj.transform.SetParent(par.transform);

        rctBloodBar = new Rect(450, 450, 150, 10);
        rctUpButton = new Rect(800, 200, 80, 40);
        rctDownButton = new Rect(800, 300, 80, 40);

    }

    void OnGUI()
    {
        GameObject obj = GameObject.Find("Black");
        Vector3 pos = obj.transform.position;
        //Debug.Log(pos);
        GUI.skin = theSkin;
        if (GUI.Button(rctUpButton, "黑方加血"))
        {
            tmpValue += 0.1f;
        }
        if (GUI.Button(rctDownButton, "黑方减血"))
        {
            tmpValue -= 0.1f;
        }
        if (tmpValue > 1.0f) tmpValue = 1.0f;
        if (tmpValue < 0.0f) tmpValue = 0.0f;

        Vector3 pos1 = pos + new Vector3(0.0f, 0.5f, 0.0f);
        Vector3 wpos = Camera.main.WorldToScreenPoint(pos);

        GUI.HorizontalScrollbar(new Rect(wpos.x-80, wpos.y,150, 10), 0.0f, tmpValue, 0.0f, 1.0f, GUI.skin.GetStyle("horizontalscrollbar"));

    }
}