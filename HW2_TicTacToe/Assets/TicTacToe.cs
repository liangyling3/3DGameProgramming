using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToe : MonoBehaviour
{
    private int turn = 1;   //1表示O的回合，-1表示X的回合
    private int[,] state = new int[3, 3];  //棋盘该处的状态，0为空，1为O，2为X

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width / 2 - 45, Screen.height / 2 + 130, 100, 50), "RESTART"))
            Reset();
        int result = checkResult();
        if (result == 1)
        {
            GUI.Label(new Rect(Screen.width / 2 - 20, Screen.height / 2 + 75, 100, 50), "O WINS！");
        }
        else if (result == 2)
        {
            GUI.Label(new Rect(Screen.width / 2 - 20, Screen.height / 2 + 75, 100, 50), "X WINS！");
        }
        else if (result == 3)
        {
            GUI.Label(new Rect(Screen.width / 2 - 20, Screen.height / 2 + 75, 100, 50), "   Tie! ");
        }
        
        for (int i = 0; i < 3; ++i)
        {
            for (int j = 0; j < 3; ++j)
            {
                if (state[i, j] == 1)
                    GUI.Button(new Rect(Screen.width / 2 - 75 + 50 * i, Screen.height / 2 - 130 + 50 * j, 50, 50), "O");
                if (state[i, j] == 2)
                    GUI.Button(new Rect(Screen.width / 2 - 75 + 50 * i, Screen.height / 2 - 130 + 50 * j, 50, 50), "X");
                if (GUI.Button(new Rect(Screen.width / 2 - 75 + 50 * i, Screen.height / 2 - 130 + 50 * j, 50, 50), ""))
                {
                    if (result == 0)
                    {
                        if (turn == 1)
                            state[i, j] = 1;
                        else
                            state[i, j] = 2;
                        turn = -turn;
                    }
                }
            }
        }
    }

    // 初始化
    void Reset()
    {
        turn = 1;
        for (int i = 0; i < 3; ++i)
        {
            for (int j = 0; j < 3; ++j)
            {
                state[i, j] = 0;
            }
        }
    }

    int checkResult()
    {
        // 横
        for (int i = 0; i < 3; ++i)
        {
            if (state[i, 0] != 0 && state[i, 0] == state[i, 1] && state[i, 1] == state[i, 2])
            {
                return state[i, 0];
            }
        }
        // 竖
        for (int i = 0; i < 3; ++i)
        {
            if (state[0, i] != 0 && state[0, i] == state[1, i] && state[1, i] == state[2, i])
            {
                return state[0, i];
            }
        }
        // 斜
        if (state[1, 1] != 0 &&
            state[0, 0] == state[1, 1] && state[1, 1] == state[2, 2] ||
            state[0, 2] == state[1, 1] && state[1, 1] == state[2, 0])
        {
            return state[1, 1];
        }

        // 平局
        int count = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (state[i, j] != 0)
                {
                    count++;
                }
            }
        }
        if (count == 9)
        {
            return 3;
        }
        return 0;
    }
}
