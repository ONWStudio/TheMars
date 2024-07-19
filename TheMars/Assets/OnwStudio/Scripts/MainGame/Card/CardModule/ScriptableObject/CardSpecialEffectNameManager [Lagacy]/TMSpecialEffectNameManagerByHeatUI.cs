using System.Linq;
using UnityEngine;
using Onw.Manager;
using Onw.Extensions;
using TMCard.SpecialEffect;
using AYellowpaper.SerializedCollections;
using Michsky.UI.Heat;

namespace TMCard.Manager
{
    public sealed partial class TMSpecialEffectNameTableByHeatUI : LocalizationSingleton<TMSpecialEffectNameTableByHeatUI>
    {
        private const string FILE_PATH = "OnwStudio/ScriptableObject/Localization/HeatUI/Resources/Tables/" + nameof(TMSpecialEffectNameTableByHeatUI);

        public override string InstanceName => nameof(TMSpecialEffectNameTableByHeatUI);

        private void Awake()
        {
            localizationSettings = Resources.Load<LocalizationSettings>(GetLocalizationSettingsName());
            if (!localizationSettings.tables.Any(table => table.tableID == InstanceName))
            {
                localizationSettings.tables.Add(new()
                {
                    tableID = InstanceName,
                    localizationTable = this
                });
            }

            foreach (LocalizationLanguage language in localizationSettings.languages.Select(language => language.localizationLanguage))
            {
                language.tableList.Add(new()
                {
                    table = this,
                });
            }

            tableID = InstanceName;
        }
    }
}
