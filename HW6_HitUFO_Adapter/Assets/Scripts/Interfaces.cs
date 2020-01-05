using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 整合接口
public interface IActionManager {
    void PlayUFO();
    void Pause();
}

public enum SSActionEventType:int { Started, Completed}

public interface ISSActionCallback {
    void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Completed, int intParam = 0, string strParam = null, Object objParam = null);
    void CheckEvent(SSAction source, SSActionEventType events = SSActionEventType.Completed, int intParam = 0, string strParam = null, Object objParam = null);
}


public interface ISceneController {
    void GenGameObjects();
}

public interface IUserAction {
    void Restart();
    void Pause();
}