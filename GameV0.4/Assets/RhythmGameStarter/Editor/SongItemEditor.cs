using UnityEditor;
using System.IO;
using System;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

namespace RhythmGameStarter
{
    [CustomEditor(typeof(SongItem))]
    public class SongItemEditor : Editor
    {
        private MidiFile FindMidiFile()
        {
            var path = AssetDatabase.GetAssetPath((SongItem)target);

            string filePath = path.Substring(0, path.Length - Path.GetFileName(path).Length);

            string[] files = Directory.GetFiles(filePath, "*.mid");
            if (files.Length > 0)
            {
                // string midiFileName = Path.Combine(filePath, );
                return MidiFile.Read(files[0]);
            }
            return null;
        }

        public override void OnInspectorGUI()
        {
            var midi = (SongItem)target;

            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("clip"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("bpm"));
            var changed = EditorGUI.EndChangeCheck();
            if (GUILayout.Button("Update bpm") || changed)
            {
                serializedObject.ApplyModifiedProperties();

                UpdateBpm(FindMidiFile(), midi);

                serializedObject.Update();
            }

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("notes"),true);
            EditorGUI.EndDisabledGroup();

            serializedObject.ApplyModifiedProperties();
        }

        public static void UpdateBpm(MidiFile rawMidi, SongItem midi)
        {
            if (rawMidi == null)
            {
                Debug.LogError("Cannot find the midi file");
                return;
            }
            midi.notes.Clear();
            if (midi.bpm <= 0) return;
            var tempoMap = TempoMap.Create(Tempo.FromBeatsPerMinute(midi.bpm));
            foreach (var note in rawMidi.GetNotes())
            {
                midi.notes.Add(new SongItem.MidiNote()
                {
                    noteName = ParseEnum<SongItem.NoteName>(note.NoteName.ToString()),
                    noteOctave = note.Octave,
                    time = GetMetricTimeSpanTotal(note.TimeAs<MetricTimeSpan>(tempoMap)),
                    noteLength = GetMetricTimeSpanTotal(note.LengthAs<MetricTimeSpan>(tempoMap))
                });
            }
            EditorUtility.SetDirty(midi);
        }

        private static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        private static float GetMetricTimeSpanTotal(MetricTimeSpan ts)
        {
            return (float)ts.TotalMicroseconds / 1000000f;
        }
    }
}