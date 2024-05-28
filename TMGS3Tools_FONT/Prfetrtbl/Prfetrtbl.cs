using Newtonsoft.Json;
using System.Text;
using Microsoft.International.Converters.PinYinConverter;
using System.Globalization;
using TMGS3Tools_FONT.Util;
using TMGS3Tools_FONT.Prfetrtbl.Entity;

namespace SharedClass.Prfetrtbl
{
    public class Prfetrtbl
    {
        static string[] GetFullLetter(char chineseCharacter)
        {
            List<string> list = new List<string>();
            try
            {
                ChineseChar chineseChar = new ChineseChar(chineseCharacter);
                foreach (var pinyin in chineseChar.Pinyins)
                {
                    if (!string.IsNullOrEmpty(pinyin))
                    {
                        list.Add(pinyin);
                    }
                }
            }
            catch
            {
                Console.WriteLine("!! Warning !!  " + chineseCharacter + "  此字符没有拼音 跳过或者删除");
            }
            return list.ToArray();
        }

        static string[] GetFirstLetter(char chineseCharacter)
        {
            List<string> list = new List<string>();
            try
            {
                ChineseChar chineseChar = new ChineseChar(chineseCharacter);
                foreach (var pinyin in chineseChar.Pinyins)
                {
                    if (!string.IsNullOrEmpty(pinyin))
                    {
                        list.Add(pinyin.Substring(0, 1));
                    }
                }
            }catch
            {
                Console.WriteLine("!! Warning !!  " + chineseCharacter + "  此字符没有拼音 跳过或者删除");
            }
            return list.ToArray();
        }

