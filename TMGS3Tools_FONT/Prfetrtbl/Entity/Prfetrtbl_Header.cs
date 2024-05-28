using TMGS3Tools_FONT.Util;

namespace TMGS3Tools_FONT.Prfetrtbl.Entity
{
    public class Prfetrtbl_Header
    {
        [FString(Length = 8)]
        //50 52 45 54 31 2E 30 30
        public string Signature;
        //30 00 00 00
        public uint FirstSelTbl_MapTableParams_StartPos;//0x30
        //05 00 00 00
        public uint FirstSelTbl_MapTableParams_Count;//5

        //58 00 00 00
        //あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもや ゆ よらりるれろわ    あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもや ゆ よらりるれろわゐゑをんがぎぐげござじずぜぞだぢづでどばびぶべぼぱぴぷぺぽっゃゅょゎぁぃぅぇぉー～   アイウエオカキクケコサシスセソタチツテトナニヌネノハヒフヘホマミムメモヤ ユ ヨラリルレロワヰヱヲンガギグゲゴザジズゼゾダヂヅデドバビブベボパピプペポッャュョヮァィゥェォー～   ＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺ    ａｂｃｄｅｆｇｈｉｊｋｌｍｎｏｐｑｒｓｔｕｖｗｘｙｚ    ０１２３４５６７８９，。，．・：；？！゛゜´｀¨＾￣＿ヽヾゝゞ〃仝々〆〇ー―‐／＼～∥｜…‥‘’“”（）〔〕 ［］｛｝ 〈〉《》 「」『』 【】＋－±×÷＝≠ ＜＞≦≧ ∞∴♂♀°′″℃￥＄￠￡％＃＆＊＠§☆★○●◎◇◆□■△▲ ▽※〒  →←↑↓ 〓⊆⊇  ⊂⊃∪∩ ∧∨￢⇒⇔∀∃∠⊥⌒∂∇≡≒ ≪≫√∽∝∵∫∬‰ ♯♭♪†‡¶    
        //这些东西的其实位置
        public uint FirstSelTbl_MapTable_StartPos;
        //3C 02 00 00                            
        //0x58东西的长度
        //572=50+90+90+230+112 字数 一个字2字节
        public uint FirstSelTbl_MapTable_CharacterCount;


        //D0 04 00 00
        //在地址04D0记录这个东西的地址
        #region 内容示例
        //0x04E0为起始|  个数
        //00 00 00 00 | 2C 00 00 00  音读
        //60 01 00 00 | 2C 00 00 00  训读
        #endregion
        public uint SecondSelTbl_MapTableParams_StartPos;
        //02 00 00 00 
        //记录上面这个的个数 2个
        public uint SecondSelTbl_MapTableParams_Count;
        //E0 04 00 00
        //第二层表的映射文字
        #region 内容示例
        //0x04E0为起始|  字数 |  假名
        //00 00 00 00 | 15 00 | 42 30 あ
        //2A 00 00 00 | 35 00 | 44 30 い
        //94 00 00 00 | 0C 00 | 46 30 う
        //AC 00 00 00 | 3A 00 | 48 30 え
        #endregion 
        public uint SecondSelTbl_MapTable_StartPos;
        //58 00 00 00
        //0x04E0为起始 一共88个字的信息
        public uint SecondSelTbl_MapTable_CharacterCount;

        //A0 07 00 00 
        //汉字开始的地址
        public uint SecondSelTbl_MapTableCharacter_StartPos;

        //A4 16 00 00 
        //5796
        //汉字表字数
        public uint SecondSelTbl_MapTableCharacter_Count;







    }


}
