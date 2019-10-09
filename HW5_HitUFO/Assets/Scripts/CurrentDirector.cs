using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentDirector : System.Object {

    private static CurrentDirector _instance;
    public MySceneController currentSceneController { get; set; }

    CurrentDirector() {
        
    }

    public static CurrentDirector getInstance() {
        if (_instance == null)
            _instance = new CurrentDirector();
        return _instance;
    }

    public int getFPS() {
        return Application.targetFrameRate;
    }

    public void setFPS(int fps) {
        Application.targetFrameRate = fps;
    }

    public void NextScene() {

    }
}
