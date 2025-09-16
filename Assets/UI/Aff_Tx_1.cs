using TMPro;
using UnityEngine;
using Naninovel;
using System.Reflection; // 添加此行以修复 BindingFlags 未定义错误


public class Aff_Tx_1 : MonoBehaviour
{
    [SerializeField] private string variableName1 = "G_Char1_Affection";

    private TMP_Text textMesh;
    private ICustomVariableManager varManager;

    private void OnEnable()
    {
        if (Engine.Initialized) Setup();
        else Engine.OnInitializationFinished += Setup;
    }

    private void OnDisable()
    {
        if (varManager != null)
            varManager.OnVariableUpdated -= OnVarChanged;
        Engine.OnInitializationFinished -= Setup;
    }

    private void Setup()
    {
        textMesh = GetComponent<TMP_Text>();
        varManager = Engine.GetService<ICustomVariableManager>();
        if (varManager == null) return;

        RefreshText();
        varManager.OnVariableUpdated += OnVarChanged;
    }

    private void OnVarChanged(CustomVariableUpdatedArgs args)
    {
        if (args.Name == variableName1)
            RefreshText();
    }

    private void RefreshText()
    {
        if (varManager == null || textMesh == null) return;

        // 一次性拼好字符串
        string v1 = GetVarString(variableName1);
        textMesh.text = $"Affection:{v1}";
    }

    private string GetVarString(string varName)
    {
        if (varManager == null) return "0";

        // 优先按字符串取，失败再尝试其他类型
        if (varManager.TryGetVariableValue(varName, out string str)) return str;
        if (varManager.TryGetVariableValue(varName, out int i)) return i.ToString();
        if (varManager.TryGetVariableValue(varName, out float f)) return f.ToString("0.##");
        if (varManager.TryGetVariableValue(varName, out bool b)) return b ? "1" : "0";

        return "0"; // 都失败了
    }
}