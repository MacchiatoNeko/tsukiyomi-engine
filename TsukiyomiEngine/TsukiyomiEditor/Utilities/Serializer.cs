using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TsukiyomiEditor.Utilities
{
    public static class Serializer
    {
        public static void ToFile<T>(T instance, string path)
        {
            try
            {
                using var fs = new FileStream(path, FileMode.Create);
                var serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(fs, instance);
            }
            catch (SerializationException e)
            {
                Debug.WriteLine("Serialization Exception: " + e.Message);
            }
            catch (IOException e)
            {
                Debug.WriteLine("IO Exception: " + e.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception: " + e.Message);
            }
        }
        internal static T FromFile<T>(string path)
        {
            try
            {
                using var fs = new FileStream(path, FileMode.Open);
                var serializer = new DataContractSerializer(typeof(T));
                T instance = (T)serializer.ReadObject(fs);
                return instance;
            }
            catch (SerializationException e)
            {
                Debug.WriteLine("Deserialization Exception: " + e.Message);
                return default(T);
            }
            catch (IOException e)
            {
                Debug.WriteLine("IO Exception: " + e.Message);
                return default(T);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception: " + e.Message);
                return default(T);
            }
        }
    }
}
