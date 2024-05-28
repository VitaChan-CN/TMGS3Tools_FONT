using TMGS3Tools_FONT.Util;

namespace TMGS3Tools_FONT.Prfetrtbl.Entity
{
    public struct SecondSelTbl_MapTable
    {

        //第二张表格的映射表
        //0x04E0为起始|  字数 |  假名
        //00 00 00 00 | 15 00 | 42 30 あ
        //2A 00 00 00 | 35 00 | 44 30 い
        //94 00 00 00 | 0C 00 | 46 30 う
        //AC 00 00 00 | 3A 00 | 48 30 え
        public uint StartPos;
        public ushort MapCharCount;
        [Unicode16LEString]
        public string Char;


    }
}
