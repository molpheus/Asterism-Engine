using UnityEngine;

namespace Asterism.Common
{
    public class ActionButtonData
    {
        public enum ButtonType
        {
            None,
            Open,
            ItemGet,
        }

        public ButtonType type = ButtonType.None;
        public Transform target = null;
    }

    public class EventData
    {
        public int eventID = 0;
        public ItemData itemData = default;
    }

    public class UIEventAction
    {
        public int eventType = 0;
    }

    public class ItemData
    {
        public int item_id = 0;
        public int num = 0;
    }

    public enum CountryCode : int
    {
        USA = 840, // �A�����A
        GBR = 826, // �C�M���X
        ITA = 380, // �C�^���A
        AUS = 036, // �I�[�X�g�����A
        NLD = 528, // �I�����_
        CAN = 124, // �J�i�_
        KOR = 410, // �؍�
        TWN = 158, // ��p
        CHN = 156, // ����
        JPN = 392, // ���{
    }

    public enum LoadState
    {
        None,   // �������ĂȂ����
        Load,   // �ǂݍ��ݑҋ@��
        End,    // �ǂݍ��ݏI���ς�
    }
}