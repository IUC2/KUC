using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerDataViewer : MonoBehaviour
{
    [SerializeField]
    private Image imageTower;
    [SerializeField]
    private TextMeshProUGUI textDamage;
    [SerializeField]
    private TextMeshProUGUI textRate;
    [SerializeField]
    private TextMeshProUGUI textRange;
    [SerializeField]
    private TextMeshProUGUI textLevel;
    [SerializeField]
    private TextMeshProUGUI textUpgradeCost;
    [SerializeField]
    private TextMeshProUGUI textSellCost;
    [SerializeField]
    private TowerAttackRange towerAttackRange;
    [SerializeField]
    private Button buttonUpgrade;
    [SerializeField]
    private SystemTextViewer systemTextViewer;

    private TowerWeapon currentTower;//현재 타워의 정보를 가져와 저장할 변수

    private void Awake()
    {
        OffPanel();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OffPanel();
        }
    }
    public void OnPanel(Transform towerWeapon)
    {
        //출력할 타워 정보를 받아와 저장
        currentTower = towerWeapon.GetComponent<TowerWeapon>();
        //타워 정보 Panel On
        gameObject.SetActive(true);
        //타워 정보 갱신
        UpdateTowerData();
        //타워 사거리 표시(타워 위치 및 사거리 표시)
        towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
    }
    public void OffPanel()
    {
        //타워 정보 Panel Off
        gameObject.SetActive(false);
        //타워 사거리 정보 숨김
        towerAttackRange.OffAttackRange();
    }

    private void UpdateTowerData()
    {
        if(currentTower.WeaponType == WeaponType.Cannon || currentTower.WeaponType == WeaponType.Laser)
        {
            imageTower.rectTransform.sizeDelta = new Vector2(90, 60);
            textDamage.text = "Damage : " + currentTower.Damage + "+" + "<color=red>" + currentTower.AddedDamage.ToString("F1") + "</color>";
        }
        else
        {
            imageTower.rectTransform.sizeDelta = new Vector2(60, 60);
            if(currentTower.WeaponType == WeaponType.Slow)
            {
                textDamage.text = "Slow : " + currentTower.Slow * 100 + "%";
            }
            else if(currentTower.WeaponType == WeaponType.Buff)
            {
                textDamage.text = "Buff : " + currentTower.Buff * 100 + "%";
            }
            
        }
        imageTower.sprite   = currentTower.TowerSprite;
        //textDamage.text     = "Damage : " + currentTower.Damage;
        textRate.text       = "Rate : " + currentTower.Rate;
        textRange.text      = "Range : " + currentTower.Range;
        textLevel.text      = "Level : " + currentTower.Level;
        textUpgradeCost.text = currentTower.UpgradeCost.ToString();
        textSellCost.text = currentTower.SellCost.ToString();

        //업그레이드가 불가능해지면, 버튼 비활성화
        buttonUpgrade.interactable = currentTower.Level < currentTower.MaxLevel ? true : false;
        //buttonUpgrade.enabled = false;
    }

    public void OnClickEventTowerUpgrade()
    {
        //타워 업그레이드 버튼 클릭시
        bool isSuccess = currentTower.Upgrade();
        if(isSuccess == true)
        {
            //타워가 업그레이드 되었기 때문에 타워 정보 갱신
            UpdateTowerData();
            //타워 주변에 보이는 공격 범위도 갱신
            towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
        }
        else
        {
            //타워 업그레이드에 필요한 비용이 부족하다 출력
            systemTextViewer.PrintText(SystemType.Money);
        }
    }

    public void OnClickEventTowerSell()
    {
        //타워 판매
        currentTower.Sell();
        //선택한 타워가 사라져서 Panel, 공격범위 Off
        OffPanel();
    }
}
