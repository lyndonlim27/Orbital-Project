using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if GAMEMODULES
using InfinityPBR.Modules.Loot;
#endif

namespace InfinityPBR
{
    public abstract class CustomEditorModules<T> : CustomEditorScriptableObject<T> where T : ScriptableObject
    {
        
        public static Color inactive = new Color(0.75f, .75f, 0.75f, 1f);
        public static Color active = new Color(0.6f, 1f, 0.6f, 1f);
        public static Color active2 = new Color(0.0f, 1f, 0.0f, 1f);
        public static Color dark = new Color(0.25f, 0.25f, 0.25f, 1f);
        public static Color mixed = Color.yellow;
        public static Color red = new Color(1f, 0.25f, 0.25f, 1f);
        public static Color blue = new Color(0.25f, 0.25f, 1f, 1f);

        protected override void OnEnable()
        {
            base.OnEnable();
            
            #if GAMEMODULES
            // Do these when the editor script is enabled
            CacheItemsArray();
            CacheLootBoxArray();
            CacheLootBoxItemsArray();
            #endif
        }
        
#if GAMEMODULES
        // --------------------------------------------------------------------------------------------------------
        // Cache lists of item objects that are otherwise very time-consuming to obtain
        // --------------------------------------------------------------------------------------------------------
        private static Items[] itemsCached = new Items[] { };
        private static LootBoxItems[] lootBoxItemsCached = new LootBoxItems[] { };
        private static LootBox[] lootBoxesCached = new LootBox[] { };
        
        // These top public methods can be used from the other scripts to get the array. Will return the cached array
        // or will create the cached array if it doesn't exist yet.
        public static Items[] GetItemsArray()
        {
            return ItemsArray();
        }
        
        public static LootBoxItems[] GetLootBoxItemsArray()
        {
            return LootBoxItemsArray();
        }
        
        public static LootBox[] GetLootBoxArray()
        {
            return LootBoxArray();
        }

        public static String[] GetItemArrayNames(string itemsUid)
        {
            return GetItemArrayNames(GetItemsObject(itemsUid));
        }

        public static String[] GetItemArrayNames(Items items)
        {
            int i = 0;
            var allItems = new string[items.items.Count];
            
            foreach(Item item in items.items)
            {
                allItems[i] = item.objName;
                i++;
            }

            return allItems;
        }
        
        public static String[] GetPrefixArrayNames(string itemsUid)
        {
            return GetPrefixArrayNames(GetItemsObject(itemsUid));
        }

        public static String[] GetPrefixArrayNames(Items items)
        {
            int i = 0;
            var allItems = new string[items.prefixes.Count];
            
            foreach(ItemMod itemMod in items.prefixes)
            {
                allItems[i] = itemMod.objName;
                i++;
            }

            return allItems;
        }
        
        public static String[] GetSuffixArrayNames(string itemsUid)
        {
            return GetSuffixArrayNames(GetItemsObject(itemsUid));
        }

        public static String[] GetSuffixArrayNames(Items items)
        {
            int i = 0;
            var allItems = new string[items.suffixes.Count];
            
            foreach(ItemMod itemMod in items.suffixes)
            {
                allItems[i] = itemMod.objName;
                i++;
            }

            return allItems;
        }

        // LOOT BOX
        private void CacheLootBoxArray()
        {
            lootBoxesCached = new LootBox[] { };
            lootBoxesCached = GetLootBoxArray();
        }
        
        public static LootBox[] LootBoxArray()
        {
            if (lootBoxesCached.Length > 0) return lootBoxesCached;
            
            int i = 0;
            string[] guids1 = AssetDatabase.FindAssets("t:LootBox", null);
            var allItems = new LootBox[guids1.Length];
            
            foreach (string guid1 in guids1)
            {
                allItems[i] = AssetDatabase.LoadAssetAtPath<LootBox>(AssetDatabase.GUIDToAssetPath(guid1));
                i++;
            }

            return allItems;
        }
        
        public static string[] LootBoxArrayNames()
        {
            int i = 0;
            var allItems = new string[LootBoxArray().Length];
            
            foreach(LootBox items in LootBoxArray())
            {
                allItems[i] = items.name;
                i++;
            }

            return allItems;
        }
        
        public static LootBox GetLootBoxObject(string uid)
        {
            foreach (LootBox items in LootBoxArray())
            {
                if (items.uid == uid)
                    return items;
            }

            return default;
        }
        
        public static int GetCurrentIndexLootBox(string lootBoxUid)
        {
            int i = 0;
            foreach(LootBox items in LootBoxArray())
            {
                if (items.uid == lootBoxUid)
                    return i;
                
                i++;
            }

            return -1;
        }
        
