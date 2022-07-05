using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Object = UnityEngine.Object;

namespace InfinityPBR
{
    [CustomEditor(typeof(Voices))]
    [CanEditMultipleObjects]
    [Serializable]
    public class VoicesEditor : CustomEditorModules<Voices>
    {
        string[] buttonNames = 
        {
            "Voices",
            "Lines",
            "Emotions"
        };

        protected override void Setup()
        {
            
        }
        
        protected override void Draw()
        {
            CheckNewClips();
            SetupVoices();
            
            // When this is first created, Unity will need to reimport scripts, which takes a few seconds. This keeps
            // errors from forming, and will show a message until that is complete.
            if (Script.voices == null)
            {
                Label("Please wait, scripts should re-import shortly...");
                return;
            }

            // Display the main buttons for the panels, and then display the panel that is selected, if any.
            ShowPanelButtons();
            // Show the auto fill options for auto populating everything magically
            ShowAutoFill();
            
            if (GetBool(name + " Voices"))
                ShowVoices(Script);
            if (GetBool(name + " Lines"))
                ShowLines(Script);
            if (GetBool(name + " Emotions"))
                ShowEmotions(Script);

            if (ShowFullInspector(name)) DrawDefaultInspector();
            
            SetDirty();
        }

        private void ShowAutoFill()
        {
            if (!Script.showAutoFill) return;

            SetBool("Show Auto Fill", 
                StartFoldoutHeaderGroup(GetBool("Show Auto Fill"),
                    GetBool("Show Auto Fill")
                    ? "Hide Auto Fill" 
                    : "Show Auto Fill"));

            if (GetBool("Show Auto Fill"))
            {
                MessageBox(
                    "You can auto populate the content of this system using a naming convention" +
                    " on your AudioClips:\n\n" +
                    "[Voice Name]_[Line Name]_[Emotion Name]_[#].wav\n\n" +
                    "The 3rd and 4th section are optional.\n\n" +
                    "Voice Name: This is the name of the \"Voice\", which is often a character name.\n" +
                    "Line Name: This is the \"Line\" that is being spoken, a common line, even if the " +
                    "contents of the line are different, that all Voices may be able to speak in your" +
                    " project.\n" +
                    "(Optional) Emotion Name: If you are using multiple emotion for a line, add the " +
                    "name of the emotion here, and it will be populated properly.\n" +
                    "(Optional) #: If you have multiple clips for the same Voice/Line/Emotion combination " +
                    "add a number or other unique string here, to keep them all named something unique.", MessageType.Info);
                DisplayAutoFillBox();
                Space();
            }
            
            EndFoldoutHeaderGroup();
        }

        private void CheckNewClips()
        {
            for (int i = 0; i < Script.voices.Count; i++)
            {
                Voice voice = Script.voices[i];
                foreach (Line line in voice.lines)
                {
                    if (line.newAudioClip)
                    {
                        //Undo.RecordObject(Script, "Add New Clip");
                        line.audioClip.Add(line.newAudioClip);
                        line.newAudioClip = null;
                    }

                    foreach (Emotion emotion in line.emotions)
                    {
                        if (emotion.newAudioClip)
                        {
                            //Undo.RecordObject(Script, "Add New Clip");
                            emotion.audioClip.Add(emotion.newAudioClip);
                            emotion.newAudioClip = null;
                        }
                    }
                }
            }
        }

        private void AddAudioClipToAllLines(string lineName, AudioClip newAudioClip, bool avoidDuplicates = true)
        {
            for (int i = 0; i < Script.voices.Count; i++)
            {
                Voice voice = Script.voices[i];
                foreach (Line line in voice.lines)
                {
                    if (line.line != lineName) continue;
                    if (avoidDuplicates && line.HasAudioClip(newAudioClip)) continue;
                    line.audioClip.Add(newAudioClip);
                }
            }
        }
        
