using TMPro;
using UnityEngine;
using Naninovel;

public class AffectionTextBinder : MonoBehaviour
{
    [SerializeField] private string variableName1 = "G_Char1_Affection"; // 变量名
    [SerializeField] private string variableName2 = "G_Char2_Affection"; // 变量名
    [SerializeField] private string variableName3 = "G_Char3_Affection"; // 变量名
    private TMP_Text textMesh;
    private ICustomVariableManager varManager;

    /* 等待引擎初始化后再取服务，避免 NullReference */
    private void OnEnable()
    {
        if (Engine.Initialized) Setup();
        else Engine.OnInitializationFinished += Setup;
    }

    private void OnDisable()
    {
        if (varManager != null) varManager.OnVariableUpdated -= OnVarChanged;
        Engine.OnInitializationFinished -= Setup;
    }

    private void Setup()
    {
        textMesh = GetComponent<TMP_Text>();
        varManager = Engine.GetService<ICustomVariableManager>();
        if (varManager == null) return;

        RefreshText();                              // 初始值
        varManager.OnVariableUpdated += OnVarChanged; // 监听变化
    }

    private void OnVarChanged(CustomVariableUpdatedArgs args)
    {
        if (args.Name == variableName1) RefreshText();
        if (args.Name == variableName2) RefreshText();
        if (args.Name == variableName3) RefreshText();
    }

    private void RefreshText()
    {
        if (varManager == null) return;
        var val1 = varManager.GetVariableValue(variableName1);
        var val2 = varManager.GetVariableValue(variableName2);
        var val3 = varManager.GetVariableValue(variableName3);
        textMesh.text = val1 != null ? val1.ToString() : "0";
        textMesh.text = val2 != null ? val2.ToString() : "0";
        textMesh.text = val3 != null ? val3.ToString() : "0";
    }
}