using UnityEngine;
using UnityEngine.UI;

public class TestButton : MonoBehaviour
{
    public Button testBtn;
    public GachaManager GachaManager; 
    public EquipmentManager equipmentManager; 
    public Transform equipSpawnLocation; 

    private void Start()
    {
        testBtn.onClick.AddListener(() =>
        {
            // 원하는 totalLevel 설정
            int desiredTotalLevel = 60;
            // 해당 totalLevel에 따른 확률 정보 갖고오기
            float[] probabilities = GachaManager.GetRankProbabilities(desiredTotalLevel);
            //장비 생성
            equipmentManager.CreateEquipPrefab(equipSpawnLocation, probabilities);
        });
    }
}