        private void AddAudioClipToAllEmotions(string emotionName, AudioClip newAudioClip, bool avoidDuplicates = true)
        {
            for (int i = 0; i < Script.voices.Count; i++)
            {
                Voice voice = Script.voices[i];
                foreach (Line line in voice.lines)
                {
                    foreach (Emotion emotion in line.emotions)
                    {
                        if (emotion.emotion != emotionName) continue;
                        if (avoidDuplicates && emotion.HasAudioClip(newAudioClip)) continue;
                        emotion.audioClip.Add(newAudioClip);
                    }
                }
            }
        }

        private void SetupVoices()
        {
            if (!HasKey(name + " " + buttonNames[0])) 
                SetBool(name + " " + buttonNames[0], true);

            if (Script.types.Count == 0)
                Script.types.Add("Voice");
            
            if (!HasKey("Voice New Type"))
                SetString("Voice New Type", "New Type");
        }

        private void ShowPanelButtons()
        {
            StartRow();
            for (int i = 0; i < buttonNames.Length; i++)
            {
                BackgroundColor(GetBool(name + " " + buttonNames[i]) ? active : dark);
                if (Button(buttonNames[i]))
                {
                    for (int b = 0; b < buttonNames.Length; b++)
                    {
                        if (b == i)
                            SetBool(name + " " + buttonNames[b], !GetBool(name + " " + buttonNames[b]));
                        else
                            SetBool(name + " " + buttonNames[b], false);
                    }
                }
            }
            ResetColor();
            EndRow();
            Space();
        }

        public void ShowVoices(Voices voices)
        {
            MessageBox(
                "These are the individual character voices who may speak during your game. Try giving them " +
                "names, or use a naming convention that is logical based on your project. You can even use this system " +
                "to manage non-voice AudioClips.", MessageType.Info);
            StartRow();
            DisplayNewVoiceBox();
            DisplayNewTypeBox();
            EndRow();
            Space();
            DisplayVoices();
            Space();
            DisplayTypes();
        }

        public void DisplayTypes()
        {
            SetBool("Voices Show Types", EditorGUILayout.Foldout(GetBool("Voices Show Types"), "Manage Types"));
            if (!GetBool("Voices Show Types")) return;

            for (int i = 0; i < Script.types.Count; i++)
            {
                StartRow();
                string tempType = DelayedText(Script.types[i], 200);

                if (!String.IsNullOrWhiteSpace(tempType) && tempType != Script.types[i])
                {
                    foreach (Voice voice in Script.voices)
                    {
                        if (voice.type != Script.types[i]) continue;
                        voice.type = tempType;
                    }
                    Script.types[i] = tempType;
                }

                if (Script.types.Count > 1)
                {
                    BackgroundColor(Color.red);
                    if (Button("X", 25))
                    {
                        Undo.RecordObject(Script, "Delete Type");
                        foreach (Voice voice in Script.voices)
                        {
                            string newType = Script.types[0];
                            if (i == 0) newType = Script.types[1];

                            if (voice.type == Script.types[i])
                                voice.type = newType;
                        }

                        Script.types.RemoveAt(i);
                        ExitGUI();
                    }
                    ResetColor();
                }
                
                EndRow();
            }
        }

        public void ShowLines(Voices voices)
        {
            MessageBox(
                "Each line is a potential thing your characters can say. Not all voices need to be able to say " +
                "all lines, but you'll have the option to populate AudioClips for each line you specify, for each voice " +
                "you've created.", MessageType.Info);
            DisplayNewLineBox();
            Space();
            DisplayLines();
        }
        
        public void ShowEmotions(Voices voices)
        {
            MessageBox(
                "For each line there is a default AudioClip. Here you can identify various emotions for each " +
                "line. In your code, you can call the default AudioClip for the line, or one of the emotions instead, " +
                "depending on the logic of your game.", MessageType.Info);
            DisplayNewEmotionBox();
            Space();
            DisplayEmotions();
        }

