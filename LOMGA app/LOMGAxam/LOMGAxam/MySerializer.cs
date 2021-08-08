using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace LOMGAxam
{
    public class MySerializer
    {
        public static byte[] serialize(object obj)
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream())
                {
                    formatter.Serialize(ms, obj);
                    return ms.ToArray();
                }
            }
            catch (Exception) { return null; }
        }
        public static object deserialize(byte[] obj)
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream(obj))
                {
                    return formatter.Deserialize(ms);
                }
            }
            catch (Exception) { return null; }
        }
    }
}
