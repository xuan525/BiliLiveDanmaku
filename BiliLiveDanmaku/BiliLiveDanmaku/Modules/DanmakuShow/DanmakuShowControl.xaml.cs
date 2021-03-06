﻿using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace BiliLiveDanmaku.Modules
{
    /// <summary>
    /// Interaction logic for DisplayControl.xaml
    /// </summary>
    public partial class DanmakuShowControl : UserControl
    {
        private DanmakuShowModule Module { get; set; }

        public DanmakuShowControl(DanmakuShowModule displayModule)
        {
            InitializeComponent();

            Module = displayModule;

            foreach (DanmakuShowConfig.DisplayFilterOptions filterOption in Enum.GetValues(typeof(DanmakuShowConfig.DisplayFilterOptions)))
            {
                bool initValue = true;
                if (displayModule.OptionDict.ContainsKey(filterOption))
                {
                    initValue = displayModule.OptionDict[filterOption];
                }
                else
                {
                    displayModule.OptionDict.Add(filterOption, initValue);
                }

                DescriptionAttribute[] attributes = (DescriptionAttribute[])filterOption
                   .GetType()
                   .GetField(filterOption.ToString())
                   .GetCustomAttributes(typeof(DescriptionAttribute), false);
                string description = attributes.Length > 0 ? attributes[0].Description : string.Empty;

                CheckBox checkBox = new CheckBox
                {
                    Content = description,
                    IsChecked = initValue,
                    Margin = new Thickness(4),
                    VerticalAlignment = VerticalAlignment.Center,
                    Tag = filterOption
                };
                checkBox.Checked += ShowOptionCkb_Checked;
                checkBox.Unchecked += ShowOptionCkb_Unchecked;
                OptionPanel.Children.Add(checkBox);
            }
        }

        private void ShowOptionCkb_Checked(object sender, RoutedEventArgs e)
        {
            if (!IsInitialized)
                return;
            CheckBox checkBox = (CheckBox)sender;
            DanmakuShowConfig.DisplayFilterOptions filterOptions = (DanmakuShowConfig.DisplayFilterOptions)checkBox.Tag;
            Module.OptionDict[filterOptions] = true;
        }

        private void ShowOptionCkb_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!IsInitialized)
                return;
            CheckBox checkBox = (CheckBox)sender;
            DanmakuShowConfig.DisplayFilterOptions filterOptions = (DanmakuShowConfig.DisplayFilterOptions)checkBox.Tag;
            Module.OptionDict[filterOptions] = false;
        }

        private void CaptureBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!IsInitialized)
                return;
            Module.CaptureSnapshot();
        }

        public void SetLock(bool value)
        {
            LockBtn.IsChecked = value;
        }

        private void LockBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (!IsInitialized)
                return;
            Module.LockWindow(true);
        }

        private void LockBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!IsInitialized)
                return;
            Module.LockWindow(false);
        }
    }
}
