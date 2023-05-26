using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameSetting
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        // 게임 시작하기 전 필요한 설정들
        if (GameManager.Instance == null)
        {
            GameObject gameManaver = new GameObject() { name = "GameManager" };
            gameManaver.AddComponent<GameManager>();
        }
    }
}