        public void Write(string inputJsonFile, string outputPretFile, string intput4 , string intputDebugString ,string inputFontJp)
        {


            string str4 = File.ReadAllText(intput4);
            string strDebugString = File.ReadAllText(intputDebugString);
            string strFontJp = File.ReadAllText(inputFontJp);
            str4 = str4.Replace(strDebugString, "").Replace(strFontJp, "").Replace("‧","").Replace("®™","").Replace(" ","").Replace("·","");
     

            PrfetrtblOutput poutput = JsonConvert.DeserializeObject<PrfetrtblOutput>(File.ReadAllText(inputJsonFile));
            poutput.secondSelTbl_MapTable_Onyomi_Detail_dic.Clear();
            poutput.secondSelTbl_MapTable_Kunyomi_Detail_dic.Clear();

            string fixaz = "abcdefghijklmnopqrstuvwxyz";
            fixaz = poutput.ftbl_str_OnyomiKunyomi.Replace("\0","");
            foreach (var item in fixaz)
            {
                if (!poutput.secondSelTbl_MapTable_Onyomi_Detail_dic.ContainsKey(item.ToString()))
                {
                    poutput.secondSelTbl_MapTable_Onyomi_Detail_dic.Add(item.ToString(), "");
                }
                if (!poutput.secondSelTbl_MapTable_Kunyomi_Detail_dic.ContainsKey(item.ToString()))
                {
                    poutput.secondSelTbl_MapTable_Kunyomi_Detail_dic.Add(item.ToString(), "");
                }
            }

            foreach (var item in str4.Reverse())
            {
                if (item == '泌')
                {
                    string test = "io";
                }
                string[] pinyinArr = GetFirstLetter(item);
                string[] pinyinFullArr = GetFullLetter(item);
                for (int i = 0; i < pinyinArr.Length; i++)
                {
                    string pinyin = pinyinArr[i].ToLower();
                    string pinyinFull = pinyinFullArr[i].ToLower();
                    if (!string.IsNullOrEmpty(pinyin))
                    {
                        if (poutput.secondSelTbl_MapTable_Onyomi_Detail_dic.ContainsKey("Z") &&
                            poutput.secondSelTbl_MapTable_Onyomi_Detail_dic.ContainsKey("C") &&
                            poutput.secondSelTbl_MapTable_Onyomi_Detail_dic.ContainsKey("S")
                            )
                        {
                            if (pinyinFull.StartsWith("zh"))
                            {
                                pinyin = "Z";
                            }
                            else if (pinyinFull.StartsWith("ch"))
                            {
                                pinyin = "C";
                            }
                            else if (pinyinFull.StartsWith("sh"))
                            {
                                pinyin = "S";
                            }
                        }

                        if (poutput.secondSelTbl_MapTable_Onyomi_Detail_dic.ContainsKey(pinyin))
                        {
                            if (!poutput.secondSelTbl_MapTable_Onyomi_Detail_dic[pinyin].Contains(item))
                            {
                                poutput.secondSelTbl_MapTable_Onyomi_Detail_dic[pinyin] += item;
                            }
                        }
                        else
                        {
                            poutput.secondSelTbl_MapTable_Onyomi_Detail_dic.Add(pinyin, item.ToString());
                        }


                        //=========================
                        if (poutput.secondSelTbl_MapTable_Kunyomi_Detail_dic.ContainsKey("Z") &&
                            poutput.secondSelTbl_MapTable_Kunyomi_Detail_dic.ContainsKey("C") &&
                            poutput.secondSelTbl_MapTable_Kunyomi_Detail_dic.ContainsKey("S")
                            )
                        {
                            if (pinyinFull.StartsWith("zh"))
                            {
                                pinyin = "Z";
                            }
                            else if (pinyinFull.StartsWith("ch"))
                            {
                                pinyin = "C";
                            }
                            else if (pinyinFull.StartsWith("sh"))
                            {
                                pinyin = "S";
                            }
                        }

                        if (poutput.secondSelTbl_MapTable_Kunyomi_Detail_dic.ContainsKey(pinyin))
                        {
                            if (!poutput.secondSelTbl_MapTable_Kunyomi_Detail_dic[pinyin].Contains(item))
                            {
                                poutput.secondSelTbl_MapTable_Kunyomi_Detail_dic[pinyin] += item;
                            }
                          
                        }
                        else
                        {
                            poutput.secondSelTbl_MapTable_Kunyomi_Detail_dic.Add(pinyin, item.ToString());
                        }
                    }
                }
            }

            CultureInfo zhCN = new CultureInfo("zh-CN");
            foreach (var kv in poutput.secondSelTbl_MapTable_Onyomi_Detail_dic)
            {
                char[] chineseChars = kv.Value.ToCharArray();
                Array.Sort(chineseChars, (x, y) => String.Compare(x.ToString(), y.ToString(),true ,zhCN));
                string sortedString = new string(chineseChars);
                poutput.secondSelTbl_MapTable_Onyomi_Detail_dic[kv.Key] = sortedString;
            }
            foreach (var kv in poutput.secondSelTbl_MapTable_Kunyomi_Detail_dic)
            {
                char[] chineseChars = kv.Value.ToCharArray();
                Array.Sort(chineseChars, (x, y) => String.Compare(x.ToString(), y.ToString(), true, zhCN));
                string sortedString = new string(chineseChars);
                poutput.secondSelTbl_MapTable_Kunyomi_Detail_dic[kv.Key] = sortedString;
            }

            // 创建文件信息对象
            FileInfo fileInfo = new FileInfo(outputPretFile);

            // 创建 StructWriter 对象
            StructWriter writer = new StructWriter(File.Open(outputPretFile, FileMode.Create));

            //重新计算长度
            poutput.innerInfo.Header.SecondSelTbl_MapTable_CharacterCount = (uint)poutput.secondSelTbl_MapTable_Onyomi_Detail_dic.Count + (uint)poutput.secondSelTbl_MapTable_Kunyomi_Detail_dic.Count;
            poutput.innerInfo.Header.SecondSelTbl_MapTableCharacter_StartPos =
                 poutput.innerInfo.Header.SecondSelTbl_MapTable_StartPos +
                8 * (uint)poutput.secondSelTbl_MapTable_Onyomi_Detail_dic.Count +
                8 * (uint)poutput.secondSelTbl_MapTable_Kunyomi_Detail_dic.Count;
            int charcount = 0;
            foreach (var kvp in poutput.secondSelTbl_MapTable_Onyomi_Detail_dic)
            {
                charcount = charcount + kvp.Value.Length;
            }
            foreach (var kvp in poutput.secondSelTbl_MapTable_Kunyomi_Detail_dic)
            {
                charcount = charcount + kvp.Value.Length;
            }
            poutput.innerInfo.Header.SecondSelTbl_MapTableCharacter_Count = (uint)charcount;


           Prfetrtbl_Header prfetrtbl_Header = poutput.innerInfo.Header;
            // 写入头部信息
            writer.WriteStruct(ref prfetrtbl_Header);

            FirstSelTbl_TableParam firstSelTbl_TableParam = new FirstSelTbl_TableParam();
            // 写入第一张表的参数
            firstSelTbl_TableParam = poutput.innerInfo.ftbl_info_OnyomiKunyomi;
            writer.WriteStruct(ref firstSelTbl_TableParam);
            firstSelTbl_TableParam = poutput.innerInfo.ftbl_info_Hirakana;
            writer.WriteStruct(ref firstSelTbl_TableParam);
            firstSelTbl_TableParam = poutput.innerInfo.ftbl_info_Katakana;
            writer.WriteStruct(ref firstSelTbl_TableParam);
            firstSelTbl_TableParam = poutput.innerInfo.ftbl_info_Eisuu;
            writer.WriteStruct(ref firstSelTbl_TableParam);
            firstSelTbl_TableParam = poutput.innerInfo.ftbl_info_Yomikata;
            writer.WriteStruct(ref firstSelTbl_TableParam);

            // 写入第一张表单数据
            WriteFirstTableData(writer, poutput.ftbl_str_OnyomiKunyomi, poutput.innerInfo.ftbl_info_OnyomiKunyomi);
            WriteFirstTableData(writer, poutput.ftbl_str_Hirakana, poutput.innerInfo.ftbl_info_Hirakana);
            WriteFirstTableData(writer, poutput.ftbl_str_Katakana, poutput.innerInfo.ftbl_info_Katakana);
            WriteFirstTableData(writer, poutput.ftbl_str_Eisuu, poutput.innerInfo.ftbl_info_Eisuu);
            WriteFirstTableData(writer, poutput.ftbl_str_Yomikata, poutput.innerInfo.ftbl_info_Yomikata);


            // 重新计算第二张表的映射参数 长度 
            poutput.innerInfo.SecondSelTbl_MapTableParams_Onyomi_StartPos = 0;
            poutput.innerInfo.SecondSelTbl_MapTableParams_Onyomi_CharacterCount = (uint)poutput.secondSelTbl_MapTable_Onyomi_Detail_dic.Count;
            poutput.innerInfo.SecondSelTbl_MapTableParams_Kunyomi_StartPos = poutput.innerInfo.SecondSelTbl_MapTableParams_Onyomi_CharacterCount * 8;
            poutput.innerInfo.SecondSelTbl_MapTableParams_Kunyomi_CharacterCount = (uint)poutput.secondSelTbl_MapTable_Kunyomi_Detail_dic.Count;

            // 写入第二张表的映射参数
            writer.Write(poutput.innerInfo.SecondSelTbl_MapTableParams_Onyomi_StartPos);
            writer.Write(poutput.innerInfo.SecondSelTbl_MapTableParams_Onyomi_CharacterCount);
            writer.Write(poutput.innerInfo.SecondSelTbl_MapTableParams_Kunyomi_StartPos);
            writer.Write(poutput.innerInfo.SecondSelTbl_MapTableParams_Kunyomi_CharacterCount);

            // 重新计算长度 secondSelTbl_MapTable_Onyomi_dic / secondSelTbl_MapTable_Kunyomi_dic
            int pos = 0;
            poutput.innerInfo.secondSelTbl_MapTable_Onyomi_dic.Clear();
            foreach (var kvp in poutput.secondSelTbl_MapTable_Onyomi_Detail_dic)
            {
                poutput.innerInfo.secondSelTbl_MapTable_Onyomi_dic.Add(kvp.Key, new SecondSelTbl_MapTable()
                {
                    Char = kvp.Key,
                    MapCharCount = ushort.Parse(kvp.Value.Length.ToString()),
                    StartPos = ushort.Parse(pos.ToString()),
                });
                pos = pos + kvp.Value.Length * 2;
            }

            poutput.innerInfo.secondSelTbl_MapTable_Kunyomi_dic.Clear();
            foreach (var kvp in poutput.secondSelTbl_MapTable_Kunyomi_Detail_dic)
            {
                poutput.innerInfo.secondSelTbl_MapTable_Kunyomi_dic.Add(kvp.Key, new SecondSelTbl_MapTable()
                {
                    Char = kvp.Key,
                    MapCharCount = ushort.Parse(kvp.Value.Length.ToString()),
                    StartPos = ushort.Parse(pos.ToString()),
                });
                pos = pos + kvp.Value.Length * 2;
            }

            // 写入第二张表的映射表数据
            WriteSecondTableMapData(writer, poutput.innerInfo.secondSelTbl_MapTable_Onyomi_dic);
            WriteSecondTableMapData(writer, poutput.innerInfo.secondSelTbl_MapTable_Kunyomi_dic);

            //写入第二张表的具体数据 文字
            //音读
            foreach (var item in poutput.innerInfo.secondSelTbl_MapTable_Onyomi_dic)
            {
                string str = poutput.secondSelTbl_MapTable_Onyomi_Detail_dic[item.Key];
                var bytes = Encoding.Unicode.GetBytes(str);
                writer.Write(bytes);
            }
            //训读
            foreach (var item in poutput.innerInfo.secondSelTbl_MapTable_Kunyomi_dic)
            {
                string str = poutput.secondSelTbl_MapTable_Kunyomi_Detail_dic[item.Key];
                var bytes = Encoding.Unicode.GetBytes(str);
                writer.Write(bytes);
            }
            writer.Close();
        }

