﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TheraBytes.BetterUi.Editor
{
    public class PersistentWizardData
    {
        private const char SEPARATOR = '=';

        private string filePath;
        private Dictionary<string, string> data;

        public int SavedDataCount { get { return (data != null) ? data.Count : 0; } }

        public PersistentWizardData(string filePath)
        {
            this.filePath = filePath;
        }

        public bool FileExists()
        {
            return File.Exists(filePath);
        }

        public bool TryDeserialize()
        {
            if (!FileExists())
                return false;

            try
            {
                this.data = new Dictionary<string, string>();
                string[] lines = File.ReadAllLines(filePath);
                foreach (string l in lines)
                {
                    string[] kv = l.Split(new char[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
                    data.Add(kv[0], kv[1]);
                }
                return true;
            }
            catch (Exception ex)
            {
                data.Clear();
                Debug.LogError("could not deserialize wizard data: " + ex);
                return false;
            }
        }

        public bool TryGetValue(string key, out string parsableValueString)
        {
            if(data == null)
            {
                if(TryDeserialize() == false)
                {
                    parsableValueString = null;
                    return false;
                }
            }

            return data.TryGetValue(key, out parsableValueString);
        }

        public void RegisterValue(string key, string parsableValueString)
        {
            if (data == null)
            {
                if (!TryDeserialize())
                {
                    data = new Dictionary<string, string>();
                }
            }

            data[key] = parsableValueString;
        }

        public bool RemoveEntry(string key)
        {
            if (data == null)
            {
                if (!TryDeserialize())
                {
                    return false;
                }
            }

            return data.Remove(key);
        }


        public void Save()
        {
            // ensure the directory exists
            string dir = Path.GetDirectoryName(filePath);
            if(!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            // ensure that there is no old data at the end of the file after save.
            if(FileExists())
            {
                File.Delete(filePath);
            }

            // save the data
            using(FileStream stream = File.OpenWrite(filePath))
            {
                using(StreamWriter sw = new StreamWriter(stream))
                {
                    foreach(KeyValuePair<string, string> kv in data)
                    {
                        sw.WriteLine(string.Format("{0}{2}{1}", kv.Key, kv.Value, SEPARATOR));
                    }
                }
            }
        }
    }
}
