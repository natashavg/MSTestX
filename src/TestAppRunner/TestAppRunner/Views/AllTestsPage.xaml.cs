﻿using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAppRunner.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TestAppRunner.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public partial class AllTestsPage : ContentPage
    {
        internal AllTestsPage()
		{
			InitializeComponent ();
            this.BindingContext = TestRunnerVM.Instance;
            TestRunnerVM.Instance.OnTestRunException += Instance_OnTestRunException;

            picker.ItemsSource = new string[] { "Category", "Namespace", "Outcome" };
            picker.SelectedIndex = 1;
        }

        private void Instance_OnTestRunException(object sender, Exception e)
        {
            ErrorHeader.Text = "Test Run Error";
            ErrorMessage.Text = e.Message;
            ErrorPanel.IsVisible = true;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (TestRunnerVM.Instance.IsRunning)
                TestRunnerVM.Instance.Cancel();
            else
            {
                try
                {
                    await TestRunnerVM.Instance.Run();
                }
                catch { }
            }
        }

        private async void list_ItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as TestResultGroup;
            if (item == null)
                return;

            await Navigation.PushAsync(new GroupByClassTestsPage(item));

            // Manually deselect item.
            (sender as ListView).SelectedItem = null;
        }

        private void picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            TestRunnerVM.Instance.UpdateGroup(((string[])picker.ItemsSource)[picker.SelectedIndex]);
        }

        private void Error_Close_Button_Clicked(object sender, EventArgs e)
        {
            ErrorPanel.IsVisible = false;
        }
    }
}