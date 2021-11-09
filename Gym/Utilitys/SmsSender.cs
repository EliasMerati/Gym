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
        Gym_DBEntities db = new Gym_DBEntities();
        public void SendMessage(string to, string body)
        {
            try
            {
                var g = db.Gym.ToList();

                string strusername = g[0].Gymsmsusername;
                string strpass = g[0].GymsmsPass;
                string strsender = g[0].Gymsmssender;

                MyWebService.Send GymSms = new MyWebService.Send();
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
            catch (Exception b)
            {
                MessageBox.Show(b.Message);
            }

        }
    }
}