        // LOOT BOX ITEMS
        private void CacheLootBoxItemsArray()
        {
            lootBoxItemsCached = new LootBoxItems[] { };
            lootBoxItemsCached = GetLootBoxItemsArray();
        }
        
        public static LootBoxItems[] LootBoxItemsArray()
        {
            if (lootBoxItemsCached.Length > 0) return lootBoxItemsCached;
            
            int i = 0;
            string[] guids1 = AssetDatabase.FindAssets("t:LootBoxItems", null);
            var allItems = new LootBoxItems[guids1.Length];
            
            foreach (string guid1 in guids1)
            {
                allItems[i] = AssetDatabase.LoadAssetAtPath<LootBoxItems>(AssetDatabase.GUIDToAssetPath(guid1));
                i++;
            }

            return allItems;
        }
        
        public static string[] LootBoxItemsArrayNames()
        {
            int i = 0;
            var allItems = new string[LootBoxItemsArray().Length];
            
            foreach(LootBoxItems items in LootBoxItemsArray())
            {
                allItems[i] = items.name;
                i++;
            }

            return allItems;
        }
        
        public static LootBoxItems GetLootBoxItemsObject(string uid)
        {
            foreach (LootBoxItems items in LootBoxItemsArray())
            {
                if (items.uid == uid)
                    return items;
            }

            return default;
        }
        
        public static int GetCurrentIndexLootBoxItems(string lootBoxItemsUid)
        {
            int i = 0;
            foreach(LootBoxItems items in LootBoxItemsArray())
            {
                if (items.uid == lootBoxItemsUid)
                    return i;
                
                i++;
            }

            return -1;
        }
        
        // ITEMS
        private void CacheItemsArray()
        {
            itemsCached = new Items[] { };
            itemsCached = GetItemsArray();
        }
        
        public static Items[] ItemsArray()
        {
            if (itemsCached.Length > 0) return itemsCached;
            
            int i = 0;
            string[] guids1 = AssetDatabase.FindAssets("t:Items", null);
            var allItems = new Items[guids1.Length];
            
            foreach (string guid1 in guids1)
            {
                allItems[i] = AssetDatabase.LoadAssetAtPath<Items>(AssetDatabase.GUIDToAssetPath(guid1));
                i++;
            }

            return allItems;
        }
        
        public static string[] ItemsArrayNames()
        {
            int i = 0;
            var allItems = new string[ItemsArray().Length];
            
            foreach(Items items in ItemsArray())
            {
                allItems[i] = items.name;
                i++;
            }

            return allItems;
        }
        
        public static Items GetItemsObject(string uid)
        {
            foreach (Items items in ItemsArray())
            {
                if (items.uid == uid)
                    return items;
            }

            return default;
        }
        
        public static int GetCurrentIndexItems(string itemsUid)
        {
            int i = 0;
            foreach(Items items in ItemsArray())
            {
                if (items.uid == itemsUid)
                    return i;
                
                i++;
            }

            return -1;
        }

        #endif
        /*
         * ----------------------------------------------------------------------------------------------
         * NO MODULE REQUIRED
         * ----------------------------------------------------------------------------------------------
         */

        public static bool ShowFullInspector(string scriptName)
        {
            Space();
            SetBool("Show full inspector " + scriptName,
                LeftCheck("Show full inspector", GetBool("Show full inspector " + scriptName)));
            return GetBool("Show full inspector " + scriptName);
        }
        
        public static void ShowKeyPairHeader()
        {
            StartRow();
            Label("", 25);
            Label("Key", "This is the key which you will use to look up the information attached to an " +
                         "object.", 200);
            Label("Default Value", "When new items, prefixes, and suffixes are added, they will" +
                                   "start with these default values. Changing the value here will not " +
                                   "have any effect on existing items, prefixes, or suffixes.", 100);
            EndRow();
        }

        public static void ShowKeyValueListHeader()
        {
            StartRow();
            Label("Key", 200, true);
            Label("Type", 100, true);
            Label("Value", 300, true);
            EndRow();
        }

        public static void ShowLinkToDictionaries()
        {
            StartRow();
            BackgroundColor(Color.magenta);
            
            if (Button("Dictionaries Module is required.\nClick to get the Module on the Asset Store.",  400, 50))
                OpenURL("http://legacy.infinitypbr.com/AssetLinks/DictionaryModule.html");
            
            ResetColor();
            EndRow();
        }

