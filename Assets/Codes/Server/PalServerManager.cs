using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class PalServerManager : MonoBehaviour
{
    public static PalServerManager instance;
    public PalServerData palServerData;
    public PalServerObject palServerObjectPrefab;
    public PalServerSlider palServerSliderPrefab;
    public PalServerList palServerListPrefab;
    public GameObject content;
    public Button buttonSave;
    public Button buttonStart;
    TextMeshProUGUI buttonText;
    List<PalServerObject> palServerObjects = new List<PalServerObject>();
    List<PalServerValue> palServerValues = new List<PalServerValue>();

    void Awake()
    {
        instance = this;
        buttonText = buttonStart.GetComponentInChildren<TextMeshProUGUI>();
        buttonSave.onClick.AddListener(Save);
        buttonStart.onClick.AddListener(ServerStart);
        Init();
        InitObjectList();
    }
    void Start()
    {
        InitObjectData();
        Screen.SetResolution(800, 600, false);
    }
    void Update()
    {
        if (palServerData.currentProcess != null)
        {

            buttonText.text = "Stop";
        }
        else
        {
            buttonText.text = "Start";
        }
    }

    void Save()
    {
        PalServerDataManager.Save(palServerData);
    }
    void ServerStart()
    {
        palServerData.ExecuteOrKillConfigFile();
    }
    void OnApplicationQuit()
    {
        palServerData.KillProcess();
    }
    void Init()
    {
        palServerData = PalServerDataManager.Load();
        if (palServerData != null) return;

        palServerData = new PalServerData();
        List<PalServerValue.ListValue> difficultyList = new List<PalServerValue.ListValue>
        {
            PalServerValue.CreateListValue("None", "커스텀"),
            PalServerValue.CreateListValue("Casual", "쉬움"),
            PalServerValue.CreateListValue("Normal", "보통"),
            PalServerValue.CreateListValue("Hard", "어려움")
        };
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.List, "Difficulty", "난이도", "None", difficultyList));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Float, "DayTimeSpeedRate", "낮 시간 배율", "1.0", minValue: 0.1f, maxValue: 5.0f));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Float, "NightTimeSpeedRate", "밤 시간 배율", "1.0", minValue: 0.1f, maxValue: 5.0f));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Float, "ExpRate", "경험치 획득 배율", "1.0", minValue: 0.1f, maxValue: 20.0f));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Float, "PalCaptureRate", "PAL 포획 확률 배율", "1.0", minValue: 0.1f, maxValue: 2.0f));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Float, "PalSpawnNumRate", "PAL 출현 빈도", "1.0", minValue: 0.1f, maxValue: 3.0f));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Float, "PalDamageRateAttack", "PAL 대미지 배율", "1.0", minValue: 0.1f, maxValue: 5.0f));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Float, "PalDamageRateDefense", "PAL 받는 대미지 배율", "1.0", minValue: 0.1f, maxValue: 5.0f));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Float, "PlayerDamageRateAttack", "PAL 포만감 감소 배율", "1.0", minValue: 0.1f, maxValue: 5.0f));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Float, "PlayerDamageRateDefense", "PAL 기력 감소 배율", "1.0", minValue: 0.1f, maxValue: 5.0f));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Float, "PlayerStomachDecreaseRate", "PAL 자동 체력 회복 배율", "1.0", minValue: 0.1f, maxValue: 5.0f));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Float, "PlayerStaminaDecreaseRate", "PAL 수면 체력 회복 배율", "1.0", minValue: 0.1f, maxValue: 5.0f));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Float, "PlayerAutoHPRegeneRate", "플레이어  대미지 배율", "1.0", minValue: 0.1f, maxValue: 5.0f));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Float, "PlayerAutoHpRegeneRateInSleep", "플레이어 받는 대미지 배율", "1.0", minValue: 0.1f, maxValue: 5.0f));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Float, "PalStomachDecreaseRate", "플레이어 포만감 감소 배율", "1.0", minValue: 0.1f, maxValue: 5.0f));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Float, "PalStaminaDecreaseRate", "플레이어 기력 감소 배율", "1.0", minValue: 0.1f, maxValue: 5.0f));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Float, "PalAutoHPRegeneRate", "플레이어 자동 체력 회복 배율", "1.0", minValue: 0.1f, maxValue: 5.0f));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Float, "PalAutoHpRegeneRateInSleep", "플레이어 수면 체력 회복 배율", "1.0", minValue: 0.1f, maxValue: 5.0f));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Float, "BuildObjectDamageRate", "건축물 받는 대미지 비율", "1.0", minValue: 0.5f, maxValue: 3.0f));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Float, "BuildObjectDeteriorationDamageRate", "건축물 노화 속도 배율", "1.0", minValue: 0f, maxValue: 10.0f));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Float, "CollectionDropRate", "채집 아이템 드랍 배율", "1.0", minValue: 0.5f, maxValue: 3.0f));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Float, "CollectionObjectHpRate", "채집 오브젝트 체력 배율", "1.0", minValue: 0.5f, maxValue: 3.0f));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Float, "CollectionObjectRespawnSpeedRate", "채집 오브젝트 리스폰 배율", "1.0", minValue: 0.5f, maxValue: 3.0f));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Float, "EnemyDropItemRate", "적 아이템 드랍 배율", "1.0", minValue: 0.5f, maxValue: 3.0f));

        List<PalServerValue.ListValue> panaltyList = new List<PalServerValue.ListValue>
        {
            PalServerValue.CreateListValue("None", "아무것도 떨어뜨리지 않는다"),
            PalServerValue.CreateListValue("Item", "장비품 이외의 아이템을 떨어뜨린다"),
            PalServerValue.CreateListValue("ItemAndEquipment", "모든 아이템을 떨어뜨린다"),
            PalServerValue.CreateListValue("All", "모두 떨어뜨린다")
        };
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.List, "DeathPenalty", "사망 패널티", "None", panaltyList));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Bool, "bEnablePlayerToPlayerDamage", "플레이어간 피해 여부", "False"));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Bool, "bEnableFriendlyFire", "아군간 피해 여부", "False"));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Bool, "bEnableInvaderEnemy", "습격 발생 여부", "True"));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Bool, "bActiveUNKO", "UNKO 활성 여부", "False"));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Bool, "bEnableAimAssistPad", "게임패드 에임 어시스트 여부", "True"));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Bool, "bEnableAimAssistKeyboard", "키보드 에임 어시스트 여부", "False"));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Int, "DropItemMaxNum", "서버 내 드롭아이템 최대 수량", "3000", minValue: 0, maxValue: 3000));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Int, "DropItemMaxNum_UNKO", "UNKO 최대 수량", "100", minValue: 0, maxValue: 300));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Int, "BaseCampMaxNum", "거점 최대 수량", "128", minValue: 2, maxValue: 128));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Int, "BaseCampWorkerMaxNum", "거점 PAL 최대 수량", "15", minValue: 1, maxValue: 20));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Float, "DropItemAliveMaxHours", "드랍 아이템 소멸 시간 배율", "1.0", minValue: 0, maxValue: 1.0f));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Bool, "bAutoResetGuildNoOnlinePlayers", "비접속 길드 자동 리셋 여부", "False"));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Float, "AutoResetGuildTimeNoOnlinePlayers", "비접속 길드 리셋 시간", "72.0", minValue: 0, maxValue: 72.0f));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Int, "GuildPlayerMaxNum", "길드 최대 인원", "20", minValue: 1, maxValue: 100));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Float, "PalEggDefaultHatchingTime", "알 부화 소요시간 배율", "72.0", minValue: 0, maxValue: 240.0f));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Float, "WorkSpeedRate", "작업 속도 배율", "1.0", minValue: 0.1f, maxValue: 5.0f));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Bool, "bIsMultiplay", "멀티플레이 활성 여부", "False"));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Bool, "bIsPvP", "PVP 활성 여부", "False"));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Bool, "bCanPickupOtherGuildDeathPenaltyDrop", "타 길드원의 드롭아이템 획득 가능 여부", "False"));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Bool, "bEnableNonLoginPenalty", "비 로그인 패널티 부과 여부", "False"));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Bool, "bEnableFastTravel", "빠른 이동 활성화", "True"));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Bool, "bIsStartLocationSelectByMap", "스타트 지점 설정 여부", "True"));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Bool, "bExistPlayerAfterLogout", "로그아웃 시 캐릭터 비활성화 여부", "False"));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Bool, "bEnableDefenseOtherGuildPlayer", "타 길드원 침입자 취급 여부", "False"));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Int, "CoopPlayerMaxNum", "인던세션 진입가능 최대 인원", "1", minValue: 1, maxValue: 4));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Int, "ServerPlayerMaxNum", "서버 최대 인원", "32", minValue: 1, maxValue: 32));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.String, "ServerName", "서버 이름", ""));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.String, "ServerDescription", "서버 설명", ""));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.String, "AdminPassword", "관리자 비밀번호", ""));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.String, "ServerPassword", "서버 비밀번호", ""));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.String, "PublicPort", "공용 포트", "8211"));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.String, "PublicIP", "외부 공개 아이피", ""));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Bool, "RCONEnabled", "원격지원콘솔 사용 여부", "False"));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.String, "RCONPort", "원격지원콘솔 포트", "25575"));
        List<PalServerValue.ListValue> serverList = new List<PalServerValue.ListValue>
        {
            PalServerValue.CreateListValue("", "비공개"),
            PalServerValue.CreateListValue("NA", "북미"),
            PalServerValue.CreateListValue("SA", "남미"),
            PalServerValue.CreateListValue("China", "중국"),
            PalServerValue.CreateListValue("Asia", "아시아"),
            PalServerValue.CreateListValue("EU", "유럽"),
            PalServerValue.CreateListValue("JP", "일본")
        };
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.List, "Region", "서버 지역", "", listValues: serverList));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.Bool, "bUseAuth", "서버 인증 여부", "True"));
        palServerData.palServerValues.Add(new PalServerValue(PalServerValue.ValueType.String, "BanListURL", "공식서버 밴리스트 URL", "https://api.palworldgame.com/api/banlist/txt"));

    }

    void InitObjectList()
    {
        palServerData.palServerValues.ForEach(palServerValue =>
        {
            switch (palServerValue.type)
            {
                case PalServerValue.ValueType.String:
                    PalServerObject palServerObject = Instantiate(palServerObjectPrefab, content.transform);
                    palServerObject.palServerValue = palServerValue;
                    palServerObjects.Add(palServerObject);
                    break;
                case PalServerValue.ValueType.Bool:
                case PalServerValue.ValueType.List:
                    PalServerList palServerList = Instantiate(palServerListPrefab, content.transform);
                    palServerList.palServerValue = palServerValue;
                    palServerObjects.Add(palServerList);
                    break;
                case PalServerValue.ValueType.Float:
                case PalServerValue.ValueType.Int:
                    PalServerSlider palServerSlider = Instantiate(palServerSliderPrefab, content.transform);
                    palServerSlider.palServerValue = palServerValue;
                    palServerSlider.Init();
                    palServerObjects.Add(palServerSlider);
                    break;
            }
            palServerValues.Add(new PalServerValue().InitPalServerValue(palServerValue));
        });
    }
    void InitObjectData()
    {
        int index = 0;
        palServerObjects.ForEach((palServerObject) =>
        {
            palServerObject.palServerValue.Value = palServerValues[index].Value;
            index++;
        });
    }

}
