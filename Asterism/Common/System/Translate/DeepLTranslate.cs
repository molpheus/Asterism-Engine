using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Asterism.System.Translate
{
    internal class DeepLTranslate(string _apiKey)
    {
        private HttpClient _client = new HttpClient();

        // c87c2087-2eb8-49ad-9ca1-31f8e16bea22:fx
        public string ApiKey { get; set; } = _apiKey;

        public enum Lang
        {
            EN,     // 英語
            EN_US,  // アメリカ英語
            EN_GB,  // イギリス英語
            DE,     // ドイツ語
            FR,     // フランス語
            ES,     // スペイン語
            IT,     // イタリア語
            NL,     // オランダ語
            PL,     // ポーランド語
            PT,     // ポルトガル語
            PT_BR,  // ポルトガル語（ブラジル）
            PT_PT,  // ポルトガル語（ポルトガル）
            RU,     // ロシア語 
            JA,     // 日本語
            ZH,     // 中国語
            TR,     // トルコ語
            UK,     // ウクライナ語
            KO,     // 韓国語
            NB,     // ノルウェー語
            SV,     // スウェーデン語
            DA,     // デンマーク語
            FI,     // フィンランド語
            EL,     // ギリシャ語
            HU,     // ハンガリー語
            BG,     // ブルガリア語
            CS,     // チェコ語
            RO,     // ルーマニア語
            SK,     // スロバキア語
            LT,     // リトアニア語
            LV,     // ラトビア語
            ET,     // エストニア語
            SL,     // スロベニア語
            HR,     // クロアチア語
        }

        public string GetLangCode(Lang lang)
        {
            return lang switch
            {
                Lang.EN => "EN",
                Lang.EN_US => "EN-US",
                Lang.EN_GB => "EN-GB",
                Lang.DE => "DE",
                Lang.FR => "FR",
                Lang.ES => "ES",
                Lang.IT => "IT",
                Lang.NL => "NL",
                Lang.PL => "PL",
                Lang.PT => "PT",
                Lang.PT_BR => "PT-BR",
                Lang.PT_PT => "PT-PT",
                Lang.RU => "RU",
                Lang.JA => "JA",
                Lang.ZH => "ZH",
                Lang.TR => "TR",
                Lang.UK => "UK",
                Lang.KO => "KO",
                Lang.NB => "NB",
                Lang.SV => "SV",
                Lang.DA => "DA",
                Lang.FI => "FI",
                Lang.EL => "EL",
                Lang.HU => "HU",
                Lang.BG => "BG",
                Lang.CS => "CS",
                Lang.RO => "RO",
                Lang.SK => "SK",
                Lang.LT => "LT",
                Lang.LV => "LV",
                Lang.ET => "ET",
                Lang.SL => "SL",
                Lang.HR => "HR",
                _ => throw new ArgumentOutOfRangeException(nameof(lang), lang, null)
            };
        }

        public async Task<string> TranslateText(string text, Lang targetLang, Lang sourceLang = Lang.EN)
        {
            var url = "https://api-free.deepl.com/v2/translate";
            var parameters = new Dictionary<string, string>
            {
                { "auth_key", ApiKey },
                { "text", text },
                { "target_lang", GetLangCode(targetLang) }
            };
            if (sourceLang != Lang.EN)
            {
                parameters.Add("source_lang", GetLangCode(sourceLang));
            }
            var content = new FormUrlEncodedContent(parameters);
            var response = await _client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            else
            {
                throw new Exception($"Error: {response.StatusCode}");
            }
        }
    }
}
