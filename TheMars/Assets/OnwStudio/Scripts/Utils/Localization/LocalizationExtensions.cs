using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Metadata;
using UnityEngine.Localization.Tables;

namespace Onw.Localization
{
    public static class LocalizationExtensions
    {
        public static string GetLocalizedString(this StringTable table, string entryName)
        {
            StringTableEntry entry = table.GetEntry(entryName);

            Comment comment = entry.GetMetadata<Comment>();
            if (comment is not null)
            {
                Debug.Log($"Fount metadata comment for {entryName} - {comment.CommentText}");
            }

            return entry.GetLocalizedString();
        }
    }
}
