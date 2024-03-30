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
        USA = 840, // アメリア
        GBR = 826, // イギリス
        ITA = 380, // イタリア
        AUS = 036, // オーストラリア
        NLD = 528, // オランダ
        CAN = 124, // カナダ
        KOR = 410, // 韓国
        TWN = 158, // 台湾
        CHN = 156, // 中国
        JPN = 392, // 日本
    }

    public enum LoadState
    {
        None,   // 何もしてない状態
        Load,   // 読み込み待機中
        End,    // 読み込み終了済み
    }
}