        public static void ShowLinkToStatAndSkills()
        {
            StartRow();
            BackgroundColor(Color.magenta);
            
            if (Button("Stats Or Skills Module is required.\nClick to get the Module on the Asset Store.",  400, 50))
                OpenURL("http://legacy.infinitypbr.com/AssetLinks/StatsOrSkillsModule.html");

            ResetColor();
            EndRow();
        }
#if GAMEMODULES
        /*
         * ----------------------------------------------------------------------------------------------
         * DICTIONARIES MODULE
         * ----------------------------------------------------------------------------------------------
         */
        public static bool ShowKeyValuePair(KeyValue keyValue, bool showedFirst, string valueType, UnityEngine.Object script, RPGData rpgData)
        {
            if (GetBool(valueType + " " + keyValue.uid))
            {
                StartRow();
                Label(showedFirst ? "" : keyValue.key, 200);
                Label(valueType, 150);
                if (valueType == "float")
                {
                    Undo.RecordObject(script, "Undo");
                    keyValue.valueFloat = Float(keyValue.valueFloat, 300);
                }
                if (valueType == "int")
                {
                    Undo.RecordObject(script, "Undo");
                    keyValue.valueInt = Int(keyValue.valueInt, 300);
                }
                if (valueType == "bool")
                {
                    Undo.RecordObject(script, "Undo");
                    keyValue.valueBool = Check(keyValue.valueBool, 300);
                }
                if (valueType == "string")
                {
                    Undo.RecordObject(script, "Undo");
                    if (keyValue.stringAsTextBox)
                    {
                        keyValue.valueString = TextArea(keyValue.valueString, 300, 50);
                    }
                    else
                    {
                        keyValue.valueString = TextField(keyValue.valueString, 300);
                    }

                }
                if (valueType == "Animation")
                {
                    Undo.RecordObject(script, "Undo");
                    keyValue.valueAnimation = Object(keyValue.valueAnimation, typeof(Animation), 300) as Animation;
                }
                if (valueType == "AudioClip")
                {
                    Undo.RecordObject(script, "Undo");
                    keyValue.valueAudioClip = Object(keyValue.valueAudioClip, typeof(AudioClip), 300) as AudioClip;
                }
                if (valueType == "Texture2D")
                {
                    Undo.RecordObject(script, "Undo");
                    keyValue.valueTexture2D = Object(keyValue.valueTexture2D, typeof(Texture2D), 300) as Texture2D;
                }
                if (valueType == "Sprite")
                {
                    Undo.RecordObject(script, "Undo");
                    keyValue.valueSprite = Object(keyValue.valueSprite, typeof(Sprite), 300) as Sprite;
                }
                if (valueType == "Prefab")
                {
                    Undo.RecordObject(script, "Undo");
                    keyValue.valuePrefab = Object(keyValue.valuePrefab, typeof(GameObject), 300) as GameObject;
                }
                if (valueType == "Color")
                {
                    Undo.RecordObject(script, "Undo");
                    keyValue.valueColor = ColorField(keyValue.valueColor, 300);
                }
                if (valueType == "Vector3")
                {
                    Undo.RecordObject(script, "Undo");
                    keyValue.valueVector3 = Vector3Field(keyValue.valueVector3, 300);
                }
                if (valueType == "Vector2")
                {
                    Undo.RecordObject(script, "Undo");
                    keyValue.valueVector2 = Vector2Field(keyValue.valueVector2, 300);
                }
                
                // Only show these if rpgData is assigned!
                if (valueType == "LearnedStatOrSkill")
                {
                    if (rpgData == null)
                    {
                        Label("RPGData object must be assigned.", 300);
                    }
                    else
                    {
                        Undo.RecordObject(script, "Undo");
                        int currentIndex = GetCurrentIndexStatOrSkill(keyValue.valueLearnedStatOrSkill, rpgData, true);
                        int newIndex = Popup(currentIndex, StatOrSkillArray(rpgData, true), 200);
                        if (newIndex != currentIndex)
                        {
                            keyValue.valueLearnedStatOrSkill = GetStatOrSkillByIndex(rpgData, newIndex, true).uid;
                        }
                        Label("uid: " + keyValue.valueLearnedStatOrSkill);
                    }
                }
                if (valueType == "StatOrSkill")
                {
                    if (rpgData == null)
                    {
                        Label("RPGData object must be assigned.", 300);
                    }
                    else
                    {
                        Undo.RecordObject(script, "Undo");
                        int currentIndex = GetCurrentIndexStatOrSkill(keyValue.valueStatOrSkill, rpgData);
                        int newIndex = Popup(currentIndex, StatOrSkillArray(rpgData), 200);
                        if (newIndex != currentIndex)
                        {
                            keyValue.valueStatOrSkill = GetStatOrSkillByIndex(rpgData, newIndex).uid;
                        }
                        Label("uid: " + keyValue.valueStatOrSkill);
                    }
                }
                if (valueType == "Item")
                {
                    Undo.RecordObject(script, "Undo");
                    // Items parent object
                    int currentParentIndex = GetCurrentIndexItems(keyValue.valueItems);
                    int newParentIndex = Popup(currentParentIndex, ItemsArrayNames(), 100);
                    if (newParentIndex != currentParentIndex)
                    {
                        if (String.IsNullOrWhiteSpace(ItemsArray()[newParentIndex].uid))
                            Debug.LogWarning("Warning: The selected Items object doesn't have a uid. Load the object to set it automatically.");
                        keyValue.valueItems = ItemsArray()[newParentIndex].uid; // Set new Items uid
                        keyValue.valueItem = ""; // Reset this
                    }

                    if (!String.IsNullOrWhiteSpace(keyValue.valueItems))
                    {
                        Items items = GetItemsObject(keyValue.valueItems);
                        
                        // Item object
                        int currentIndex = GetCurrentIndexItem(keyValue.valueItem, items);
                        int newIndex = Popup(currentIndex, ItemArray(items), 150);
                        if (newIndex != currentIndex)
                        {
                            keyValue.valueItem = GetItemByIndex(items, newIndex).uid;
                        }
                        Label("uid: " + keyValue.valueItem);
                    }
                }
                
                
                EndRow();
                showedFirst = true;
            }

            return showedFirst;
        }



