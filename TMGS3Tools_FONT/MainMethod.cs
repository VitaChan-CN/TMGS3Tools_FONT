using System.Text;
using System.Text.RegularExpressions;

namespace TMGS3Tools_FONT
{
    public class MainMethod
    {
        //选名位置的导出导入
        public static void ExportPrfetrtbl(string inputFile)
        {
            var prfetrtbl = new SharedClass.Prfetrtbl.Prfetrtbl();
            prfetrtbl.Read(inputFile);
        }

        public static void SavePrfetrtbl(string inputJsonFile, string outputPretFile, string intput4, string intputDebugString, string inputFontJp)
        {
            var prfetrtbl = new SharedClass.Prfetrtbl.Prfetrtbl();
            prfetrtbl.Write(inputJsonFile, outputPretFile, intput4,intputDebugString,inputFontJp);
        }

        //Room文本的导入导出
        public static void ImportRoomText(string inputFolder,string outputFolder, string fonttextpath)
        {
            foreach (string filepath in Directory.EnumerateFiles(inputFolder,"*.txt"))
            {
                string inputfile = filepath;
                FileInfo fileinfo =new FileInfo(inputfile);
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }
                string outputfile = Path.Combine(outputFolder,fileinfo.Name.Replace(".txt", ""));
                byte[] bytesEnd = new byte[] { 0x00, 0x80 };
                string line = File.ReadAllText(inputfile);
                var fonts = File.ReadAllText(fonttextpath);
                List<byte> bytes = new List<byte>();
                foreach (var c in line)
                {
                    ushort index = 0;
                    if (!fonts.Contains(c))
                    {
                        Console.WriteLine("Error!! ImportRoomText[缺失文字] 请先运行calc_font_num 执行文字统计");
                    }
                    else
                    {
                        index = Convert.ToUInt16(fonts.IndexOf(c));
                    }
                    byte[] byteArray = BitConverter.GetBytes(index);
                    bytes.AddRange(byteArray);
                }
                bytes.AddRange(bytesEnd);
                var originalBytes = bytes.ToArray();
                // 计算需要填充的字节数
                int paddingBytes = 16 - (originalBytes.Length % 16);
                if (paddingBytes == 16)
                {
                    // 如果原始字节数组已经是16字节对齐，不需要填充
                    paddingBytes = 0;
                }
                // 创建新的字节数组，包含原始数据和填充数据
                byte[] alignedBytes = new byte[originalBytes.Length + paddingBytes];
                Array.Copy(originalBytes, alignedBytes, originalBytes.Length);

                //alignedBytes
                File.WriteAllBytes(outputfile, alignedBytes);
                Console.WriteLine("输出完成:" + outputfile);
            }
        }

        public static void ExportRoomText(string folderPath,string jpfonttextpath)
        {
            foreach (string filepath in Directory.EnumerateFiles(folderPath))
            {
                var strs = File.ReadAllText(jpfonttextpath);
                byte[] bytesEnd = new byte[] { 0x00, 0x80 };
                var ms = new MemoryStream(File.ReadAllBytes(filepath));
                string str = "";
                byte[] bytes = new byte[2];
                do
                {
                    ms.Read(bytes, 0, 2);
                    if (!bytes.SequenceEqual(bytesEnd))
                    {
                        var index = BitConverter.ToUInt16(bytes, 0);
                        str += strs[index];
                    }
                }
                while (!bytes.SequenceEqual(new byte[] { 0x00, 0x80 }));

                Console.WriteLine(str);
                File.WriteAllText(filepath + ".txt", str);
            }
        }

        //font\fontjjp.txt
        //intermediate\script_ftext
        //intermediate\title_ftext\
        //room_text_path
        //intermediate\map_ftext
        //intermediate\eboot.txt
        //font
        //font\debugstring.txt
        public static string debugstring = "ぁあぃいぅうぇえぉおかがきぎくぐけげこごさざしじすずせぜそぞただちぢっつづてでとどなにぬねのはばぱひびぴふぶぷへべぺほぼぽまみむめもゃやゅゆょよらりるれろゎわゐゑをん゛゜ゝゞァアィイゥウェエォオカガキギクグケゲコゴサザシジスズセゼソゾタダチヂッツヅテデトドナニヌネノハバパヒビピフブプヘベペホボポマミムメモャヤュユョヨラリルレロヮワヰヱヲンヴヶ・ーヽヾ";
        public static string table0_empty_fix = "　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　";
        public static string table0_empty_fix1 = "　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　";

