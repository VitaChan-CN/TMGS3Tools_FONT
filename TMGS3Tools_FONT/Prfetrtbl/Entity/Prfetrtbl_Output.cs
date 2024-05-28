using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Text;

namespace TMGS3Tools_FONT.Prfetrtbl.Entity
{
    public class InnerInfo
    {
        public InnerInfo()
        {
            Header = new Prfetrtbl_Header();
            ftbl_info_OnyomiKunyomi = new FirstSelTbl_TableParam();
            ftbl_info_Hirakana = new FirstSelTbl_TableParam();
            ftbl_info_Katakana = new FirstSelTbl_TableParam();
            ftbl_info_Eisuu = new FirstSelTbl_TableParam();
            ftbl_info_Yomikata = new FirstSelTbl_TableParam();
            secondSelTbl_MapTable_Onyomi_dic = new Dictionary<string, SecondSelTbl_MapTable>();
            secondSelTbl_MapTable_Kunyomi_dic = new Dictionary<string, SecondSelTbl_MapTable>();
        }
        public Prfetrtbl_Header Header { get; set; }
        public FirstSelTbl_TableParam ftbl_info_OnyomiKunyomi { get; set; }
        public FirstSelTbl_TableParam ftbl_info_Hirakana { get; set; }
        public FirstSelTbl_TableParam ftbl_info_Katakana { get; set; }
        public FirstSelTbl_TableParam ftbl_info_Eisuu { get; set; }
        public FirstSelTbl_TableParam ftbl_info_Yomikata { get; set; }

        public Dictionary<string, SecondSelTbl_MapTable> secondSelTbl_MapTable_Onyomi_dic { get; set; }
        public Dictionary<string, SecondSelTbl_MapTable> secondSelTbl_MapTable_Kunyomi_dic { get; set; }

        public uint SecondSelTbl_MapTableParams_Onyomi_StartPos { get; set; }
        public uint SecondSelTbl_MapTableParams_Onyomi_CharacterCount { get; set; }
        public uint SecondSelTbl_MapTableParams_Kunyomi_StartPos { get; set; }
        public uint SecondSelTbl_MapTableParams_Kunyomi_CharacterCount { get; set; }
    }
    public class PrfetrtblOutput
    {
        public PrfetrtblOutput()
        {
            innerInfo = new InnerInfo();
            secondSelTbl_MapTable_Onyomi_Detail_dic = new Dictionary<string, string>();
            secondSelTbl_MapTable_Kunyomi_Detail_dic = new Dictionary<string, string>();
        }
        public InnerInfo innerInfo { get; set; }

        public string ftbl_str_OnyomiKunyomi { get; set; }
        public string ftbl_str_Hirakana { get; set; }
        public string ftbl_str_Katakana { get; set; }
        public string ftbl_str_Eisuu { get; set; }
        public string ftbl_str_Yomikata { get; set; }
        public Dictionary<string, string> secondSelTbl_MapTable_Onyomi_Detail_dic { get; set; }
        public Dictionary<string, string> secondSelTbl_MapTable_Kunyomi_Detail_dic { get; set; }
    }


}