        /*
         * TO DO: These can all be moved to their parent classes -- Items to Items, StatsAndSkills to RPGData etc
         */
        private static int GetCurrentIndexItem(string itemUid, Items items)
        {
            int i = 0;
            foreach(Item item in items.items)
            {
                if (item.uid == itemUid)
                    return i;
                
                i++;
            }

            return -1;
        }

        private static string[] ItemArray(Items items)
        {
            List<String> names = new List<string>();
            foreach(Item item in items.items)
            {
                names.Add(item.fullName);
            }

            return names.ToArray();
        }

        private static Item GetItemByIndex(Items items, int index)
        {
            int i = 0;
            foreach(Item item in items.items)
            {
                if (i == index)
                    return item;
                
                i++;
            }

            return default;
        }

        private static int GetCurrentIndexStatOrSkill(string statOrSkillUid, RPGData rpgData, bool canBeTrainedOnly = false)
        {
            int i = 0;
            foreach(StatOrSkill ss in rpgData.statOrSkills.statOrSkill)
            {
                if (canBeTrainedOnly && !ss.canBeTrained) continue;
                
                if (ss.uid == statOrSkillUid)
                    return i;
                
                i++;
            }

            return -1;
        }

        private static string[] StatOrSkillArray(RPGData rpgData, bool canBeTrainedOnly = false)
        {
            List<String> names = new List<string>();
            foreach(StatOrSkill ss in rpgData.statOrSkills.statOrSkill)
            {
                if (canBeTrainedOnly && !ss.canBeTrained) continue;
                
                names.Add(ss.name);
            }

            return names.ToArray();
        }

        private static StatOrSkill GetStatOrSkillByIndex(RPGData rpgData, int index, bool canBeTrainedOnly = false)
        {
            int i = 0;
            foreach(StatOrSkill ss in rpgData.statOrSkills.statOrSkill)
            {
                if (canBeTrainedOnly && !ss.canBeTrained) continue;

                if (i == index)
                    return ss;
                
                i++;
            }

            return default;
        }
        
        public static KeyValue ShowAddNewKeyValuePair(Dictionary dictionary)
        {
            KeyValue addToAllDictionaries = null;
            if (String.IsNullOrEmpty(GetString("New KeyValue " + dictionary.uid)))
            {
                SetString("New KeyValue " + dictionary.uid, "New keyValue");
            }
            BackgroundColor(mixed);
            StartVerticalBox();
            Label("Add new keyvalue", true);
            StartRow();
            SetString("New KeyValue " + dictionary.uid, TextField(GetString("New KeyValue " + dictionary.uid), 200));
            if (Button("Add", 100))
            {
                var newKey = GetString("New KeyValue " + dictionary.uid).Trim();
                if (dictionary.HasKey(newKey))
                    Log("There is already a Key named " + newKey);
                else
                {
                    KeyValue newKeyValue = dictionary.AddNewKeyValue(newKey);
                    addToAllDictionaries = newKeyValue;
                }
            }

            EndRow();
            EndVerticalBox();
            ResetColor();

            return addToAllDictionaries;
        }

        public static void ShowDictionaryHelpBox()
        {
            EditorGUILayout.HelpBox("\"Dictionaries\" in this context are not exactly the same as other Dictionaries that you " +
                                    "may be used to. These are key/value lists which can be serialized and are intended to be " +
                                    "used in a similar fashion to Dictionaries.\n\n" +
                                    "This will allow you to add your own custom key/value pairs. Anything added here will be added " +
                                    "to all of the Stat or Skills.\n\nSet keys here, and manage values on individual " +
                                    "Stat or Skill objects.", MessageType.Info);
        }
        