        private void WriteFirstTableData(StructWriter writer, string data, FirstSelTbl_TableParam ftbl_info)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(data);

            // 写入表单数据
            foreach (byte b in bytes)
            {
                writer.Write(b);
            }

            // 补充字节，以满足表的要求
            int totalBytesNeeded = ftbl_info.ColumnsCount * ftbl_info.RowsCount * 2;
            int bytesWritten = bytes.Length;
            if (bytesWritten < totalBytesNeeded)
            {
                writer.Write(new byte[totalBytesNeeded - bytesWritten]);
            }
        }

        private void WriteSecondTableMapData(StructWriter writer, Dictionary<string, SecondSelTbl_MapTable> mapTable)
        {
            foreach (var kvp in mapTable)
            {
                SecondSelTbl_MapTable secondSelTbl_MapTable = kvp.Value;
                // 写入映射表数据
                writer.WriteStruct(ref secondSelTbl_MapTable);
            }
        }


        public string ReadFirstTableData(ref StructReader Reader, FirstSelTbl_TableParam ftbl_info)
        {
            string ftbl_str = "";
            int total = ftbl_info.ColumnsCount * ftbl_info.RowsCount;
            for (int i = 0; i < total; i++)
            {
                var bytes = Reader.ReadBytes(2);
                ftbl_str += Encoding.Unicode.GetString(bytes);
            }
            return ftbl_str;
        }

