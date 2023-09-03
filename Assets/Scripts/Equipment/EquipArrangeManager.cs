using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EquipArrangeManager : MonoBehaviour
{
    public List<EquipmentStatus> equipList = new List<EquipmentStatus>();
    private List<EquipmentStatus> filteredList = new List<EquipmentStatus>();
    public EquipmentManager equipmentManager;
    public GameObject equipGridGameObject;

    public void UpdateEquipList()
    {
        equipList.Clear();
        foreach (Transform child in equipGridGameObject.transform)
        {
            EquipmentStatus status = child.GetComponent<EquipmentStatus>();
            if (status != null)
            {
                equipList.Add(status);
            }
        }
    }
    

    public void AddEquipment(EquipmentStatus equipment)
    {
        equipList.Add(equipment);
    }

    public void SortBySlotType()
    {
        UpdateEquipList();
        equipList = equipList.OrderBy(e => e.slotType).ToList();
        for (int i = 0; i < equipList.Count; i++)
        {
            equipList[i].transform.SetSiblingIndex(i);
        }
    }

    // 랭크별로 정렬하는 메서드
    public void SortByRank()
    {
        UpdateEquipList(); // 리스트를 업데이트

        // 모든 아이템 활성화
        foreach (var item in equipList)
        {
            item.gameObject.SetActive(true);
        }

        equipList = equipList.OrderByDescending(e => GetRankValue(e.equipRank)).ToList();
        for (int i = 0; i < equipList.Count; i++)
        {
            equipList[i].transform.SetSiblingIndex(i);
        }
    }


    public void FilterAndSortByCook()
    {
        DeactivateAll();
        filteredList = equipList.Where(e => e.slotType == EquipmentManager.SlotType.Cook).ToList();
        ActivateFiltered();
        UpdateEquipGrid(filteredList);
    }

    public void FilterAndSortByCap()
    {
        DeactivateAll();
        filteredList = equipList.Where(e => e.slotType == EquipmentManager.SlotType.Cap).ToList();
        ActivateFiltered();
        UpdateEquipGrid(filteredList);
    }

    public void FilterAndSortByCloth()
    {
        DeactivateAll();
        filteredList = equipList.Where(e => e.slotType == EquipmentManager.SlotType.Cloth).ToList();
        ActivateFiltered();
        UpdateEquipGrid(filteredList);
    }

    public void FilterAndSortByShoes()
    {
        DeactivateAll();
        filteredList = equipList.Where(e => e.slotType == EquipmentManager.SlotType.Shoes).ToList();
        ActivateFiltered();
        UpdateEquipGrid(filteredList);
    }

    public void UpdateEquipGrid(List<EquipmentStatus> listToSort)
    {
        for (int i = 0; i < listToSort.Count; i++)
        {
            listToSort[i].transform.SetSiblingIndex(i);
        }
    }
    
    

    private int GetRankValue(EquipmentManager.Rank rank)
    {
        switch (rank)
        {
            case EquipmentManager.Rank.SS:
                return 6;
            case EquipmentManager.Rank.S:
                return 5;
            case EquipmentManager.Rank.A:
                return 4;
            case EquipmentManager.Rank.B:
                return 3;
            case EquipmentManager.Rank.C:
                return 2;
            case EquipmentManager.Rank.D:
                return 1;
            case EquipmentManager.Rank.F:
            default:
                return 0;
        }
    }
    
    public void DeactivateAll()
    {
        foreach (var item in equipList)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void ActivateFiltered()
    {
        foreach (var item in filteredList)
        {
            item.gameObject.SetActive(true);
        }
    }
}