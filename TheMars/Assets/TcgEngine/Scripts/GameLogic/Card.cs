using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace TcgEngine
{
    //Represent the current state of a card during the game (data only)

    [System.Serializable]
    public class Card
    {
        [FormerlySerializedAs("card_id")]
        public string CardID;
        [FormerlySerializedAs("uid")]
        public string Uid;
        [FormerlySerializedAs("player_id")]
        public int PlayerID;
        [FormerlySerializedAs("variant_id")]
        public string VariantID;

        [FormerlySerializedAs("slot")]
        public Slot Slot;
        [FormerlySerializedAs("exhausted")]
        public bool Exhausted;
        [FormerlySerializedAs("damage")]
        public int Damage = 0;

        public int mana = 0;
        public int attack = 0;
        public int hp = 0;

        public int mana_ongoing = 0;
        public int attack_ongoing = 0;
        public int hp_ongoing = 0;

        public string equipped_uid = null;

        public List<CardTrait> traits = new List<CardTrait>();
        public List<CardTrait> ongoing_traits = new List<CardTrait>();

        public List<CardStatus> status = new List<CardStatus>();
        public List<CardStatus> ongoing_status = new List<CardStatus>();

        public List<string> abilities = new List<string>();
        public List<string> abilities_ongoing = new List<string>();

        [System.NonSerialized]
        private int hash = 0;
        [System.NonSerialized]
        private CardData data = null;
        [System.NonSerialized]
        private VariantData vdata = null;
        [System.NonSerialized]
        private List<AbilityData> abilities_data = null;

        public Card(string card_id, string uid, int player_id)
        {
            this.CardID = card_id;
            this.Uid = uid;
            this.PlayerID = player_id;
        }

        public virtual void Refresh() { Exhausted = false; }
        public virtual void ClearOngoing()
        {
            ongoing_status.Clear();
            ongoing_traits.Clear();
            ClearOngoingAbility();
            attack_ongoing = 0;
            hp_ongoing = 0;
            mana_ongoing = 0;
        }

        public virtual void Clear()
        {
            ClearOngoing();
            Refresh();
            Damage = 0;
            status.Clear();
            SetCard(CardData, VariantData); //Reset to initial stats
            equipped_uid = null;
        }

        public virtual int GetAttack() { return Mathf.Max(attack + attack_ongoing, 0); }
        public virtual int GetHp() { return Mathf.Max(hp + hp_ongoing - Damage, 0); }
        public virtual int GetHpMax() { return Mathf.Max(hp + hp_ongoing, 0); }
        public virtual int GetMana() { return Mathf.Max(mana + mana_ongoing, 0); }

        public virtual void SetCard(CardData icard, VariantData cvariant)
        {
            data = icard;
            CardID = icard.ID;
            VariantID = cvariant.id;
            attack = icard.Attack;
            hp = icard.Hp;
            mana = icard.Mana;
            SetTraits(icard);
            SetAbilities(icard);
        }

        public void SetTraits(CardData icard)
        {
            traits.Clear();
            foreach (TraitData trait in icard.Traits)
                SetTrait(trait.id, 0);
            if (icard.Stats != null)
            {
                foreach (TraitStat stat in icard.Stats)
                    SetTrait(stat.trait.id, stat.value);
            }
        }

        public void SetAbilities(CardData icard)
        {
            abilities.Clear();
            abilities_ongoing.Clear();
            abilities_data?.Clear();
            foreach (AbilityData ability in icard.Abilities)
                AddAbility(ability);
        }

        //------ Custom Traits/Stats ---------

        public void SetTrait(string id, int value)
        {
            CardTrait trait = GetTrait(id);
            if (trait != null)
            {
                trait.value = value;
            }
            else
            {
                trait = new CardTrait(id, value);
                traits.Add(trait);
            }
        }

        public void AddTrait(string id, int value)
        {
            CardTrait trait = GetTrait(id);
            if (trait != null)
                trait.value += value;
            else
                SetTrait(id, value);
        }

        public void AddOngoingTrait(string id, int value)
        {
            CardTrait trait = GetOngoingTrait(id);
            if (trait != null)
            {
                trait.value += value;
            }
            else
            {
                trait = new CardTrait(id, value);
                ongoing_traits.Add(trait);
            }
        }

        public void RemoveTrait(string id)
        {
            for (int i = traits.Count - 1; i >= 0; i--)
            {
                if (traits[i].id == id)
                    traits.RemoveAt(i);
            }
        }

        public CardTrait GetTrait(string id)
        {
            return traits.FirstOrDefault(trait => trait.id == id);
        }

        public CardTrait GetOngoingTrait(string id)
        {
            return ongoing_traits.FirstOrDefault(trait => trait.id == id);
        }

        public int GetTraitValue(TraitData trait)
        {
            return trait ? GetTraitValue(trait.id) : 0;
        }

        public virtual int GetTraitValue(string id)
        {
            int val = 0;
            CardTrait stat1 = GetTrait(id);
            CardTrait stat2 = GetOngoingTrait(id);
            if (stat1 != null)
                val += stat1.value;
            if (stat2 != null)
                val += stat2.value;
            return val;
        }

        public bool HasTrait(TraitData trait)
        {
            return trait && HasTrait(trait.id);
        }

        public bool HasTrait(string id)
        {
            return GetTrait(id) != null || GetOngoingTrait(id) != null;
        }

        public List<CardTrait> GetAllTraits()
        {
            List<CardTrait> all_traits = new List<CardTrait>();
            all_traits.AddRange(traits);
            all_traits.AddRange(ongoing_traits);
            return all_traits;
        }

        //Alternate names since traits/stats are stored in same var
        public void SetStat(string id, int value) => SetTrait(id, value);
        public void AddStat(string id, int value) => AddTrait(id, value);
        public void AddOngoingStat(string id, int value) => AddOngoingTrait(id, value);
        public void RemoveStat(string id) => RemoveTrait(id);
        public int GetStatValue(TraitData trait) => GetTraitValue(trait);
        public int GetStatValue(string id) => GetTraitValue(id);
        public bool HasStat(TraitData trait) => HasTrait(trait);
        public bool HasStat(string id) => HasTrait(id);
        public List<CardTrait> GetAllStats() => GetAllTraits();

        //------  Status Effects ---------

        public void AddStatus(StatusData status, int value, int duration)
        {
            if (status)
                AddStatus(status.effect, value, duration);
        }

        public void AddOngoingStatus(StatusData status, int value)
        {
            if (status)
                AddOngoingStatus(status.effect, value);
        }

        public void AddStatus(StatusType type, int value, int duration)
        {
            if (type != StatusType.None)
            {
                CardStatus status = GetStatus(type);
                if (status == null)
                {
                    status = new CardStatus(type, value, duration);
                    this.status.Add(status);
                }
                else
                {
                    status.value += value;
                    status.duration = Mathf.Max(status.duration, duration);
                    status.permanent = status.permanent || duration == 0;
                }
            }
        }

        public void AddOngoingStatus(StatusType type, int value)
        {
            if (type != StatusType.None)
            {
                CardStatus status = GetOngoingStatus(type);
                if (status == null)
                {
                    status = new CardStatus(type, value, 0);
                    ongoing_status.Add(status);
                }
                else
                {
                    status.value += value;
                }
            }
        }

        public void RemoveStatus(StatusType type)
        {
            for (int i = status.Count - 1; i >= 0; i--)
            {
                if (status[i].type == type)
                    status.RemoveAt(i);
            }
        }

        public List<CardStatus> GetAllStatus()
        {
            List<CardStatus> all_status = new List<CardStatus>();
            all_status.AddRange(status);
            all_status.AddRange(ongoing_status);
            return all_status;
        }

        public bool HasStatus(StatusType type)
        {
            return GetStatus(type) != null || GetOngoingStatus(type) != null;
        }

        public CardStatus GetStatus(StatusType type)
        {
            return status.FirstOrDefault(status => status.type == type);
        }

        public CardStatus GetOngoingStatus(StatusType type)
        {
            return ongoing_status.FirstOrDefault(status => status.type == type);
        }

        public virtual int GetStatusValue(StatusType type)
        {
            CardStatus status1 = GetStatus(type);
            CardStatus status2 = GetOngoingStatus(type);
            int v1 = status1?.value ?? 0;
            int v2 = status2?.value ?? 0;
            return v1 + v2;
        }

        public virtual void ReduceStatusDurations()
        {
            for (int i = status.Count - 1; i >= 0; i--)
            {
                if (!status[i].permanent)
                {
                    status[i].duration -= 1;
                    if (status[i].duration <= 0)
                        status.RemoveAt(i);
                }
            }
        }

        //----- Abilities ------------

        public void AddAbility(AbilityData ability)
        {
            abilities.Add(ability.id);
            abilities_data?.Add(ability);
        }

        public void RemoveAbility(AbilityData ability)
        {
            abilities.Remove(ability.id);
            abilities_data?.Remove(ability);
        }

        public void AddOngoingAbility(AbilityData ability)
        {
            if (!abilities_ongoing.Contains(ability.id) && !abilities.Contains(ability.id))
            {
                abilities_ongoing.Add(ability.id);
                abilities_data?.Add(ability);
            }
        }

        public void ClearOngoingAbility()
        {
            if (abilities_data != null)
            {
                for (int i = abilities_data.Count - 1; i >= 0; i--)
                {
                    AbilityData ability = abilities_data[i];
                    if (abilities_ongoing.Contains(ability.id))
                        abilities_data.RemoveAt(i);
                }
            }

            abilities_ongoing.Clear();
        }

        public AbilityData GetAbility(AbilityTrigger trigger)
        {
            return GetAbilities().FirstOrDefault(iability => iability.trigger == trigger);
        }

        public bool HasAbility(AbilityData ability)
        {
            return GetAbilities().Any(iability => iability.id == ability.id);
        }

        public bool HasAbility(AbilityTrigger trigger)
        {
            AbilityData iability = GetAbility(trigger);
            return iability != null;
        }

        public bool HasAbility(AbilityTrigger trigger, AbilityTarget target)
        {
            return GetAbilities().Any(iability => iability.trigger == trigger && iability.target == target);
        }

        public bool HasActiveAbility(Game data, AbilityTrigger trigger)
        {
            AbilityData iability = GetAbility(trigger);
            return iability != null && CanDoAbilities() && iability.AreTriggerConditionsMet(data, this);
        }

        public bool AreAbilityConditionsMet(AbilityTrigger ability_trigger, Game data, Card caster, Card triggerer)
        {
            return GetAbilities().Any(ability => ability && ability.trigger == ability_trigger && ability.AreTriggerConditionsMet(data, caster, triggerer));
        }

        public List<AbilityData> GetAbilities()
        {
            //Load abilities data, important to do this here since this array will be null after being sent through networking (cant serialize it)
            if (abilities_data == null)
            {
                abilities_data = new List<AbilityData>(abilities.Count + abilities_ongoing.Count);
                foreach (string t in abilities)
                    abilities_data.Add(AbilityData.Get(t));
                foreach (string t in abilities_ongoing)
                    abilities_data.Add(AbilityData.Get(t));
            }

            //Return
            return abilities_data;
        }

        //---- Action Check ---------

        public virtual bool CanAttack(bool skipCost = false)
        {
            if (HasStatus(StatusType.Paralysed))
                return false;
            return skipCost || !Exhausted;
            //no more action
        }

        public virtual bool CanMove(bool skip_cost = false)
        {
            //In demo we can move freely, since it has no effect
            //if (HasStatusEffect(StatusEffect.Paralysed))
            //   return false;
            //if (!skip_cost && exhausted)
            //    return false; //no more action
            return true;
        }

        public virtual bool CanDoActivatedAbilities()
        {
            if (HasStatus(StatusType.Paralysed))
                return false;
            return !HasStatus(StatusType.Silenced);

        }

        public virtual bool CanDoAbilities()
        {
            return !HasStatus(StatusType.Silenced);
        }

        public virtual bool CanDoAnyAction()
        {
            return CanAttack() || CanMove() || CanDoActivatedAbilities();
        }

        //----------------

        public CardData CardData
        {
            get
            {
                if (!data || data.ID != CardID)
                    data = CardData.Get(CardID); //Optimization, store for future use
                return data;
            }
        }

        public VariantData VariantData
        {
            get
            {
                if (!vdata || vdata.id != VariantID)
                    vdata = VariantData.Get(VariantID); //Optimization, store for future use
                return vdata;
            }
        }

        public CardData Data => CardData; //Alternate name

        public int Hash
        {
            get
            {
                if (hash == 0)
                    hash = Mathf.Abs(Uid.GetHashCode()); //Optimization, store for future use
                return hash;
            }
        }

        public static Card Create(CardData icard, VariantData ivariant, Player player)
        {
            return Create(icard, ivariant, player, GameTool.GenerateRandomID(11, 15));
        }

        public static Card Create(CardData icard, VariantData ivariant, Player player, string uid)
        {
            Card card = new Card(icard.ID, uid, player.player_id);
            card.SetCard(icard, ivariant);
            player.cards_all[card.Uid] = card;
            return card;
        }

        public static Card CloneNew(Card source)
        {
            Card card = new Card(source.CardID, source.Uid, source.PlayerID);
            Clone(source, card);
            return card;
        }

        //Clone all card variables into another var, used mostly by the AI when building a prediction tree
        public static void Clone(Card source, Card dest)
        {
            dest.CardID = source.CardID;
            dest.Uid = source.Uid;
            dest.PlayerID = source.PlayerID;

            dest.VariantID = source.VariantID;
            dest.Slot = source.Slot;
            dest.Exhausted = source.Exhausted;
            dest.Damage = source.Damage;

            dest.attack = source.attack;
            dest.hp = source.hp;
            dest.mana = source.mana;

            dest.mana_ongoing = source.mana_ongoing;
            dest.attack_ongoing = source.attack_ongoing;
            dest.hp_ongoing = source.hp_ongoing;

            dest.equipped_uid = source.equipped_uid;

            CardTrait.CloneList(source.traits, dest.traits);
            CardTrait.CloneList(source.ongoing_traits, dest.ongoing_traits);
            CardStatus.CloneList(source.status, dest.status);
            CardStatus.CloneList(source.ongoing_status, dest.ongoing_status);
            GameTool.CloneList(source.abilities, dest.abilities);
            GameTool.CloneList(source.abilities_ongoing, dest.abilities_ongoing);
            GameTool.CloneListRefNull(source.abilities_data, ref dest.abilities_data); //No need to deep copy since AbilityData doesn't change dynamically, its just a reference
        }

        //Clone a var that could be null
        public static void CloneNull(Card source, ref Card dest)
        {
            //Source is null
            if (source == null)
            {
                dest = null;
                return;
            }

            //Dest is null
            if (dest == null)
            {
                dest = CloneNew(source);
                return;
            }

            //Both arent null, just clone
            Clone(source, dest);
        }

        //Clone dictionary completely
        public static void CloneDict(Dictionary<string, Card> source, Dictionary<string, Card> dest)
        {
            foreach (KeyValuePair<string, Card> pair in source)
            {
                bool valid = dest.TryGetValue(pair.Key, out Card val);
                if (valid)
                    Clone(pair.Value, val);
                else
                    dest[pair.Key] = CloneNew(pair.Value);
            }
        }

        //Clone list by keeping references from ref_dict
        public static void CloneListRef(Dictionary<string, Card> ref_dict, List<Card> source, List<Card> dest)
        {
            for (int i = 0; i < source.Count; i++)
            {
                Card scard = source[i];
                bool valid = ref_dict.TryGetValue(scard.Uid, out Card rcard);
                if (valid)
                {
                    if (i < dest.Count)
                        dest[i] = rcard;
                    else
                        dest.Add(rcard);
                }
            }

            if (dest.Count > source.Count)
                dest.RemoveRange(source.Count, dest.Count - source.Count);
        }
    }

    [System.Serializable]
    public class CardStatus
    {
        public StatusType type;
        public int value;
        public int duration = 1;
        public bool permanent = true;

        [System.NonSerialized]
        private StatusData data = null;

        public CardStatus() { }

        public CardStatus(StatusType type, int value, int duration)
        {
            this.type = type;
            this.value = value;
            this.duration = duration;
            this.permanent = (duration == 0);
        }

        public StatusData StatusData
        {
            get
            {
                if (!data || data.effect != type)
                    data = StatusData.Get(type);
                return data;
            }
        }

        public StatusData Data => StatusData; //Alternate name

        public static CardStatus CloneNew(CardStatus copy)
        {
            CardStatus status = new CardStatus(copy.type, copy.value, copy.duration);
            status.permanent = copy.permanent;
            return status;
        }

        public static void Clone(CardStatus source, CardStatus dest)
        {
            dest.type = source.type;
            dest.value = source.value;
            dest.duration = source.duration;
            dest.permanent = source.permanent;
        }

        public static void CloneList(List<CardStatus> source, List<CardStatus> dest)
        {
            for (int i = 0; i < source.Count; i++)
            {
                if (i < dest.Count)
                    Clone(source[i], dest[i]);
                else
                    dest.Add(CloneNew(source[i]));
            }

            if (dest.Count > source.Count)
                dest.RemoveRange(source.Count, dest.Count - source.Count);
        }
    }

    [System.Serializable]
    public class CardTrait
    {
        public string id;
        public int value;

        [System.NonSerialized]
        private TraitData data = null;

        public CardTrait(string id, int value)
        {
            this.id = id;
            this.value = value;
        }

        public CardTrait(TraitData trait, int value)
        {
            this.id = trait.id;
            this.value = value;
        }

        public TraitData TraitData
        {
            get
            {
                if (data == null || data.id != id)
                    data = TraitData.Get(id);
                return data;
            }
        }

        public TraitData Data => TraitData; //Alternate name


        public static CardTrait CloneNew(CardTrait copy)
        {
            CardTrait status = new CardTrait(copy.id, copy.value);
            return status;
        }

        public static void Clone(CardTrait source, CardTrait dest)
        {
            dest.id = source.id;
            dest.value = source.value;
        }

        public static void CloneList(List<CardTrait> source, List<CardTrait> dest)
        {
            for (int i = 0; i < source.Count; i++)
            {
                if (i < dest.Count)
                    Clone(source[i], dest[i]);
                else
                    dest.Add(CloneNew(source[i]));
            }

            if (dest.Count > source.Count)
                dest.RemoveRange(source.Count, dest.Count - source.Count);
        }
    }
}