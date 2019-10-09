using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface MyUserAction {
    void Restart();
    void Pause();
}

public interface MySceneController {
    void GenGameObjects();
}

public class FirstSceneController : MonoBehaviour, MyUserAction, MySceneController {
    public ActionController actionCtrl;
    public GameObject disk;
    protected UFOFactory factory;
    public int flag = 0;
    private float interval = 3;
    public int score = 0;
    public static int times = 0;

    private void Awake() {
        CurrentDirector director = CurrentDirector.getInstance();
        director.setFPS(60);
        director.currentSceneController = this;
        this.gameObject.AddComponent<UFOFactory>();
        this.gameObject.AddComponent<ActionController>();
        this.gameObject.AddComponent<UserGUI>();
        factory = Singleton<UFOFactory>.Instance;
    }

    private void Start() {

    }

    public void GenGameObjects () {

    }

    public void Restart() {
        SceneManager.LoadScene("1");
    }

    public void Pause () {
        actionCtrl.Pause();
    }
    
    public void Update() {
        if (times < 30 && flag == 0) {
            if (interval <= 0) {
                interval = Random.Range(3, 5);
                times++;
                int num = Random.Range(1,4);
                for (int i = 0; i < num; i++) {
                    factory.produceUFO();
                }
            }
            interval -= Time.deltaTime;
        }
    }
}

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

	protected static T instance;

	public static T Instance {  
		get {  
			if (instance == null) { 
				instance = (T)FindObjectOfType (typeof(T));  
				if (instance == null) {
					Debug.LogError ("An instance of " + typeof(T) +
					" is needed in the scene, but there is none.");  
				}
			}
			return instance;  
		}
	}
}
