using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
public class SpeechBubbleSheetAssetPostprocessor : AssetPostprocessor 
{
    private static readonly string filePath = "Assets/Sheet/Data.xlsx";
    private static readonly string assetFilePath = "Assets/Sheet/SpeechBubbleSheet.asset";
    private static readonly string sheetName = "SpeechBubbleSheet";
    
    static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets) 
        {
            if (!filePath.Equals (asset))
                continue;
                
            SpeechBubbleSheet data = (SpeechBubbleSheet)AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(SpeechBubbleSheet));
            if (data == null) {
                data = ScriptableObject.CreateInstance<SpeechBubbleSheet> ();
                data.SheetName = filePath;
                data.WorksheetName = sheetName;
                AssetDatabase.CreateAsset ((ScriptableObject)data, assetFilePath);
                //data.hideFlags = HideFlags.NotEditable;
            }
            
            //data.dataArray = new ExcelQuery(filePath, sheetName).Deserialize<SpeechBubbleSheetData>().ToArray();		

            //ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
            //EditorUtility.SetDirty (obj);

            ExcelQuery query = new ExcelQuery(filePath, sheetName);
            if (query != null && query.IsValid())
            {
                data.dataArray = query.Deserialize<SpeechBubbleSheetData>().ToArray();
                ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
                EditorUtility.SetDirty (obj);
            }
        }
    }
}
