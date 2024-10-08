using UnityEngine;
using UnityEngine.EventSystems;

public class SlotButton : MonoBehaviour, 
    IPointerEnterHandler, 
    IPointerDownHandler,
    IPointerExitHandler
{
    public int slotNumber;
    private bool slotInMouse = false;
    public  ItemSlotUI itemSlotUI;

    private void OnDisable()
    {
        slotInMouse = false;
    }
    private void Update()
    {
        if (itemSlotUI == UIManager.Instance.playerInventoryUI && true == Input.GetKeyDown(KeyCode.Q) && true == slotInMouse)
        {
            if (true == Input.GetKey(KeyCode.LeftShift))
                UIManager.Instance.playerInventory.DropItemFromInventoy(slotNumber, 64);
            else
                UIManager.Instance.playerInventory.DropItemFromInventoy(slotNumber, 1);
        }
    }
    public void SetItemSlotUI(ItemSlotUI itemSlotUI)
    {
        this.itemSlotUI = itemSlotUI;
    }

    // �̹��� ����
    public void OnPointerDown(PointerEventData eventData)
    {
        if(false == Input.GetKey(KeyCode.LeftShift))
        {
            if (PointerEventData.InputButton.Left == eventData.button)
                itemSlotUI.LeftClickSlot(slotNumber);

            if (PointerEventData.InputButton.Right == eventData.button)
                itemSlotUI.RightClickSlot(slotNumber);
        }
        else
        {
            if (PointerEventData.InputButton.Left == eventData.button)
                itemSlotUI.LeftShiftClickSlot(slotNumber);
        }
    }

    // �̹����� ���콺�� ������ ȣ��
    public void OnPointerEnter(PointerEventData eventData)
    {
        slotInMouse = true;
    }

    // �̹������� ���콺�� �־����� ȣ��(�����Ŀ� ȣ�Ⱑ��)
    public void OnPointerExit(PointerEventData eventData)
    {
        slotInMouse = false;
    }
}
