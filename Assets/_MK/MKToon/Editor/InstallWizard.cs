﻿//////////////////////////////////////////////////////
// MK Install Wizard Base                      	    //
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2021 All rights reserved.            //
//////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace MK.Toon.Editor.InstallWizard
{
    public sealed class InstallWizard : EditorWindow
    {
        #pragma warning disable CS0414
        private static readonly string _version = "3.0.14.4C";
        #pragma warning restore CS0414
        
        private static readonly Vector2Int _referenceResolution = new Vector2Int(2560, 1440);
        private static float _sizeScale;
        private static int _scaledWidth;
        private static int _scaledHeight;
        private static Vector2 _windowScrollPos;

        private static readonly int _rawWidth = 360;
        private static readonly int _rawHeight = 640;
        private static readonly string _title = "MK Toon Install Wizard";

        private GUIStyle _flowTextStyle { get { return new GUIStyle(EditorStyles.label) { wordWrap = true }; } }
        private static readonly int _loadTimeInFrames = 72;
        private static int _waitFramesTillReload = _loadTimeInFrames;

        private static InstallWizard _window;
        private static RenderPipeline _targetRenderPipeline = RenderPipeline.Built_in;
        private static bool _showInstallerOnReload = true;

        [MenuItem("Window/MK/Toon/Install Wizard")]
        private static void ShowWindow()
        {
            if(Screen.currentResolution.height > Screen.currentResolution.width)
                _sizeScale = (float) Screen.currentResolution.width / (float)_referenceResolution.x;
            else
                _sizeScale = (float) Screen.currentResolution.height / (float)_referenceResolution.y;

            _scaledWidth = (int)((float)_rawWidth * _sizeScale);
            _scaledHeight = (int)((float)_rawHeight * _sizeScale);
            _window = (InstallWizard)EditorWindow.GetWindow<InstallWizard>(true, _title, true);
            _window.minSize = new Vector2(_scaledWidth, _scaledHeight);
            _window.maxSize = new Vector2(_scaledWidth * 2, _scaledHeight * 2);
            Configuration.ConfigureGlobalShaderFeatures();
            Configuration.BeginRegisterChangesOnGlobalShaderFeatures();
            Configuration.SetCompileDirectives();
            _window.Show();
        }

        [InitializeOnLoadMethod]
        private static void ShowInstallerOnReload()
        {
            QueryReload();
        }

        private static void QueryReload()
        {
            _waitFramesTillReload = _loadTimeInFrames;
            EditorApplication.update += Reload;
        }

        private static void Reload()
        {
            if (_waitFramesTillReload > 0)
            {
                --_waitFramesTillReload;
            }
            else
            {
                EditorApplication.update -= Reload;
                if(Configuration.isReady && Configuration.TryGetShowInstallerOnReload())
                    ShowWindow();
            }
        }

        private void OnGUI()
        {
            if(Configuration.isReady)
            {
                _windowScrollPos = EditorGUILayout.BeginScrollView(_windowScrollPos);
                Texture2D titleImage = Configuration.TryGetTitleImage();
                if(titleImage)
                {
                    float titleScaledWidth = EditorGUIUtility.currentViewWidth - EditorGUIUtility.standardVerticalSpacing * 4;
                    float titleScaledHeight = titleScaledWidth * ((float)titleImage.height / (float)titleImage.width);
                    Rect titleRect = EditorGUILayout.GetControlRect();
                    titleRect.width = titleScaledWidth;
                    titleRect.height = titleScaledHeight;
                    GUI.DrawTexture(titleRect, titleImage, ScaleMode.ScaleToFit);
                    GUILayout.Label("", GUILayout.Height(titleScaledHeight - 20));
                    Divider();
                }
                EditorGUILayout.LabelField("1. Select your Render Pipeline", UnityEditor.EditorStyles.boldLabel);
                _targetRenderPipeline = Configuration.TryGetRenderPipeline();
                EditorGUI.BeginChangeCheck();
                _targetRenderPipeline = (RenderPipeline) EditorGUILayout.EnumPopup("Render Pipeline", _targetRenderPipeline);
                if(EditorGUI.EndChangeCheck())
                    Configuration.TrySetRenderPipeline(_targetRenderPipeline);
                VerticalSpace();
                Divider();
                VerticalSpace();
                EditorGUILayout.LabelField("2. Import Package", UnityEditor.EditorStyles.boldLabel);
                if(GUILayout.Button("Import / Update Package"))
                {
                    EditorUtility.DisplayProgressBar("MK Toon Install Wizard", "Importing Package", 0.5f);
                    Configuration.ImportShaders(_targetRenderPipeline);
                    EditorUtility.ClearProgressBar();
                }
                VerticalSpace();
                Divider();
                VerticalSpace();
                int readMeNumber = 4;
                /*
                if(_targetRenderPipeline == RenderPipeline.Lightweight)
                {
                    readMeNumber = 3;
                    EditorGUILayout.LabelField("3. Examples are not available for the Lightweight Render Pipeline.", _flowTextStyle);
                    VerticalSpace();
                    Divider();
                }
                else
                */
                {
                    EditorGUILayout.LabelField("3. Import Examples (optional)", UnityEditor.EditorStyles.boldLabel);
                    switch(_targetRenderPipeline)
                    {
                        case RenderPipeline.Built_in:
                        EditorGUILayout.LabelField("Make sure Postprocessing Stack v2 and Text Mesh Pro is installed.", _flowTextStyle);
                        EditorGUILayout.LabelField("Example Scenes are based on Linear Color Space. Make sure to change from Gamma to Linear Color Space via Project Settings.", _flowTextStyle);
                        break;
                        //case RenderPipeline.Lightweight:
                        //EditorGUILayout.LabelField("Make sure Text Mesh Pro is installed first!", _flowTextStyle);
                        //break;
                        case RenderPipeline.Universal:
                        EditorGUILayout.LabelField("Make sure Text Mesh Pro is installed.", _flowTextStyle);
                        break;
                    }
                    EditorGUILayout.LabelField("Old Input Manager is used for the examples.", _flowTextStyle);
                    if(GUILayout.Button("Import Examples"))
                    {
                        EditorUtility.DisplayProgressBar("MK Toon Install Wizard", "Importing Examples", 0.5f);
                        Configuration.ImportExamples(_targetRenderPipeline);
                        EditorUtility.ClearProgressBar();
                    }
                    VerticalSpace();
                    Divider();
                    ExampleContainer[] examples = Configuration.TryGetExamples();
                    if(examples.Length > 0 && examples[0].scene != null)
                    {
                        VerticalSpace();
                        EditorGUILayout.LabelField("Example Scenes:");
                        EditorGUILayout.BeginHorizontal();
                        for(int i = 0; i < examples.Length; i++)
                        {
                            if(examples[i].scene != null)
                                examples[i].DrawEditorButton();
                        }
                        EditorGUILayout.EndHorizontal();
                        VerticalSpace();
                        Divider();
                    }
                }
                VerticalSpace();
                EditorGUILayout.LabelField(readMeNumber.ToString() + ". Read Me (Recommended)", UnityEditor.EditorStyles.boldLabel);
                if(GUILayout.Button("Open Read Me"))
                {
                    Configuration.OpenReadMe();
                }

                VerticalSpace();
                Divider();
                VerticalSpace();

                EditorGUILayout.LabelField("5. Customize the Shaders (optional)", UnityEditor.EditorStyles.boldLabel);

                EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth * 0.67f;
                Configuration.DrawGlobalShaderFeaturesInspector();
                
                if(Configuration.CheckGlobalShaderFeaturesChanges())
                {
                    if(GUILayout.Button("Apply Changes"))
                    {
                        Configuration.ConfigureGlobalShaderFeatures();
                        Configuration.BeginRegisterChangesOnGlobalShaderFeatures();
                    }
                }

                VerticalSpace();
                Divider();
                VerticalSpace();
                _showInstallerOnReload = Configuration.TryGetShowInstallerOnReload();
                EditorGUI.BeginChangeCheck();
                _showInstallerOnReload = EditorGUILayout.Toggle("Show Installer On Reload", _showInstallerOnReload);
                if(EditorGUI.EndChangeCheck())
                    Configuration.TrySetShowInstallerOnReload(_showInstallerOnReload);

                EditorGUILayout.EndScrollView();
                GUI.FocusControl(null);
            }
            else
            {
                Repaint();
            }
        }

        private static void VerticalSpace()
        {
            GUILayoutUtility.GetRect(1f, EditorGUIUtility.standardVerticalSpacing);
        }

        private static void Divider()
        {
            GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(2) });
        }
    }
}
#endif