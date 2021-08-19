using System;
using System.Collections.Generic;
using System.Text;

namespace LOMGAgameClass
{
    [Serializable]
    public class Account
    {
        public string nickname;

        public Account(string nickname)
        {
            this.nickname = nickname;
        }
        public Account()
        {
            this.nickname = "\"Empty name\"";
        }
    }
}
