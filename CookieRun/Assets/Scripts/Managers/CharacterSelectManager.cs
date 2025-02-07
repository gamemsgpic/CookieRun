using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectManager : MonoBehaviour
{
    public List<Sprite> characterSprites; // 캐릭터 기본 스프라이트 리스트
    // public List<RuntimeAnimatorController> characterAnimators; // 애니메이터 리스트 (사용 안 함)
    public Image characterImage; // 캐릭터를 표시할 UI Image
    public TextMeshProUGUI characterInfoText; // 캐릭터 설명 UI

    private int currentIndex = 0; // 현재 선택된 캐릭터 인덱스

    void Start()
    {
        //ShowCharacter(currentIndex); // 첫 번째 캐릭터 표시
    }

    public void ShowCharacter(int index)
    {
        // 스프라이트 변경
        characterImage.sprite = characterSprites[index];

        /*
        // Animator 변경 (주석 처리)
        Animator animator = characterImage.GetComponent<Animator>();
        if (animator != null)
        {
            animator.runtimeAnimatorController = characterAnimators[index];

            // 애니메이션 강제 재생 (주석 처리)
            animator.Rebind();
            animator.Update(0);
        }
        */

        // 캐릭터 설명 업데이트
        string[] characterDescriptions = { "용감한 쿠키\n 능력치가 기본인 쿠키",
            "천사 쿠키\n 작은 범위의 자석이 계속 활성화 되어있습니다.\n 자석 아이템을 먹으면 범위가 넓어집니다.", 
            "좀비 쿠키\n 목숨이 여러개 있습니다.\n 사망시 잠시 멈추고 일정시간 후 부활 합니다." };
        //characterInfoText.text = characterDescriptions[index];
    }

    public void NextCharacter()
    {
        currentIndex++;
        if (currentIndex >= characterSprites.Count) currentIndex = 0; // 마지막이면 처음으로

        ShowCharacter(currentIndex);
    }

    public void PreviousCharacter()
    {
        currentIndex--;
        if (currentIndex < 0) currentIndex = characterSprites.Count - 1; // 처음이면 끝으로

        ShowCharacter(currentIndex);
    }
}
