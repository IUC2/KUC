using UnityEngine;
using UnityEngine.UI;

public class EnemyHPViewer : MonoBehaviour
{
    private EnemyHP enemyHP;
    private Slider hpSlider;

    public void Setup(EnemyHP enemyHP)
    {
        this.enemyHP = enemyHP;
        hpSlider = GetComponent<Slider>();
    }

    void Update()
    {
        hpSlider.value = enemyHP.CurrentHP / enemyHP.MaxHP;//프로퍼티로 구현한 Get & Set함수를 통해 구현
    }
}