        public static void SetDictionaryEditorPrefs(Dictionary dictionary)
        {
            foreach (KeyValue keyValue in dictionary.keyValues)
            {
                if (!HasKey("float " + keyValue.uid))
                    SetBool("float " + keyValue.uid, false);
                if (!HasKey("int " + keyValue.uid))
                    SetBool("int " + keyValue.uid, false);
                if (!HasKey("bool " + keyValue.uid))
                    SetBool("bool " + keyValue.uid, false);
                if (!HasKey("string " + keyValue.uid))
                    SetBool("string " + keyValue.uid, false);
                if (!HasKey("Animation " + keyValue.uid))
                    SetBool("Animation " + keyValue.uid, false);
                if (!HasKey("Texture2D " + keyValue.uid))
                    SetBool("Texture2D " + keyValue.uid, false);
                if (!HasKey("Sprite " + keyValue.uid))
                    SetBool("Sprite " + keyValue.uid, false);
                if (!HasKey("AudioClip " + keyValue.uid))
                    SetBool("AudioClip " + keyValue.uid, false);
                if (!HasKey("Prefab " + keyValue.uid))
                    SetBool("Prefab " + keyValue.uid, false);
                if (!HasKey("Vector3 " + keyValue.uid))
                    SetBool("Vector3 " + keyValue.uid, false);
                if (!HasKey("Vector2 " + keyValue.uid)) 
                    SetBool("Vector2 " + keyValue.uid, false);
                if (!HasKey("Color " + keyValue.uid))
                    SetBool("Color " + keyValue.uid, false);
                if (!HasKey("LearnedStatOrSkill " + keyValue.uid)) 
                    SetBool("LearnedStatOrSkill " + keyValue.uid, false);
                if (!HasKey("StatOrSkill " + keyValue.uid))
                    SetBool("StatOrSkill " + keyValue.uid, false);
                if (!HasKey("Item " + keyValue.uid))
                    SetBool("Item " + keyValue.uid, false);
            }
        }
        
        public static void ShowDictionary(Dictionary dictionary, List<Dictionary> dictionaries, UnityEngine.Object Item, RPGData rpgData = null)
        {
            Space();
            StartVerticalBox();
            AddToAllDictionaries(ShowAddNewKeyValuePair(dictionary), dictionaries, Item);
            Space();

            ShowKeyPairHeader();
            
            if (dictionary.keyValues == null)
            {
                EndVerticalBox();
                return;
            }

            for (int i = 0; i < dictionary.keyValues.Count; i++)
            {
                KeyValue keyValue = dictionary.keyValues[i];

                ShowDictionaryFloat(keyValue, dictionary, dictionaries, Item); // Float (And first row)
                ShowDictionaryInt(keyValue, dictionary, dictionaries, Item); // This row includes the "Close" X button
                ShowDictionaryBool(keyValue, Item);
                ShowDictionaryString(keyValue, Item);
                ShowDictionaryAnimation(keyValue, Item);
                ShowDictionaryTexture2D(keyValue, Item);
                ShowDictionarySprite(keyValue, Item);
                ShowDictionaryAudioClip(keyValue, Item);
                ShowDictionaryPrefab(keyValue, Item);
                ShowDictionaryColor(keyValue, Item);
                ShowDictionaryVector3(keyValue, Item);
                ShowDictionaryVector2(keyValue, Item);
                // Only show these if rpgData is assigned!
                if (rpgData != null)
                { 
                    ShowDictionaryLearnedStatOrSkills(Item, keyValue);
                    ShowDictionaryStatOrSkills(Item, keyValue);
                }
                ShowDictionaryItem(Item, keyValue);
                Space();

            }
            EndVerticalBox();
        }
        
        public static void ShowDictionaryItem(UnityEngine.Object items, KeyValue keyValue)
        {
            if (!GetBool("Expand " + keyValue.uid))
                return;

            StartRow();
            Label("", 25);
            Label("", 200);
            BackgroundColor(GetBool("Item " + keyValue.uid) ? Color.white : dark);
            if (Button("Item",
                "Toggle whether this type is used for this keyValue pair", 200))
            {
                Undo.RecordObject(items, "Undo");
                SetBool("Item " + keyValue.uid, !GetBool("Item " + keyValue.uid));
            }

            ResetColor();

            EndRow();
        }
        
        public static void ShowDictionaryLearnedStatOrSkills(UnityEngine.Object items, KeyValue keyValue)
        {
            if (!GetBool("Expand " + keyValue.uid))
                return;

            StartRow();
            Label("", 25);
            Label("", 200);
            BackgroundColor(GetBool("LearnedStatOrSkill " + keyValue.uid) ? Color.white : dark);
            if (Button("Learned StatOrSkill",
                "Toggle whether this type is used for this keyValue pair", 200))
            {
                Undo.RecordObject(items, "Undo");
                SetBool("LearnedStatOrSkill " + keyValue.uid, !GetBool("LearnedStatOrSkill " + keyValue.uid));
            }

            ResetColor();

            EndRow();
        }
        
