using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
   public static EquipmentUI instance;
   private EquipmentStatus equipmentStatus;


   private void Awake()
   {
      instance = this;
   }

   public void UpdateLevelUI()
   {
      equipmentStatus.equipLevelText.text = "Lv. " + equipmentStatus.equipLevel;
      equipmentStatus.rankLevelText.text = equipmentStatus.rankLevel.ToString();
   }

   public void UpdateEquipNameExplainUI()
   {
      
   }

   public void UpdateEquipLevelExplainUI()
   {
      
   }

   public void UpdateEquipGoldUpExplainUI()
   {
      
   }
}
