using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class NPCDialogueData
{
  [SerializeField]
  int id;
  public int ID { get {return id; } set { this.id = value;} }
  
  [SerializeField]
  int npcid;
  public int NPCID { get {return npcid; } set { this.npcid = value;} }
  
  [SerializeField]
  int number;
  public int Number { get {return number; } set { this.number = value;} }
  
  [SerializeField]
  string talk;
  public string Talk { get {return talk; } set { this.talk = value;} }
  
  [SerializeField]
  string select1;
  public string SELECT1 { get {return select1; } set { this.select1 = value;} }
  
  [SerializeField]
  int select1condition1;
  public int Select1condition1 { get {return select1condition1; } set { this.select1condition1 = value;} }
  
  [SerializeField]
  int condition1quantity;
  public int Condition1quantity { get {return condition1quantity; } set { this.condition1quantity = value;} }
  
  [SerializeField]
  int condition1nexttalkid;
  public int Condition1nexttalkid { get {return condition1nexttalkid; } set { this.condition1nexttalkid = value;} }
  
  [SerializeField]
  int nexttalkid1;
  public int Nexttalkid1 { get {return nexttalkid1; } set { this.nexttalkid1 = value;} }
  
  [SerializeField]
  string select2;
  public string SELECT2 { get {return select2; } set { this.select2 = value;} }
  
  [SerializeField]
  int select2condition2;
  public int Select2condition2 { get {return select2condition2; } set { this.select2condition2 = value;} }
  
  [SerializeField]
  int condition2quantity;
  public int Condition2quantity { get {return condition2quantity; } set { this.condition2quantity = value;} }
  
  [SerializeField]
  int condition2nexttalkid;
  public int Condition2nexttalkid { get {return condition2nexttalkid; } set { this.condition2nexttalkid = value;} }
  
  [SerializeField]
  int nexttalkid2;
  public int Nexttalkid2 { get {return nexttalkid2; } set { this.nexttalkid2 = value;} }
  
  [SerializeField]
  int getitemid;
  public int Getitemid { get {return getitemid; } set { this.getitemid = value;} }
  
  [SerializeField]
  int getitemea;
  public int Getitemea { get {return getitemea; } set { this.getitemea = value;} }
  
  [SerializeField]
  int outitemid;
  public int Outitemid { get {return outitemid; } set { this.outitemid = value;} }
  
  [SerializeField]
  int outitemea;
  public int Outitemea { get {return outitemea; } set { this.outitemea = value;} }
  
  [SerializeField]
  string soudeffect;
  public string Soudeffect { get {return soudeffect; } set { this.soudeffect = value;} }
  
  [SerializeField]
  string animation;
  public string Animation { get {return animation; } set { this.animation = value;} }
  
}