        public static void ShowDictionaryStatOrSkills(UnityEngine.Object items, KeyValue keyValue)
        {
            if (!GetBool("Expand " + keyValue.uid))
                return;

            StartRow();
            Label("", 25);
            Label("", 200);
            BackgroundColor(GetBool("StatOrSkill " + keyValue.uid) ? Color.white : dark);
            if (Button("StatOrSkill",
                "Toggle whether this type is used for this keyValue pair", 200))
            {
                Undo.RecordObject(items, "Undo");
                SetBool("StatOrSkill " + keyValue.uid, !GetBool("StatOrSkill " + keyValue.uid));
            }

            ResetColor();

            EndRow();
        }
        
        public static void AddToAllDictionaries(KeyValue newKeyValue, List<Dictionary> dictionaries, UnityEngine.Object Item)
        {
            if (newKeyValue == null)
                return;

            if (dictionaries == null)
                return;
            
            foreach (Dictionary d in dictionaries)
            {
                Undo.RecordObject(Item, "Undo");
                KeyValue newKeyValueItem = d.AddNewKeyValue(newKeyValue.key);
                newKeyValueItem.uid = newKeyValue.uid;

                // Set Dirty
                EditorUtility.SetDirty(Item);
            }
        }
        
        public static void ShowDictionaryVector2(KeyValue keyValue, UnityEngine.Object Item)
        {
            if (!GetBool("Expand " + keyValue.uid))
                return;
            
            StartRow();
            Label("", 25);
            Label("", 200);
            BackgroundColor(GetBool("Vector2 " + keyValue.uid) ? Color.white : dark);
            if (Button("Vector2", "Toggle whether this type is used for this keyValue pair", 200))
            {
                Undo.RecordObject(Item, "Undo");
                SetBool("Vector2 " + keyValue.uid, !GetBool("Vector2 " + keyValue.uid));
            }

            ResetColor();

            EndRow();
        }

        public static void ShowDictionaryVector3(KeyValue keyValue, UnityEngine.Object Item)
        {
            if (!GetBool("Expand " + keyValue.uid))
                return;
            
            StartRow();
            Label("", 25);
            Label("", 200);
            BackgroundColor(GetBool("Vector3 " + keyValue.uid) ? Color.white : dark);
            if (Button("Vector3", "Toggle whether this type is used for this keyValue pair", 200))
            {
                Undo.RecordObject(Item, "Undo");
                SetBool("Vector3 " + keyValue.uid, !GetBool("Vector3 " + keyValue.uid));
            }

            ResetColor();

            EndRow();
        }

        public static void ShowDictionaryColor(KeyValue keyValue, UnityEngine.Object Item)
        {
            if (!GetBool("Expand " + keyValue.uid))
                return;
            
            StartRow();
            Label("", 25);
            Label("", 200);
            BackgroundColor(GetBool("Color " + keyValue.uid) ? Color.white : dark);
            if (Button("Color",
                "Toggle whether this type is used for this keyValue pair", 200))
            {
                Undo.RecordObject(Item, "Undo");
                SetBool("Color " + keyValue.uid, !GetBool("Color " + keyValue.uid));
            }

            ResetColor();

            EndRow();
        }

        public static void ShowDictionaryPrefab(KeyValue keyValue, UnityEngine.Object Item)
        {
            if (!GetBool("Expand " + keyValue.uid))
                return;
            
            StartRow();
            Label("", 25);
            Label("", 200);
            BackgroundColor(GetBool("Prefab " + keyValue.uid) ? Color.white : dark);
            if (Button("Prefab",
                "Toggle whether this type is used for this keyValue pair", 200))
            {
                Undo.RecordObject(Item, "Undo");
                SetBool("Prefab " + keyValue.uid, !GetBool("Prefab " + keyValue.uid));
            }

            ResetColor();

            EndRow();
        }

        public static void ShowDictionaryAudioClip(KeyValue keyValue, UnityEngine.Object Item)
        {
            if (!GetBool("Expand " + keyValue.uid))
                return;
            
            StartRow();
            Label("", 25);
            Label("", 200);
            BackgroundColor(GetBool("AudioClip " + keyValue.uid) ? Color.white : dark);
            if (Button("AudioClip", "Toggle whether this type is used for this keyValue pair", 200))
            {
                Undo.RecordObject(Item, "Undo");
                SetBool("AudioClip " + keyValue.uid,
                    !GetBool("AudioClip " + keyValue.uid));
            }

            ResetColor();

            EndRow();
        }

