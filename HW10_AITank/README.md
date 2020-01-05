## 视频演示
[优酷](https://v.youku.com/v_show/id_XNDQ5NTY2ODUyMA==.html?spm=a2h3j.8428770.3416059.1)


## 坦克对战游戏 AI 设计

从商店下载游戏：“Kawaii” Tank 或 其他坦克模型，构建 AI 对战坦克。具体要求：
- 使用“感知-思考-行为”模型，建模 AI 坦克
- 场景中要放置一些障碍阻挡对手视线
- 坦克需要放置一个矩阵包围盒触发器，以保证 AI 坦克能使用射线探测对手方位
- AI 坦克必须在有目标条件下使用导航，并能绕过障碍。（失去目标时策略自己思考）
- 实现人机对战

### 效果截图
![在这里插入图片描述](https://img-blog.csdnimg.cn/20200105224241322.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2xpYW5neWxpbmcz,size_16,color_FFFFFF,t_70)
![在这里插入图片描述](https://img-blog.csdnimg.cn/20200105224350665.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2xpYW5neWxpbmcz,size_16,color_FFFFFF,t_70)
### 具体实现
本次作业难度较大，根据已有的代码进行修改。
参考博客：[坦克对战游戏 AI 设计](https://blog.csdn.net/Z_J_Q_/article/details/80732809)

重要代码如下：
#### TankAI
TankAI类作为“感知-思考-行为”模型中的“思考”。跟踪速度和角速度随距离发生变化，距离越近速度越慢角速度越快。射线检测玩家在正前方时发射子弹，使用射线使AI坦克不会频繁发射子弹，行为更自然。
```c#
using System;
using UnityEngine;
namespace Complete
{
    [RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
    public class TankAI : MonoBehaviour
    {
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }
        public Transform target;
        public float angle = 60f;

        private float countTime = 0;
        private void Start()
        {
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            agent.updateRotation = true;
            agent.updatePosition = true;
        }

        private void Update()
        {
            // 距离与40的比例
            float ratio = (target.transform.position - transform.position).magnitude / 40;
            // 导航速度与距离线性正相关
            agent.speed = 2f * ratio + 1.5f;
            // 导航角速度与距离线性负相关
            agent.angularSpeed = 120f - 60f * ratio;
            if (target != null)
                agent.SetDestination(target.position);
            // 玩家在正前方时，以0.2秒一次的频率发射子弹
            if (countTime >= 0.2f) {
                Ray ray = new Ray (transform.position, transform.forward);
                RaycastHit hit;  
                if (Physics.Raycast (ray, out hit, Mathf.Infinity)) { 
                    if (hit.collider.gameObject.tag == "Player")
                        GetComponent<TankShooting> ().MyShoot();
                }
                countTime = 0;
            }
            countTime += Time.deltaTime;
        }
    }
}
```
#### TankManager
如果对象是AI，不激活TankMovement组件，使AI坦克不能被用户控制。
```c#
public void EnableControl ()
{
    if (!AI)
        m_Movement.enabled = true;
    else
        m_Instance.GetComponent<TankAI> ().enabled = true;
    ...
}
```
#### GameManager
设置Player2坦克的TankManager的AI为true，TankAI的导航目标target为Player1的位置。

```c
private void SpawnAllTanks()
{
    ...
    m_Tanks [0].AI = false;
    m_Tanks [1].AI = true;
    m_Tanks [1].m_Instance.GetComponent<TankAI> ().target = m_Tanks [0].m_Instance.transform;
    m_Tanks [1].m_Instance.GetComponent<TankAI> ().enabled = false;
}
```

