using UnityEngine;
using UnityEngine.UI;

public class TestButton : MonoBehaviour
{
    public Button testBtn;
    public GachaManager GachaManager; // GachaManager 컴포넌트를 여기에 드래그앤드롭합니다.
    public EquipmentManager equipmentManager; // EquipmentManager 컴포넌트를 여기에 드래그앤드롭합니다.
    public Transform equipSpawnLocation; // 장비를 생성할 위치를 지정합니다. 이것도 드래그앤드롭으로 할당하거나 코드로 찾을 수 있습니다.

    private void Start()
    {
        testBtn.onClick.AddListener(() =>
        {
            // 원하는 totalLevel 값을 여기에 넣어주세요. 예를 들어 60이면 60을 넣으면 됩니다.
            int desiredTotalLevel = 60;
            // 해당 totalLevel에 따른 확률 정보를 얻습니다.
            float[] probabilities = GachaManager.GetRankProbabilities(desiredTotalLevel);
            
            // 장비 프리팹 생성 메서드 호출
            equipmentManager.CreateEquipPrefab(equipSpawnLocation, probabilities);
        });
    }
}