        public static void ShowDictionaryTexture2D(KeyValue keyValue, UnityEngine.Object Item)
        {
            if (!GetBool("Expand " + keyValue.uid))
                return;
            
            StartRow();
            Label("", 25);
            Label("", 200);
            BackgroundColor(GetBool("Texture2D " + keyValue.uid) ? Color.white : dark);
            if (Button("Texture2D", "Toggle whether this type is used for this keyValue pair", 200))
            {
                Undo.RecordObject(Item, "Undo");
                SetBool("Texture2D " + keyValue.uid,
                    !GetBool("Texture2D " + keyValue.uid));
            }

            ResetColor();

            EndRow();
        }
        
        public static void ShowDictionarySprite(KeyValue keyValue, UnityEngine.Object Item)
        {
            if (!GetBool("Expand " + keyValue.uid))
                return;
            
            StartRow();
            Label("", 25);
            Label("", 200);
            BackgroundColor(GetBool("Sprite " + keyValue.uid) ? Color.white : dark);
            if (Button("Sprite", "Toggle whether this type is used for this keyValue pair", 200))
            {
                Undo.RecordObject(Item, "Undo");
                SetBool("Sprite " + keyValue.uid,
                    !GetBool("Sprite " + keyValue.uid));
            }

            ResetColor();

            EndRow();
        }

        public static void ShowDictionaryAnimation(KeyValue keyValue, UnityEngine.Object Item)
        {
            if (!GetBool("Expand " + keyValue.uid))
                return;
            
            StartRow();
            Label("", 25);
            Label("", 200);
            BackgroundColor(GetBool("Animation " + keyValue.uid) ? Color.white : dark);
            if (Button("Animation", "Toggle whether this type is used for this keyValue pair", 200))
            {
                Undo.RecordObject(Item, "Undo");
                SetBool("Animation " + keyValue.uid, !GetBool("Animation " + keyValue.uid));
            }

            ResetColor();

            EndRow();
        }

        public static void ShowDictionaryString(KeyValue keyValue, UnityEngine.Object Item)
        {
            if (!GetBool("Expand " + keyValue.uid))
                return;
            
            StartRow();
            Label("", 25);
            Label("", 200);

            BackgroundColor(GetBool("string " + keyValue.uid) ? Color.white : dark);
            if (Button("string", "Toggle whether this type is used for this keyValue pair", 200))
            {
                Undo.RecordObject(Item, "Undo");
                SetBool("string " + keyValue.uid, !GetBool("string " + keyValue.uid));
            }

            ResetColor();

            EndRow();
        }

        public static void ShowDictionaryBool(KeyValue keyValue, UnityEngine.Object Item)
        {
            if (!GetBool("Expand " + keyValue.uid))
                return;
            
            StartRow();
            Label("", 25);
            Label("", 200);

            BackgroundColor(GetBool("bool " + keyValue.uid) ? Color.white : dark);
            if (Button("bool", "Toggle whether this type is used for this keyValue pair", 200))
            {
                Undo.RecordObject(Item, "Undo");
                SetBool("bool " + keyValue.uid, !GetBool("bool " + keyValue.uid));
            }

            ResetColor();

            EndRow();
        }

        public static void ShowDictionaryInt(KeyValue keyValue, Dictionary dictionary, List<Dictionary> dictionaries, UnityEngine.Object Item)
        {
            if (!GetBool("Expand " + keyValue.uid))
                return;
            
            StartRow();
            BackgroundColor(red);
            if (Button("X", 25))
            {
                if (Dialog("Delete Custom Key/Value?", "Do you really want to delete " + keyValue.key +
                                                       "? This will remove it from all of the items, prefixes, and suffixes."))
                {
                    RemoveFromAllDictionaries(keyValue.key, dictionary, dictionaries, Item);
                    ExitGUI();
                }
            }

            ResetColor();
            Label("", 200);

            BackgroundColor(GetBool("int " + keyValue.uid) ? Color.white : dark);
            if (Button("int", "Toggle whether this type is used for this keyValue pair", 200))
            {
                Undo.RecordObject(Item, "Undo");
                SetBool("int " + keyValue.uid, !GetBool("int " + keyValue.uid));
            }

            ResetColor();

            EndRow();
        }

