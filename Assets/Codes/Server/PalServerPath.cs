using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // EventTrigger 사용을 위해 추가
using TMPro;
using SimpleFileBrowser;
using System; // 파일 브라우저 네임스페이스 추가

public class PalServerPath : MonoBehaviour
{
    TMP_InputField inputValue;

    void Awake()
    {
        inputValue = GetComponentInChildren<TMP_InputField>(true);

        EventTrigger trigger = inputValue.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { OnInputFieldClick((PointerEventData)data); });
        trigger.triggers.Add(entry);

        // 파일 브라우저 초기화
        FileBrowser.SetFilters(true, new FileBrowser.Filter("All Files", "*"));
        FileBrowser.SetDefaultFilter("*");
        FileBrowser.AddQuickLink("Home", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), null);
    }

    void Update()
    {
        inputValue.text = PalServerManager.instance.palServerData.PalServerPath;
    }

    public void OnInputFieldClick(PointerEventData data)
    {
        StartCoroutine(ShowLoadDialogCoroutine());
    }

    IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Folders, false, null, "Select File", "Select");

        if (FileBrowser.Success)
        {
            string path = FileBrowser.Result[0];
            PalServerManager.instance.palServerData.PalServerPath = path;
            inputValue.text = path; // 입력 필드에 경로 업데이트
        }
    }
}