        private void DisplayLines()
        {
            for (int i = 0; i < Script.lineNames.Count; i++)
            {
                StartRow();
                string tempNameLine = Script.lineNames[i];
                Script.lineNames[i] = StrippedString(DelayedText(Script.lineNames[i]));
                if (TryToChangeFileNames(tempNameLine, Script.lineNames[i], 1))
                    UpdateLineNames(Script.lineNames[i], i);
                else
                    Script.lineNames[i] = tempNameLine;
                BackgroundColor(Color.red);
                if (Button("X", 25))
                {
                    Undo.RecordObject(Script, "Delete Line");
                    Script.lineNames.RemoveAt(i);
                    foreach (Voice voice in Script.voices)
                    {
                        voice.lines.RemoveAt(i);
                    }
                    ExitGUI();
                }
                ResetColor();
                EndRow();
            }
        }
        
        private void DisplayEmotions()
        {
            for (int i = 0; i < Script.emotionNames.Count; i++)
            {
                StartRow();
                var tempNameEmotion = Script.emotionNames[i];
                Script.emotionNames[i] = StrippedString(DelayedText(Script.emotionNames[i]));
                if (TryToChangeFileNames(tempNameEmotion, Script.emotionNames[i], 2))
                    UpdateEmotionNames(Script.emotionNames[i], i);
                else
                    Script.emotionNames[i] = tempNameEmotion;
                BackgroundColor(Color.red);
                if (Button("X", 25))
                {
                    Undo.RecordObject(Script, "Delete Emotion");
                    Script.emotionNames.RemoveAt(i);
                    foreach (Voice voice in Script.voices)
                    {
                        foreach (Line line in voice.lines)
                        {
                            line.emotions.RemoveAt(i);
                        }
                    }
                    ExitGUI();
                }
                ResetColor();
                EndRow();
            }
        }

        private bool IsInt(string test)
        {
            int testInt;
            if (int.TryParse(test, out testInt))
                return true;

            return false;
        }

        private string StrippedString(string stringToStrip)
        {
            stringToStrip = stringToStrip.Replace(" ", "");
            return Regex.Replace(stringToStrip, @"[^A-Za-z0-9]+", "");
        }
        
        private bool TryToChangeFileNames(string oldName, string newName, int segment)
        {
            if (String.IsNullOrWhiteSpace(newName)) return false; // Make sure the newName isn't empty
            if (oldName == newName) return false; // If the names don't match, return

            // Check to make sure the newName isn't just a number. It needs to be a string.
            if (IsInt(newName))
            {
                Debug.LogError("Error: You can't change a name segment into a number.");
                return false;
            }

            
            // Make a List<AudioCLip> with all the clips assigned through the Voices module.
            List<AudioClip> audioClips = new List<AudioClip>();
            foreach (Voice voice in Script.voices)
            {
                foreach (Line line in voice.lines)
                {
                    foreach (AudioClip clip in line.audioClip)
                        audioClips.Add(clip); // Add a clip

                    foreach (Emotion emotion in line.emotions)
                    {
                        foreach (AudioClip clip in emotion.audioClip)
                            audioClips.Add(clip); // Add a clip
                    }
                }
            }

            // Work on each clip, if the clip can be worked on...
            foreach (AudioClip clip in audioClips)
            {
                // Split the name into the array nameSegments
                string[] nameSegments = clip.name.Split(char.Parse("_"));
                
                // Make sure we have enough segments -- or don't do this clip at all, skip it
                if (nameSegments.Length <= segment) 
                    continue;
                
                // Make sure the string we are replacing is not a number
                if (IsInt(nameSegments[segment]))
                    continue;
                
                // Continue if the segment name does not match oldName
                if (nameSegments[segment] != oldName)
                    continue;

                // Rebuild the fullName with the newName replacing the selected segment
                nameSegments[segment] = newName;
                string fullName = "";
                for (int i = 0; i < nameSegments.Length; i++)
                {
                    if (i > 0)
                        fullName = fullName + "_";

                    fullName = fullName + nameSegments[i];
                }
                
                // Rename the file
                var path = AssetDatabase.GetAssetPath(clip);
                AssetDatabase.RenameAsset(path, fullName);
            }

            return true;
        }

