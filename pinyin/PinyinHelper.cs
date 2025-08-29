using hyjiacan.py4n;
using System;
using System.Text;

namespace PinyinHelper
{
    public static class PinyinConverter
    {
        /// <summary>
        /// 将汉字转换为拼音首字母，字母和符号不变。
        /// </summary>
        /// <param name="chineseText">输入的汉字字符串。</param>
        /// <returns>拼音首字母字符串。</returns>
        public static string ConvertToPinyinInitials(string chineseText)
        {
            if (string.IsNullOrEmpty(chineseText))
            {
                return chineseText;
            }

            StringBuilder pinyinInitials = new StringBuilder();
            foreach (char c in chineseText)
            {
                if (!PinyinUtil.IsHanzi(c))
                {
                    // 如果是字母或数字或符号，直接添加到结果中
                    pinyinInitials.Append(c);
                }
                else
                {
                    // 获取汉字的拼音首字母
                    string pinyin = GetFirstPinyin(c);
                    if (!string.IsNullOrEmpty(pinyin))
                    {
                        pinyinInitials.Append(pinyin[0]);
                    }
                }
            }
            return pinyinInitials.ToString();
        }

        private static string GetFirstPinyin(char c)
        {

            string[] pinyinArray = Pinyin4Net.GetPinyin(c);
            return pinyinArray?[0];
        }
    }
}