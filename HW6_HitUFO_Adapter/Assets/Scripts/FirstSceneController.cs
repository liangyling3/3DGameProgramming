using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstSceneController : MonoBehaviour, IUserAction, ISceneController{
    public IActionManager actionManager;    // 修改为新的接口
    public GameObject disk;
    protected UFOFactory factory;
    public int flag = 0;
    private float interval = 3;
    public int score = 0;
    public static int times = 0;

    private void Awake() {
        SSDirector director = SSDirector.getInstance();
        director.setFPS(60);
        director.currentSceneController = this;
        this.gameObject.AddComponent<UFOFactory>();
        this.gameObject.AddComponent<PhysicsActionManager>();   // 更改为物理引擎
        this.gameObject.AddComponent<UserGUI>();
        factory = Singleton<UFOFactory>.Instance;
    }

    private void Start() {

    }

    public void GenGameObjects() {

    }

    public void Restart() {
        SceneManager.LoadScene("untitle");
    }

    public void Pause () {
        actionManager.Pause();
    }

    public void Update() {
        if (times < 30 && flag == 0) {
            if (interval <= 0) {
                interval = Random.Range(3, 5);
                times++;
                int num = Random.Range(1, 4);
                for (int i = 0; i < num; i++) {
                    factory.produceUFO();
                }
            }
            interval -= Time.deltaTime;
        }
        actionManager.PlayUFO();
    }
}