        //计算文本中的需要添加的字库字数
        public static void calc_font_num(
            string fontjjppath,
            string script_ftext_path,
            string title_ftext_path,
            string room_text_path,
            string map_ftext_path,
            string eboottxtpath,
            string outputpath,
            string debugstringpath
            )
        {
            if (File.Exists(debugstringpath))
            {
                debugstring = File.ReadAllText(debugstringpath);
            }
            Dictionary<string,int> chars = new Dictionary<string,int>();
            string fontjjp = File.ReadAllLines(fontjjppath)[0];
            var files = Directory.GetFiles(script_ftext_path, "*.txt");
            Console.WriteLine(script_ftext_path + "载入文本数量:" + files.Length);
            var files2 = Directory.GetFiles(title_ftext_path, "*.txt");
            Console.WriteLine(title_ftext_path + "载入文本数量:" + files2.Length);
            var files3 = Directory.GetFiles(room_text_path, "*.txt", SearchOption.AllDirectories);
            Console.WriteLine(room_text_path + "载入文本数量:" + files3.Length);
            var files4 = Directory.GetFiles(map_ftext_path, "*.txt");
            Console.WriteLine(map_ftext_path + "载入文本数量:" + files4.Length);
            var fileeboot = eboottxtpath;
            foreach (var file_path in files)
            {
                foreach (var line in File.ReadAllLines(file_path))
                {
                    Regex regex = new Regex(@"^●(\d*)\|(.+?)\|(.+?)●[ ](.*)");
                    if (regex.IsMatch(line))
                    {
                        foreach (var c in line)
                        {
                            if (!fontjjp.Contains(c) && c.ToString()!= "	" &&  !debugstring.Contains(c))
                            {
                                if (chars.ContainsKey(c.ToString()))
                                {
                                    chars[c.ToString()]++;
                                }
                                else
                                {
                                    chars.Add(c.ToString(),0);
                                }
                            }
                        }
                    }
                }
            }
            foreach (var file_path in files2)
            {
                foreach (var line in File.ReadAllLines(file_path))
                {
                    Regex regex = new Regex(@"^●(\d*)\|(.+?)\|(.+?)●[ ](.*)");
                    if (regex.IsMatch(line))
                    {
                        foreach (var c in line)
                        {
                            if (!fontjjp.Contains(c) && c.ToString() != "	" && !debugstring.Contains(c))
                            {
                                if (chars.ContainsKey(c.ToString()))
                                {
                                    chars[c.ToString()]++;
                                }
                                else
                                {
                                    chars.Add(c.ToString(), 0);
                                }
                            }
                        }
                    }
                }
            }

            foreach (var file_path in files3)
            {
                foreach (var line in File.ReadAllLines(file_path))
                {
                    //Regex regex = new Regex(@"^●(\d*)\|(.+?)\|(.+?)●[ ](.*)");
                    //if (regex.IsMatch(line))
                    //{
                        foreach (var c in line)
                        {
                            if (!fontjjp.Contains(c) && c.ToString() != "	" && !debugstring.Contains(c))
                            {
                                if (chars.ContainsKey(c.ToString()))
                                {
                                    chars[c.ToString()]++;
                                }
                                else
                                {
                                    chars.Add(c.ToString(), 0);
                                }
                            }
                        }
                   // }
                }
            }

            foreach (var file_path in files4)
            {
                foreach (var line in File.ReadAllLines(file_path))
                {
                    Regex regex = new Regex(@"^●(\d*)\|(.+?)\|(.+?)●[ ](.*)");
                    if (regex.IsMatch(line))
                    {
                        foreach (var c in line)
                        {
                            if (!fontjjp.Contains(c) && c.ToString() != "	" && !debugstring.Contains(c))
                            {
                                if (chars.ContainsKey(c.ToString()))
                                {
                                    chars[c.ToString()]++;
                                }
                                else
                                {
                                    chars.Add(c.ToString(), 0);
                                }
                            }
                        }
                    }
                }
            }

            string[] lines = File.ReadAllLines(fileeboot);
            foreach (var line in lines)
            {
                Regex regex = new Regex(@"^●(\d*)\|(.+?)\|(.+?)●[ ](.*)");
                if (regex.IsMatch(line))
                {
                    foreach (var c in line)
                    {
                        if (!fontjjp.Contains(c) && c.ToString() != "	" && !debugstring.Contains(c))
                        {
                            if (chars.ContainsKey(c.ToString()))
                            {
                                chars[c.ToString()]++;
                            }
                            else
                            {
                                chars.Add(c.ToString(), 0);
                            }
                        }
                    }
                }
            }
            //552
            //1024
            //1024
            string tableall = "";
            string table0 = table0_empty_fix;
            string table0_1 = table0_empty_fix+ table0_empty_fix1;
            string table1 = "";
            string table2 = "";
            string table3 = "";
            Console.WriteLine("固定符号部分:"+fontjjp.Length);
            Console.WriteLine("添加汉字部分:"+chars.Count);
            Console.WriteLine("总计:"+(int)(fontjjp.Length + chars.Count));
            Console.WriteLine("上限:3504");
            var sorteddict = chars.OrderBy(x => x.Value);
            var sortedchars = sorteddict.Select(x => x.Key).ToList(); 

            for (int i = 0; i < sortedchars.Count; i++)
            {
                //176
                tableall += sortedchars[i];
                if (i < 176)
                {
                    table0 += sortedchars[i];
                }
                else if (i < 728)
                {
                    table0_1 += sortedchars[i];
                }
                else if (i < 1024 + 728)
                {
                    table1 += sortedchars[i];
                }
                else if (i < 1024 + 1024 + 728)
                {
                    table2 += sortedchars[i];
                }
                else
                {
                    table3 += sortedchars[i];
                }
            }
            File.WriteAllText(Path.Combine(outputpath, "4_rebuild.0.cn.txt"), table0);
            File.WriteAllText(Path.Combine(outputpath, "4_rebuild.0_1.cn.txt"), table0_1);
            File.WriteAllText(Path.Combine(outputpath, "4_rebuild.1.txt"), table1);
            File.WriteAllText(Path.Combine(outputpath, "4_rebuild.2.txt"), table2);
            File.WriteAllText(Path.Combine(outputpath, "4_rebuild.3.txt"), table3 + debugstring);

            File.WriteAllText(Path.Combine(outputpath, "4"), fontjjp+ tableall + debugstring);
            // re_line2 = re.compile(r"^●(\d*)\|(.+?)\|(.+?)●[ ](.*)")
            //File.ReadAllText(@"E:\Trans_WorkDir\Test\TMGS3Script\build\font\sss")
        }

        //分割字库文字 一页一张图
        public void SplitFontTxt(string file)
        {
            string allfont = File.ReadAllText(file);
            int filecount = 0;
            int count = 0;
            StringBuilder sb = new StringBuilder();
            foreach (var item in allfont)
            {
                sb.Append(item);
                count++;
                if (count == 32 * 32)
                {
                    File.WriteAllText(file + "." + filecount + ".txt", sb.ToString()); ;
                    filecount++;
                    count = 0;
                    sb.Clear();
                }
            }
            if (count > 0)
            {
                File.WriteAllText(file + "." + filecount + ".txt", sb.ToString()); ;
                filecount++;
                count = 0;
                sb.Clear();
            }
        }


        /// <summary>
        /// 判断目标是文件夹还是目录(目录包括磁盘)
        /// </summary>
        /// <param name="filepath">路径</param>
        /// <returns>返回true为一个文件夹，返回false为一个文件</returns>
        public static bool IsDir(string filepath)
        {
            FileInfo fi = new FileInfo(filepath);
            if ((fi.Attributes & FileAttributes.Directory) != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