        public static void ShowDictionaryFloat(KeyValue keyValue, Dictionary dictionary, List<Dictionary> dictionaries, UnityEngine.Object Item)
        {
            // WARNING THIS IS ALSO THE FIRST ROW!!!
            
            StartRow();
            if (Button(GetBool("Expand " + keyValue.uid) ? "-" : "+", 25))
            {
                Undo.RecordObject(Item, "Undo");
                SetBool("Expand " + keyValue.uid, !GetBool("Expand " + keyValue.uid));
            }

            string tempNameDictionary = keyValue.key;
            keyValue.key = DelayedText(keyValue.key, 200);
            if (tempNameDictionary != keyValue.key)
            {
                if (!ChangeNameDictionary(tempNameDictionary, keyValue.key, dictionary, dictionaries, Item))
                    keyValue.key = tempNameDictionary;
            }

            if (GetBool("Expand " + keyValue.uid))
            {
                BackgroundColor(GetBool("float " + keyValue.uid) ? Color.white : dark);
                if (Button("float", "Toggle whether this type is used for this keyValue pair", 200))
                {
                    Undo.RecordObject(Item, "Undo");
                    SetBool("float " + keyValue.uid, !GetBool("float " + keyValue.uid));
                }

                ResetColor();
            }

            EndRow();
        }
        
        private static void RemoveFromAllDictionaries(string key, Dictionary dictionary, List<Dictionary> dictionaries, UnityEngine.Object Item)
        {
            dictionary.Remove(key);
            
            if (dictionaries == null)
                return;
            
            foreach (Dictionary d in dictionaries)
            {
                Undo.RecordObject(Item, "Undo");
                d.Remove(key);
            }
        }
        
        private static bool ChangeNameDictionary(string oldName, string newName, Dictionary dictionary, List<Dictionary> dictionaries, UnityEngine.Object Item)
        {
            if (dictionary.CountKeys(newName) > 1)
                return false;

            if (dictionaries == null)
                return true;
            
            for (int i = 0; i < dictionaries.Count; i++)
            {
                Undo.RecordObject(Item, "Undo");
                dictionaries[i].RenameKey(oldName, newName);
            }

            return true;
        }

        public static void MatchParentDictionary(Dictionary child, Dictionary parent)
        {
            // Remove any keyValues that exist here but do not exist on the main statsOrSkills object.
            foreach (KeyValue keyValue in child.keyValues)
            {
                if (!parent.HasKeyValue(keyValue.uid))
                {
                    child.Remove(keyValue.key);
                    ExitGUI();
                }
            }
        }

        public static void ShowDictionaryKeyValues(Dictionary dictionary, UnityEngine.Object item, RPGData rpgData = null)
        {
            foreach (KeyValue keyValue in dictionary.keyValues)
            {
                bool showedFirst = false;

                showedFirst = ShowKeyValuePair(keyValue, showedFirst, "float", item, rpgData);
                showedFirst = ShowKeyValuePair(keyValue, showedFirst, "int", item, rpgData);
                showedFirst = ShowKeyValuePair(keyValue, showedFirst, "bool", item, rpgData);
                showedFirst = ShowKeyValuePair(keyValue, showedFirst, "string", item, rpgData);
                showedFirst = ShowKeyValuePair(keyValue, showedFirst, "Animation", item, rpgData);
                showedFirst = ShowKeyValuePair(keyValue, showedFirst, "AudioClip", item, rpgData);
                showedFirst = ShowKeyValuePair(keyValue, showedFirst, "Texture2D", item, rpgData);
                showedFirst = ShowKeyValuePair(keyValue, showedFirst, "Sprite", item, rpgData);
                showedFirst = ShowKeyValuePair(keyValue, showedFirst, "Prefab", item, rpgData);
                showedFirst = ShowKeyValuePair(keyValue, showedFirst, "Color", item, rpgData);
                showedFirst = ShowKeyValuePair(keyValue, showedFirst, "Vector3", item, rpgData);
                showedFirst = ShowKeyValuePair(keyValue, showedFirst, "Vector2", item, rpgData);
                showedFirst = ShowKeyValuePair(keyValue, showedFirst, "LearnedStatOrSkill", item, rpgData);
                showedFirst = ShowKeyValuePair(keyValue, showedFirst, "StatOrSkill", item, rpgData);
                showedFirst = ShowKeyValuePair(keyValue, showedFirst, "Item", item, rpgData);
            }
        }

        /*
         * ----------------------------------------------------------------------------------------------
         * STATS AND SKILLS MODULE
         * ----------------------------------------------------------------------------------------------
         */
        // Checks to make sure RPGData is populated for the StatsAndSkills module        
        public static RPGData CheckRPGData(RPGData rpgData)
        {
            if (rpgData != null) return rpgData;
            BackgroundColor(red);
            ContentColor(red);
            Label("Warning: RPGData is not assigned. Please link your RPGData object here.", 
                "Somehow the RPGData object became missing. Without this, errors will occur. Please " +
                "select your RPGData object here.", true);
            rpgData = Object(rpgData, typeof(RPGData)) as RPGData;
            ResetColor();
            return rpgData;
        }
#endif
    }
}