        private void UpdateLineNames(string newName, int i)
        {
            foreach (Voice voice in Script.voices)
            {
                voice.lines[i].line = newName;
            }
        }
        
        private void UpdateEmotionNames(string newName, int i)
        {
            foreach (Voice voice in Script.voices)
            {
                foreach (Line line in voice.lines)
                {
                    line.emotions[i].emotion = newName;   
                }
            }
        }

        private void DisplayVoices()
        {
            for (int i = 0; i < Script.voices.Count; i++)
            {
                Voice voice = Script.voices[i];
                StartRow();
                
                // Expand button
                BackgroundColor(Color.green);
                if (Button(GetBool("Show Voice " + i) ? "-" : "O", 25))
                {
                    SetBool("Show Voice " + i, !GetBool("Show Voice " + i));
                }
                ResetColor();
                
                // Name of voice
                string tempNameVoice = voice.voice;
                voice.voice = StrippedString(DelayedText(voice.voice, 300));
                if (TryToChangeFileNames(tempNameVoice, voice.voice, 0))
                    UpdateEmotionNames(voice.voice, i);
                else
                    voice.voice = tempNameVoice;
                
                // Types
                Undo.RecordObject(Script, "Type type");
                voice.type = Script.types[Popup(Script.GetTypeIndex(voice.type), Script.types.ToArray(), 200)];
                
                EndRow();
                if (!GetBool("Show Voice " + i))
                    continue;

                foreach (Line line in voice.lines)
                {
                    StartRow();
                    StartVertical();
                    ContentColor(line.audioClip.Count == 0 ? Color.grey : Color.white);
                    Label(line.line + " - Default", 200);
                    ResetColor();
                    EndVertical();
                    StartVertical();
                    for (int l = 0; l < line.audioClip.Count; l++)
                    {
                        StartRow();
                        line.audioClip[l] = Object(line.audioClip[l], typeof(AudioClip), 150) as AudioClip;
                        if (Button("Play", 70))
                        {
                            PlayClip(line.audioClip[l], 0, false);
                        }

                        if (Button("Copy > Voices", "Copy this AudioClip to all " + line.line + " - Default lists on all other voices.", 100))
                        {
                            Undo.RecordObject(Script, "Copy To All");
                            AddAudioClipToAllLines(line.line, line.audioClip[l]);
                        }
                        BackgroundColor(Color.red);
                        if (Button("X", 25))
                        {
                            Undo.RecordObject(Script, "Delete Clip");
                            line.audioClip.RemoveAt(l);
                            ExitGUI();
                        }
                        ResetColor();
                        EndRow();
                    }
                    StartRow();
                    BackgroundColor(Color.grey);
                    ContentColor(Color.grey);
                    line.newAudioClip = Object(line.newAudioClip, typeof(AudioClip), 150) as AudioClip;
                    Label("Add new AudioClip here");
                    ResetColor();
                    EndRow();
                    EndVertical();
                    EndRow();
                    
                    foreach (Emotion emotion in line.emotions)
                    {
                        StartRow();
                        StartVertical();
                        ContentColor(emotion.audioClip.Count == 0 ? Color.grey : Color.white);
                        Label(line.line + " - " + emotion.emotion, 200);
                        EndVertical();
                        StartVertical();
                        for (int e = 0; e < emotion.audioClip.Count; e++)
                        {
                            StartRow();
                            emotion.audioClip[e] = Object(emotion.audioClip[e], typeof(AudioClip), 150) as AudioClip;
                            if (Button("Play", 70))
                            {
                                PlayClip(emotion.audioClip[e], 0, false);
                            }
                            if (Button("Copy > Voices", "Copy this AudioClip to all " + line.line + " - " + emotion.emotion + " lists on all other voices.", 100))
                            {
                                Undo.RecordObject(Script, "Copy To All");
                                AddAudioClipToAllEmotions(emotion.emotion, emotion.audioClip[e]);
                            }
                            BackgroundColor(Color.red);
                            if (Button("X", 25))
                            {
                                Undo.RecordObject(Script, "Delete Clip");
                                emotion.audioClip.RemoveAt(e);
                                ExitGUI();
                            }
                            ResetColor();
                            EndRow();
                        }
                        StartRow();
                        BackgroundColor(Color.grey);
                        ContentColor(Color.grey);
                        emotion.newAudioClip = Object(emotion.newAudioClip, typeof(AudioClip), 150) as AudioClip;
                        Label("Add new AudioClip here");
                        ResetColor();
                        EndRow();
                        EndVertical();
                        EndRow();
                    }
                    if (line.emotions.Count > 0) Space();
                }
                
                BackgroundColor(Color.red);
                if (Button("Delete " + voice.voice, 300))
                {
                    if (Dialog("Delete voice", "Do you really want to delete " + voice.voice + "?"))
                    {
                        Undo.RecordObject(Script, "Delete Voice");
                        Script.voices.RemoveAt(i);
                        ExitGUI();
                    }
                }
                ResetColor();

                Space();
            }
        }

