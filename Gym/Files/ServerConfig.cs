using System;
using System.Windows.Controls;

namespace Gym.Files
{
    public class ServerConfig
    {
        public char c { get; set; }
        public int AI { get; set; }
        public Int64 j { get; set; }
        public Int64 A { get; set; }
        public string a { get; set; }
        public string u { get; set; }
        ListBox la = new ListBox();
        ListBox ls = new ListBox();



        public string IAmBook(string s)
        {
            //------------------------------------جدا کردن ارقام شماره سریال دریافتی و افزودن به لیست باکس 1
            la.Items.Clear();
            ls.Items.Clear();
            for (int i = 0; i < s.Length; i++)
            {
                a = s.Substring(i, 1);
                la.Items.Add(a);
            }
            //-------------------------------------تبدیل به کد اسکی و افزودن به لیست باکس 2
            for (int i = 0; i < la.Items.Count; i++)
            {
                a = la.Items[i].ToString();
                c = char.Parse(a);
                AI = Convert.ToInt32(c);
                ls.Items.Add(AI);
            }
            A = 0;
            //-------------------------------------جمع کردن مقادیر لیست باکس 2 و نوشتن الگوریتم نهایی برای تولید قفل اصلی  
            for (int i = 0; i < ls.Items.Count; i++)
            {
                a = ls.Items[i].ToString();
                j = int.Parse(a);
                A += j;
            }

            u = (((A * 140975) + 220962) * 150765 * 11).ToString();
            return u;
        }
    }
}
