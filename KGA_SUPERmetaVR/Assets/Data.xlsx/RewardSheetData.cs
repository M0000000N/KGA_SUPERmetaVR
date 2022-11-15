using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class RewardSheetData
{
  [SerializeField]
  int id;
  public int ID { get {return id; } set { this.id = value;} }
  
  [SerializeField]
  int itemid;
  public int Itemid { get {return itemid; } set { this.itemid = value;} }
  
  [SerializeField]
  string discription;
  public string Discription { get {return discription; } set { this.discription = value;} }
  
  [SerializeField]
  int period;
  public int Period { get {return period; } set { this.period = value;} }
  
  [SerializeField]
  int probability;
  public int Probability { get {return probability; } set { this.probability = value;} }
  
}