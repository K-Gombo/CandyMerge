using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaBtn : MonoBehaviour
{
    public MixManager mixManager;  // 인스펙터에서 설정 가능
    private Button gachaButton;  // 이 게임 오브젝트에 붙은 버튼 컴포넌트
    
    void Start()
    {
        gachaButton = GetComponent<Button>();
        gachaButton.onClick.AddListener(OnClickGacha);  // 이벤트 리스너 추가
    }

    // Update is called once per frame
    void Update()
    {
        // MixManager에서 캔디 개수 확인 후 버튼 상태 업데이트
        if (mixManager != null)
        {
            gachaButton.interactable = mixManager.CheckCandiesCount();
        }
    }

    void OnClickGacha()
    {    Debug.Log("OnClickGacha called");
        if (mixManager != null)
        {   
            mixManager.EquipGacha();
        }
    }
}