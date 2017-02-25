﻿using System;
using Xamarin.Forms;
using Showdown.Examples;

namespace Showdown
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        async void OpenListViewPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListViewPage1());
        }
    }
}