        private void DisplayAutoFillBox()
        {
            Space();
            StartVerticalBox();
            Label(String.IsNullOrWhiteSpace(Script.autoFillDirectory) ? 
                "Select the directory holding your audio files" : 
                Script.autoFillDirectory, true);
            StartRow();

            
            
            Object tempObject = AssetDatabase.LoadAssetAtPath(Script.autoFillDirectory, typeof(Object));

            Object userObject = Object(tempObject, typeof(Object), 300);
            if (userObject && tempObject != userObject)
                tempObject = userObject;
            
            Script.autoFillDirectory = AssetDatabase.GetAssetPath(tempObject);

            EndRow();

            if (tempObject)
            {
                BackgroundColor(Color.green);
                if (Button("Auto Fill Contents"))
                {
                    Undo.RecordObject(Script, "Auto Fill Contents");
                    AutoFillContents();
                }
            }
            
            EndVertical();
            ResetColor();
        }

        /// <summary>
        /// This will auto fill all the contents into the structure from the selected folder
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void AutoFillContents()
        {
            string[] guidsToFiles = AssetDatabase.FindAssets("t:AudioClip",new[] {Script.autoFillDirectory});
            
            foreach (string guid in guidsToFiles)
            {
                AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(AssetDatabase.GUIDToAssetPath(guid));
                string clipName = clip.name;
                string[] nameSegments = clipName.Split(char.Parse("_"));

                if (nameSegments.Length < 2)
                {
                    Debug.LogError("Error: There are only " + nameSegments.Length + " segements " +
                                   "in the name " + clipName + ". Please read the instructions on " +
                                   "how to properly name your AudioClip files for this module. " +
                                   "We will skip this file.");
                    continue;
                }
                
                // Add Voice, Line, Emotion
                if (!Script.HasVoice(nameSegments[0]))
                    Script.AddVoice(nameSegments[0]);
                
                if (!Script.HasLine(nameSegments[1]))
                    Script.AddLine(nameSegments[1]);

                bool hasEmotion = false;
                if (nameSegments.Length >= 3)
                {
                    if (IsInt(nameSegments[2]))
                    {
                        Debug.LogWarning("Warning: The third segment was a number, so we did not " +
                                         "try to make it into an \"Emotion\". We will now skip the " +
                                         "file " + clipName);
                    }
                    else
                    {
                        hasEmotion = true;
                        if (!Script.HasEmotion(nameSegments[2]))
                            Script.AddEmotion(nameSegments[2]);
                    }
                }
                
                // Fill the AudioClip field
                Voice voice = Script.GetVoice(nameSegments[0]);
                Line line = voice.GetLine(nameSegments[1]);
                if (!hasEmotion)
                {
                    if (!line.HasAudioClip(clip))
                        line.audioClip.Add(clip);
                }
                else
                {
                    Emotion emotion = line.GetEmotion(nameSegments[2]);
                    if (!emotion.HasAudioClip(clip))
                        emotion.audioClip.Add(clip);
                }
            }
        }

