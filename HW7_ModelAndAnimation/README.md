## 视频演示
[优酷](https://v.youku.com/v_show/id_XNDQ5NTY5Mjg0MA==.html?spm=a2h3j.8428770.3416059.1)
## 智能巡逻兵
#### 游戏设计要求：
 - 创建一个地图和若干巡逻兵(使用动画)；
 - 每个巡逻兵走一个3~5个边的凸多边型，位置数据是相对地址。即每次确定下一个目标位置，用自己当前位置为原点计算；
 - 巡逻兵碰撞到障碍物，则会自动选下一个点为目标； 
 - 巡逻兵在设定范围内感知到玩家，会自动追击玩家； 
 - 失去玩家目标后，继续巡逻；
 - 计分：玩家每次甩掉一个巡逻兵计一分，与巡逻兵碰撞游戏结束；
#### 程序设计要求：
- 必须使用订阅与发布模式传消息
	- subject：OnLostGoal
	- Publisher: ?
	- Subscriber: ?
- 工厂模式生产巡逻兵
### 效果截图
![在这里插入图片描述](https://img-blog.csdnimg.cn/20200105232939478.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2xpYW5neWxpbmcz,size_16,color_FFFFFF,t_70)

![在这里插入图片描述](https://img-blog.csdnimg.cn/20200105233019604.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2xpYW5neWxpbmcz,size_16,color_FFFFFF,t_70)
### 具体实现
重要代码如下：
#### Patrol类
```c
public class Patrol :MonoBehaviour
{
    public enum PatrolState { PATROL,FOLLOW};
    public int sign;        //the patrol in which area
    public bool isFollowPlayer = false;
    public GameObject player=null;       //the player
    public Vector3 startPos,nextPos;
    private float minPosX,minPosZ;  // the range of this patrol can move;
    private bool isMoving = false;
    private float distance;
    private float speed = 1.2f;
    PatrolState state = PatrolState.PATROL;
    private void Start()
    {
        minPosX = startPos.x - 2.5f;
        minPosZ = startPos.z - 2.5f;
        isMoving = false;
        AreaCollide.canFollow += changeStateToFollow;
    }

    public void FixedUpdate()
    {
        if((SSDirector.getInstance().currentScenceController as FirstController).gameState == GameState.END)
        {
            return;
        }
        if(state == PatrolState.PATROL)
        {
            GoPatrol();
        }
        else if(state == PatrolState.FOLLOW)
        {
            Follow();
        }
    }
    public void GoPatrol()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, nextPos, speed * Time.deltaTime);
            distance = Vector3.Distance(this.transform.position, nextPos);
            if(distance < 0.5)
            {
                isMoving = false;
            }
            return;
        }
        float posX = Random.Range(0f, 5f);
        float posZ = Random.Range(0f, 5f);
        nextPos = new Vector3(minPosX+posX, 0, minPosZ+posZ);
        isMoving = true;    
    }

    public void Follow()
    {
        if(player != null)
        {
            nextPos = player.transform.position;
            transform.position = Vector3.MoveTowards(this.transform.position, nextPos, speed * Time.deltaTime);
        }
    }

    public void changeStateToFollow(int sign_,bool isEnter)
    {
        if(sign == sign_ )
        {
            if (isEnter)
            {
                state = PatrolState.FOLLOW;
                player = (SSDirector.getInstance().currentScenceController as FirstController).player;
                isFollowPlayer = true;
            }           
            else
            {
                isFollowPlayer = false;
                state = PatrolState.PATROL;
                player = null;
                isMoving = false;
            }
        }
        
    }
}
```
#### 巡逻兵工厂
```c
public class PatrolFactory:MonoBehaviour
{
    private List<GameObject> used = new List<GameObject>();    // the used patrol
    private Vector3[] PatrolPos = new Vector3[3];
    private bool isProduce = false;
    FirstController firstController;
    private void Start()
    {
        firstController = SSDirector.getInstance().currentScenceController as FirstController;
    }
    public List<GameObject> GetPatrols()
    {
        firstController = SSDirector.getInstance().currentScenceController as FirstController;
        if (!isProduce)
        {
            int index = 0;
            float[] posZ = { 3.75f, -3.75f };
            float[] posX = { 3.75f, -3.75f };
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if(posX[j] > 0 && posZ[i] > 0)
                    {
                        continue;
                    }
                    PatrolPos[index++] = new Vector3(posX[j], 0, posZ[i]);
                }
            }
            for (int i = 0; i < 3; i++)
            {
                GameObject patrol = Instantiate(Resources.Load<GameObject>("Prefabs/Patrol"));
                patrol.transform.parent = firstController.plane.transform;
                patrol.transform.position = PatrolPos[i];
                patrol.GetComponent<Patrol>().sign = i + 1;
                patrol.GetComponent<Patrol>().startPos = PatrolPos[i];
                used.Add(patrol);
            }
            isProduce = true;
        }     
        return used;
    }

    public void destoryFactory()
    {
        foreach(var a in used)
        {
            DestroyImmediate(a);
        }
        used = new List<GameObject>();
        isProduce = false;
    }
}
```
#### 场景障碍物
```c
public class AreaCollide : MonoBehaviour
{
    public int sign;
    public delegate void CanFollow(int state,bool isEnter);
    public static event CanFollow canFollow;

    public delegate void AddScore();
    public static event AddScore addScore;
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            canFollow(sign,true);
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            canFollow(sign,false);
            addScore();
        }
    }
}
```
