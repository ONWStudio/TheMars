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
            var entry = table.GetEntry(entryName);

            var comment = entry.GetMetadata<Comment>();
            if (comment is not null)
            {
                Debug.Log($"Fount metadata comment for {entryName} - {comment.CommentText}");
            }

            return entry.GetLocalizedString();
        }
    }
}
