using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Windows;
using DataLayer;

namespace Gym.Utilitys
{
    public class SmsSender
    {
        public void SendMessage(string to, string body)
        {
            string strusername = "";
            string strpass = "";
            string strsender = "";
            Gym_DBEntities db = new Gym_DBEntities();
            MyWebService.Send GymSms = new MyWebService.Send();
            try
            {
                db.GetSmsValue(new ObjectParameter(strusername, strusername), new ObjectParameter(strpass, strpass), new ObjectParameter(strsender, strsender));
            }
            catch(Exception n) 
            {
                MessageBox.Show(n.Message);
            }
            
            long[] Recid = null;
            byte[] status = null;

            string[] strnumbers = new string[] { to.ToString() };

            int result = GymSms.SendSms(strusername, strpass, strsender, strnumbers, body, false, ref status,
                ref Recid);

            switch (result)
            {
                case 0:
                    {
                        MessageBox.Show("پیام ارسال شد");
                        break;
                    }
                case -1:
                    {
                        MessageBox.Show("نام کاربری یا کلمه عبور اشتباه است");
                        break;
                    }
                case 1:
                    {
                        MessageBox.Show("اعتبار کافی نیست");
                        break;
                    }
                case 3:
                    {
                        MessageBox.Show("شماره فرستنده اشتباه است");
                        break;
                    }
            }

        }
    }
}
