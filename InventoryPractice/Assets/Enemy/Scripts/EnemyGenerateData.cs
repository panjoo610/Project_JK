using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyGenerateData", menuName = "Enemy/EnemyGenerateData")]
public class EnemyGenerateData : ScriptableObject {
    public int EnemyCount;
    public int WaveCount;
    public bool IsBossStage;
    public List<Vector3> GeneratePosition;
}
