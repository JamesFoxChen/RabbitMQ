using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace _07_Model
{
    public class SerializableUtil
    {
        public static byte[] Serialize(object data)
        {
            //MemoryStream ms = new MemoryStream();
            //BinaryFormatter bf = new BinaryFormatter();
            //bf.Serialize(ms, data);
            //ms.Seek(0, 0);
            //StreamReader rdr = new StreamReader(ms);
            //string str = rdr.ReadToEnd();
            //byte[] byteArray = Encoding.UTF8.GetBytes(str);
            //return byteArray;

            using (MemoryStream ms = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, data); 
                return ms.GetBuffer();
            } 
        }

        public static object Deserialize(byte[] data)
        {
            //MemoryStream stream = new MemoryStream(data);
            //stream.Seek(0, 0);
            //BinaryFormatter bf = new BinaryFormatter();
            //object d = bf.Deserialize(stream);
            //return d;

            using (MemoryStream ms = new MemoryStream(data))
            {
                IFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(ms);
            } 
        }
    }
}
