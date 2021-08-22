using System;
using System.Collections.Generic;
using System.Text;

namespace LOMGAgameClass
{
    [Serializable]
    public class Account
    {
        public int index = 0;
        public string nickname;

        public Account(string nickname, int index)
        {
            this.index = index;
            this.nickname = nickname;
        }
        public Account()
        {
            this.nickname = "\"Empty name\"";
        }
    }
}
