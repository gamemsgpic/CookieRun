using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectManager : MonoBehaviour
{
    public List<Sprite> characterSprites; // ĳ���� �⺻ ��������Ʈ ����Ʈ
    // public List<RuntimeAnimatorController> characterAnimators; // �ִϸ����� ����Ʈ (��� �� ��)
    public Image characterImage; // ĳ���͸� ǥ���� UI Image
    public TextMeshProUGUI characterInfoText; // ĳ���� ���� UI

    private int currentIndex = 0; // ���� ���õ� ĳ���� �ε���

    void Start()
    {
        //ShowCharacter(currentIndex); // ù ��° ĳ���� ǥ��
    }

    public void ShowCharacter(int index)
    {
        // ��������Ʈ ����
        characterImage.sprite = characterSprites[index];

        /*
        // Animator ���� (�ּ� ó��)
        Animator animator = characterImage.GetComponent<Animator>();
        if (animator != null)
        {
            animator.runtimeAnimatorController = characterAnimators[index];

            // �ִϸ��̼� ���� ��� (�ּ� ó��)
            animator.Rebind();
            animator.Update(0);
        }
        */

        // ĳ���� ���� ������Ʈ
        string[] characterDescriptions = { "�밨�� ��Ű\n �ɷ�ġ�� �⺻�� ��Ű",
            "õ�� ��Ű\n ���� ������ �ڼ��� ��� Ȱ��ȭ �Ǿ��ֽ��ϴ�.\n �ڼ� �������� ������ ������ �о����ϴ�.", 
            "���� ��Ű\n ����� ������ �ֽ��ϴ�.\n ����� ��� ���߰� �����ð� �� ��Ȱ �մϴ�." };
        //characterInfoText.text = characterDescriptions[index];
    }

    public void NextCharacter()
    {
        currentIndex++;
        if (currentIndex >= characterSprites.Count) currentIndex = 0; // �������̸� ó������

        ShowCharacter(currentIndex);
    }

    public void PreviousCharacter()
    {
        currentIndex--;
        if (currentIndex < 0) currentIndex = characterSprites.Count - 1; // ó���̸� ������

        ShowCharacter(currentIndex);
    }
}
