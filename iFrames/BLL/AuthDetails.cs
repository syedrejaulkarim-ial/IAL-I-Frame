using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace iFrames.BLL
{
    public class AuthToken
    {
      public  string Token { get; set; }
    }
    public class AuthDetails
    {
        private enum Properties : int
        {
            InstanceId = 0, WebServiceUserId, RequestStarts, RequestEnds, GuId, ClientIp, CheckSum,
            UserName, EmailId, IsLogIn, IsIcrauser, UserSessionId, IsConcurrentUser, Password
            , AuthenticationType
        }

        public AuthDetails()
        {
            RequestEnds = DateTime.MinValue;
            ClientIp = GuId = UserName = EmailId = UserSessionId = Password =
             AuthenticationType = string.Empty;
            IsLogIn = false;
            IsIcrauser = false;
            IsConcurrentUser = false;
        }
        private object this[Properties Properties]
        {
            get { return GetterSetter(Properties, true); }
            set { GetterSetter(Properties, false, value); }
        }
        private object GetterSetter(Properties Prop, bool IsGet, object value = null)
        {
            switch (Prop)
            {
                case Properties.WebServiceUserId:
                    if (!IsGet)
                        this.WebServiceUserId = Convert.ToDecimal(value);
                    return this.WebServiceUserId;
                case Properties.InstanceId:
                    if (!IsGet)
                        this.InstanceId = Convert.ToInt32(value);
                    return this.InstanceId;
                case Properties.RequestStarts:
                    if (!IsGet)
                        this.RequestStarts = new DateTime(Convert.ToInt64(value));
                    return this.RequestStarts.Ticks;
                case Properties.RequestEnds:
                    if (!IsGet)
                        this.RequestEnds = new DateTime(Convert.ToInt64(value));
                    return this.RequestEnds.Ticks;
                case Properties.GuId:
                    if (!IsGet)
                        this.GuId = value.ToString();
                    return this.GuId;
                case Properties.ClientIp:
                    if (!IsGet)
                        this.ClientIp = value.ToString();
                    return this.ClientIp;
                case Properties.CheckSum:
                    if (!IsGet)
                        this.CheckSum = Convert.ToInt32(value);
                    return this.CheckSum;
                case Properties.EmailId:
                    if (!IsGet)
                        this.EmailId = value.ToString();
                    return this.EmailId;
                case Properties.UserName:
                    if (!IsGet)
                        this.UserName = value.ToString();
                    return this.UserName;
                case Properties.Password:
                    if (!IsGet)
                        this.Password = value.ToString();
                    return this.Password;
                case Properties.IsLogIn:
                    if (!IsGet)
                        this.IsLogIn = Convert.ToBoolean(value);
                    return this.IsLogIn;
                case Properties.IsIcrauser:
                    if (!IsGet)
                        this.IsIcrauser = Convert.ToBoolean(value);
                    return this.IsIcrauser;
                case Properties.UserSessionId:
                    if (!IsGet)
                        this.UserSessionId = Convert.ToString(value);
                    return this.UserSessionId;
                case Properties.IsConcurrentUser:
                    if (!IsGet)
                        this.IsConcurrentUser = Convert.ToBoolean(value);
                    return this.IsConcurrentUser;
                case Properties.AuthenticationType:
                    if (!IsGet)
                        this.AuthenticationType = Convert.ToString(value);
                    return this.AuthenticationType;
                default:
                    throw new Exception("Error in Property Name");
            }

        }

        public string GuId { get; set; } // 36
        public string ClientIp { get; set; } // 16
        public decimal WebServiceUserId { get; set; } // 8
        public int InstanceId { get; set; }// 8
        public DateTime RequestStarts { get; set; }//20
        public DateTime RequestEnds { get; set; }//20        
        public int CheckSum { get; set; }//8
        public string EmailId { get; set; } // 255
        public string UserName { get; set; } //100
        public string UserSessionId { get; set; }
        public bool IsLogIn { get; set; }
        public bool IsIcrauser { get; set; }
        public bool IsConcurrentUser { get; set; }
        public string Password { get; set; }
        public string AuthenticationType { get; set; }
        public string GetUniqueId()
        {
            StringBuilder sb = new StringBuilder();
            CheckSum = 0;
            int RandAppend = 0;
            var Rand = new Random();

            foreach (Properties prop in Enum.GetValues(typeof(Properties)))//in new[] { WebServiceUserId, InstanceId, RequestStarts.Ticks, RequestEnds.Ticks })
            {
                CheckSum += Rand.Next(3978);
                RandAppend = Rand.Next(0, 9);
                if (prop != Properties.GuId &&
                        prop != Properties.ClientIp &&
                        prop != Properties.UserName &&
                        prop != Properties.Password &&
                        prop != Properties.EmailId &&
                        prop != Properties.UserSessionId &&
                        prop != Properties.AuthenticationType)
                {
                    sb.Append("[" + EncryptDecrypt.ICRANumericEncode(Convert.ToInt64(this[prop])) + "]" + RandAppend.ToString());
                }
                else
                {
                    sb.Append("[" + (string)this[prop] + "]");
                }
            }
            //CheckSum += Rand.Next(3978);
            //sb.Append("[" + this.GuId + "]");
            //sb.Append("[" + this.ClientIp + "]");
            //sb.Append("[" + this.EmailId + "]");
            //sb.Append("[" + this.UserName + "]");
            //sb.Append("[" + EncryptDecrypt.ICRANumericEncode((long)CheckSum) + "]");
            return EncryptDecrypt.DESEnCode(sb.ToString());
        }
        //[1]1[Suvab]
        public static implicit operator AuthDetails(string Text)
        {
            return GetInstance(Text);
        }
        private static AuthDetails GetInstance(string EncryptionText)
        {
            try
            {
                var Text = EncryptDecrypt.DESDeCode(EncryptionText);
                var Instance = new AuthDetails();
                var Matches = System.Text.RegularExpressions.Regex.Matches(Text, "\\[(.*?)\\]");
                foreach (Properties prop in Enum.GetValues(typeof(Properties)))
                {
                    if (prop != Properties.GuId &&
                        prop != Properties.ClientIp &&
                        prop != Properties.UserName &&
                        prop != Properties.Password &&
                        prop != Properties.EmailId &&
                        prop != Properties.UserSessionId &&
                        prop != Properties.AuthenticationType)
                    {
                        Instance[prop] = EncryptDecrypt.ICRANumericDecode(Matches[(int)prop].Groups[1].ToString());
                    }
                    else
                    {
                        Instance[prop] = Matches[(int)prop].Groups[1].ToString();
                    }
                }

                return Instance;
            }
            catch (Exception)
            {

                return null;
            }
        }

    }
}