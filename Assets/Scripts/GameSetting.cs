using UnityEngine;


[CreateAssetMenu(menuName = "Setting/GameSetting")]
public class GameSetting : ScriptableObject
{
     public float gameTime = 120f;
     public float scoreToWin = 3;
}