        private void DisplayNewVoiceBox()
        {
            Space();
            BackgroundColor(mixed);
            StartVerticalBox();
            Label("Create new Voice", true);
            StartRow();
            newVoiceName = TextField(newVoiceName, 200);
            if (Button("Add", 100))
            {
                newVoiceName = newVoiceName.Trim();
                if (Script.HasVoice(newVoiceName))
                    Debug.Log("There is already a Voice named " + newVoiceName);
                else
                {
                    Undo.RecordObject(Script, "Add Voice");
                    Script.AddVoice(newVoiceName);
                }
            }

            EndRow();
            EndVertical();
            ResetColor();
        }
        
        private void DisplayNewTypeBox()
        {
            Space();
            BackgroundColor(mixed);
            StartVerticalBox();
            Label("Add new type", true);
            StartRow();
            SetString("Voice New Type", TextField(GetString("Voice New Type"), 200));
            if (Button("Add", 100))
            {
                if (Script.HasType(GetString("Voice New Type")))
                    Debug.Log("There is already a type named " + GetString("Voice New Type"));
                else
                {
                    Undo.RecordObject(Script, "Add Type");
                    Script.AddType(GetString("Voice New Type"));
                }
            }

            EndRow();
            EndVertical();
            ResetColor();
        }
        
        private void DisplayNewLineBox()
        {
            Space();
            BackgroundColor(mixed);
            StartVerticalBox();
            Label("Create new Line", true);
            StartRow();
            newLineName = TextField(newLineName, 200);
            if (Button("Add", 100))
            {
                newLineName = newLineName.Trim();
                if (Script.HasLine(newLineName))
                    Debug.Log("There is already a Line named " + newLineName);
                else
                {
                    Undo.RecordObject(Script, "Add Line");
                    Script.AddLine(newLineName);
                }
            }

            EndRow();
            EndVertical();
            ResetColor();
        }
        
        private void DisplayNewEmotionBox()
        {
            Space();
            BackgroundColor(mixed);
            StartVerticalBox();
            Label("Create new Emotion", true);
            StartRow();
            newEmotionName = TextField(newEmotionName, 200);
            if (Button("Add", 100))
            {
                newEmotionName = newEmotionName.Trim();
                if (Script.HasEmotion(newEmotionName))
                    Debug.Log("There is already an Emotion named " + newEmotionName);
                else
                {
                    Undo.RecordObject(Script, "Add Emotion");
                    Script.AddEmotion(newEmotionName);
                }
            }

            EndRow();
            EndVertical();
            ResetColor();
        }

        private void PlayClip(AudioClip clip , int startSample , bool loop) {
            Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
            Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            MethodInfo method = audioUtilClass.GetMethod(
                "PlayClip",
                BindingFlags.Static | BindingFlags.Public,
                null,
                new System.Type[] {
                    typeof(AudioClip),
                    typeof(Int32),
                    typeof(Boolean)
                },
                null
            );
            method.Invoke(
                null,
                new object[] {
                    clip,
                    startSample,
                    loop
                }
            );
        }










        private string newVoiceName = "New Voice Name";
        private string newLineName = "New Line";
        private string newEmotionName = "New Emotion";
        
        
        
    }
}