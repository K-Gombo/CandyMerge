using UnityEngine;
using UnityEngine.UI;

public class TestButton : MonoBehaviour
{
    public GachaManager GachaManager; 
    public EquipmentManager equipmentManager; 
    public Transform equipSpawnLocation; 

   
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) 
        {
            // 원하는 totalLevel 설정
            int desiredTotalLevel = 180;
            // 해당 totalLevel에 따른 확률 정보 갖고오기
            float[] probabilities = GachaManager.GetRankProbabilities(desiredTotalLevel);
            //장비 생성
            equipmentManager.CreateEquipPrefab(equipSpawnLocation, probabilities);
        }
    }
}