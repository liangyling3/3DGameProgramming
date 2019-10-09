using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour {
    private FirstSceneController action;
    private GUIStyle fontstyle1 = new GUIStyle();
    
    void Start () {
        action = CurrentDirector.getInstance().currentSceneController as FirstSceneController;
        fontstyle1.fontSize = 30;
    }

    private void OnGUI() {
        if (GUI.Button(new Rect(Screen.width/2+100, 15, 100, 60), "重新开始")) {
            action.Restart();
        }

        if (GUI.Button(new Rect(Screen.width/2+230, 15, 100, 60), "暂停")) {
            action.Pause();
        }

        if (action.flag == 0) {
            fontstyle1.normal.textColor = Color.black;
            GUI.Label(new Rect(Screen.width/2-280, 30, 300, 100), "Score: " +
                action.score, fontstyle1);
            GUI.Label(new Rect(Screen.width/2-100, 30, 300, 100),"Round: " + (Mathf.CeilToInt(FirstSceneController.times/10)+1), fontstyle1);
        }
        else if (action.flag == 1) {
            fontstyle1.normal.textColor = Color.red;
            GUI.Label(new Rect(Screen.width/2, Screen.height/2, 300, 100), "最后得分 : " + action.score, fontstyle1);
        }
        else {
            fontstyle1.normal.textColor = Color.black;
            GUI.Label(new Rect(Screen.width/2-280, 30, 300, 100), "Score: " +
                action.score, fontstyle1);
            GUI.Label(new Rect(Screen.width/2-100, 30, 300, 100),"Round: " + (Mathf.CeilToInt(FirstSceneController.times/10)+1), fontstyle1);

            fontstyle1.normal.textColor = Color.red;
            GUI.Label(new Rect(Screen.width / 2, Screen.height/2, 300, 100), "暂停", fontstyle1);
        }
    }
}
