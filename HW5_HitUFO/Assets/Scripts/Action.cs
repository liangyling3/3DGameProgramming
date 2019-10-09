using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : ScriptableObject {
    public bool enable = true;
    public bool destory = false;

    public GameObject gameObject { get; set; }
    public Transform transform { get; set; }
    public ActionCallback callback { get; set; }

    public virtual void Start () {
        throw new System.NotImplementedException();
	}

    public virtual void Update() {
        throw new System.NotImplementedException();
    }
}

public class MoveAction : Action {
    public Vector3 target;
    public float speed;

    public static MoveAction GetAction(Vector3 target, float speed) {
        MoveAction action = CreateInstance<MoveAction>();
        action.target = target;
        action.speed = speed;
        return action;
    }

    public override void Start() {
        
    }

    public override void Update() {
        if(enable) {
            this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed);
            if (this.transform.position == target) {
                this.enable = false;
                this.callback.ActionEvent(this);
            }
        }
    }
}

public class UserClickAction : Action {
    public static UserClickAction GetAction() {
        UserClickAction action = CreateInstance<UserClickAction>();
        return action;
    }

    public override void Start() {
        
    }

    public override void Update() {
        if(enable) {
            FirstSceneController sc = CurrentDirector.getInstance().currentSceneController as FirstSceneController;
            sc.score = sc.score + Mathf.CeilToInt(FirstSceneController.times/10) + Mathf.FloorToInt(120 / (transform.rotation.x + 30));
            destory = true;
        }
    }
}

