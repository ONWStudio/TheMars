using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace TcgEngine
{
    public enum CardType
    {
        None = 0,
        Hero = 5,
        Character = 10,
        Spell = 20,
        Artifact = 30,
        Secret = 40,
        Equipment = 50,
    }

    /// <summary>
    /// Defines all card data
    /// </summary>

    [CreateAssetMenu(fileName = "card", menuName = "TcgEngine/CardData", order = 5)]
    public class CardData : ScriptableObject
    {
        [FormerlySerializedAs("id")]
        public string ID;

        [FormerlySerializedAs("title")]
        [Header("Display")]
        public string Title;
        [FormerlySerializedAs("art_full")]
        public Sprite ArtFull;
        [FormerlySerializedAs("art_board")]
        public Sprite ArtBoard;

        [FormerlySerializedAs("type")]
        [Header("Stats")]
        public CardType Type;
        [FormerlySerializedAs("team")]
        public TeamData Team;
        [FormerlySerializedAs("rarity")]
        public RarityData Rarity;
        [FormerlySerializedAs("mana")]
        public int Mana;
        [FormerlySerializedAs("attack")]
        public int Attack;
        [FormerlySerializedAs("hp")]
        public int Hp;

        [FormerlySerializedAs("traits")]
        [Header("Traits")]
        public TraitData[] Traits;
        [FormerlySerializedAs("stats")]
        public TraitStat[] Stats;

        [FormerlySerializedAs("abilities")]
        [Header("Abilities")]
        public AbilityData[] Abilities;

        [FormerlySerializedAs("text")]
        [Header("Card Text")]
        [TextArea(3, 5)]
        public string Text;

        [FormerlySerializedAs("desc")]
        [Header("Description")]
        [TextArea(5, 10)]
        public string Desc;

        [FormerlySerializedAs("spawn_fx")]
        [Header("FX")]
        public GameObject SpawnFX;
        [FormerlySerializedAs("death_fx")]
        public GameObject DeathFX;
        [FormerlySerializedAs("attack_fx")]
        public GameObject AttackFX;
        [FormerlySerializedAs("damage_fx")]
        public GameObject DamageFX;
        [FormerlySerializedAs("idle_fx")]
        public GameObject IdleFX;
        [FormerlySerializedAs("spawn_audio")]
        public AudioClip SpawnAudio;
        [FormerlySerializedAs("death_audio")]
        public AudioClip DeathAudio;
        [FormerlySerializedAs("attack_audio")]
        public AudioClip AttackAudio;
        [FormerlySerializedAs("damage_audio")]
        public AudioClip DamageAudio;

        [FormerlySerializedAs("deckbuilding")]
        [Header("Availability")]
        public bool Deckbuilding = false;
        [FormerlySerializedAs("cost")]
        public int Cost = 100;
        [FormerlySerializedAs("packs")]
        public PackData[] Packs;

        public static readonly List<CardData> CardList = new List<CardData>();                              //Faster access in loops
        public static readonly Dictionary<string, CardData> CardDict = new Dictionary<string, CardData>(); //Faster access in Get(id)

        public static void Load(string folder = "")
        {
            if (CardList.Count == 0)
            {
                CardList.AddRange(Resources.LoadAll<CardData>(folder));

                foreach (CardData card in CardList)
                    CardDict.Add(card.ID, card);
            }
        }

        public Sprite GetBoardArt(VariantData variant)
        {
            return ArtBoard;
        }

        public Sprite GetFullArt(VariantData variant)
        {
            return ArtFull;
        }

        public string GetTitle()
        {
            return Title;
        }

        public string GetText()
        {
            return Text;
        }

        public string GetDesc()
        {
            return Desc;
        }

        public string GetTypeId() => Type switch
        {
            CardType.Hero => "hero",
            CardType.Character => "character",
            CardType.Artifact => "artifact",
            CardType.Spell => "spell",
            CardType.Secret => "secret",
            CardType.Equipment => "equipment",
            _ => ""
        };

        public string GetAbilitiesDesc()
        {
            return Abilities
                .Where(ability => !string.IsNullOrWhiteSpace(ability.desc))
                .Aggregate("", (current, ability) => current + ("<b>" + ability.GetTitle() + ":</b> " + ability.GetDesc(this) + "\n"));
        }

        public bool IsCharacter()
        {
            return Type == CardType.Character;
        }

        public bool IsSecret()
        {
            return Type == CardType.Secret;
        }

        public bool IsBoardCard()
        {
            return Type is CardType.Character or CardType.Artifact;
        }

        public bool IsRequireTarget()
        {
            return Type == CardType.Equipment || IsRequireTargetSpell();
        }

        public bool IsRequireTargetSpell()
        {
            return Type == CardType.Spell && HasAbility(AbilityTrigger.OnPlay, AbilityTarget.PlayTarget);
        }

        public bool IsEquipment()
        {
            return Type == CardType.Equipment;
        }

        public bool HasTrait(string trait)
        {
            return Traits.Any(t => t.id == trait);
        }

        public bool HasTrait(TraitData trait)
        {
            return trait && HasTrait(trait.id);
        }

        public bool HasStat(string trait)
        {
            return Stats != null && Stats.Any(stat => stat.trait.id == trait);

        }

        public bool HasStat(TraitData trait)
        {
            return trait != null && HasStat(trait.id);
        }

        public int GetStat(string traitID)
        {
            return null != Stats ? 
                (from stat
                     in Stats
                 where stat.trait.id == traitID
                 select stat.value)
                    .FirstOrDefault() : 
                0;

        }

        public int GetStat(TraitData trait)
        {
            return trait != null ? GetStat(trait.id) : 0;
        }

        public bool HasAbility(AbilityData tability)
        {
            return Abilities.Any(ability => ability && ability.id == tability.id);
        }

        public bool HasAbility(AbilityTrigger trigger)
        {
            return Abilities.Any(ability => ability && ability.trigger == trigger);
        }

        public bool HasAbility(AbilityTrigger trigger, AbilityTarget target)
        {
            return Abilities.Any(ability => ability && ability.trigger == trigger && ability.target == target);
        }

        public AbilityData GetAbility(AbilityTrigger trigger)
        {
            return Abilities.FirstOrDefault(ability => ability && ability.trigger == trigger);
        }

        public bool HasPack(PackData pack)
        {
            return Packs.Any(apack => apack == pack);
        }

        public static CardData Get(string id)
        {
            return id == null ? null : CardDict.GetValueOrDefault(id);
        }

        public static List<CardData> GetAllDeckbuilding()
        {
            return GetAll()
                .Where(acard => acard.Deckbuilding)
                .ToList();
        }

        public static List<CardData> GetAll(PackData pack)
        {
            return GetAll()
                .Where(acard => acard.HasPack(pack))
                .ToList();
        }

        public static List<CardData> GetAll()
        {
            return CardList;
        }
    }
}