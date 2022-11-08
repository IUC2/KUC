using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IPointerUpHandler, IDragHandler, IPointerDownHandler
{
    [SerializeField]
    private TowerSpawner towerSpawner;
    [SerializeField]
    private int towerindex;
    private float isDragTime;
    private Camera mainCamera;
    private Ray mouseBtnUpRay;
    private RaycastHit mouseBtnUpHit;
    private Transform mouseBtnUpHitTransform = null;//마우스 픽킹으로 선택한 오브젝트 임시 저장
    private GameObject clickedTower = null;

    [SerializeField]
    private Button button;
    private RectTransform buttonClickedTransform;

    private void Awake()
    {
        mainCamera = Camera.main;
        button = GetComponent<Button>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        towerSpawner.ReadyToSpawnTower(towerindex);
    }

    public void OnDrag(PointerEventData eventData)
    {
        int i = 0 ;
        if (i == 1)
        {
            towerSpawner.followTowerClone.SetActive(true);
            i++;
        }
        isDragTime += Time.deltaTime;
        towerSpawner.SetDragPosition(mainCamera, towerSpawner.followTowerClone);
        Debug.Log("UIOnDrag");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(isDragTime >= 0.01f)
        {
            //Drag
            isDragTime = 0f;
            int layerMask = (-1) - (1 << LayerMask.NameToLayer("Tower"));  // Everything에서 Player 레이어만 제외하고 충돌 체크함
            mouseBtnUpRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseBtnUpRay, out mouseBtnUpHit, Mathf.Infinity, layerMask))
            {
                mouseBtnUpHitTransform = mouseBtnUpHit.transform;
                if (mouseBtnUpHit.transform.CompareTag("PlacedTower"))
                {
                    //합체
                    Debug.Log("1단계 합체 구간");
                }
                else if (mouseBtnUpHit.transform.CompareTag("Tile"))
                {
                    clickedTower = towerSpawner.SpawnTower(mouseBtnUpHitTransform);
                }
                else
                {
                    //필요 구간
                }

            }
            else
            {
                //필요 구간
            }
            towerSpawner.DestroyFollowTowerClone();
            mouseBtnUpHitTransform = null;
        }
        else
        {
            //click
            towerSpawner.DestroyFollowTowerClone();
            isDragTime = 0f;
        }
        //내가 지금 처리해줘야 하는 것이 UI에 배치된 상황 or 길목에 배치된 상황

    }
}