        public void Read(string filepath)
        {
            FileInfo dinfo = new FileInfo(filepath);
            StructReader Reader = new StructReader(new MemoryStream(File.ReadAllBytes(filepath)));

            //输出信息
            PrfetrtblOutput poutput = new PrfetrtblOutput();

            try
            {
                var header = new Prfetrtbl_Header();
                Reader.ReadStruct(ref header);
                if (!header.Signature.StartsWith("PRET1.00"))
                {
                    Console.WriteLine("非PRET1.00");
                    return;
                }
                poutput.innerInfo.Header = header;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return;
            }

            #region 读取第一张表的参数
            //第一张表
            // 0x58为起始 |  行数 | 每行个数           | 字数
            //00 00 00 00 | 0A 00 | 05 00 //音读 训读  | 50
            //あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもや ゆ よらりるれろわ    
            //64 00 00 00 | 12 00 | 05 00 //平假名     | 90
            //あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもや ゆ よらりるれろわゐゑをんがぎぐげござじずぜぞだぢづでどばびぶべぼぱぴぷぺぽっゃゅょゎぁぃぅぇぉー～   
            //18 01 00 00 | 12 00 | 05 00 //片假名     | 90
            //アイウエオカキクケコサシスセソタチツテトナニヌネノハヒフヘホマミムメモヤ ユ ヨラリルレロワヰヱヲンガギグゲゴザジズゼゾダヂヅデドバビブベボパピプペポッャュョヮァィゥェォー～   
            //CC 01 00 00 | 2E 00 | 05 00 //英数       | 230
            //ＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺ    ａｂｃｄｅｆｇｈｉｊｋｌｍｎｏｐｑｒｓｔｕｖｗｘｙｚ    ０１２３４５６７８９，。，．・：；？！゛゜´｀¨＾￣＿ヽヾゝゞ〃仝々〆〇ー―‐／＼～∥｜…‥‘’“”（）〔〕 ［］｛｝ 〈〉《》 「」『』 【】＋－±×÷＝≠ ＜＞≦≧ ∞∴♂♀°′″℃￥＄￠￡％＃＆＊＠§☆★○●◎◇◆□■△▲ ▽※〒  →←↑↓ 〓⊆⊇  ⊂⊃∪∩ ∧∨￢⇒⇔∀∃∠⊥⌒∂∇≡≒ ≪≫√∽∝∵∫∬‰ ♯♭♪†‡¶    
            //98 03 00 00 | 0E 00 | 08 00 //读音表     | 112 按一行8个字算的
            //42 30 | 44 30 | 46 30 | 48 30 | 4A 30 | 00 00 | 00 00 | 00 00 //あいうえお
            //4B 30 | 4D 30 | 4F 30 | 51 30 | 53 30 | 00 00 | 00 00 | 00 00 //かきくけこ
            //55 30 | 57 30 | 59 30 | 5B 30 | 5D 30 | 00 00 | 00 00 | 00 00 //さしすせそ
            #endregion
            var ftbl_info = new FirstSelTbl_TableParam();
            Reader.ReadStruct(ref ftbl_info);
            poutput.innerInfo.ftbl_info_OnyomiKunyomi = ftbl_info;
            Reader.ReadStruct(ref ftbl_info);
            poutput.innerInfo.ftbl_info_Hirakana = ftbl_info;
            Reader.ReadStruct(ref ftbl_info);
            poutput.innerInfo.ftbl_info_Katakana = ftbl_info;
            Reader.ReadStruct(ref ftbl_info);
            poutput.innerInfo.ftbl_info_Eisuu = ftbl_info;
            Reader.ReadStruct(ref ftbl_info);
            poutput.innerInfo.ftbl_info_Yomikata = ftbl_info;

            //读取第一张表单
            poutput.ftbl_str_OnyomiKunyomi = ReadFirstTableData(ref Reader, poutput.innerInfo.ftbl_info_OnyomiKunyomi);
            poutput.ftbl_str_Hirakana = ReadFirstTableData(ref Reader, poutput.innerInfo.ftbl_info_Hirakana);
            poutput.ftbl_str_Katakana = ReadFirstTableData(ref Reader, poutput.innerInfo.ftbl_info_Katakana);
            poutput.ftbl_str_Eisuu = ReadFirstTableData(ref Reader, poutput.innerInfo.ftbl_info_Eisuu);
            poutput.ftbl_str_Yomikata = ReadFirstTableData(ref Reader, poutput.innerInfo.ftbl_info_Yomikata);

            //第二张表格的映射参数
            //0x04E0为起始|  个数
            //00 00 00 00 | 2C 00 00 00  音读
            //60 01 00 00 | 2C 00 00 00  训读

            poutput.innerInfo.SecondSelTbl_MapTableParams_Onyomi_StartPos = Reader.ReadUInt32();
            poutput.innerInfo.SecondSelTbl_MapTableParams_Onyomi_CharacterCount = Reader.ReadUInt32();
            poutput.innerInfo.SecondSelTbl_MapTableParams_Kunyomi_StartPos = Reader.ReadUInt32();
            poutput.innerInfo.SecondSelTbl_MapTableParams_Kunyomi_CharacterCount = Reader.ReadUInt32();

            //第二张表格的映射表
            //0x04E0为起始|  字数 |  假名
            //00 00 00 00 | 15 00 | 42 30 あ
            //2A 00 00 00 | 35 00 | 44 30 い
            //94 00 00 00 | 0C 00 | 46 30 う
            //AC 00 00 00 | 3A 00 | 48 30 え

            var secondSelTbl_MapTable_Onyomi = new SecondSelTbl_MapTable();
            var secondSelTbl_MapTable_Kunyomi = new SecondSelTbl_MapTable();
            for (int i = 0; i < poutput.innerInfo.SecondSelTbl_MapTableParams_Onyomi_CharacterCount; i++)
            {
                Reader.ReadStruct(ref secondSelTbl_MapTable_Onyomi);
                poutput.innerInfo.secondSelTbl_MapTable_Onyomi_dic.Add(secondSelTbl_MapTable_Onyomi.Char, secondSelTbl_MapTable_Onyomi);

            }
            for (int i = 0; i < poutput.innerInfo.SecondSelTbl_MapTableParams_Kunyomi_CharacterCount; i++)
            {
                Reader.ReadStruct(ref secondSelTbl_MapTable_Kunyomi);
                poutput.innerInfo.secondSelTbl_MapTable_Kunyomi_dic.Add(secondSelTbl_MapTable_Kunyomi.Char, secondSelTbl_MapTable_Kunyomi);
            }



            foreach (var item in poutput.innerInfo.secondSelTbl_MapTable_Onyomi_dic)
            {
                //读取对应字
                Reader.Seek(poutput.innerInfo.Header.SecondSelTbl_MapTableCharacter_StartPos + item.Value.StartPos, SeekOrigin.Begin);
                var bytes = Reader.ReadBytes(item.Value.MapCharCount * 2);
                poutput.secondSelTbl_MapTable_Onyomi_Detail_dic.Add(item.Value.Char, Encoding.Unicode.GetString(bytes));
            }
            foreach (var item in poutput.innerInfo.secondSelTbl_MapTable_Kunyomi_dic)
            {
                //读取对应字
                Reader.Seek(poutput.innerInfo.Header.SecondSelTbl_MapTableCharacter_StartPos + item.Value.StartPos, SeekOrigin.Begin);
                var bytes = Reader.ReadBytes(item.Value.MapCharCount * 2);
                poutput.secondSelTbl_MapTable_Kunyomi_Detail_dic.Add(item.Value.Char, Encoding.Unicode.GetString(bytes));
            }

            string jsonString = JsonConvert.SerializeObject(poutput, Formatting.Indented);

            // 将对象序列化为 JSON 字符串
            //string jsonString = JsonSerializer.Serialize(poutput, new JsonSerializerOptions
            //{
            //    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            //    WriteIndented = true // 设置为 true 以便更容易阅读
            //});

            // 输出格式化后的 JSON 字符串
            File.WriteAllText(Path.Combine(dinfo.DirectoryName, dinfo.Name.Replace(dinfo.Extension, ".json")), jsonString);
        }


    }
}
