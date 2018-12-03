﻿using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WpfGame.Generals;
using WpfGame.Views;

namespace WpfGame.Controllers.Views
{
    class StartWindowViewController : ViewController
    {
        private StartWindowView _startWindowView;

        public StartWindowViewController(MainWindow mainWindow) : base(mainWindow)
        {
            _startWindowView = new StartWindowView();

            SetContentOfMain(mainWindow, _startWindowView);

            SetButtonEvents(_startWindowView.btnCloseGame, BtnCloseGame_Click);
            SetButtonEvents(_startWindowView.btnDesignLevel, BtnDesignLevel_Click);
            SetButtonEvents(_startWindowView.btnStartGame, BtnStartGameOnClick);
            SetButtonEvents(_startWindowView.CancelSelectPlgrnd, BtnCancelSelect_Click);
            SetButtonEvents(_startWindowView.SelectPlgrnd, BtnConfirmSelect_Click);
            SetButtonEvents(_startWindowView.BtnHighScoreTable, BtnHighScoreTable_Click);

            SetKeyEvents(_startWindowView.Grid, Grid_KeyDown);
            
            _startWindowView.Grid.Loaded += OnGridLoaded;
        }

        private void OnGridLoaded(object sender, RoutedEventArgs e)
        {
            _startWindowView.Grid.Focus();
        }
        
        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F2:
                    try
                    {
                        FileInfo[] files =
                            new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + General.playgroundPath)
                                .GetFiles();
                        _startWindowView.ListBoxForPlaygroundFiles.ItemsSource = files;
                        _startWindowView.SelectPlaygroundMenu.Visibility = Visibility.Visible;

                    }
                    catch (DirectoryNotFoundException)
                    {
                        MessageBox.Show(
                            "Your exe-folder doesn't contain a Playgrounds folder. Unable to create a playgrounds-selection",
                            "Playgrounds folder not found", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Something went horrible wrong. Please contact your software-supplier." + ex.Message + " " + ex.StackTrace,
                            "Help", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;
            }
        }
        private void BtnCancelSelect_Click(object sender, RoutedEventArgs e)
        {
            _startWindowView.SelectPlaygroundMenu.Visibility = Visibility.Collapsed;
        }

        private void BtnConfirmSelect_Click(object sender, RoutedEventArgs e)
        {
            _startWindowView.SelectPlaygroundMenu.Visibility = Visibility.Collapsed;
            MessageBox.Show("Level selected, you can start playing", "Well done", MessageBoxButton.OK,
                MessageBoxImage.Information);
            Settings.Default.Level = _startWindowView.ListBoxForPlaygroundFiles.SelectedItem.ToString();
        }

        private void BtnStartGameOnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Settings.Default.Level))
            {
                //if non is selected we select the newest playground
                try
                {
                    var directory = new DirectoryInfo($"{Environment.CurrentDirectory}\\Playgrounds");

                    Settings.Default.Level =
                        (from f in directory.GetFiles()
                            orderby f.LastWriteTime descending
                            select f).First().ToString();
                }
                catch (DirectoryNotFoundException)
                {
                    MessageBox.Show("Your exe-folder doesn't contain a Playgrounds folder. Unable to start a game.",
                        "Playgrounds folder not found", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (InvalidOperationException)
                {

                    MessageBox.Show("Your Playgrounds folder doesn't contain a file. Please create or download a playground.",
                        "Playgrounds folder empty", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something went horrible wrong. Please contact your software-supplier." + ex.Message + " " + ex.StackTrace,
                        "Help", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
            
            //Call the UsernameViewController when the Username has not been set
            if (string.IsNullOrEmpty(Settings.Default.Username))
            {
                new UsernameViewController(_mainWindow, Settings.Default.Level);
                return;
            }

            new GameViewController(_mainWindow, Settings.Default.Level);

        }

        private void BtnDesignLevel_Click(object sender, RoutedEventArgs e)
        {
            new EditorViewController(_mainWindow);
        }

        private void BtnCloseGame_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnHighScoreTable_Click (object sender, RoutedEventArgs e)
        {
            new HighScoreViewController(_mainWindow);
        }
    }
}
