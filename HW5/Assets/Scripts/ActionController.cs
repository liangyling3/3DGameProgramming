using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionEventType:int {Started, Completed}

public interface ActionCallback {
    void ActionEvent(Action source, ActionEventType events = ActionEventType.Completed, int intParam = 0, string strParam = null, Object objParam = null);
    void CheckEvent(Action source, ActionEventType events = ActionEventType.Completed, int intParam = 0, string strParam = null, Object objParam = null);
}

public class ActionManager : MonoBehaviour {
    public Dictionary<int, Action> actions = new Dictionary<int, Action>();
    public List<Action> waitingAdd = new List<Action>();
    public List<int> waitingDelete = new List<int>();

    protected void Update() {
        foreach (Action ac in waitingAdd)
            actions[ac.GetInstanceID()] = ac;
        waitingAdd.Clear();
        foreach (KeyValuePair <int, Action> kv in actions) {
            if (kv.Value.destory)
                waitingDelete.Add(kv.Value.GetInstanceID());
            else
                kv.Value.Update();
        }
        foreach (int key in waitingDelete) {
            DestroyObject(actions[key]);
            actions.Remove(key);
        }
        waitingDelete.Clear();
    }
    public void RunAction(GameObject gameObject, Action action, ActionCallback manager) {
        Debug.Log(gameObject.GetInstanceID());
        action.gameObject = gameObject;
        action.transform = gameObject.transform;
        action.callback = manager;
        waitingAdd.Add(action);
        action.Start();
    }
}

public class ActionController : ActionManager, ActionCallback {
    public FirstSceneController sceneController;
    public List<MoveAction> seq = new List<MoveAction>();
    public UserClickAction userClickAction;
    public UFOFactory disks;
    
    public void Start() {
        sceneController = (FirstSceneController)CurrentDirector.getInstance().currentSceneController;
        sceneController.actionCtrl = this;
        disks = Singleton<UFOFactory>.Instance;
    }

    public void Update() {
        if (disks.used.Count > 0) {
            GameObject disk = disks.used[0];
            float x = Random.Range(-10, 10);
            MoveAction moveToAction = MoveAction.GetAction(new Vector3(x, 12, 0), 3 * (Mathf.CeilToInt(FirstSceneController.times / 10) + 1) * Time.deltaTime);
            seq.Add(moveToAction);
            this.RunAction(disk, moveToAction, this);
            disks.used.RemoveAt(0);
        }
        
        if (Input.GetMouseButtonDown(0) && sceneController.flag == 0) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitGameObject;
            if (Physics.Raycast(ray, out hitGameObject)) {
                GameObject gameObject = hitGameObject.collider.gameObject;
                if (gameObject.tag == "disk") {
                    foreach(var k in seq) {
                        if (k.gameObject == gameObject)
                            k.transform.position = k.target;
                    }
                    userClickAction = UserClickAction.GetAction();
                    this.RunAction(gameObject, userClickAction, this);
                }
            }
        }
        base.Update();
    }

    public void ActionEvent(Action source, ActionEventType events = ActionEventType.Completed, int intParam = 0, string strParam = null, Object objParam = null) {
        disks.recycleUFO(source.gameObject);
        seq.Remove(source as MoveAction);
        source.destory = true;
        if (FirstSceneController.times >= 30)
            sceneController.flag = 1;
    }

    public void CheckEvent(Action source, ActionEventType events = ActionEventType.Completed, int intParam = 0, string strParam = null, Object objParam = null) {
    }

    public void Pause() {
        if(sceneController.flag == 0) {
            foreach (var k in seq) {
                k.enable = false;
            }
            sceneController.flag = 2;
        }
        else if(sceneController.flag == 2) {
            foreach (var k in seq) {
                k.enable = true;
            }
            sceneController.flag = 0;
        }
    }
}
